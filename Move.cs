using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace C20_Ex02
{
    public class Move
    {
        private int m_DestinationRow;
        private int m_DestinationColumn;
        private bool m_isSkip;
        private StringBuilder m_MoveString;
        private Piece m_SourcePiece;

        public Move(Piece i_SourcePiece, int i_RowDestination, int i_ColumnDestination)
        {
            m_SourcePiece = new Piece(i_SourcePiece.Row, i_SourcePiece.Column, i_SourcePiece.PieceSign);
            m_DestinationRow = i_RowDestination;
            m_DestinationColumn = i_ColumnDestination;
            m_isSkip = IsSkipMove();
            m_MoveString = new StringBuilder();
            m_MoveString.Append((char)('A' + m_SourcePiece.Column));
            m_MoveString.Append((char)('a' + m_SourcePiece.Row));
            m_MoveString.Append('>');
            m_MoveString.Append((char)('A' + m_DestinationColumn));
            m_MoveString.Append((char)('a' + m_DestinationRow));
        }

        public int DestinationRow
        {
            get
            {
                return m_DestinationRow;
            }
        }

        public int DestinationColumn
        {
            get
            {
                return m_DestinationColumn;
            }
        }

        public bool isSkip
        {
            get
            {
                return m_isSkip;
            }

            set
            {
                m_isSkip = value;
            }
        }

        public StringBuilder MoveString
        {
            get
            {
                return m_MoveString;
            }
        }

        public Piece SourcePiece
        {
            get
            {
                return m_SourcePiece;
            }

            set
            {
                m_SourcePiece = value;
            }
        }

        private static bool isLegalKingMove(Piece i_ChosenPiece, int i_DestinationRow, int i_DestinationColumn, Board i_Board)
        {
            bool isLegalMove = true;
            int moveRowDistance = Math.Abs(i_ChosenPiece.Row - i_DestinationRow);
            int moveColumnDistance = Math.Abs(i_ChosenPiece.Column - i_DestinationColumn);
            if (moveRowDistance == 2 && moveColumnDistance == 2)
            {
                if (i_Board.GameBoard[i_DestinationRow, i_DestinationColumn] != ' ')
                {
                    isLegalMove = false;
                }
                else
                {
                    int eatenPieceRow = (i_ChosenPiece.Row + i_DestinationRow) / 2;
                    int eatenPieceColumn = (i_ChosenPiece.Column + i_DestinationColumn) / 2;

                    if (i_ChosenPiece.PieceSign == Checkers.k_BottomPlayerKingSign)
                    {
                        isLegalMove =
                            i_Board.GameBoard[eatenPieceRow, eatenPieceColumn] == Checkers.k_TopPlayerKingSign
                            || i_Board.GameBoard[eatenPieceRow, eatenPieceColumn] == Checkers.k_TopPlayerSoldierSign;
                    }
                    else
                    {
                        isLegalMove =
                            i_Board.GameBoard[eatenPieceRow, eatenPieceColumn] == Checkers.k_BottomPlayerKingSign
                            || i_Board.GameBoard[eatenPieceRow, eatenPieceColumn] == Checkers.k_BottomPlayerSoldierSign;
                    }
                }
            }
            else if (moveRowDistance == 1 && moveColumnDistance == 1)
            {
                if (i_Board.GameBoard[i_DestinationRow, i_DestinationColumn] != ' ')
                {
                    isLegalMove = false;
                }
            }
            else
            {
                isLegalMove = false;
            }

            return isLegalMove;
        }

        private static bool isLegalBottomSoldierMove(Piece i_ChosenPiece, int i_DestinationRow, int i_DestinationColumn, Board i_Board)
        {
            bool isLegalMove = true;
            int moveRowDistance = i_ChosenPiece.Row - i_DestinationRow;
            int moveColumnDistance = Math.Abs(i_ChosenPiece.Column - i_DestinationColumn);

            if (moveRowDistance == 2 && moveColumnDistance == 2)
            {
                if (i_Board.GameBoard[i_DestinationRow, i_DestinationColumn] != ' ')
                {
                    isLegalMove = false;
                }
                else
                {
                    int eatenPieceRow = (i_ChosenPiece.Row + i_DestinationRow) / 2;
                    int eatenPieceColumn = (i_ChosenPiece.Column + i_DestinationColumn) / 2;

                    isLegalMove =
                        i_Board.GameBoard[eatenPieceRow, eatenPieceColumn] == Checkers.k_TopPlayerKingSign
                        || i_Board.GameBoard[eatenPieceRow, eatenPieceColumn] == Checkers.k_TopPlayerSoldierSign;
                }
            }
            else if (moveRowDistance == 1 && moveColumnDistance == 1)
            {
                if (i_Board.GameBoard[i_DestinationRow, i_DestinationColumn] != ' ')
                {
                    isLegalMove = false;
                }
            }
            else
            {
                isLegalMove = false;
            }

            return isLegalMove;
        }

        private static bool isLegalTopSoldierMove(Piece i_ChosenPiece, int i_DestinationRow, int i_DestinationColumn, Board i_Board)
        {
            bool isLegalMove = true;
            int moveRowDistance = i_DestinationRow - i_ChosenPiece.Row;
            int moveColumnDistance = Math.Abs(i_ChosenPiece.Column - i_DestinationColumn);

            if (moveRowDistance == 2 && moveColumnDistance == 2)
            {
                if (i_Board.GameBoard[i_DestinationRow, i_DestinationColumn] != ' ')
                {
                    isLegalMove = false;
                }
                else
                {
                    int eatenPieceRow = (i_ChosenPiece.Row + i_DestinationRow) / 2;
                    int eatenPieceColumn = (i_ChosenPiece.Column + i_DestinationColumn) / 2;

                    isLegalMove =
                        i_Board.GameBoard[eatenPieceRow, eatenPieceColumn] == Checkers.k_BottomPlayerKingSign
                        || i_Board.GameBoard[eatenPieceRow, eatenPieceColumn] == Checkers.k_BottomPlayerSoldierSign;
                }
            }
            else if (moveRowDistance == 1 && moveColumnDistance == 1)
            {
                if (i_Board.GameBoard[i_DestinationRow, i_DestinationColumn] != ' ')
                {
                    isLegalMove = false;
                }
            }
            else
            {
                isLegalMove = false;
            }

            return isLegalMove;
        }

        public static bool IsLegalMove(Piece i_ChosenPiece, int i_DestinationRow, int i_DestinationColumn, Board i_Board)
        {
            bool isLegalMove = false;
            if (i_ChosenPiece.PieceSign == Checkers.k_BottomPlayerKingSign
               || i_ChosenPiece.PieceSign == Checkers.k_TopPlayerKingSign)
            {
                isLegalMove = isLegalKingMove(i_ChosenPiece, i_DestinationRow, i_DestinationColumn, i_Board);
            }
            else if (i_ChosenPiece.PieceSign == Checkers.k_BottomPlayerSoldierSign)
            {
                isLegalMove = isLegalBottomSoldierMove(i_ChosenPiece, i_DestinationRow, i_DestinationColumn, i_Board);
            }
            else
            {
                isLegalMove = isLegalTopSoldierMove(i_ChosenPiece, i_DestinationRow, i_DestinationColumn, i_Board);
            }

            return isLegalMove;
        }

        public bool IsSkipMove()
        {
            int moveRowDistance = Math.Abs(this.SourcePiece.Row - this.DestinationRow);
            int moveColumnDistance = Math.Abs(this.SourcePiece.Column - this.DestinationColumn);
            return moveRowDistance == 2 && moveColumnDistance == 2;
        }
    }
}
