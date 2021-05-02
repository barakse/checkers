using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using C20_Ex05;

namespace C20_Ex02
{
    public class Player
    {
        private readonly Random r_MoveSelector = new Random();
        private readonly bool r_IsPlayerPc;
        private readonly string r_PlayerName;
        private int m_CurrentPieces;
        private int m_CurrentPoints;
        private int m_Soldiers;
        private int m_Kings;
        private char m_BelongedSoldierSign;
        private char m_BelongedKingSign;

        public Player(string i_PlayerName, int i_CurrentPieces, bool i_IsPlayerPC, char i_BelongedSoldierSign, char i_BelongedKingSign)
        {
            r_IsPlayerPc = i_IsPlayerPC;
            r_PlayerName = i_PlayerName;
            m_CurrentPoints = 0;
            m_CurrentPieces = i_CurrentPieces;
            m_Soldiers = m_CurrentPieces;
            m_Kings = 0;
            m_BelongedSoldierSign = i_BelongedSoldierSign;
            m_BelongedKingSign = i_BelongedKingSign;
        }

        public char SoldierSign
        {
            get
            {
                return m_BelongedSoldierSign;
            }

            set
            {
                m_BelongedSoldierSign = value;
            }
        }

        public char KingSign
        {
            get
            {
                return m_BelongedKingSign;
            }

            set
            {
                m_BelongedKingSign = value;
            }
        }

        public string Name
        {
            get
            {
                return r_PlayerName;
            }
        }

        public int CurrentPieces
        {
            get
            {
                return m_CurrentPieces;
            }

            set
            {
                m_CurrentPieces = value;
            }
        }

        public int Soldiers
        {
            get
            {
                return m_Soldiers;
            }

            set
            {
                m_Soldiers = value;
            }
        }

        public int Kings
        {
            get
            {
                return m_Kings;
            }

            set
            {
                m_Kings = value;
            }
        }

        public int CurrentPoints
        {
            get
            {
                return m_CurrentPoints;
            }

            set
            {
                m_CurrentPoints = value;
            }
        }

        public bool IsPlayerPC
        {
            get
            {
                return r_IsPlayerPc;
            }
        }

        public void CalculatePoints(int i_OpponentSoldiers, int i_OpponentKings)
        {
            int finalPointsOfCurrentGame = (m_Soldiers + (4 * m_Kings)) - (i_OpponentSoldiers + (4 * i_OpponentKings));

            if (finalPointsOfCurrentGame > 0)
            {
                m_CurrentPoints += finalPointsOfCurrentGame;
            }
        }

        public bool IsSkipMoveAvailable(Board i_Board)
        {
            bool isSkipMovesToCheck = true;
            bool isAnySkipMove = this.CurrentPieces == 0 ? false : i_Board.IsMoveInBoardForPlayer(this, isSkipMovesToCheck);
            return isAnySkipMove;
        }

        public bool IsRegularMoveAvailable(Board i_Board)
        {
            bool isSkipMovesToCheck = true;
            bool isAnyRegularMove = this.CurrentPieces == 0 ? false : i_Board.IsMoveInBoardForPlayer(this, !isSkipMovesToCheck);
            return isAnyRegularMove;
        }

        public void MakeMove(Board i_Board, ref Piece io_Piece, int i_RowToMove, int i_ColumnToMove, Player i_Opponent)
        {
            i_Board.UpdateSquare(io_Piece.Row, io_Piece.Column, ' ');

            switch(io_Piece.PieceSign)
            {
                case Checkers.k_BottomPlayerSoldierSign:
                    {
                        makeMoveForX(i_Board, ref io_Piece, i_RowToMove, i_ColumnToMove, i_Opponent);
                        break;
                    }

                case Checkers.k_TopPlayerSoldierSign:
                    {
                        makeMoveForO(i_Board, ref io_Piece, i_RowToMove, i_ColumnToMove, i_Opponent);
                        break;
                    }

                case Checkers.k_BottomPlayerKingSign:
                    {
                        makeMoveForK(i_Board, ref io_Piece, i_RowToMove, i_ColumnToMove, i_Opponent);
                        break;
                    }

                case Checkers.k_TopPlayerKingSign:
                    {
                        makeMoveForU(i_Board, ref io_Piece, i_RowToMove, i_ColumnToMove, i_Opponent);
                        break;
                    }
            }
        }

        private void makeMoveForX(Board i_Board, ref Piece io_Piece, int i_RowToMove, int i_ColumnToMove, Player i_Opponent)
        {
            if (io_Piece.Row - 2 == i_RowToMove)
            {
                int rowOfEatenPiece = (io_Piece.Row + i_RowToMove) / 2;
                int columnOfEatenPiece = (io_Piece.Column + i_ColumnToMove) / 2;

                i_Opponent.CurrentPieces -= 1;

                if(i_Board.GameBoard[rowOfEatenPiece, columnOfEatenPiece] == Checkers.k_TopPlayerSoldierSign)
                {
                    i_Opponent.Soldiers -= 1;
                }
                else
                {
                    i_Opponent.Kings -= 1;
                }

                i_Board.UpdateSquare(rowOfEatenPiece, columnOfEatenPiece, ' ');
            }

            if (i_RowToMove == 0)
            {
                io_Piece.UpdatePiece(i_RowToMove, i_ColumnToMove, Checkers.k_BottomPlayerKingSign);
                i_Board.UpdateSquare(i_RowToMove, i_ColumnToMove, Checkers.k_BottomPlayerKingSign);
                this.Kings += 1;
                this.Soldiers -= 1;
            }
            else
            {
                io_Piece.UpdatePiece(i_RowToMove, i_ColumnToMove, Checkers.k_BottomPlayerSoldierSign);
                i_Board.UpdateSquare(i_RowToMove, i_ColumnToMove, Checkers.k_BottomPlayerSoldierSign);
            }
        }

        private void makeMoveForO(Board i_Board, ref Piece io_Piece, int i_RowToMove, int i_ColumnToMove, Player i_Opponent)
        {
            if (io_Piece.Row == i_RowToMove - 2)
            {
                int rowOfEatenPiece = (io_Piece.Row + i_RowToMove) / 2;
                int columnOfEatenPiece = (io_Piece.Column + i_ColumnToMove) / 2;

                i_Opponent.CurrentPieces -= 1;

                if (i_Board.GameBoard[rowOfEatenPiece, columnOfEatenPiece] == Checkers.k_BottomPlayerSoldierSign)
                {
                    i_Opponent.Soldiers -= 1;
                }
                else
                {
                    i_Opponent.Kings -= 1;
                }

                i_Board.UpdateSquare(rowOfEatenPiece, columnOfEatenPiece, ' ');
            }

            if (i_RowToMove == (int)i_Board.Size - 1)
            {
                io_Piece.UpdatePiece(i_RowToMove, i_ColumnToMove, Checkers.k_TopPlayerKingSign);
                i_Board.UpdateSquare(i_RowToMove, i_ColumnToMove, Checkers.k_TopPlayerKingSign);
                this.Kings += 1;
                this.Soldiers -= 1;
            }
            else
            {
                io_Piece.UpdatePiece(i_RowToMove, i_ColumnToMove, Checkers.k_TopPlayerSoldierSign);
                i_Board.UpdateSquare(i_RowToMove, i_ColumnToMove, Checkers.k_TopPlayerSoldierSign);
            }
        }

        private void makeMoveForK(Board i_Board, ref Piece io_Piece, int i_RowToMove, int i_ColumnToMove, Player i_Opponent)
        {
            if (io_Piece.Row - 2 == i_RowToMove || io_Piece.Row == i_RowToMove - 2)
            {
                int rowOfEatenPiece = (io_Piece.Row + i_RowToMove) / 2;
                int columnOfEatenPiece = (io_Piece.Column + i_ColumnToMove) / 2;

                i_Opponent.CurrentPieces -= 1;

                if (i_Board.GameBoard[rowOfEatenPiece, columnOfEatenPiece] == Checkers.k_TopPlayerSoldierSign)
                {
                    i_Opponent.Soldiers -= 1;
                }
                else
                {
                    i_Opponent.Kings -= 1;
                }

                i_Board.UpdateSquare(rowOfEatenPiece, columnOfEatenPiece, ' ');
            }

            io_Piece.UpdatePiece(i_RowToMove, i_ColumnToMove, Checkers.k_BottomPlayerKingSign);
            i_Board.UpdateSquare(i_RowToMove, i_ColumnToMove, Checkers.k_BottomPlayerKingSign);
        }

        private void makeMoveForU(Board i_Board, ref Piece io_Piece, int i_RowToMove, int i_ColumnToMove, Player i_Opponent)
        {
            if (io_Piece.Row - 2 == i_RowToMove || io_Piece.Row == i_RowToMove - 2)
            {
                int rowOfEatenPiece = (io_Piece.Row + i_RowToMove) / 2;
                int columnOfEatenPiece = (io_Piece.Column + i_ColumnToMove) / 2;

                i_Opponent.CurrentPieces -= 1;

                if (i_Board.GameBoard[rowOfEatenPiece, columnOfEatenPiece] == Checkers.k_BottomPlayerSoldierSign)
                {
                    i_Opponent.Soldiers -= 1;
                }
                else
                {
                    i_Opponent.Kings -= 1;
                }

                i_Board.UpdateSquare(rowOfEatenPiece, columnOfEatenPiece, ' ');
            }

            io_Piece.UpdatePiece(i_RowToMove, i_ColumnToMove, Checkers.k_TopPlayerKingSign);
            i_Board.UpdateSquare(i_RowToMove, i_ColumnToMove, Checkers.k_TopPlayerKingSign);
        }

        public void MakeComputerMove(ComputerOptimalMoves i_ComputerMoves, Board i_Board, Player i_Opponent, CheckersForm i_CheckersForm)
        {
            if(i_ComputerMoves.OptimalSkipMoves.Count != 0)
            {
                makeComputerSkipMove(i_ComputerMoves, i_Board, i_Opponent, i_CheckersForm);
            }
            else if (i_ComputerMoves.AllOptimalMoves.Count != 0)
            {
                makeComputerRegularMove(i_ComputerMoves, i_Board, i_Opponent, i_CheckersForm);
            }
        }

        private async void makeComputerSkipMove(ComputerOptimalMoves i_ComputerMoves, Board i_Board, Player i_Opponent, CheckersForm i_CheckersForm)
        {
            int chosenMoveIndex = r_MoveSelector.Next(0, i_ComputerMoves.OptimalSkipMoves.Count - 1);
            LinkedListNode<PositionNode> currentMovePosition = i_ComputerMoves.OptimalSkipMoves[chosenMoveIndex].First;
            Piece currentSourcePiece = null;

            while (currentMovePosition.Next != null)
            {
                await Task.Delay(750);
                int currentRowPosition = currentMovePosition.Value.Row;
                int currentColumnPosition = currentMovePosition.Value.Column;
                int nextRowPosition = currentMovePosition.Next.Value.Row;
                int nextColumnPosition = currentMovePosition.Next.Value.Column;
                char currentPositionPieceSign = i_Board.GameBoard[currentRowPosition, currentColumnPosition];
                currentSourcePiece = new Piece(
                    currentRowPosition,
                    currentColumnPosition,
                    currentPositionPieceSign);

                this.MakeMove(i_Board, ref currentSourcePiece, nextRowPosition, nextColumnPosition, i_Opponent);
                i_CheckersForm.OnBoardChanged();
                currentMovePosition = currentMovePosition.Next;
            }

            bool isSkipToCheck = true;
            bool isSkipMovesStillExistInCurrentPiece = i_Board.IsMoveAvailableForPieceInBoard(
                i_Board.GameBoard[currentSourcePiece.Row, currentSourcePiece.Column],
                currentSourcePiece.Row,
                currentSourcePiece.Column,
                Checkers.k_BottomPlayerSoldierSign,
                Checkers.k_BottomPlayerKingSign,
                isSkipToCheck);

            if(isSkipMovesStillExistInCurrentPiece == true)
            {
                i_ComputerMoves = new ComputerOptimalMoves();
                i_ComputerMoves.findOptimalMoveForPiece(currentSourcePiece, i_Board);
                i_ComputerMoves.extractOptimalSkipMoves();
                makeComputerSkipMove(i_ComputerMoves, i_Board, i_Opponent, i_CheckersForm);
            }
            else
            {
                i_CheckersForm.OnComputerTurnEnd(this, i_Opponent);
            }
        }

        private async void makeComputerRegularMove(ComputerOptimalMoves i_ComputerMoves, Board i_Board, Player i_Opponent, CheckersForm i_CheckersForm)
        {
            int chosenMoveIndex = r_MoveSelector.Next(0, i_ComputerMoves.AllOptimalMoves.Count - 1);
            LinkedListNode<PositionNode> currentMovePosition = i_ComputerMoves.AllOptimalMoves[chosenMoveIndex].First;

            while(currentMovePosition.Next != null)
            {
                await Task.Delay(750);
                int currentRowPosition = currentMovePosition.Value.Row;
                int currentColumnPosition = currentMovePosition.Value.Column;
                int nextRowPosition = currentMovePosition.Next.Value.Row;
                int nextColumnPosition = currentMovePosition.Next.Value.Column;
                char currentPositionPieceSign = i_Board.GameBoard[currentRowPosition, currentColumnPosition];
                Piece currentSourcePiece = new Piece(
                    currentRowPosition,
                    currentColumnPosition,
                    currentPositionPieceSign);

                this.MakeMove(i_Board, ref currentSourcePiece, nextRowPosition, nextColumnPosition, i_Opponent);
                i_CheckersForm.OnBoardChanged();
                currentMovePosition = currentMovePosition.Next;
            }

            i_CheckersForm.OnComputerTurnEnd(this, i_Opponent);
        }
    }
}