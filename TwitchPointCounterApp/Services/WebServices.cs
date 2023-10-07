using NHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TwitchPointCounterApp.Services
{
    public class WebServices
    {
        private readonly string clientId     = Properties.Settings.Default.clientid;
        private readonly string clientSecret = Properties.Settings.Default.clientsecret;

        private readonly string redirectURL = "http://localhost";

        private readonly List<string> scopes = new List<string> {
            "user:edit", "chat:read", "chat:edit", "channel:moderate", "whispers:read", "whispers:edit", "bits:read", "channel:read:subscriptions", "user:read:email", "user:read:subscriptions"
        };

        private HttpServer   webServer;
        private TwitchClient twitchClient;
        private TwitchAPI    twitchAPI;

        private string cachedOwnerOfChannelAccessToken = "needsaccesstoken";
        private string twitchChannelId;
        private string twitchChannelName = "kami_sama_de_l_eternite";
        private string botUsername       = "BOT_COUNT_POINTER";
        private string botOAuthToken     = "BOT_TOKEN";

        public Dictionary<string, string> commandStaticResponses = new Dictionary<string, string>
        {
            {"about", "command about" },
            {"donate", "give me your money" }
        };

        public WebServices()
        {
            InitializeWebServer();
            webServer.RequestReceived += async (s, e) =>
            {
                using (var writer = new StreamWriter(e.Response.OutputStream))
                {
                    Console.WriteLine("RESPONSE : "+e.Response.OutputStream);
                    Console.WriteLine("DATA : " + e.Request.QueryString.AllKeys.ToString());
                    if (e.Request.QueryString.AllKeys.Any("code".Contains))
                    {
                        var code = e.Request.QueryString["code"];
                        MessageBox.Show(code);
                        //var ownerOfChanelAccessAndRefresh = await getAccessAndRefreshTokens(code);
                        //CachedOwnerOfChannelAccessToken = ownerOfChanelAccessAndRefresh.Item1;
                        //SetNameAndIdByOauthedUser(CachedOwnerOfChannelAccessToken);//.Wait()
                        //InitializeOwnerOfChanenelConnection(TwitchChannelName, CachedOwnerOfChannelAccessToken);
                        //InitializeTwitchAPI(CachedOwnerOfChannelAccessToken);
                    }
                }
            };
            webServer.Start();
            StartCallTwitchAPI();
        }

        /// <summary>
        /// Récupère un token en appelant l'API Twitch
        /// </summary>
        public void StartCallTwitchAPI()
        {
            var authURL = $"https://id.twitch.tv/oauth2/authorize?client_id={clientId}";
            authURL += $"&redirect_uri={redirectURL}";
            authURL += "&response_type=token";
            authURL += $"&scope={String.Join("+", scopes)}";
            Console.WriteLine(authURL);

            System.Diagnostics.Process.Start(authURL);
        }

        /// <summary>
        /// Initialisation du serveur WEB
        /// </summary>
        public void InitializeWebServer()
        {
            webServer = new HttpServer();
            webServer.EndPoint = new IPEndPoint(IPAddress.Loopback, 80);
        }

        /// <summary>
        /// Initialisation Client Twitch
        /// </summary>
        /// <param name="username"></param>
        /// <param name="accessToken"></param>
        public void InitializeOwnerOfChanenelConnection(string username, string accessToken)
        {
            twitchClient = new TwitchClient();
            twitchClient.Initialize(new ConnectionCredentials(username, accessToken), twitchChannelName);

            //twitchClient.OnConnected += Client_OnConnected;
            //twitchClient.OnDisconnected += OwnerOfChannelConnection_OnDisconnected;
            //twitchClient.OnChatCommandReceived += Bot_OnChatCommandReceived;
            // OwnerOfChannelConnection.OnUserJoined += BotConnection_OnUserJoined;
            // OwnerOfChannelConnection.OnMessageReceived += Client_OnMessageReceived;

            twitchClient.Connect();
        }
    }
}
