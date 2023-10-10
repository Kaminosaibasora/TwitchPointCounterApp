using System.Collections.Generic;
using System.IO;
using TwitchPointCounterApp.objects;

namespace TwitchPointCounterApp.Services
{
    /// <summary>
    /// Classe de gestion de l'overlay.
    /// Overlay management class.
    /// </summary>
    public class Overlay
    {
        public string htmlOverlayName           = "overlayTwitchPointCounterBase.html";
        public string htmlOverlayDefinitiveName = "overlayTwitchPointCounter.html";
        public string overlayPath               = "../../overlay/";
        public string fullPathBaseOverlay;
        public string fullPathOverlay;

        public Overlay()
        {
            fullPathBaseOverlay = Path.Combine(overlayPath, htmlOverlayName);
            fullPathOverlay = Path.Combine(overlayPath, htmlOverlayDefinitiveName);
        }

        /// <summary>
        /// Mise à jour de l'overlay selon les scores.
        /// </summary>
        /// <param name="score">instance de score</param>
        public void MajOverlay(Score score)
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
            using (StreamReader sr = new StreamReader(fullPathBaseOverlay))
            {
                dataHtml = sr.ReadToEnd();
            }
            // WRITE
            int idex = dataHtml.IndexOf("||");
            dataHtml = dataHtml.Substring(0, idex) + scoresHTML + dataHtml.Substring(idex + 2);
            using (StreamWriter sw = new StreamWriter(fullPathOverlay))
            {
                sw.Write(dataHtml);
            }
        }

    }
}
