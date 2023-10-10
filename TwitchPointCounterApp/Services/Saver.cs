using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using TwitchPointCounterApp.objects;

namespace TwitchPointCounterApp.Services
{
    /// <summary>
    /// Classe de gestion de la sauvegarde et continuité des données.
    /// Backup management class and data continuity.
    /// </summary>
    public class Saver
    {
        private const string FILENAME   = "saver.SCORE";
        private const string CONFIGNAME = "tpca_config.json";

        private static string _applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string _applicationPath     = Path.Combine(_applicationDataPath, "TwitchPointCounterAPP");

        public static string FileName { get; }       = Path.Combine(_applicationPath, FILENAME);
        public static string ConfigFileName { get; } = Path.Combine(_applicationPath, CONFIGNAME);

        /// <summary>
        /// Créer le dossier dans AppData si celui-ci n'existe pas.
        /// Creates the folder in AppData if it does not exist.
        /// </summary>
        public Saver()
        {
            if (!Directory.Exists(_applicationPath))
            {
                Directory.CreateDirectory(_applicationPath);
            }
        }

        /// <summary>
        /// Ecriture des scores dans le fichier de sauvegarde saver.SCORE.
        /// Writing scores to saver.SCORE file.
        /// </summary>
        /// <param name="score">instance de score</param>
        public void Save(Score score)
        {
            using (StreamWriter sw = File.CreateText(FileName))
            {
                sw.WriteAsync(score.GetSaverScore());
            }
        }

        /// <summary>
        /// Chargement du fichier saver.SCORE et interprétation des données.
        /// Loading the saver.SCORE file and interpreting the data.
        /// </summary>
        /// <returns>Nouvelle instance de score</returns>
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
                                data[line.Substring(0, index)] = int.Parse(line.Substring(index + 2));
                            }
                            catch (Exception e) { MessageBox.Show("ERROR : " + e); }
                        }
                    }
                    return new Score(data);
                }
            }
            return new Score();
        }

        /// <summary>
        /// Importation d'un nouveau fichier de score.
        /// Importing a new score file.
        /// </summary>
        /// <param name="path">chemin vers le nouveau fichier SCORE</param>
        public async void ImportScore(string path)
        {
            string data = String.Empty;
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    data = sr.ReadToEnd();
                }
            }
            using (StreamWriter sw = File.CreateText(FileName))
            {
                if (data != String.Empty)
                {
                    await sw.WriteAsync(data);
                    MessageBox.Show("Importation Score : SUCCESS");
                }
            }
        }

        /// <summary>
        /// Ecriture d'une copie du fichier SCORE.
        /// Writing a copy of the SCORE file.
        /// </summary>
        /// <param name="path"></param>
        public async void ExportScore(string path)
        {
            string data = String.Empty;
            using (StreamReader streamReader = File.OpenText(FileName))
            {
                data = streamReader.ReadToEnd();
            }
            using (StreamWriter sw = File.CreateText(path))
            {
                await sw.WriteAsync(data);
                MessageBox.Show("Export Score : SUCCESS");
            }
        }

        /// <summary>
        /// Remise à zéro du fichier SCORE.
        /// Resetting the SCORE file.
        /// </summary>
        public void ResetScore()
        {
            using (StreamWriter sw = File.CreateText(FileName))
            {
                sw.Write("");
                MessageBox.Show("Reset Score : SUCCESS");
            }
        }

        /// <summary>
        /// Sauvegarde des token de connexion à Twitch.
        /// Saving Twitch connection tokens.
        /// </summary>
        /// <param name="jsontoken"></param>
        public void SaveToken(string jsontoken)
        {
            using (StreamWriter sw = File.CreateText(ConfigFileName))
            {
                sw.WriteAsync(jsontoken);
            }
            Console.WriteLine("Configuration sauvegardée avec succès");
        }

        /// <summary>
        /// Chargement des token de connexion enregistrés.
        /// Loading saved connection tokens.
        /// </summary>
        /// <returns>Dictionnaire de token : AccessToken, ID, Name</returns>
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

                if (racine.TryGetProperty("AccessToken", out JsonElement accTokElement) &&
                    racine.TryGetProperty("ID", out JsonElement IdElement) &&
                    racine.TryGetProperty("Name", out JsonElement NameElement))
                {
                    return new Dictionary<string, string> {
                        { "AccessToken" , accTokElement.GetString()},
                        { "ID"          , IdElement.GetString()},
                        { "Name"        , NameElement.GetString()},
                    };
                }
            }
            return new Dictionary<string, string> { };
        }

        /// <summary>
        /// Supprime le fichier de sauvegarde de token.
        /// </summary>
        public void DestructionToken()
        {
            if (File.Exists(ConfigFileName))
            {
                File.Delete(ConfigFileName);
            }
        }

    }
}
