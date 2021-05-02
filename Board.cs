using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C20_Ex05;

namespace C20_Ex02
{
    public class Board
    {
        private readonly eBoardSize r_BoardSize;
        private char[,] m_GameBoard;
        private char m_RowBoundaryLetter;
        private char m_ColumnBoundaryLetter;
        public readonly Dictionary<int, int> r_PiecesAccordingBoardSizeDictionary;

        public Board(eBoardSize i_BoardSize)
        {
            r_BoardSize = i_BoardSize;
            m_GameBoard = new char[(int)r_BoardSize, (int)r_BoardSize];
            BoardInitialize();
            initializeBoundariesLetters();
            r_PiecesAccordingBoardSizeDictionary = new Dictionary<int, int>()
                                                       {
                                                           { (int)eBoardSize.SixOnSixTable, 6 },
                                                           { (int)eBoardSize.EightOnEightTable, 12 },
                                                           { (int)eBoardSize.TenOnTenTable, 20 }
                                                       };
        }

        public char[,] GameBoard
        {
            get
            {
                return m_GameBoard;
            }

            set
            {
                m_GameBoard = value;
            }
        }

        public eBoardSize Size
        {
            get
            {
                return r_BoardSize;
            }
        }

        public char RowBoundaryLetter
        {
            get
            {
                return m_RowBoundaryLetter;
            }
        }

        public char ColumnBoundaryLetter
        {
            get
            {
                return m_ColumnBoundaryLetter;
            }
        }

        private static void buildLineSeparator(ref StringBuilder io_LineSeparator)
        {
            io_LineSeparator.Append(' ');

            for (int i = 0; i < io_LineSeparator.MaxCapacity; i++)
            {
                io_LineSeparator.Append('=');
            }
        }

        private void initializeBoundariesLetters()
        {
            switch (r_BoardSize)
            {
                case eBoardSize.SixOnSixTable:
                    {
                        m_ColumnBoundaryLetter = 'F';
                        m_RowBoundaryLetter = 'f';
                        break;
                    }

                case eBoardSize.EightOnEightTable:
                    {
                        m_ColumnBoundaryLetter = 'H';
                        m_RowBoundaryLetter = 'h';
                        break;
                    }

                case eBoardSize.TenOnTenTable:
                    {
                        m_ColumnBoundaryLetter = 'J';
                        m_RowBoundaryLetter = 'j';
                        break;
                    }
            }
        }

        public bool IsMoveInBoardForPlayer(Player i_Player, bool i_IsSkipToCheck)
        {
            bool isSkipMoveAvailable = false;

            if(i_Player.SoldierSign == Checkers.k_BottomPlayerSoldierSign)
            {
                isSkipMoveAvailable = MovesOnBoardForPlayerCheck(
                    Checkers.k_BottomPlayerSoldierSign,
                    Checkers.k_BottomPlayerKingSign,
                    Checkers.k_TopPlayerSoldierSign,
                    Checkers.k_TopPlayerKingSign,
                    i_IsSkipToCheck);
            }
            else
            {
                isSkipMoveAvailable = MovesOnBoardForPlayerCheck(
                    Checkers.k_TopPlayerSoldierSign,
                    Checkers.k_TopPlayerKingSign,
                    Checkers.k_BottomPlayerSoldierSign,
                    Checkers.k_BottomPlayerKingSign,
                    i_IsSkipToCheck);
            }

            return isSkipMoveAvailable;
        }

        private bool MovesOnBoardForPlayerCheck(char i_CurrentPlayerSoldierSign, char i_CurrentPlayerKingSign, char i_OpponentSoldierSign, char i_OpponentKingSign, bool i_IsSkipToCheck)
        {
            bool isMoveAvailable = false;

            for(int i = 0; i < (int)r_BoardSize; i++)
            {
                for(int j = 0; j < (int)r_BoardSize; j++)
                {
                    if(m_GameBoard[i, j] == i_CurrentPlayerKingSign || m_GameBoard[i, j] == i_CurrentPlayerSoldierSign)
                    {
                        isMoveAvailable = IsMoveAvailableForPieceInBoard(
                            m_GameBoard[i, j],
                            i,
                            j,
                            i_OpponentSoldierSign,
                            i_OpponentKingSign,
                            i_IsSkipToCheck);

                        if(isMoveAvailable == true)
                        {
                            break;
                        }
                    }
                }

                if(isMoveAvailable == true)
                {
                    break;
                }
            }

            return isMoveAvailable;
        }

        public bool IsMoveAvailableForPieceInBoard(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign, bool i_IsSkipToCheck)
        {
            bool isMoveAvailable = false;
            
            if(i_Row == 0 || i_Row == 1)
            {
                isMoveAvailable = topEdgeMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign, i_IsSkipToCheck);
            }
            else if(i_Row == (int)r_BoardSize - 1 || i_Row == (int)r_BoardSize - 2)
            {
                isMoveAvailable = bottomEdgeMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign, i_IsSkipToCheck);
            }
            else
            {
                if(i_Column == 0 || i_Column == 1)
                {
                    isMoveAvailable = leftEdgeMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign, i_IsSkipToCheck);
                }
                else if(i_Column == (int)r_BoardSize - 1 || i_Column == (int)r_BoardSize - 2)
                {
                    isMoveAvailable = rightEdgeMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign, i_IsSkipToCheck);
                }
                else
                {
                    isMoveAvailable = middleBoardMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign, i_IsSkipToCheck);
                }
            }

            return isMoveAvailable;
        }

        private bool topEdgeMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign, bool i_IsSkipToCheck)
        {
            bool isMoveAvailable = false;
            
            if (i_IsSkipToCheck == true)
            {
                isMoveAvailable = topEdgeSkipMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
            }
            else
            {
                isMoveAvailable = topEdgeRegularMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
            }

            return isMoveAvailable;
        }

        private bool topEdgeSkipMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign)
        {
            bool isMoveAvailable = false;
            int leftEdgeColumn = (i_Row == 0) ? 1 : 0;
            int rightEdgeColumn = (i_Row == 0) ? ((int)r_BoardSize - 1) : ((int)r_BoardSize - 2);

            if (i_CurrentPieceSign != Checkers.k_BottomPlayerSoldierSign)
            {
                if (i_Column == leftEdgeColumn)
                {
                    isMoveAvailable = (m_GameBoard[i_Row + 1, i_Column + 1] == i_OpponentSoldierSign
                                        || m_GameBoard[i_Row + 1, i_Column + 1] == i_OpponentKingSign)
                                       && m_GameBoard[i_Row + 2, i_Column + 2] == ' ';
                }
                else if (i_Column == rightEdgeColumn)
                {
                    isMoveAvailable = (m_GameBoard[i_Row + 1, i_Column - 1] == i_OpponentSoldierSign
                                        || m_GameBoard[i_Row + 1, i_Column - 1] == i_OpponentKingSign)
                                       && m_GameBoard[i_Row + 2, i_Column - 2] == ' ';
                }
                else
                {
                    bool isLeftBottomMove = (m_GameBoard[i_Row + 1, i_Column - 1] == i_OpponentSoldierSign
                                         || m_GameBoard[i_Row + 1, i_Column - 1] == i_OpponentKingSign)
                                        && m_GameBoard[i_Row + 2, i_Column - 2] == ' ';
                    bool isRightBottomMove = (m_GameBoard[i_Row + 1, i_Column + 1] == i_OpponentSoldierSign
                                          || m_GameBoard[i_Row + 1, i_Column + 1] == i_OpponentKingSign)
                                         && m_GameBoard[i_Row + 2, i_Column + 2] == ' ';

                    isMoveAvailable = isLeftBottomMove || isRightBottomMove;
                }
            }

            return isMoveAvailable;
        }

        private bool topEdgeRegularMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign)
        {
            bool isMoveAvailable = false;
            
            if(i_Row == 0)
            {
                bool isLeftBottomMove = m_GameBoard[i_Row + 1, i_Column - 1] == ' ';
                if(i_Column == (int)r_BoardSize - 1)
                {
                    isMoveAvailable = isLeftBottomMove;
                }
                else
                {
                    bool isRightBottomMove = m_GameBoard[i_Row + 1, i_Column + 1] == ' ';
                    isMoveAvailable = isLeftBottomMove || isRightBottomMove;
                }
            }
            else
            {
                if(i_Column == 0)
                {
                    isMoveAvailable = leftEdgeRegularMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
                }
                else
                {
                    isMoveAvailable = middleBoardRegularMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
                }
            }

            return isMoveAvailable;
        }

        private bool bottomEdgeMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign, bool i_IsSkipToCheck)
        {
            bool isMoveAvailable = false;
            
            if(i_IsSkipToCheck == true)
            {
                isMoveAvailable = bottomEdgeSkipMoveCheck(
                    i_CurrentPieceSign,
                    i_Row,
                    i_Column,
                    i_OpponentSoldierSign,
                    i_OpponentKingSign);
            }
            else
            {
                isMoveAvailable = bottomEdgeRegularMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
            }

            return isMoveAvailable;
        }

        private bool bottomEdgeSkipMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign)
        {
            bool isSkipMoveAvailable = false;
            int leftEdgeColumn = (i_Row == (int)r_BoardSize - 1) ? 0 : 1;
            int rightEdgeColumn = (i_Row == (int)r_BoardSize - 1) ? ((int)r_BoardSize - 2) : ((int)r_BoardSize - 1);

            if(i_CurrentPieceSign != Checkers.k_TopPlayerSoldierSign)
            {
                if(i_Column == leftEdgeColumn)
                {
                    isSkipMoveAvailable =
                        (m_GameBoard[i_Row - 1, i_Column + 1] == i_OpponentSoldierSign
                          || m_GameBoard[i_Row - 1, i_Column + 1] == i_OpponentKingSign) && m_GameBoard[i_Row - 2, i_Column + 2] == ' ';
                }
                else if(i_Column == rightEdgeColumn)
                {
                    isSkipMoveAvailable = (m_GameBoard[i_Row - 1, i_Column - 1] == i_OpponentSoldierSign
                                            || m_GameBoard[i_Row - 1, i_Column - 1] == i_OpponentKingSign)
                                           && m_GameBoard[i_Row - 2, i_Column - 2] == ' ';
                }
                else
                {
                    bool isLeftTopSkip = (m_GameBoard[i_Row - 1, i_Column - 1] == i_OpponentSoldierSign
                                           || m_GameBoard[i_Row - 1, i_Column - 1] == i_OpponentKingSign)
                                          && m_GameBoard[i_Row - 2, i_Column - 2] == ' ';
                    bool isRightTopSkip = (m_GameBoard[i_Row - 1, i_Column + 1] == i_OpponentSoldierSign
                                            || m_GameBoard[i_Row - 1, i_Column + 1] == i_OpponentKingSign)
                                           && m_GameBoard[i_Row - 2, i_Column + 2] == ' ';

                    isSkipMoveAvailable = isLeftTopSkip || isRightTopSkip;
                }
            }

            return isSkipMoveAvailable;
        }

        private bool bottomEdgeRegularMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign)
        {
            bool isMoveAvailable = false;
            
            if(i_Row == (int)r_BoardSize - 1)
            {
                bool isRightTopMove = m_GameBoard[i_Row - 1, i_Column + 1] == ' ';
                if(i_Column == 0)
                {
                    isMoveAvailable = isRightTopMove;
                }
                else
                {
                    bool isLeftTopMove = m_GameBoard[i_Row - 1, i_Column - 1] == ' ';
                    isMoveAvailable = isRightTopMove || isLeftTopMove;
                }
            }
            else
            {
                if(i_Column == (int)r_BoardSize - 1)
                {
                    isMoveAvailable = rightEdgeRegularMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
                }
                else
                {
                    isMoveAvailable = middleBoardRegularMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
                }
            }

            return isMoveAvailable;
        }

        private bool leftEdgeMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign, bool i_IsSkipToCheck)
        {
            bool isMoveAvailable = false;

            if(i_IsSkipToCheck == true)
            {
                isMoveAvailable = leftEdgeSkipMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
            }
            else
            {
                isMoveAvailable = leftEdgeRegularMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
            }

            return isMoveAvailable;
        }

        private bool leftEdgeSkipMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign)
        {
            bool isRightTopSkip = false;
            bool isRightBottomSkip = false;

            if (i_CurrentPieceSign != Checkers.k_TopPlayerSoldierSign)
            {
                isRightTopSkip = (m_GameBoard[i_Row - 1, i_Column + 1] == i_OpponentSoldierSign
                                   || m_GameBoard[i_Row - 1, i_Column + 1] == i_OpponentKingSign)
                                  && m_GameBoard[i_Row - 2, i_Column + 2] == ' ';
            }

            if (i_CurrentPieceSign != Checkers.k_BottomPlayerSoldierSign)
            {
                isRightBottomSkip =
                    (m_GameBoard[i_Row + 1, i_Column + 1] == i_OpponentSoldierSign
                      || m_GameBoard[i_Row + 1, i_Column + 1] == i_OpponentKingSign)
                     && m_GameBoard[i_Row + 2, i_Column + 2] == ' ';
            }

            return isRightTopSkip || isRightBottomSkip;
        }

        private bool leftEdgeRegularMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign)
        {
            bool isMoveAvailable = false;
            bool isRightBottomMove = m_GameBoard[i_Row + 1, i_Column + 1] == ' ';
            bool isRightTopMove = m_GameBoard[i_Row - 1, i_Column + 1] == ' ';

            if(i_Column == 0)
            {
                if(i_CurrentPieceSign == Checkers.k_BottomPlayerSoldierSign)
                {
                    isMoveAvailable = isRightTopMove;
                }

                if(i_CurrentPieceSign == Checkers.k_TopPlayerSoldierSign)
                {
                    isMoveAvailable = isRightBottomMove;
                }
                else
                {
                    isMoveAvailable = isRightTopMove || isRightBottomMove;
                }
            }
            else
            {
                isMoveAvailable = middleBoardRegularMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
            }

            return isMoveAvailable;
        }

        private bool rightEdgeMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign, bool i_IsSkipToCheck)
        {
            bool isMoveAvailable = false;

            if(i_IsSkipToCheck == true)
            {
                isMoveAvailable = rightEdgeSkipMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
            }
            else
            {
                isMoveAvailable = rightEdgeRegularMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
            }

            return isMoveAvailable;
        }

        private bool rightEdgeSkipMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign)
        {
            bool isLeftBottomSkip = false;
            bool isLeftTopSkip = false;

            if (i_CurrentPieceSign != Checkers.k_TopPlayerSoldierSign)
            {
                isLeftTopSkip = (m_GameBoard[i_Row - 1, i_Column - 1] == i_OpponentSoldierSign
                                  || m_GameBoard[i_Row - 1, i_Column - 1] == i_OpponentKingSign)
                                 && m_GameBoard[i_Row - 2, i_Column - 2] == ' ';
            }

            if (i_CurrentPieceSign != Checkers.k_BottomPlayerSoldierSign)
            {
                isLeftBottomSkip = (m_GameBoard[i_Row + 1, i_Column - 1] == i_OpponentSoldierSign
                                     || m_GameBoard[i_Row + 1, i_Column - 1] == i_OpponentKingSign)
                                    && m_GameBoard[i_Row + 2, i_Column - 2] == ' ';
            }

            return isLeftBottomSkip || isLeftTopSkip;
        }

        private bool rightEdgeRegularMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign)
        {
            bool isMoveAvailable = false;
            bool isLeftBottomMove = m_GameBoard[i_Row + 1, i_Column - 1] == ' ';
            bool isLeftTopMove = m_GameBoard[i_Row - 1, i_Column - 1] == ' ';

            if(i_Column == (int)r_BoardSize - 1)
            {
                if(i_CurrentPieceSign == Checkers.k_BottomPlayerSoldierSign)
                {
                    isMoveAvailable = isLeftTopMove;
                }

                if(i_CurrentPieceSign == Checkers.k_TopPlayerSoldierSign)
                {
                    isMoveAvailable = isLeftBottomMove;
                }
                else
                {
                    isMoveAvailable = isLeftTopMove || isLeftBottomMove;
                }
            }
            else
            {
                isMoveAvailable = middleBoardRegularMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
            }

            return isMoveAvailable;
        }

        private bool middleBoardMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign, bool i_IsSkipToCheck)
        {
            bool isMoveAvailable = false;

            if(i_IsSkipToCheck == true)
            {
                isMoveAvailable = middleBoardSkipMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
            }
            else
            {
                isMoveAvailable = middleBoardRegularMoveCheck(i_CurrentPieceSign, i_Row, i_Column, i_OpponentSoldierSign, i_OpponentKingSign);
            }

            return isMoveAvailable;
        }

        private bool middleBoardSkipMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign)
        {
            bool isRightTopSkip = false;
            bool isRightBottomSkip = false;
            bool isLeftTopSkip = false;
            bool isLeftBottomSkip = false;

            if (i_CurrentPieceSign != Checkers.k_TopPlayerSoldierSign)
            {
                isRightTopSkip = (m_GameBoard[i_Row - 1, i_Column + 1] == i_OpponentSoldierSign
                                   || m_GameBoard[i_Row - 1, i_Column + 1] == i_OpponentKingSign)
                                  && m_GameBoard[i_Row - 2, i_Column + 2] == ' ';
                isLeftTopSkip =
                    (m_GameBoard[i_Row - 1, i_Column - 1] == i_OpponentSoldierSign
                      || m_GameBoard[i_Row - 1, i_Column - 1] == i_OpponentKingSign)
                     && m_GameBoard[i_Row - 2, i_Column - 2] == ' ';
            }

            if (i_CurrentPieceSign != Checkers.k_BottomPlayerSoldierSign)
            {
                isRightBottomSkip = (m_GameBoard[i_Row + 1, i_Column + 1] == i_OpponentSoldierSign
                                      || m_GameBoard[i_Row + 1, i_Column + 1] == i_OpponentKingSign)
                                     && m_GameBoard[i_Row + 2, i_Column + 2] == ' ';
                isLeftBottomSkip =
                    (m_GameBoard[i_Row + 1, i_Column - 1] == i_OpponentSoldierSign
                      || m_GameBoard[i_Row + 1, i_Column - 1] == i_OpponentKingSign)
                     && m_GameBoard[i_Row + 2, i_Column - 2] == ' ';
            }

            return isRightTopSkip || isRightBottomSkip || isLeftTopSkip || isLeftBottomSkip;
        }

        private bool middleBoardRegularMoveCheck(char i_CurrentPieceSign, int i_Row, int i_Column, char i_OpponentSoldierSign, char i_OpponentKingSign)
        {
            bool isMoveAvailable = false;
            bool isLeftBottomMove = m_GameBoard[i_Row + 1, i_Column - 1] == ' ';
            bool isRightBottomMove = m_GameBoard[i_Row + 1, i_Column + 1] == ' ';
            bool isLeftTopMove = m_GameBoard[i_Row - 1, i_Column - 1] == ' ';
            bool isRightTopMove = m_GameBoard[i_Row - 1, i_Column + 1] == ' ';

            if (i_CurrentPieceSign == Checkers.k_BottomPlayerSoldierSign)
            {
                isMoveAvailable = isRightTopMove || isLeftTopMove;
            }

            if (i_CurrentPieceSign == Checkers.k_TopPlayerSoldierSign)
            {
                isMoveAvailable = isRightBottomMove || isLeftBottomMove;
            }
            else
            {
                isMoveAvailable = isRightTopMove || isRightBottomMove || isLeftBottomMove || isLeftTopMove;
            }

            return isMoveAvailable;
        }

        public void BoardInitialize()
        {
            boardFill(0, ((int)r_BoardSize / 2) - 1, Checkers.k_TopPlayerSoldierSign);
            boardFill(((int)r_BoardSize / 2) - 1, ((int)r_BoardSize / 2) + 1, ' ');
            boardFill(((int)r_BoardSize / 2) + 1, (int)r_BoardSize, Checkers.k_BottomPlayerSoldierSign);
        }

        public void UpdateSquare(int i_Row, int i_Column, char i_Sign)
        {
            this.m_GameBoard[i_Row, i_Column] = i_Sign;
        }

        private void boardFill(int i_StartLine, int i_EndLine, char i_PlayerSign)
        {
            for (int i = i_StartLine; i < i_EndLine; i++)
            {
                for (int j = 0; j < (int)r_BoardSize; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 != 0)
                        {
                            m_GameBoard[i, j] = i_PlayerSign;
                        }
                        else
                        {
                            m_GameBoard[i, j] = ' ';
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            m_GameBoard[i, j] = i_PlayerSign;
                        }
                        else
                        {
                            m_GameBoard[i, j] = ' ';
                        }
                    }
                }
            }
        }

        public void PrintBoard()
        {
            char topLetters = 'A';
            char sideLetters = 'a';
            StringBuilder lineSeparator = new StringBuilder(0, (4 * (int)r_BoardSize) + 1);

            buildLineSeparator(ref lineSeparator);

            while (topLetters <= 'A' + (int)r_BoardSize - 1)
            {
                Console.Write("   " + topLetters);
                topLetters++;
            }

            Console.WriteLine("  ");
            Console.WriteLine(lineSeparator);

            for (int i = 0; i < (int)r_BoardSize; i++)
            {
                Console.Write(sideLetters + "|");

                for (int j = 0; j < (int)r_BoardSize; j++)
                {
                    Console.Write(" " + m_GameBoard[i, j] + " |");
                }

                Console.Write(System.Environment.NewLine);
                Console.WriteLine(lineSeparator);
                sideLetters++;
            }
        }
    }
}