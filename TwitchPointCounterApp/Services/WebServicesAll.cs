﻿using Newtonsoft.Json.Linq;
using NHttp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Subscriptions;
using TwitchLib.Api.Helix.Models.Users.GetUsers;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchPointCounterApp.DataProtected;
using TwitchPointCounterApp.objects;

namespace TwitchPointCounterApp.Services
{
        // https://youtu.be/Ufgq6_QhVKw?si=lql8qcjQuGFql5WI&t=2477
    public class WebServicesAll
    {
        private MainForm _form;
        private HttpServer webServer;

        private readonly string ClientId     = Properties.Settings.Default.clientid;
        private readonly string ClientSecret = Properties.Settings.Default.clientsecret;

        private string CachedOwnerOfChannelAccessToken = "needsaccesstoken";
        private string TwitchChannelId;
        private string TwitchChannelName;

        private readonly List<string> Scopes = new List<string> { 
            "user:edit", "chat:read", "chat:edit", "channel:moderate", "whispers:read", "whispers:edit", "bits:read", "channel:read:subscriptions", "user:read:email", "user:read:subscriptions"
        };
        private readonly string RedirectURL = "http://localhost";

        private TwitchClient OwnerOfChannelConnection;
        private TwitchAPI TheTwitchAPI;

        public Dictionary<string, string> CommandStaticResponses;

        public WebServicesAll(MainForm form, Dictionary<string, string> command, Dictionary<string, string> tokenData)
        {
            _form = form;
            CommandStaticResponses = command;
            if (tokenData.ContainsKey("AccessToken"))
            {
                CachedOwnerOfChannelAccessToken = tokenData["AccessToken"];
                TwitchChannelId                 = tokenData["ID"];
                TwitchChannelName               = tokenData["Name"];
                InitializeOwnerOfChanenelConnection(TwitchChannelName, CachedOwnerOfChannelAccessToken);
                InitializeTwitchAPI(CachedOwnerOfChannelAccessToken);
            }
        }
        
        public WebServicesAll(MainForm form, Dictionary<string, string> command)
        {
            _form = form;
            CommandStaticResponses = command;
        }
        
        public void InitializeWebServer()
        {
            webServer = new HttpServer();
            webServer.EndPoint = new IPEndPoint(IPAddress.Loopback, 3000);

            webServer.RequestReceived += async (s, e) =>
            {
                using (StreamWriter writer = new StreamWriter(e.Response.OutputStream))
                {
                    if (e.Request.QueryString.AllKeys.Any("code".Contains))
                    {
                        Log("PROCESS TOKEN : START");
                        string code = e.Request.QueryString["code"];
                        Log("TOKEN : "+code);
                        Tuple<string, string> ownerOfChanelAccessAndRefresh = await GetAccessAndRefreshTokens(code);
                        CachedOwnerOfChannelAccessToken = ownerOfChanelAccessAndRefresh.Item1;
                        SetNameAndIdByOauthedUser(CachedOwnerOfChannelAccessToken).Wait();
                        InitializeOwnerOfChanenelConnection(TwitchChannelName, CachedOwnerOfChannelAccessToken);
                        InitializeTwitchAPI(CachedOwnerOfChannelAccessToken);
                    }
                }
            };
            webServer.Start();
            Console.WriteLine($"Webserver started on : {webServer.EndPoint}");
        }

        /// <summary>
        /// S'identifier auprès de twitch comme bot.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<Tuple<string, string>> GetAccessAndRefreshTokens(string code)
        {
            HttpClient client = new HttpClient();
            Dictionary<string, string> values = new Dictionary<string, string> {
                {"client_id",     ClientId },
                {"client_secret", ClientSecret },
                {"code",          code},
                {"grant_type",    "authorization_code" },
                {"redirect_uri",  RedirectURL + ":3000"}
            };

            FormUrlEncodedContent   content        = new FormUrlEncodedContent(values);
            HttpResponseMessage     response       = await client.PostAsync("https://id.twitch.tv/oauth2/token", content);
            string                  responseString = await response.Content.ReadAsStringAsync();
            JObject                 json           = JObject.Parse(responseString);
            Log($"TOKEN access and refresh : {json["access_token"]} - {json["refresh_token"]}");
            return new Tuple<string, string>(json["access_token"].ToString(), json["refresh_token"].ToString());
        }

        /// <summary>
        /// Enregistrement des ID, Login et Token de notre bot.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task SetNameAndIdByOauthedUser(string accessToken)
        {
            TwitchAPI api = new TwitchAPI();
            api.Settings.ClientId = ClientId;
            api.Settings.AccessToken = accessToken;

            GetUsersResponse oauthedUser = await api.Helix.Users.GetUsersAsync();
            TwitchChannelId   = oauthedUser.Users[0].Id;
            TwitchChannelName = oauthedUser.Users[0].Login;
        }

        /// <summary>
        /// Connecter le compte BOT aux différentes actions.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="accessToken"></param>
        public void InitializeOwnerOfChanenelConnection(string username, string accessToken)
        {
            OwnerOfChannelConnection = new TwitchClient();
            OwnerOfChannelConnection.Initialize(
                new ConnectionCredentials(username, accessToken), 
                TwitchChannelName
            );

            OwnerOfChannelConnection.OnConnected            += Client_OnConnected;
            OwnerOfChannelConnection.OnDisconnected         += OwnerOfChannelConnection_OnDisconnected;
            OwnerOfChannelConnection.OnChatCommandReceived  += Bot_OnChatCommandReceived;
            OwnerOfChannelConnection.OnUserJoined           += BotConnection_OnUserJoined;
            OwnerOfChannelConnection.OnMessageReceived      += Client_OnMessageReceived;

            OwnerOfChannelConnection.Connect();
        }

        /// <summary>
        /// Initialise l'API pour faire des demandes à Twitch.
        /// </summary>
        /// <param name="accessToken"></param>
        public void InitializeTwitchAPI(string accessToken)
        {
            TheTwitchAPI = new TwitchAPI();
            TheTwitchAPI.Settings.ClientId    = ClientId;
            TheTwitchAPI.Settings.AccessToken = accessToken;
        }

        /// <summary>
        /// ouvre la page web de confirmation dans le navigateur.
        /// </summary>
        public void Start()
        {
            var authURL = $"https://id.twitch.tv/oauth2/authorize?client_id={ClientId}";
            authURL += $"&redirect_uri={RedirectURL}:3000";
            authURL += "&response_type=code";
            authURL += $"&scope={String.Join("+", Scopes)}";
            Console.WriteLine( authURL );
            System.Diagnostics.Process.Start(authURL);
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Log($"User {e.BotUsername} connected (bot access) !");
            _form.Invoke(new MethodInvoker(delegate ()
            {
                _form.menuStrip.connectAccountMenu.Text = e.BotUsername;
                return;
            }));
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            Log(e.ChatMessage.Username + " : " + e.ChatMessage.Message);
            _form.Invoke(new MethodInvoker(delegate ()
            {
                _form.AddMessageChat(e.ChatMessage.Username,e.ChatMessage.Message);
                return;
            }));
        }

        private void OwnerOfChannelConnection_OnDisconnected(object sender, TwitchLib.Communication.Events.OnDisconnectedEventArgs e)
        {
            Log($"On disconnected Event !");
        }

        private void Bot_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            string commandText = e.Command.CommandText.ToLower();
            string userCommand = e.Command.ChatMessage.DisplayName;

            if (commandText.Equals("pc", StringComparison.OrdinalIgnoreCase))
            {
                OwnerOfChannelConnection.SendMessage(TwitchChannelName, $"Vous avez {_form.score.GetScore(userCommand)} points !");

            }else if (commandText.Equals("top", StringComparison.OrdinalIgnoreCase))
            {
                string content = "";
                int nb = 0;
                foreach (var item in _form.score.GetTopScores())
                {
                    if (nb < 5)
                        content += item + " !\n";
                    nb++;
                }
                OwnerOfChannelConnection.SendMessage(TwitchChannelName, $"Le TOP 5 : \n{content}");
            }else if (CommandStaticResponses.ContainsKey(commandText))
            {
                OwnerOfChannelConnection.SendMessage(TwitchChannelName, CommandStaticResponses[commandText]);
            }
        }

        public void First(string username)
        {
            OwnerOfChannelConnection.SendMessage(TwitchChannelName, $"Congratulation @{username} ! FIRST !!");
        }

        private void BotConnection_OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            _form.Invoke(new MethodInvoker(delegate ()
            {
                _form.viewversList.Items.Add(e.Username);
                return;
            }));
        }

        public void GetNumberSub()
        {
            GetBroadcasterSubscriptionsResponse subscribers =
                TheTwitchAPI.Helix.Subscriptions.GetBroadcasterSubscriptionsAsync(TwitchChannelId,
                                                                                  first: 100,
                                                                                  accessToken: CachedOwnerOfChannelAccessToken).Result;
            Log($"vous avez {subscribers.Data.Length} subscribers");
        }

        private void Log(string message)
        {
            Console.WriteLine("WEBSERVICE : "+message);
        }

        /// <summary>
        /// Libère le service.
        /// </summary>
        public void Closed()
        {
            if (OwnerOfChannelConnection != null)
            {
                OwnerOfChannelConnection.Disconnect();
            }
            if (webServer != null)
            {
                webServer.Stop();
                webServer.Dispose();
            }
        }

        public string GetJsonTokenSaver()
        {
            var dataJson = new
            {
                AccessToken = CachedOwnerOfChannelAccessToken,
                ID          = TwitchChannelId,
                Name        = TwitchChannelName,
            };
            return JsonSerializer.Serialize(dataJson);
        }

    }
}
