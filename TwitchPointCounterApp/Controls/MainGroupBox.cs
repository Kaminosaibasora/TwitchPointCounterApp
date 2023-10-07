using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchPointCounterApp.Controls
{
    public class MainGroupBox : GroupBox
    {
        public MainGroupBox()
        {
            AutoSize = false;
            Width = 50;
            Height = 50;
        }

        public MainGroupBox(string name)
        {
            Text = name;
            AutoSize = false;
            Width = 100;
        }

        // fonction de disposition
        // Fonction de taille
        // fonction de contenu
    }
}
