using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C20_Ex02
{
    public class Piece
    {
        private char m_PieceSign;
        private int m_Row;
        private int m_Column;

        public Piece(int i_Row, int i_Column, char i_PieceSign)
        {
            m_Row = i_Row;
            m_Column = i_Column;
            m_PieceSign = i_PieceSign;
        }

        public char PieceSign
        {
            get
            {
                return m_PieceSign;
            }

            set
            {
                m_PieceSign = value;
            }
        }

        public int Row
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public int Column
        {
            get
            {
                return m_Column;
            }

            set
            {
                m_Column = value;
            }
        }

        public static int ConvertRowCharToInt(char i_Row)
        {
            return i_Row - 'a';
        }

        public static int ConvertColumnCharToInt(char i_Column)
        {
            return i_Column - 'A';
        }

        public void UpdatePiece(int i_Row, int i_Column, char i_PieceType)
        {
            this.Row = i_Row;
            this.Column = i_Column;
            this.PieceSign = i_PieceType;
        }
    }
}
