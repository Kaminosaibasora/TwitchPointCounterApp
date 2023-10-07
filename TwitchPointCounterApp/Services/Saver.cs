using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using TwitchPointCounterApp.objects;

namespace TwitchPointCounterApp.Services
{
    public class Saver
    {
        private const string FILENAME = "saver.SCORE";
        private const string CONFIGNAME = "tpca_config.json";
        private static string _applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string _applicationPath = Path.Combine(_applicationDataPath, "TwitchPointCounterAPP");

        public static string FileName { get; } = Path.Combine(_applicationPath, FILENAME);
        public static string ConfigFileName { get; } = Path.Combine(_applicationPath, CONFIGNAME);

        public Saver()
        {
            if (!Directory.Exists(_applicationPath))
            {
                Directory.CreateDirectory(_applicationPath);
            }
        }

        public void Save(Score score) 
        {
            using (StreamWriter sw = File.CreateText(FileName))
            {
                sw.WriteAsync(score.GetSaverScore());
            }
        }

        public Score Load()
        {
            if (File.Exists(FileName))
            {
                using (StreamReader sr = File.OpenText(FileName)) 
                { 
                    string content = sr.ReadToEnd();
                    Dictionary<string, int> data = new Dictionary<string, int>();
                    foreach (var line in content.Split('\n'))
                    {
                        if (line.Length > 1)
                        {
                            var index = line.IndexOf("||");
                            try
                            {
                                data[line.Substring(0, index)] = int.Parse(line.Substring(index+2));
                            }
                            catch (Exception e) { MessageBox.Show("ERROR : " + e); }
                        }
                    }
                    return new Score(data);
                }
            }
            return new Score();
        }

        public void SaveToken(string jsontoken)
        {
            using (StreamWriter sw = File.CreateText(ConfigFileName))
            {
                sw.WriteAsync(jsontoken);
            }
            Console.WriteLine("Configuration sauvegardée avec succès");
        }

        public Dictionary<string, string> LoadToken()
        {
            string json = "";
            using (StreamReader sr = File.OpenText(ConfigFileName))
            {
                json = sr.ReadToEnd();
            }
            using (JsonDocument jdoc = JsonDocument.Parse(json))
            {
                JsonElement racine = jdoc.RootElement;

                if (racine.TryGetProperty("AccessToken",out JsonElement accTokElement) &&
                    racine.TryGetProperty("ID",         out JsonElement IdElement) &&
                    racine.TryGetProperty("Name",       out JsonElement NameElement))
                {
                    return new Dictionary<string, string> {
                        { "AccessToken" , accTokElement.GetString()},
                        { "ID"          , IdElement.GetString()},
                        { "Name"        , NameElement.GetString()},
                    };
                }
            }
            return new Dictionary<string, string> {};
        }

    }
}
