using System.Windows.Forms;

namespace TwitchPointCounterApp.Controls
{
    public class MainGroupBox : GroupBox
    {
        public MainGroupBox()
        {
            AutoSize = false;
            Width    = 50;
            Height   = 50;
        }

        public MainGroupBox(string name)
        {
            Text     = name;
            AutoSize = false;
            Width    = 100;
        }
    }
}
