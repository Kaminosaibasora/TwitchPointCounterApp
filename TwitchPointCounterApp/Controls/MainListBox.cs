using System;
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

        /// <summary>
        /// Changement de la valeur sélectionnée.
        /// Changing the selected value.
        /// </summary>
        /// <param name="e"></param>
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
