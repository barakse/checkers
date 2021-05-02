using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C20_Ex05
{
    public class Program
    {
        public static void Main()
        {
            invokeUserInterface();
        }

        private static void invokeUserInterface()
        {
            GameSettingsForm gameSettings = new GameSettingsForm();
            gameSettings.ShowDialog();

            if(gameSettings.DialogResult == DialogResult.OK)
            {
                CheckersForm checkers = new CheckersForm(gameSettings);
                checkers.ShowDialog();
            }
        }
    }
}
