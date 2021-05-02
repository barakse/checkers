using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C20_Ex05
{
    public partial class GameSettingsForm : Form
    {
        private eBoardSize m_BoardSize = eBoardSize.SixOnSixTable;
        private bool m_IsPlayerPc = true;
        private string m_FirstPlayerName;
        private string m_SecondPlayerName;

        public eBoardSize BoardSize
        {
            get
            {
                return m_BoardSize;
            }
        }

        public bool IsPlayerPc
        {
            get
            {
                return m_IsPlayerPc;
            }
        }

        public string FirstPlayerName
        {
            get
            {
                return m_FirstPlayerName;
            }
        }

        public string SecondPlayerName
        {
            get
            {
                return m_SecondPlayerName;
            }
        }

        public GameSettingsForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.Checkers_Icon;
        }

        private static bool isPlayerNameValid(string i_PlayerName)
        {
            return !string.IsNullOrEmpty(i_PlayerName) && !i_PlayerName.Contains(" ") && i_PlayerName.Length <= 20;
        }

        private void radioButtonSixOnSixBoardSize_Click(object sender, EventArgs e)
        {
            m_BoardSize = eBoardSize.SixOnSixTable;
        }

        private void radioButtonEightOnEightBoardSize_Click(object sender, EventArgs e)
        {
            m_BoardSize = eBoardSize.EightOnEightTable;
        }

        private void radioButtonTenOnTenBoardSize_Click(object sender, EventArgs e)
        {
            m_BoardSize = eBoardSize.TenOnTenTable;
        }

        private void checkBoxIsSecondPlayer_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxIsSecondPlayer.Checked == true)
            {
                textBoxSecondPlayerName.Text = null;
                textBoxSecondPlayerName.Enabled = true;
                m_IsPlayerPc = false;
            }
            else
            {
                textBoxSecondPlayerName.Text = "[Computer]";
                textBoxSecondPlayerName.Enabled = false;
                m_IsPlayerPc = true;
            }
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            if(!isPlayerNameValid(textBoxFirstPlayerName.Text))
            {
                MessageBox.Show(
                    @"Player 1's name is invalid.
It must not be empty, up to 20 letters and with out spaces.",
                    "Authentication Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            else if(!isPlayerNameValid(textBoxSecondPlayerName.Text))
            {
                MessageBox.Show(
                    @"Player 2's name is invalid.
It must not be empty, up to 20 letters and with out spaces.",
                    "Authentication Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            else
            {
                m_FirstPlayerName = textBoxFirstPlayerName.Text;
                m_SecondPlayerName = m_IsPlayerPc == true ? "Computer" : textBoxSecondPlayerName.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
