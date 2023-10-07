using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchPointCounterApp.Services;

namespace TwitchPointCounterApp.Controls
{
    public class MainMenuStrip : MenuStrip
    {
        private const string MENU_NAME = "MainMenuStrip";
        private MainForm _form;
        public ToolStripMenuItem connectAccountMenu;

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

        public void TwitchMenu()
        {
            ToolStripMenuItem twitchMenu         = new ToolStripMenuItem("Twitch");
            connectAccountMenu = new ToolStripMenuItem("Connect");
            //ToolStripMenuItem testConnectionMenu = new ToolStripMenuItem("Test connection API Twitch");

            twitchMenu.DropDownItems.AddRange(new[] { connectAccountMenu });
            Items.Add(twitchMenu);

            connectAccountMenu.Click += (s, e) => {
                if (connectAccountMenu.Text == "Connect")
                {
                    _form.twichService.Start();
                } else
                {
                    MessageBox.Show("Disconnected");
                    // TODO : disconnection
                }
            };

            //testConnectionMenu.Click += (s, e) => {
            //    MessageBox.Show("test connection");
            //};
        }

        public void DataMenu()
        {
            ToolStripMenuItem dataMenu   = new ToolStripMenuItem("Data");
            ToolStripMenuItem importMenu = new ToolStripMenuItem("Import");
            ToolStripMenuItem exportMenu = new ToolStripMenuItem("Export");
            ToolStripMenuItem resetMenu  = new ToolStripMenuItem("Reset");

            dataMenu.DropDownItems.AddRange(new[] { importMenu, exportMenu, resetMenu});
            Items.Add(dataMenu);

            importMenu.Click += (s, e) => { };
            exportMenu.Click += (s, e) => { };
            resetMenu.Click += (s, e) => { };
        }

        public void OptionMenu()
        {
            ToolStripMenuItem optionMenu        = new ToolStripMenuItem("Option");
            ToolStripMenuItem commandeChatMenu  = new ToolStripMenuItem("Commande Chat");
            ToolStripMenuItem nbMenu            = new ToolStripMenuItem("!nb");
            ToolStripMenuItem configAdminMenu   = new ToolStripMenuItem("Config Admin");
            ToolStripMenuItem affichageChatMenu = new ToolStripMenuItem("Affichage Chat");
            ToolStripMenuItem linkOverlayMenu   = new ToolStripMenuItem("lien vers l'overlay");

            affichageChatMenu.Checked = true;

            optionMenu.DropDownItems.AddRange(new[] { commandeChatMenu, affichageChatMenu, linkOverlayMenu });
            commandeChatMenu.DropDownItems.AddRange(new[] { nbMenu, configAdminMenu });
            Items.Add(optionMenu);

            nbMenu.Click += (s, e) => {
                if (nbMenu.Checked)
                {
                    nbMenu.Checked = false;
                }
                else
                {
                    nbMenu.Checked = true;
                }
            };

            configAdminMenu.Click += (s, e) => { };

            affichageChatMenu.Click += (s, e) => {
                if (affichageChatMenu.Checked)
                {
                    affichageChatMenu.Checked = false;
                    _form.chatBox.Visible = false;
                }
                else
                {
                    affichageChatMenu.Checked = true;
                    _form.chatBox.Visible = true;
                }
            };

            linkOverlayMenu.Click += (s, e) => {
                string path = Path.Combine(Environment.CurrentDirectory, _form.overlay.fullPathOverlay.Replace("/", "\\"));
                Clipboard.SetText(path);
                MessageBox.Show($"Lien à copier dans OBS : {path}\n (Copié automatiquement dans le press papier)");
            };
        }

    }
}
