using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchPointCounterApp.Controls;
using TwitchPointCounterApp.objects;
using TwitchPointCounterApp.Services;

namespace TwitchPointCounterApp
{
    public partial class MainForm : Form
    {
        public MainMenuStrip menuStrip;
        public GroupBox      chatBox;
        public GroupBox      viewversBox;
        public GroupBox      topBox;
        public MainListBox   viewversList;
        public Label         currentViewverLabel;
        public Score         score;
        public WebServicesAll twichService;
        public Saver         saver;
        public Overlay       overlay;
        public bool          first = false;
        public MainForm()
        {
            saver   = new Saver();
            overlay = new Overlay();

            twichService = new WebServicesAll(this, new Dictionary<string, string>
            {
                {"PCcommand", "Commandes disponibles : pc; top" }
            }, saver.LoadToken());
            twichService.InitializeWebServer();


            InitializeComponent();

            Text = "Twitch Point Counter";

            menuStrip    = mainMenuStrip1;
            viewversBox  = mainGroupBox1;
            topBox       = mainGroupBox2;
            chatBox      = mainGroupBox3;
            viewversList = mainListBox1;
            currentViewverLabel  = label1;
            Button addOneButton  = button1;
            Button lessOneButton = button2;

            // viewversBox.Controls.Add(viewversList);
            currentViewverLabel.Text = "Viewver choisi";
            addOneButton.Text        = "+ 1";
            lessOneButton.Text       = "- 1";

            Controls.AddRange(new Control[] { menuStrip, viewversBox, currentViewverLabel, addOneButton, lessOneButton, topBox, chatBox });

            score = saver.Load();
            majTopView();
            overlay.majOverlay(score);

            // ################# TEST ####################

            // ========================== ACTION ==========================

        }

        public void majTopView()
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void mainMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void addOneButton_Click(object sender, EventArgs e)
        {
            score.AddOne(currentViewverLabel.Text);
            majTopView();
            overlay.majOverlay(score);
        }

        private void lessOneButton_Click(object sender, EventArgs e)
        {
            score.LessOne(currentViewverLabel.Text);
            majTopView();
            overlay.majOverlay(score);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            saver.Save(score);
            saver.SaveToken(twichService.GetJsonTokenSaver());
            twichService.Closed();
        }

        public void AddMessageChat(string username, string message)
        {
            chatListBox.Items.Add($"{username} : {message}");
            if (!first)
            {
                switch(MessageBox.Show($"{username} est-t-il le premier à écrire dans le chat ?", "First", MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                        score.AddOne(username);
                        majTopView();
                        overlay.majOverlay(score);
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
