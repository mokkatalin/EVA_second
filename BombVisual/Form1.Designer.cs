namespace BombVisual
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Rbtn1 = new System.Windows.Forms.RadioButton();
            this.Rbtn2 = new System.Windows.Forms.RadioButton();
            this.Rbtn3 = new System.Windows.Forms.RadioButton();
            this.NewGameButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.timeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.enemyLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StopResumeBttn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.QuitButton = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Rbtn1
            // 
            this.Rbtn1.AutoSize = true;
            this.Rbtn1.Location = new System.Drawing.Point(65, 64);
            this.Rbtn1.Name = "Rbtn1";
            this.Rbtn1.Size = new System.Drawing.Size(43, 19);
            this.Rbtn1.TabIndex = 0;
            this.Rbtn1.TabStop = true;
            this.Rbtn1.Tag = "Input\\inp.txt";
            this.Rbtn1.Text = "10x10";
            this.Rbtn1.UseVisualStyleBackColor = true;
            this.Rbtn1.CheckedChanged += new System.EventHandler(this.Rbtn_CheckedChanged);
            // 
            // Rbtn2
            // 
            this.Rbtn2.AutoSize = true;
            this.Rbtn2.Location = new System.Drawing.Point(65, 89);
            this.Rbtn2.Name = "Rbtn2";
            this.Rbtn2.Size = new System.Drawing.Size(43, 19);
            this.Rbtn2.TabIndex = 1;
            this.Rbtn2.TabStop = true;
            this.Rbtn2.Tag = "Input\\inp2.txt";
            this.Rbtn2.Text = "9x9";
            this.Rbtn2.UseVisualStyleBackColor = true;
            this.Rbtn2.CheckedChanged += new System.EventHandler(this.Rbtn_CheckedChanged);
            // 
            // Rbtn3
            // 
            this.Rbtn3.AutoSize = true;
            this.Rbtn3.Location = new System.Drawing.Point(65, 114);
            this.Rbtn3.Name = "Rbtn3";
            this.Rbtn3.Size = new System.Drawing.Size(43, 19);
            this.Rbtn3.TabIndex = 2;
            this.Rbtn3.TabStop = true;
            this.Rbtn3.Tag = "Input\\inp3.txt";
            this.Rbtn3.Text = "8x8";
            this.Rbtn3.UseVisualStyleBackColor = true;
            this.Rbtn3.CheckedChanged += new System.EventHandler(this.Rbtn_CheckedChanged);
            // 
            // NewGameButton
            // 
            this.NewGameButton.Location = new System.Drawing.Point(48, 153);
            this.NewGameButton.Name = "NewGameButton";
            this.NewGameButton.Size = new System.Drawing.Size(75, 23);
            this.NewGameButton.TabIndex = 3;
            this.NewGameButton.Text = "New Game";
            this.NewGameButton.UseVisualStyleBackColor = true;
            this.NewGameButton.Click += new System.EventHandler(this.NewGameButton_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(193, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(386, 380);
            this.panel1.TabIndex = 5;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timeLabel,
            this.enemyLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 288);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(350, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // timeLabel
            // 
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(84, 17);
            this.timeLabel.Text = "Time passed: 0";
            // 
            // enemyLabel
            // 
            this.enemyLabel.Name = "enemyLabel";
            this.enemyLabel.Size = new System.Drawing.Size(97, 17);
            this.enemyLabel.Text = "Enemies Down: 0";
            // 
            // StopResumeBttn
            // 
            this.StopResumeBttn.Location = new System.Drawing.Point(18, 36);
            this.StopResumeBttn.Name = "StopResumeBttn";
            this.StopResumeBttn.Size = new System.Drawing.Size(75, 23);
            this.StopResumeBttn.TabIndex = 6;
            this.StopResumeBttn.Text = "Stop";
            this.StopResumeBttn.UseVisualStyleBackColor = true;
            this.StopResumeBttn.Click += new System.EventHandler(this.StopResumeBttn_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.QuitButton);
            this.panel2.Controls.Add(this.NewGameButton);
            this.panel2.Controls.Add(this.Rbtn3);
            this.panel2.Controls.Add(this.Rbtn2);
            this.panel2.Controls.Add(this.Rbtn1);
            this.panel2.Location = new System.Drawing.Point(41, 52);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(181, 271);
            this.panel2.TabIndex = 7;
            // 
            // QuitButton
            // 
            this.QuitButton.Location = new System.Drawing.Point(15, 224);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(75, 23);
            this.QuitButton.TabIndex = 4;
            this.QuitButton.Text = "Quit";
            this.QuitButton.UseVisualStyleBackColor = true;
            this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(588, 42);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(120, 100);
            this.panel3.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        private RadioButton Rbtn1;
        private RadioButton Rbtn2;
        private RadioButton Rbtn3;
        private Button NewGameButton;

        #endregion

        private Label[,] labels = null;
        private Panel panel1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel timeLabel;
        private ToolStripStatusLabel enemyLabel;
        private Button StopResumeBttn;
        private Panel panel2;
        private Panel panel3;
        private Button QuitButton;
    }
}