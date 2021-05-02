namespace C20_Ex05
{
    public partial class GameSettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonSixOnSixBoardSize = new System.Windows.Forms.RadioButton();
            this.radioButtonEightOnEightBoardSize = new System.Windows.Forms.RadioButton();
            this.radioButtonTenOnTenBoardSize = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFirstPlayerName = new System.Windows.Forms.TextBox();
            this.checkBoxIsSecondPlayer = new System.Windows.Forms.CheckBox();
            this.textBoxSecondPlayerName = new System.Windows.Forms.TextBox();
            this.buttonDone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 100;
            this.label1.Text = "Board Size:";
            // 
            // radioButtonSixOnSixBoardSize
            // 
            this.radioButtonSixOnSixBoardSize.AutoSize = true;
            this.radioButtonSixOnSixBoardSize.Checked = true;
            this.radioButtonSixOnSixBoardSize.Location = new System.Drawing.Point(25, 29);
            this.radioButtonSixOnSixBoardSize.Name = "radioButtonSixOnSixBoardSize";
            this.radioButtonSixOnSixBoardSize.Size = new System.Drawing.Size(48, 17);
            this.radioButtonSixOnSixBoardSize.TabIndex = 1;
            this.radioButtonSixOnSixBoardSize.TabStop = true;
            this.radioButtonSixOnSixBoardSize.Text = "6 x 6";
            this.radioButtonSixOnSixBoardSize.UseVisualStyleBackColor = true;
            this.radioButtonSixOnSixBoardSize.Click += new System.EventHandler(this.radioButtonSixOnSixBoardSize_Click);
            // 
            // radioButtonEightOnEightBoardSize
            // 
            this.radioButtonEightOnEightBoardSize.AutoSize = true;
            this.radioButtonEightOnEightBoardSize.Location = new System.Drawing.Point(96, 29);
            this.radioButtonEightOnEightBoardSize.Name = "radioButtonEightOnEightBoardSize";
            this.radioButtonEightOnEightBoardSize.Size = new System.Drawing.Size(48, 17);
            this.radioButtonEightOnEightBoardSize.TabIndex = 2;
            this.radioButtonEightOnEightBoardSize.Text = "8 x 8";
            this.radioButtonEightOnEightBoardSize.UseVisualStyleBackColor = true;
            this.radioButtonEightOnEightBoardSize.Click += new System.EventHandler(this.radioButtonEightOnEightBoardSize_Click);
            // 
            // radioButtonTenOnTenBoardSize
            // 
            this.radioButtonTenOnTenBoardSize.AutoSize = true;
            this.radioButtonTenOnTenBoardSize.Location = new System.Drawing.Point(162, 29);
            this.radioButtonTenOnTenBoardSize.Name = "radioButtonTenOnTenBoardSize";
            this.radioButtonTenOnTenBoardSize.Size = new System.Drawing.Size(60, 17);
            this.radioButtonTenOnTenBoardSize.TabIndex = 3;
            this.radioButtonTenOnTenBoardSize.Text = "10 x 10";
            this.radioButtonTenOnTenBoardSize.UseVisualStyleBackColor = true;
            this.radioButtonTenOnTenBoardSize.Click += new System.EventHandler(this.radioButtonTenOnTenBoardSize_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Players:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Player 1:";
            // 
            // textBoxFirstPlayerName
            // 
            this.textBoxFirstPlayerName.Location = new System.Drawing.Point(96, 86);
            this.textBoxFirstPlayerName.Name = "textBoxFirstPlayerName";
            this.textBoxFirstPlayerName.Size = new System.Drawing.Size(127, 20);
            this.textBoxFirstPlayerName.TabIndex = 6;
            // 
            // checkBoxIsSecondPlayer
            // 
            this.checkBoxIsSecondPlayer.AutoSize = true;
            this.checkBoxIsSecondPlayer.Location = new System.Drawing.Point(25, 115);
            this.checkBoxIsSecondPlayer.Name = "checkBoxIsSecondPlayer";
            this.checkBoxIsSecondPlayer.Size = new System.Drawing.Size(67, 17);
            this.checkBoxIsSecondPlayer.TabIndex = 8;
            this.checkBoxIsSecondPlayer.Text = "Player 2:";
            this.checkBoxIsSecondPlayer.UseVisualStyleBackColor = true;
            this.checkBoxIsSecondPlayer.CheckedChanged += new System.EventHandler(this.checkBoxIsSecondPlayer_CheckedChanged);
            // 
            // textBoxSecondPlayerName
            // 
            this.textBoxSecondPlayerName.Enabled = false;
            this.textBoxSecondPlayerName.Location = new System.Drawing.Point(96, 115);
            this.textBoxSecondPlayerName.Name = "textBoxSecondPlayerName";
            this.textBoxSecondPlayerName.Size = new System.Drawing.Size(127, 20);
            this.textBoxSecondPlayerName.TabIndex = 9;
            this.textBoxSecondPlayerName.Text = "[Computer]";
            // 
            // buttonDone
            // 
            this.buttonDone.Location = new System.Drawing.Point(82, 149);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 10;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // GameSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 184);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.textBoxSecondPlayerName);
            this.Controls.Add(this.checkBoxIsSecondPlayer);
            this.Controls.Add(this.textBoxFirstPlayerName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radioButtonTenOnTenBoardSize);
            this.Controls.Add(this.radioButtonEightOnEightBoardSize);
            this.Controls.Add(this.radioButtonSixOnSixBoardSize);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameSettingsForm";
            this.Text = "Game Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonSixOnSixBoardSize;
        private System.Windows.Forms.RadioButton radioButtonEightOnEightBoardSize;
        private System.Windows.Forms.RadioButton radioButtonTenOnTenBoardSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFirstPlayerName;
        private System.Windows.Forms.CheckBox checkBoxIsSecondPlayer;
        private System.Windows.Forms.TextBox textBoxSecondPlayerName;
        private System.Windows.Forms.Button buttonDone;
    }
}