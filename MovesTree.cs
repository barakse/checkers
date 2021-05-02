using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C20_Ex02
{
    public class MovesTree
    {
        private PositionNode m_Root;

        public MovesTree(int i_Row, int i_Column)
        {
            m_Root = new PositionNode(i_Row, i_Column);
        }

        public MovesTree()
        {
            m_Root = null;
        }

        public PositionNode Root
        {
            get
            {
                return m_Root;
            }

            set
            {
                m_Root = value;
            }
        }

        public static PositionNode CreateMovesTreeNode(
            int i_Row,
            int i_Column,
            int i_SkipsSoFar,
            PositionNode i_Left,
            PositionNode i_Right)
        {
            PositionNode movesTreeNode = new PositionNode(i_Row, i_Column);
            movesTreeNode.SkipsSoFar = i_SkipsSoFar;
            movesTreeNode.Left = i_Left;
            movesTreeNode.Right = i_Right;

            return movesTreeNode;
        }
    }
}
