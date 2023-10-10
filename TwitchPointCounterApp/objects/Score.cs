using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitchPointCounterApp.objects
{
    /// <summary>
    /// Classe représentant les scores des viewers. Les scores sont enregistrés par pseudo dans un dictionnaire.
    /// Class representing viewer scores. Scores are recorded by nickname in a dictionary.
    /// </summary>
    public class Score
    {
        public Dictionary<string, int> score;

        /// <summary>
        /// Initialisation du dictionnaire de score à vide.
        /// Initialization of the empty score dictionary.
        /// </summary>
        public Score()
        {
            score = new Dictionary<string, int>();
        }

        /// <summary>
        /// Chargement de données dans le dictionnaire de score.
        /// Loading data into the score dictionary.
        /// </summary>
        /// <param name="score">Dictionnaire de score</param>
        public Score(Dictionary<string, int> score)
        {
            this.score = score;
        }

        /// <summary>
        /// Ajoute 1 point à un viewer.
        /// </summary>
        /// <param name="viewverName">Pseudo du viewer</param>
        public void AddOne(string viewverName)
        {
            if (!score.ContainsKey(viewverName))
            {
                InitViewver(viewverName);
            }
            score[viewverName] += 1;
        }

        /// <summary>
        /// Retire 1 point à un viewer.
        /// </summary>
        /// <param name="viewverName">Pseudo du viewer</param>
        public void LessOne(string viewverName)
        {
            if (!score.ContainsKey(viewverName))
            {
                InitViewver(viewverName);
            }
            score[viewverName] -= 1;
        }

        /// <summary>
        /// Initialise le viewer dans le dictionnaire des scores.
        /// </summary>
        /// <param name="viewverName">Pseudo du viewer</param>
        public void InitViewver(string viewverName)
        {
            score[viewverName] = 0;
        }

        /// <summary>
        /// Fournit une liste des scores par ordre de point sous la forme "pseudo - point".
        /// Provides a list of scores in point order in "pseudo-point" form.
        /// </summary>
        /// <returns>Liste de string : "pseudo - points"</returns>
        public List<string> GetTopScores()
        {
            List<string> top = new List<string>();
            foreach (var viewv in score.OrderByDescending(x => x.Value))
            {
                top.Add(viewv.Key + " - " + viewv.Value);
            }
            return top;
        }

        /// <summary>
        /// Transforme le dictionnaire de score en string pour la sauvegarde des données.
        /// Transforms the score dictionary into a string for saving data.
        /// </summary>
        /// <returns>String sous la forme "pseudo||points\n"</returns>
        public string GetSaverScore()
        {
            string contentChained = "";
            foreach (var item in score)
            {
                contentChained += $"{item.Key}||{item.Value}\n";
            }
            return contentChained;
        }

        /// <summary>
        /// Donne le score d'un viewer.
        /// Gives a viewer's score.
        /// </summary>
        /// <param name="viewverName">Pseudo du viewer</param>
        /// <returns>score du viewer, 0 s'il n'est pas connu</returns>
        public int GetScore(string viewverName)
        {
            if (score.ContainsKey(viewverName))
            {
                Console.WriteLine(viewverName + ": " + score[viewverName]);
                return score[viewverName];
            }
            return 0;
        }
    }
}
