using System;
using System.IO;
using System.Windows.Forms;

namespace TwitchPointCounterApp.Controls
{
    public class MainMenuStrip : MenuStrip
    {
        private const string MENU_NAME = "MainMenuStrip";
        private MainForm _form;
        public ToolStripMenuItem connectAccountMenu;
        public bool cpMenuCheck = true;

        public MainMenuStrip()
        {
            Name = MENU_NAME;
            Dock = DockStyle.Top;

            // ================================================

            TwitchMenu();
            DataMenu();
            OptionMenu();

            // ================================================

            HandleCreated += (s, e) =>
            {
                _form = FindForm() as MainForm;
            };
        }

        /// <summary>
        /// Met en place le menu "Twitch" et son bouton de connexion / déconexion.
        /// Sets up the “Twitch” menu and its connection/disconnection button.
        /// </summary>
        public void TwitchMenu()
        {
            ToolStripMenuItem twitchMenu = new ToolStripMenuItem("Twitch");
            connectAccountMenu = new ToolStripMenuItem("Connect");

            twitchMenu.DropDownItems.AddRange(new[] { connectAccountMenu });
            Items.Add(twitchMenu);

            connectAccountMenu.Click += (s, e) =>
            {
                if (connectAccountMenu.Text == "Connect")
                {
                    /// Lance l'appel de l'API Twitch
                    _form.twichService.Start();
                }
                else
                {
                    _form.saver.Save(_form.score);
                    _form.saver.DestructionToken();
                    connectAccountMenu.Text = "Connect";
                    _form.twichService.DestructionToken();
                    _form.viewversList.Items.Clear();
                    MessageBox.Show("Disconnected");
                }
            };
        }

        /// <summary>
        /// Met en place le menu "Data" qui permet de gérer les données SCORE.
        /// Sets up the "Data" menu which allows you to manage SCORE data.
        /// </summary>
        public void DataMenu()
        {
            ToolStripMenuItem dataMenu   = new ToolStripMenuItem("Data");
            ToolStripMenuItem importMenu = new ToolStripMenuItem("Import");
            ToolStripMenuItem exportMenu = new ToolStripMenuItem("Export");
            ToolStripMenuItem resetMenu  = new ToolStripMenuItem("Reset");

            dataMenu.DropDownItems.AddRange(new[] { importMenu, exportMenu, resetMenu });
            Items.Add(dataMenu);

            importMenu.Click += (s, e) =>
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "Chargement d'un nouveau fichier score";
                dialog.Filter = "score file (*.SCORE)|*.SCORE";
                dialog.FilterIndex = 1;
                dialog.ShowDialog();
                _form.saver.ImportScore(dialog.FileName);
                MajForm();
            };

            exportMenu.Click += (s, e) =>
            {
                _form.saver.Save(_form.score);
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "score file (*.SCORE)|*.SCORE";
                    saveFileDialog.Title = "Enregistrer sous un fichier SCORE";
                    saveFileDialog.DefaultExt = "SCORE";
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.FileName = "messcores.SCORE";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        _form.saver.ExportScore(saveFileDialog.FileName);
                    }
                }
            };

            resetMenu.Click += (s, e) =>
            {
                DialogResult reset = MessageBox.Show("Voulez-vous remettre les scores à zéro ?", "Reset Score", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (reset == DialogResult.Yes)
                {
                    _form.saver.ResetScore();
                    MajForm();
                }
            };
        }

        /// <summary>
        /// Recharge les SCORE depuis le fichier et met à jour l'interface.
        /// Reloads the SCOREs from the file and updates the interface.
        /// </summary>
        public void MajForm()
        {
            _form.score = _form.saver.Load();
            _form.MajTopView();
            _form.overlay.MajOverlay(_form.score);
        }

        /// <summary>
        /// Met en place le menu "option" qui permet de configurer les différents éléments du logiciel.
        /// Sets up the “option” menu which allows you to configure the different elements of the software.
        /// </summary>
        public void OptionMenu()
        {
            ToolStripMenuItem optionMenu        = new ToolStripMenuItem("Option");
            ToolStripMenuItem commandeChatMenu  = new ToolStripMenuItem("Commande Chat");
            ToolStripMenuItem nbMenu            = new ToolStripMenuItem("!pc");
            ToolStripMenuItem affichageChatMenu = new ToolStripMenuItem("Affichage Chat");
            ToolStripMenuItem linkOverlayMenu   = new ToolStripMenuItem("lien vers l'overlay");

            affichageChatMenu.Checked = true;
            nbMenu.Checked            = true;
            cpMenuCheck               = true;

            optionMenu.DropDownItems.AddRange(new[] { commandeChatMenu, affichageChatMenu, linkOverlayMenu });
            commandeChatMenu.DropDownItems.AddRange(new[] { nbMenu });
            Items.Add(optionMenu);

            nbMenu.Click += (s, e) =>
            {
                if (nbMenu.Checked)
                {
                    cpMenuCheck    = false;
                    nbMenu.Checked = false;
                }
                else
                {
                    cpMenuCheck    = true;
                    nbMenu.Checked = true;
                }
            };

            affichageChatMenu.Click += (s, e) =>
            {
                if (affichageChatMenu.Checked)
                {
                    affichageChatMenu.Checked = false;
                    _form.chatBox.Visible     = false;
                }
                else
                {
                    affichageChatMenu.Checked = true;
                    _form.chatBox.Visible     = true;
                }
            };

            linkOverlayMenu.Click += (s, e) =>
            {
                string path = Path.Combine(Environment.CurrentDirectory, _form.overlay.fullPathOverlay.Replace("/", "\\"));
                Clipboard.SetText(path);
                MessageBox.Show($"Lien à copier dans OBS : {path}\n (Copié automatiquement dans le press papier)");
            };
        }

    }
}
