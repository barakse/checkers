using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C20_Ex05
{
    public partial class SquareButton : Button
    {
        private const int k_SquareSize = 50;
        private char m_Sign;
        private int m_rowInBoard;
        private int m_columnInBoard;

        public SquareButton(int i_RowInBoard, int i_ColumnInBoard)
        {
            m_rowInBoard = i_RowInBoard;
            m_columnInBoard = i_ColumnInBoard;
            m_Sign = ' ';
            this.BackColor = Color.FromArgb(120, 120, 120);
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(k_SquareSize, k_SquareSize);
            this.Enabled = false;
            this.Padding = new Padding(0, 0, 1, 2);
            this.ImageAlign = ContentAlignment.MiddleCenter;
            this.Name = "BoardSquare";
        }

        public static int SquareSize
        {
            get
            {
                return k_SquareSize;
            }
        }

        public char Sign
        {
            get
            {
                return m_Sign;
            }

            set
            {
                m_Sign = value;
            }
        }

        public int Row
        {
            get
            {
                return m_rowInBoard;
            }
        }

        public int Column
        {
            get
            {
                return m_columnInBoard;
            }
        }

        public SquareButton(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}