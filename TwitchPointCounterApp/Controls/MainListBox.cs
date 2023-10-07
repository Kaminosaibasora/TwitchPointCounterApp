using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchPointCounterApp.Controls
{
    public class MainListBox : ListBox
    {
        private MainForm _form;
        public MainListBox()
        {
            HandleCreated += (s, e) =>
            {
                _form = FindForm() as MainForm;
            };
        }

        // fonction de mise à jour dynamique

        protected override void OnSelectedValueChanged(EventArgs e)
        {
            base.OnSelectedValueChanged(e);
            if (this.SelectedItem != null)
            {
                _form.currentViewverLabel.Text = this.SelectedItem.ToString();
            }
        }
    }
}
