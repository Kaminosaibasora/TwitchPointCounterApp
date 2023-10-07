using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchPointCounterApp.objects
{
    public class Score
    {
        public Dictionary<string, int> score;
        public Score() 
        { 
            score = new Dictionary<string, int>();
        }

        public Score(Dictionary<string, int> score)
        {
            this.score = score;
        }

        public void AddOne(string viewverName)
        {
            if (!score.ContainsKey(viewverName))
            {
                InitViewver(viewverName);
            }
            score[viewverName] += 1;
        }
        
        public void LessOne(string viewverName)
        {
            if (!score.ContainsKey(viewverName))
            {
                InitViewver(viewverName);
            }
            score[viewverName] -= 1;
        }

        public void InitViewver(string viewverName)
        {
            score[viewverName] = 0;
        }

        public List<string> GetTopScores() { 
            List<string> top = new List<string>();

            foreach(var viewv in score.OrderByDescending(x => x.Value))
            {
                top.Add(viewv.Key + " - " + viewv.Value);
            }

            return top;
        }

        public string GetSaverScore()
        {
            string contentChained = "";
            foreach (var item in score)
            {
                contentChained += $"{item.Key}||{item.Value}\n";
            }
            return contentChained;
        }

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
