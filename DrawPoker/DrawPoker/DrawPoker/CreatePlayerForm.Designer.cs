namespace DrawPoker
{
    partial class CreatePlayerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreatePlayerForm));
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.NamePlayer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ante = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.AddPlayers = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("GOST type B", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(256, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(971, 54);
            this.label1.TabIndex = 0;
            this.label1.Text = "Введите имена игроков и их личный банк";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.DarkGreen;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NamePlayer,
            this.Bank,
            this.Ante});
            this.dataGridView1.GridColor = System.Drawing.Color.DarkGreen;
            this.dataGridView1.Location = new System.Drawing.Point(265, 151);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(962, 286);
            this.dataGridView1.TabIndex = 1;
            // 
            // NamePlayer
            // 
            this.NamePlayer.HeaderText = "Имя";
            this.NamePlayer.MinimumWidth = 8;
            this.NamePlayer.Name = "NamePlayer";
            this.NamePlayer.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.NamePlayer.Width = 200;
            // 
            // Bank
            // 
            this.Bank.HeaderText = "Личный Банк";
            this.Bank.MinimumWidth = 8;
            this.Bank.Name = "Bank";
            this.Bank.Width = 200;
            // 
            // Ante
            // 
            this.Ante.HeaderText = "Ante";
            this.Ante.MinimumWidth = 8;
            this.Ante.Name = "Ante";
            this.Ante.Width = 175;
            // 
            // AddPlayers
            // 
            this.AddPlayers.BackColor = System.Drawing.Color.DarkGreen;
            this.AddPlayers.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
            this.AddPlayers.FlatAppearance.BorderSize = 2;
            this.AddPlayers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddPlayers.Font = new System.Drawing.Font("GOST type B", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddPlayers.ForeColor = System.Drawing.Color.Yellow;
            this.AddPlayers.Location = new System.Drawing.Point(979, 669);
            this.AddPlayers.Name = "AddPlayers";
            this.AddPlayers.Size = new System.Drawing.Size(412, 91);
            this.AddPlayers.TabIndex = 3;
            this.AddPlayers.Text = "Продолжить";
            this.AddPlayers.UseVisualStyleBackColor = false;
            this.AddPlayers.Click += new System.EventHandler(this.AddPlayers_Click);
            // 
            // CreatePlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1439, 802);
            this.Controls.Add(this.AddPlayers);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Name = "CreatePlayerForm";
            this.Text = "Draw Poker";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn NamePlayer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bank;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Ante;
        private System.Windows.Forms.Button AddPlayers;
    }
}