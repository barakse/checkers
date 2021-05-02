using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;

namespace C20_Ex02
{
    public class ComputerOptimalMoves
    {
        private List<LinkedList<PositionNode>> m_AllOptimalMoves;
        private List<LinkedList<PositionNode>> m_OptimalSkipMoves;

        public ComputerOptimalMoves()
        {
            m_AllOptimalMoves = new List<LinkedList<PositionNode>>();
            m_OptimalSkipMoves = new List<LinkedList<PositionNode>>();
        }

        public List<LinkedList<PositionNode>> AllOptimalMoves
        {
            get
            {
                return m_AllOptimalMoves;
            }
        }

        public List<LinkedList<PositionNode>> OptimalSkipMoves
        {
            get
            {
                return m_OptimalSkipMoves;
            }
        }

        private static MovesTree findComputerSourcePieceMoves(Board i_Board, Piece i_SourcePiece, bool i_IsKing)
        {
            MovesTree movesTree = new MovesTree();
            PositionNode movesTreeRoot = movesTree.Root;
            int totalSkips = 0;

            if(!i_IsKing)
            {
                createComputerPieceTopMoves(
                    ref movesTreeRoot,
                    i_Board,
                    totalSkips,
                    i_SourcePiece.Row,
                    i_SourcePiece.Column);
            }
            else
            {
                createComputerPieceBottomMoves(
                    ref movesTreeRoot,
                    i_Board,
                    totalSkips,
                    i_SourcePiece.Row,
                    i_SourcePiece.Column);
            }

            movesTree.Root = movesTreeRoot;

            return movesTree;
        }

        private static List<LinkedList<PositionNode>> findPieceOptimalMoves(MovesTree i_MovesTree)
        {
            List<LinkedList<PositionNode>> optimalMovesList = new List<LinkedList<PositionNode>>();
            List<LinkedList<PositionNode>> longestMoves = new List<LinkedList<PositionNode>>();
            LinkedList<PositionNode> currentLongestMove = new LinkedList<PositionNode>();
            int maxSkipMoves = findMaxSkipsOfPieceInMovesTree(i_MovesTree.Root);

            getAllLongestMovesInMovesTree(ref longestMoves, i_MovesTree.Root, currentLongestMove);

            foreach(LinkedList<PositionNode> currentMove in longestMoves)
            {
                if(currentMove.Last.Value.SkipsSoFar == maxSkipMoves)
                {
                    optimalMovesList.Add(currentMove);
                }
            }

            return optimalMovesList;
        }

        private static int findMaxSkipsOfPieceInMovesTree(PositionNode i_MovesTreeRoot)
        {
            int maxSkips = i_MovesTreeRoot.SkipsSoFar;

            if (i_MovesTreeRoot.Left != null)
            {
                maxSkips = Math.Max(maxSkips, findMaxSkipsOfPieceInMovesTree(i_MovesTreeRoot.Left));
            }

            if (i_MovesTreeRoot.Right != null)
            {
                maxSkips = Math.Max(maxSkips, findMaxSkipsOfPieceInMovesTree(i_MovesTreeRoot.Right));
            }

            return maxSkips;
        }

        private static void getAllLongestMovesInMovesTree(
            ref List<LinkedList<PositionNode>> io_LongestMoves,
            PositionNode i_MovesTreeRoot,
            LinkedList<PositionNode> i_CurrentLongestMove)
        {
            if (i_MovesTreeRoot != null)
            {
                i_CurrentLongestMove.AddLast(i_MovesTreeRoot);

                if (i_MovesTreeRoot.Left == null && i_MovesTreeRoot.Right == null)
                {
                    io_LongestMoves.Add(i_CurrentLongestMove);
                }
                else
                {
                    getAllLongestMovesInMovesTree(ref io_LongestMoves, i_MovesTreeRoot.Left, new LinkedList<PositionNode>(i_CurrentLongestMove));
                    getAllLongestMovesInMovesTree(ref io_LongestMoves, i_MovesTreeRoot.Right, new LinkedList<PositionNode>(i_CurrentLongestMove));
                }
            }

            return;
        }

        private static void createComputerPieceTopMoves(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            int i_TotalSkips,
            int i_CurrentPieceRow,
            int i_CurrentPieceColumn)
        {
            io_MovesTreeRoot = MovesTree.CreateMovesTreeNode(
                i_CurrentPieceRow,
                i_CurrentPieceColumn,
                i_TotalSkips,
                null,
                null);

            if(i_CurrentPieceRow != (int)i_Board.Size - 1)
            {
                if(i_CurrentPieceRow < (int)i_Board.Size - 2)
                {
                    if(i_CurrentPieceRow % 2 == 0)
                    {
                        findTopMovesInEvenRows(
                                ref io_MovesTreeRoot,
                                i_Board,
                                ref i_TotalSkips,
                                ref i_CurrentPieceRow,
                                ref i_CurrentPieceColumn);
                    }
                    else
                    {
                        findTopMovesInOddRows(
                            ref io_MovesTreeRoot,
                            i_Board,
                            ref i_TotalSkips,
                            ref i_CurrentPieceRow,
                            ref i_CurrentPieceColumn);
                    }
                }
                else
                {
                    findTopMovesInBottomEdge(
                        ref io_MovesTreeRoot,
                        i_Board,
                        ref i_TotalSkips,
                        ref i_CurrentPieceRow,
                        ref i_CurrentPieceColumn);
                }
            }

            return;
        }

        private static void createComputerPieceBottomMoves(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            int i_TotalSkips,
            int i_CurrentPieceRow,
            int i_CurrentPieceColumn)
        {
            io_MovesTreeRoot = MovesTree.CreateMovesTreeNode(
                i_CurrentPieceRow,
                i_CurrentPieceColumn,
                i_TotalSkips,
                null,
                null);

            if (i_CurrentPieceRow != 0)
            {
                if (i_CurrentPieceRow > 1)
                {
                    if (i_CurrentPieceRow % 2 == 0)
                    {
                        findBottomMovesInEvenRows(
                            ref io_MovesTreeRoot,
                            i_Board,
                            ref i_TotalSkips,
                            ref i_CurrentPieceRow,
                            ref i_CurrentPieceColumn);
                    }
                    else
                    {
                        findBottomMovesInOddRows(
                            ref io_MovesTreeRoot,
                            i_Board,
                            ref i_TotalSkips,
                            ref i_CurrentPieceRow,
                            ref i_CurrentPieceColumn);
                    }
                }
                else
                {
                    findBottomMovesInTopEdge(
                        ref io_MovesTreeRoot,
                        i_Board,
                        ref i_TotalSkips,
                        ref i_CurrentPieceRow,
                        ref i_CurrentPieceColumn);
                }
            }

            return;
        }

        private static void findTopMovesInEvenRows(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            if (io_CurrentPieceColumn == (int)i_Board.Size - 1)
            {
                findTopMovesInRightCornerOfEvenRow(
                    ref io_MovesTreeRoot,
                    i_Board,
                    ref io_TotalSkips,
                    ref io_CurrentPieceRow,
                    ref io_CurrentPieceColumn);
            }
            else if (io_CurrentPieceColumn == 1)
            {
                findTopMovesInLeftCornerOfEvenRow(
                    ref io_MovesTreeRoot,
                    i_Board,
                    ref io_TotalSkips,
                    ref io_CurrentPieceRow,
                    ref io_CurrentPieceColumn);
            }
            else
            {
                findTopMovesInMiddle(
                    ref io_MovesTreeRoot,
                    i_Board,
                    ref io_TotalSkips,
                    ref io_CurrentPieceRow,
                    ref io_CurrentPieceColumn);
            }
        }

        private static void findTopMovesInOddRows(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            if (io_CurrentPieceColumn == 0)
            {
                findTopMovesInLeftCornerOfOddRow(
                    ref io_MovesTreeRoot,
                    i_Board,
                    ref io_TotalSkips,
                    ref io_CurrentPieceRow,
                    ref io_CurrentPieceColumn);
            }
            else if (io_CurrentPieceColumn == (int)i_Board.Size - 2)
            {
                findTopMovesInRightCornerOfOddRow(
                    ref io_MovesTreeRoot,
                    i_Board,
                    ref io_TotalSkips,
                    ref io_CurrentPieceRow,
                    ref io_CurrentPieceColumn);
            }
            else
            {
                findTopMovesInMiddle(
                    ref io_MovesTreeRoot,
                    i_Board,
                    ref io_TotalSkips,
                    ref io_CurrentPieceRow,
                    ref io_CurrentPieceColumn);
            }
        }

        private static void findBottomMovesInEvenRows(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            if (io_CurrentPieceColumn == (int)i_Board.Size - 1)
            {
                findBottomMovesInRightCornerOfEvenRow(
                    ref io_MovesTreeRoot,
                    i_Board,
                    ref io_TotalSkips,
                    ref io_CurrentPieceRow,
                    ref io_CurrentPieceColumn);
            }
            else if (io_CurrentPieceColumn == 1)
            {
                findBottomMovesInLeftCornerOfEvenRow(
                    ref io_MovesTreeRoot,
                    i_Board,
                    ref io_TotalSkips,
                    ref io_CurrentPieceRow,
                    ref io_CurrentPieceColumn);
            }
            else
            {
                findBottomMovesInMiddle(
                    ref io_MovesTreeRoot,
                    i_Board,
                    ref io_TotalSkips,
                    ref io_CurrentPieceRow,
                    ref io_CurrentPieceColumn);
            }
        }

        private static void findBottomMovesInOddRows(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            if (io_CurrentPieceColumn == 0)
            {
                findBottomMovesInLeftCornerOfOddRow(
                    ref io_MovesTreeRoot,
                    i_Board,
                    ref io_TotalSkips,
                    ref io_CurrentPieceRow,
                    ref io_CurrentPieceColumn);
            }
            else if (io_CurrentPieceColumn == (int)i_Board.Size - 2)
            {
                findBottomMovesInRightCornerOfOddRow(
                    ref io_MovesTreeRoot,
                    i_Board,
                    ref io_TotalSkips,
                    ref io_CurrentPieceRow,
                    ref io_CurrentPieceColumn);
            }
            else
            {
                findBottomMovesInMiddle(
                    ref io_MovesTreeRoot,
                    i_Board,
                    ref io_TotalSkips,
                    ref io_CurrentPieceRow,
                    ref io_CurrentPieceColumn);
            }
        }

        private static void findTopMovesInRightCornerOfEvenRow(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            PositionNode leftMoveNode = io_MovesTreeRoot.Left;
            int tempTotalSkips = io_TotalSkips;
            int tempRow = io_CurrentPieceRow;
            int tempColumn = io_CurrentPieceColumn;

            bool isOpponentExistInLeft =
                i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1] == Checkers.k_BottomPlayerSoldierSign
                || i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1]
                == Checkers.k_BottomPlayerKingSign;

            if(i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1] == ' ')
            {
                if(io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Left = new PositionNode(io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1);
                }
            }
            else if(isOpponentExistInLeft == true)
            {
                if(i_Board.GameBoard[io_CurrentPieceRow + 2, io_CurrentPieceColumn - 2] == ' ')
                {
                    createComputerPieceTopMoves(
                        ref leftMoveNode,
                        i_Board,
                        io_TotalSkips + 1,
                        io_CurrentPieceRow + 2,
                        io_CurrentPieceColumn - 2);

                    io_MovesTreeRoot.Left = leftMoveNode;
                    io_TotalSkips = tempTotalSkips;
                    io_CurrentPieceColumn = tempColumn;
                    io_CurrentPieceRow = tempRow;
                }
            }
        }

        private static void findTopMovesInLeftCornerOfEvenRow(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            PositionNode rightMoveNode = io_MovesTreeRoot.Right;
            int tempTotalSkips = io_TotalSkips;
            int tempRow = io_CurrentPieceRow;
            int tempColumn = io_CurrentPieceColumn;
            bool isOpponentExistInRight =
                i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1] == Checkers.k_BottomPlayerSoldierSign
                || i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1]
                == Checkers.k_BottomPlayerKingSign;

            if(i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1] == ' ')
            {
                if(io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Left = new PositionNode(io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1);
                }
            }

            if(i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1] == ' ')
            {
                if(io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Right = new PositionNode(io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1);
                }
            }

            if(isOpponentExistInRight == true)
            {
                if(i_Board.GameBoard[io_CurrentPieceRow + 2, io_CurrentPieceColumn + 2] == ' ')
                {
                    createComputerPieceTopMoves(
                        ref rightMoveNode,
                        i_Board,
                        io_TotalSkips + 1,
                        io_CurrentPieceRow + 2,
                        io_CurrentPieceColumn + 2);
                    io_MovesTreeRoot.Right = rightMoveNode;
                    io_TotalSkips = tempTotalSkips;
                    io_CurrentPieceColumn = tempColumn;
                    io_CurrentPieceRow = tempRow;
                }
            }
        }

        private static void findTopMovesInRightCornerOfOddRow(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            PositionNode leftMoveNode = io_MovesTreeRoot.Left;
            int tempTotalSkips = io_TotalSkips;
            int tempRow = io_CurrentPieceRow;
            int tempColumn = io_CurrentPieceColumn;

            bool isOpponentExistInLeft =
                i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1] == Checkers.k_BottomPlayerSoldierSign
                || i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1]
                == Checkers.k_BottomPlayerKingSign;

            if(i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1] == ' ')
            {
                if(io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Left = new PositionNode(io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1);
                }
            }

            if(i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1] == ' ')
            {
                if(io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Right = new PositionNode(io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1);
                }
            }

            if(isOpponentExistInLeft == true)
            {
                if(i_Board.GameBoard[io_CurrentPieceRow + 2, io_CurrentPieceColumn - 2] == ' ')
                {
                    createComputerPieceTopMoves(
                        ref leftMoveNode,
                        i_Board,
                        io_TotalSkips + 1,
                        io_CurrentPieceRow + 2,
                        io_CurrentPieceColumn - 2);
                    io_MovesTreeRoot.Left = leftMoveNode;
                    io_TotalSkips = tempTotalSkips;
                    io_CurrentPieceColumn = tempColumn;
                    io_CurrentPieceRow = tempRow;
                }
            }
        }

        private static void findTopMovesInLeftCornerOfOddRow(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            PositionNode rightMoveNode = io_MovesTreeRoot.Right;
            int tempTotalSkips = io_TotalSkips;
            int tempRow = io_CurrentPieceRow;
            int tempColumn = io_CurrentPieceColumn;
            bool isOpponentExistInRight =
                i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1] == Checkers.k_BottomPlayerSoldierSign
                || i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1]
                == Checkers.k_BottomPlayerKingSign;

            if(i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1] == ' ')
            {
                if(io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Right = new PositionNode(io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1);
                }
            }
            else if(isOpponentExistInRight == true)
            {
                if(i_Board.GameBoard[io_CurrentPieceRow + 2, io_CurrentPieceColumn + 2] == ' ')
                {
                    createComputerPieceTopMoves(
                        ref rightMoveNode,
                        i_Board,
                        io_TotalSkips + 1,
                        io_CurrentPieceRow + 2,
                        io_CurrentPieceColumn + 2);
                    io_MovesTreeRoot.Right = rightMoveNode;
                    io_TotalSkips = tempTotalSkips;
                    io_CurrentPieceColumn = tempColumn;
                    io_CurrentPieceRow = tempRow;
                }
            }
        }

        private static void findTopMovesInMiddle(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            PositionNode rightMoveNode = io_MovesTreeRoot.Right;
            PositionNode leftMoveNode = io_MovesTreeRoot.Left;
            int tempTotalSkips = io_TotalSkips;
            int tempRow = io_CurrentPieceRow;
            int tempColumn = io_CurrentPieceColumn;

            bool isOpponentExistInRight =
                i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1] == Checkers.k_BottomPlayerSoldierSign
                || i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1]
                == Checkers.k_BottomPlayerKingSign;

            bool isOpponentExistInLeft =
                i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1] == Checkers.k_BottomPlayerSoldierSign
                || i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1]
                == Checkers.k_BottomPlayerKingSign;

            if(i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1] == ' ')
            {
                if(io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Left = new PositionNode(io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1);
                }
            }

            if(i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1] == ' ')
            {
                if(io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Right = new PositionNode(io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1);
                }
            }

            if(isOpponentExistInLeft == true)
            {
                if(i_Board.GameBoard[io_CurrentPieceRow + 2, io_CurrentPieceColumn - 2] == ' ')
                {
                    createComputerPieceTopMoves(
                        ref leftMoveNode,
                        i_Board,
                        io_TotalSkips + 1,
                        io_CurrentPieceRow + 2,
                        io_CurrentPieceColumn - 2);
                    io_MovesTreeRoot.Left = leftMoveNode;
                    io_TotalSkips = tempTotalSkips;
                    io_CurrentPieceColumn = tempColumn;
                    io_CurrentPieceRow = tempRow;
                }
            }

            if(isOpponentExistInRight == true)
            {
                if(i_Board.GameBoard[io_CurrentPieceRow + 2, io_CurrentPieceColumn + 2] == ' ')
                {
                    createComputerPieceTopMoves(
                        ref rightMoveNode,
                        i_Board,
                        io_TotalSkips + 1,
                        io_CurrentPieceRow + 2,
                        io_CurrentPieceColumn + 2);
                    io_MovesTreeRoot.Right = rightMoveNode;
                    io_TotalSkips = tempTotalSkips;
                    io_CurrentPieceColumn = tempColumn;
                    io_CurrentPieceRow = tempRow;
                }
            }
        }

        private static void findTopMovesInBottomEdge(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            if (i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1] == ' ')
            {
                if (io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Left = new PositionNode(io_CurrentPieceRow + 1, io_CurrentPieceColumn - 1);
                }
            }

            if (io_CurrentPieceColumn != (int)i_Board.Size - 1)
            {
                if (i_Board.GameBoard[io_CurrentPieceRow + 1, io_CurrentPieceColumn + 1] == ' ')
                {
                    if (io_TotalSkips == 0)
                    {
                        io_MovesTreeRoot.Right = new PositionNode(
                            io_CurrentPieceRow + 1,
                            io_CurrentPieceColumn + 1);
                    }
                }
            }
        }

        private static void findBottomMovesInRightCornerOfEvenRow(
                   ref PositionNode io_MovesTreeRoot,
                   Board i_Board,
                   ref int io_TotalSkips,
                   ref int io_CurrentPieceRow,
                   ref int io_CurrentPieceColumn)
        {
            PositionNode leftMoveNode = io_MovesTreeRoot.Left;
            int tempTotalSkips = io_TotalSkips;
            int tempRow = io_CurrentPieceRow;
            int tempColumn = io_CurrentPieceColumn;

            bool isOpponentExistInLeft =
                i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1] == Checkers.k_BottomPlayerSoldierSign
                || i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1]
                == Checkers.k_BottomPlayerKingSign;

            if (i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1] == ' ')
            {
                if (io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Left = new PositionNode(io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1);
                }
            }
            else if (isOpponentExistInLeft == true)
            {
                if (i_Board.GameBoard[io_CurrentPieceRow - 2, io_CurrentPieceColumn - 2] == ' ')
                {
                    createComputerPieceBottomMoves(
                        ref leftMoveNode,
                        i_Board,
                        io_TotalSkips + 1,
                        io_CurrentPieceRow - 2,
                        io_CurrentPieceColumn - 2);
                    io_MovesTreeRoot.Left = leftMoveNode;
                    io_TotalSkips = tempTotalSkips;
                    io_CurrentPieceColumn = tempColumn;
                    io_CurrentPieceRow = tempRow;
                }
            }
        }

        private static void findBottomMovesInLeftCornerOfEvenRow(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            PositionNode rightMoveNode = io_MovesTreeRoot.Right;
            int tempTotalSkips = io_TotalSkips;
            int tempRow = io_CurrentPieceRow;
            int tempColumn = io_CurrentPieceColumn;
            bool isOpponentExistInRight =
                i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1] == Checkers.k_BottomPlayerSoldierSign
                || i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1]
                == Checkers.k_BottomPlayerKingSign;

            if (i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1] == ' ')
            {
                if (io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Left = new PositionNode(io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1);
                }
            }

            if (i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1] == ' ')
            {
                if (io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Right = new PositionNode(io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1);
                }
            }

            if (isOpponentExistInRight == true)
            {
                if(i_Board.GameBoard[io_CurrentPieceRow - 2, io_CurrentPieceColumn + 2] == ' ')
                {
                    createComputerPieceBottomMoves(
                        ref rightMoveNode,
                        i_Board,
                        io_TotalSkips + 1,
                        io_CurrentPieceRow - 2,
                        io_CurrentPieceColumn + 2);
                    io_MovesTreeRoot.Right = rightMoveNode;
                    io_TotalSkips = tempTotalSkips;
                    io_CurrentPieceColumn = tempColumn;
                    io_CurrentPieceRow = tempRow;
                }
            }
        }

        private static void findBottomMovesInRightCornerOfOddRow(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            PositionNode leftMoveNode = io_MovesTreeRoot.Left;
            int tempTotalSkips = io_TotalSkips;
            int tempRow = io_CurrentPieceRow;
            int tempColumn = io_CurrentPieceColumn;

            bool isOpponentExistInLeft =
                i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1] == Checkers.k_BottomPlayerSoldierSign
                || i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1]
                == Checkers.k_BottomPlayerKingSign;

            if (i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1] == ' ')
            {
                if (io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Left = new PositionNode(io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1);
                }
            }

            if (i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1] == ' ')
            {
                if (io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Right = new PositionNode(io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1);
                }
            }

            if (isOpponentExistInLeft == true)
            {
                if(i_Board.GameBoard[io_CurrentPieceRow - 2, io_CurrentPieceColumn - 2] == ' ')
                {
                    createComputerPieceBottomMoves(
                        ref leftMoveNode,
                        i_Board,
                        io_TotalSkips + 1,
                        io_CurrentPieceRow - 2,
                        io_CurrentPieceColumn - 2);
                    io_MovesTreeRoot.Left = leftMoveNode;
                    io_TotalSkips = tempTotalSkips;
                    io_CurrentPieceColumn = tempColumn;
                    io_CurrentPieceRow = tempRow;
                }
            }
        }

        private static void findBottomMovesInLeftCornerOfOddRow(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            PositionNode rightMoveNode = io_MovesTreeRoot.Right;
            int tempTotalSkips = io_TotalSkips;
            int tempRow = io_CurrentPieceRow;
            int tempColumn = io_CurrentPieceColumn;
            bool isOpponentExistInRight =
                i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1] == Checkers.k_BottomPlayerSoldierSign
                || i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1]
                == Checkers.k_BottomPlayerKingSign;

            if (i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1] == ' ')
            {
                if (io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Right = new PositionNode(io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1);
                }
            }
            else if (isOpponentExistInRight == true)
            {
                if (i_Board.GameBoard[io_CurrentPieceRow - 2, io_CurrentPieceColumn + 2] == ' ')
                {
                    createComputerPieceBottomMoves(
                        ref rightMoveNode,
                        i_Board,
                        io_TotalSkips + 1,
                        io_CurrentPieceRow - 2,
                        io_CurrentPieceColumn + 2);
                    io_MovesTreeRoot.Right = rightMoveNode;
                    io_TotalSkips = tempTotalSkips;
                    io_CurrentPieceColumn = tempColumn;
                    io_CurrentPieceRow = tempRow;
                }
            }
        }

        private static void findBottomMovesInMiddle(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            PositionNode rightMoveNode = io_MovesTreeRoot.Right;
            PositionNode leftMoveNode = io_MovesTreeRoot.Left;
            int tempTotalSkips = io_TotalSkips;
            int tempRow = io_CurrentPieceRow;
            int tempColumn = io_CurrentPieceColumn;

            bool isOpponentExistInRight =
                i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1] == Checkers.k_BottomPlayerSoldierSign
                || i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1]
                == Checkers.k_BottomPlayerKingSign;

            bool isOpponentExistInLeft =
                i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1] == Checkers.k_BottomPlayerSoldierSign
                || i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1]
                == Checkers.k_BottomPlayerKingSign;

            if (i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1] == ' ')
            {
                if (io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Left = new PositionNode(io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1);
                }
            }

            if (i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1] == ' ')
            {
                if (io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Right = new PositionNode(io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1);
                }
            }

            if (isOpponentExistInLeft == true)
            {
                if (i_Board.GameBoard[io_CurrentPieceRow - 2, io_CurrentPieceColumn - 2] == ' ')
                {
                    createComputerPieceBottomMoves(
                        ref leftMoveNode,
                        i_Board,
                        io_TotalSkips + 1,
                        io_CurrentPieceRow - 2,
                        io_CurrentPieceColumn - 2);
                    io_MovesTreeRoot.Left = leftMoveNode;
                    io_TotalSkips = tempTotalSkips;
                    io_CurrentPieceColumn = tempColumn;
                    io_CurrentPieceRow = tempRow;
                }
            }

            if (isOpponentExistInRight == true)
            {
                if(i_Board.GameBoard[io_CurrentPieceRow - 2, io_CurrentPieceColumn + 2] == ' ')
                {
                    createComputerPieceBottomMoves(
                        ref rightMoveNode,
                        i_Board,
                        io_TotalSkips + 1,
                        io_CurrentPieceRow - 2,
                        io_CurrentPieceColumn + 2);
                    io_MovesTreeRoot.Right = rightMoveNode;
                    io_TotalSkips = tempTotalSkips;
                    io_CurrentPieceColumn = tempColumn;
                    io_CurrentPieceRow = tempRow;
                }
            }
        }

        private static void findBottomMovesInTopEdge(
            ref PositionNode io_MovesTreeRoot,
            Board i_Board,
            ref int io_TotalSkips,
            ref int io_CurrentPieceRow,
            ref int io_CurrentPieceColumn)
        {
            if (i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1] == ' ')
            {
                if (io_TotalSkips == 0)
                {
                    io_MovesTreeRoot.Right = new PositionNode(io_CurrentPieceRow - 1, io_CurrentPieceColumn + 1);
                }
            }

            if (io_CurrentPieceColumn != 0)
            {
                if (i_Board.GameBoard[io_CurrentPieceRow - 1, io_CurrentPieceColumn - 1] == ' ')
                {
                    if (io_TotalSkips == 0)
                    {
                        io_MovesTreeRoot.Left = new PositionNode(
                            io_CurrentPieceRow - 1,
                            io_CurrentPieceColumn - 1);
                    }
                }
            }
        }

        public void findAllComputerOptimalMoves(Board i_Board)
        {
            for(int i = 0; i < (int)i_Board.Size; i++)
            {
                for(int j = 0; j < (int)i_Board.Size; j++)
                {
                    bool isBelongedPiece = i_Board.GameBoard[i, j] == Checkers.k_TopPlayerSoldierSign
                                           || i_Board.GameBoard[i, j] == Checkers.k_TopPlayerKingSign;

                    if(isBelongedPiece == true)
                    {
                        Piece currentPiece = new Piece(i, j, i_Board.GameBoard[i, j]);
                        findOptimalMoveForPiece(currentPiece, i_Board);
                    }
                }
            }

            extractOptimalSkipMoves();
        }

        public void extractOptimalSkipMoves()
        {
            foreach(LinkedList<PositionNode> currentOptimalMove in m_AllOptimalMoves)
            {
                if(currentOptimalMove.Last.Value.SkipsSoFar > 0)
                {
                    m_OptimalSkipMoves.Add(currentOptimalMove);
                }
            }
        }

        public void findOptimalMoveForPiece(Piece i_SourcePiece, Board i_Board)
        {
            bool isKing = true;
            MovesTree pieceMovesTree = new MovesTree();
            List<LinkedList<PositionNode>> pieceOptimalMoves = new List<LinkedList<PositionNode>>();

            pieceMovesTree = findComputerSourcePieceMoves(i_Board, i_SourcePiece, !isKing);

            if(pieceMovesTree.Root.Left != null || pieceMovesTree.Root.Right != null)
            {
                pieceOptimalMoves = findPieceOptimalMoves(pieceMovesTree);
                m_AllOptimalMoves.AddRange(pieceOptimalMoves);
            }

            if(i_Board.GameBoard[i_SourcePiece.Row, i_SourcePiece.Column] == Checkers.k_TopPlayerKingSign)
            {
                pieceMovesTree = findComputerSourcePieceMoves(i_Board, i_SourcePiece, isKing);

                if(pieceMovesTree.Root.Left != null || pieceMovesTree.Root.Right != null)
                {
                    pieceOptimalMoves = findPieceOptimalMoves(pieceMovesTree);
                    m_AllOptimalMoves.AddRange(pieceOptimalMoves);
                }
            }
        }
    }
}
