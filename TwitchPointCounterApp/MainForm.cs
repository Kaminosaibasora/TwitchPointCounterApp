using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TwitchPointCounterApp.Controls;
using TwitchPointCounterApp.objects;
using TwitchPointCounterApp.Services;

namespace TwitchPointCounterApp
{
    public partial class MainForm : Form
    {
        public MainMenuStrip menuStrip;
        public GroupBox chatBox;
        public GroupBox viewversBox;
        public GroupBox topBox;
        public MainListBox viewversList;
        public Label currentViewverLabel;
        public Score score;
        public WebServicesAll twichService;
        public Saver saver;
        public Overlay overlay;
        public bool first = false;
        public MainForm()
        {
            // INITIALISATION SERVICE
            saver   = new Saver();
            overlay = new Overlay();
            twichService = new WebServicesAll(this, new Dictionary<string, string>
            {
                {"PCcommand", "Commandes disponibles : pc; top" }
            }, saver.LoadToken());
            twichService.InitializeWebServer();

            // INITIALISATION COMPOSANT
            InitializeComponent();

            Text = "Twitch Point Counter";

            menuStrip            = mainMenuStrip1;
            viewversBox          = mainGroupBox1;
            topBox               = mainGroupBox2;
            chatBox              = mainGroupBox3;
            viewversList         = mainListBox1;
            currentViewverLabel  = label1;
            Button addOneButton  = button1;
            Button lessOneButton = button2;

            currentViewverLabel.Text = "Viewver choisi";
            addOneButton.Text        = "+ 1";
            lessOneButton.Text       = "- 1";

            Controls.AddRange(new Control[] { menuStrip, viewversBox, currentViewverLabel, addOneButton, lessOneButton, topBox, chatBox });

            // Chargement SCORE
            score = saver.Load();
            MajTopView();
            overlay.MajOverlay(score);
        }

        /// <summary>
        /// Mise à jour de l'apperçu des scores.
        /// </summary>
        public void MajTopView()
        {
            topList.Items.Clear();
            List<string> topListData = score.GetTopScores();
            foreach (var item in topListData)
            {
                int index = item.IndexOf(" - ");
                ListViewItem lvi = new ListViewItem(item.Substring(0, index));
                lvi.SubItems.Add(item.Substring(index + 3));
                topList.Items.Add(lvi);
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// Ajoute 1 point à un viewer.
        /// Adds 1 point to a viewer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddOneButton_Click(object sender, EventArgs e)
        {
            score.AddOne(currentViewverLabel.Text);
            MajTopView();
            overlay.MajOverlay(score);
        }

        /// <summary>
        /// Retire 1 point à un viewer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LessOneButton_Click(object sender, EventArgs e)
        {
            score.LessOne(currentViewverLabel.Text);
            MajTopView();
            overlay.MajOverlay(score);
        }

        /// <summary>
        /// Sauvegarde les données et ferme les services lors de la fermeture du logiciel.
        /// Backs up data and closes services when closing the software.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            saver.Save(score);
            saver.SaveToken(twichService.GetJsonTokenSaver());
            twichService.Closed();
        }

        /// <summary>
        /// Ajoute un message dans le chat et gère le FIRST.
        /// Add a message to the chat and manage FIRST.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="message"></param>
        public void AddMessageChat(string username, string message)
        {
            chatListBox.Items.Add($"{username} : {message}");
            // GESTION DU FIRST
            if (!first)
            {
                switch (MessageBox.Show($"{username} est-t-il le premier à écrire dans le chat ?", "First", MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                        score.AddOne(username);
                        MajTopView();
                        overlay.MajOverlay(score);
                        twichService.First(username);
                        first = !first;
                        break;
                    case DialogResult.No:
                        break;
                }
            }
        }
    }
}
