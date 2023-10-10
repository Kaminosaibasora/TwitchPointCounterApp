namespace TwitchPointCounterApp
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.mainGroupBox3 = new TwitchPointCounterApp.Controls.MainGroupBox();
            this.chatListBox = new System.Windows.Forms.ListBox();
            this.mainGroupBox2 = new TwitchPointCounterApp.Controls.MainGroupBox();
            this.topList = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pointColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mainGroupBox1 = new TwitchPointCounterApp.Controls.MainGroupBox();
            this.mainListBox1 = new TwitchPointCounterApp.Controls.MainListBox();
            this.mainMenuStrip1 = new TwitchPointCounterApp.Controls.MainMenuStrip();
            this.mainGroupBox3.SuspendLayout();
            this.mainGroupBox2.SuspendLayout();
            this.mainGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(214, 204);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Viewver select";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(329, 152);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "+1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AddOneButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(329, 263);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "-1";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.LessOneButton_Click);
            // 
            // mainGroupBox3
            // 
            this.mainGroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainGroupBox3.Controls.Add(this.chatListBox);
            this.mainGroupBox3.Location = new System.Drawing.Point(635, 27);
            this.mainGroupBox3.Name = "mainGroupBox3";
            this.mainGroupBox3.Size = new System.Drawing.Size(157, 388);
            this.mainGroupBox3.TabIndex = 6;
            this.mainGroupBox3.TabStop = false;
            this.mainGroupBox3.Text = "CHAT";
            // 
            // chatListBox
            // 
            this.chatListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatListBox.FormattingEnabled = true;
            this.chatListBox.Location = new System.Drawing.Point(6, 19);
            this.chatListBox.Name = "chatListBox";
            this.chatListBox.Size = new System.Drawing.Size(145, 355);
            this.chatListBox.TabIndex = 0;
            // 
            // mainGroupBox2
            // 
            this.mainGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.mainGroupBox2.Controls.Add(this.topList);
            this.mainGroupBox2.Location = new System.Drawing.Point(464, 27);
            this.mainGroupBox2.Name = "mainGroupBox2";
            this.mainGroupBox2.Size = new System.Drawing.Size(165, 388);
            this.mainGroupBox2.TabIndex = 5;
            this.mainGroupBox2.TabStop = false;
            this.mainGroupBox2.Text = "TOP";
            // 
            // topList
            // 
            this.topList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.pointColumn});
            this.topList.GridLines = true;
            this.topList.HideSelection = false;
            this.topList.LabelWrap = false;
            this.topList.Location = new System.Drawing.Point(6, 19);
            this.topList.MultiSelect = false;
            this.topList.Name = "topList";
            this.topList.Size = new System.Drawing.Size(153, 363);
            this.topList.TabIndex = 0;
            this.topList.UseCompatibleStateImageBehavior = false;
            this.topList.View = System.Windows.Forms.View.Details;
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 100;
            // 
            // pointColumn
            // 
            this.pointColumn.Text = "Point";
            this.pointColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.pointColumn.Width = 40;
            // 
            // mainGroupBox1
            // 
            this.mainGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.mainGroupBox1.Controls.Add(this.mainListBox1);
            this.mainGroupBox1.Location = new System.Drawing.Point(28, 27);
            this.mainGroupBox1.Name = "mainGroupBox1";
            this.mainGroupBox1.Size = new System.Drawing.Size(180, 407);
            this.mainGroupBox1.TabIndex = 1;
            this.mainGroupBox1.TabStop = false;
            this.mainGroupBox1.Text = "Viewvers";
            // 
            // mainListBox1
            // 
            this.mainListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainListBox1.FormattingEnabled = true;
            this.mainListBox1.Location = new System.Drawing.Point(6, 19);
            this.mainListBox1.Name = "mainListBox1";
            this.mainListBox1.Size = new System.Drawing.Size(168, 381);
            this.mainListBox1.TabIndex = 0;
            // 
            // mainMenuStrip1
            // 
            this.mainMenuStrip1.Location = new System.Drawing.Point(25, 0);
            this.mainMenuStrip1.Name = "mainMenuStrip1";
            this.mainMenuStrip1.Size = new System.Drawing.Size(750, 24);
            this.mainMenuStrip1.TabIndex = 7;
            this.mainMenuStrip1.Text = "mainMenuStrip1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainGroupBox3);
            this.Controls.Add(this.mainGroupBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mainGroupBox1);
            this.Controls.Add(this.mainMenuStrip1);
            this.MainMenuStrip = this.mainMenuStrip1;
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(25, 0, 25, 25);
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.mainGroupBox3.ResumeLayout(false);
            this.mainGroupBox2.ResumeLayout(false);
            this.mainGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.MainGroupBox mainGroupBox1;
        private Controls.MainListBox mainListBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private Controls.MainGroupBox mainGroupBox2;
        private Controls.MainGroupBox mainGroupBox3;
        private Controls.MainMenuStrip mainMenuStrip1;
        private System.Windows.Forms.ListView topList;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader pointColumn;
        private System.Windows.Forms.ListBox chatListBox;
    }
}

