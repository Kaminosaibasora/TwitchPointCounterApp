using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TwitchPointCounterApp.objects;

namespace TwitchPointCounterApp.Services
{
    public class Saver
    {
        private const string FILENAME = "saver.SCORE";
        private static string _applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string _applicationPath = Path.Combine(_applicationDataPath, "TwitchPointCounterAPP");

        public static string FileName { get; } = Path.Combine(_applicationPath, FILENAME);

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

    }
}
