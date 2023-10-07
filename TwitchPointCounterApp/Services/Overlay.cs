using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchPointCounterApp.objects;

namespace TwitchPointCounterApp.Services
{
    public class Overlay
    {
        public string htmlOverlayName = "overlayTwitchPointCounterBase.html";
        public string htmlOverlayDefinitiveName = "overlayTwitchPointCounter.html";
        public string cssOverlayName = "overlayTwitchPointCounter.css";
        public string overlayPath = "../../overlay/";
        public string fullPathBaseOverlay;
        public string fullPathOverlay;

        public Overlay() 
        { 
            fullPathBaseOverlay = Path.Combine(overlayPath, htmlOverlayName);
            fullPathOverlay = Path.Combine(overlayPath, htmlOverlayDefinitiveName);
        }

        public void majOverlay(Score score)
        {
            // PREPARE
            string scoresHTML = "";
            List<string> scores = score.GetTopScores();
            foreach (string item in scores)
            {
                int index = item.IndexOf(" - ");
                scoresHTML += $"<li>{item.Substring(0, index)}<strong>{item.Substring(index + 3)}</strong></li>";
            }
            // READ
            string dataHtml;
            using(StreamReader sr = new StreamReader(fullPathBaseOverlay))
            {
                dataHtml = sr.ReadToEnd();
            }
            // WRITE
            int idex = dataHtml.IndexOf("||");
            dataHtml = dataHtml.Substring(0, idex) + scoresHTML + dataHtml.Substring(idex+2);
            using (StreamWriter sw = new StreamWriter(fullPathOverlay))
            {
                sw.Write(dataHtml);
            }
        }

    }
}
