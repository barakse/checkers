using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C20_Ex02
{
    public class PositionNode
    {
        private int m_Row;
        private int m_Column;
        private int m_SkipsSoFar;
        private PositionNode m_Left;
        private PositionNode m_Right;

        public PositionNode(int i_Row, int i_Column)
        {
            m_Row = i_Row;
            m_Column = i_Column;
            m_Left = null;
            m_Right = null;
            m_SkipsSoFar = 0;
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

        public int SkipsSoFar
        {
            get
            {
                return m_SkipsSoFar;
            }

            set
            {
                m_SkipsSoFar = value;
            }
        }

        public PositionNode Left
        {
            get
            {
                return m_Left;
            }

            set
            {
                m_Left = value;
            }
        }

        public PositionNode Right
        {
            get
            {
                return m_Right;
            }

            set
            {
                m_Right = value;
            }
        }
    }
}
