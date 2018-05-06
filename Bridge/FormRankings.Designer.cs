namespace Bridge {
    partial class FormRankings {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "BLACKROCK",
            "Prisoners of Irreality",
            "1"}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Din-Din",
            "Pink Fluffy Unicorns",
            "2"}, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "LoneWanderer",
            "",
            "3"}, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "NoEpicLoot",
            "RNG sucks",
            "4"}, -1);
            this.labelSearch = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.radioButtonRankingsClan = new System.Windows.Forms.RadioButton();
            this.radioButtonRankingsUsers = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(63, 9);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(44, 13);
            this.labelSearch.TabIndex = 4;
            this.labelSearch.Text = "Search:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(110, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 3;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8});
            this.listView1.Location = new System.Drawing.Point(1, 31);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(229, 199);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 81;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Clan";
            this.columnHeader2.Width = 106;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Rank";
            this.columnHeader3.Width = 38;
            // 
            // radioButtonRankingsClan
            // 
            this.radioButtonRankingsClan.AutoSize = true;
            this.radioButtonRankingsClan.Location = new System.Drawing.Point(6, 15);
            this.radioButtonRankingsClan.Name = "radioButtonRankingsClan";
            this.radioButtonRankingsClan.Size = new System.Drawing.Size(51, 17);
            this.radioButtonRankingsClan.TabIndex = 1;
            this.radioButtonRankingsClan.Text = "Clans";
            this.radioButtonRankingsClan.UseVisualStyleBackColor = true;
            // 
            // radioButtonRankingsUsers
            // 
            this.radioButtonRankingsUsers.AutoSize = true;
            this.radioButtonRankingsUsers.Checked = true;
            this.radioButtonRankingsUsers.Location = new System.Drawing.Point(6, 0);
            this.radioButtonRankingsUsers.Name = "radioButtonRankingsUsers";
            this.radioButtonRankingsUsers.Size = new System.Drawing.Size(52, 17);
            this.radioButtonRankingsUsers.TabIndex = 0;
            this.radioButtonRankingsUsers.TabStop = true;
            this.radioButtonRankingsUsers.Text = "Users";
            this.radioButtonRankingsUsers.UseVisualStyleBackColor = true;
            // 
            // FormRankings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 231);
            this.Controls.Add(this.labelSearch);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.radioButtonRankingsUsers);
            this.Controls.Add(this.radioButtonRankingsClan);
            this.Enabled = false;
            this.Name = "FormRankings";
            this.Text = "FormRankings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.RadioButton radioButtonRankingsClan;
        private System.Windows.Forms.RadioButton radioButtonRankingsUsers;
    }
}