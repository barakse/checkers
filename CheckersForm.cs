using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C20_Ex02;

namespace C20_Ex05
{
    public partial class CheckersForm : Form
    {
        private const int k_SpacingPixels = 50;
        private readonly Color r_ChosenSquareColor;
        private readonly Color r_SquareColor;
        private readonly int r_SquareSize;
        private readonly Checkers r_Checkers;
        private readonly SquareButton[,] r_Board;
        private Point m_CurrentPoint;
        private TableLayoutPanel m_PlayerResultsTable;
        private TableLayoutPanel m_CurrentTurnTable;
        private Label m_FirstPlayerResultsLabel;
        private Label m_SecondPlayerResultsLabel;
        private Label m_CurrentTurnLabel;
        private Player m_CurrentPlayer;
        private Player m_CurrentOpponent;
        private int m_CurrentPlayerTurn;
        private int m_LastPieceRow;
        private int m_LastPieceColumn;
        private bool m_IsComputerTurn;
        private eTurnStatus m_TurnStatus;
        private SquareButton m_ChosenSourceSquare;
        
        public CheckersForm(GameSettingsForm i_GameSettings)
        {
            r_Board = new SquareButton[(int)i_GameSettings.BoardSize, (int)i_GameSettings.BoardSize];
            Board checkersLogicBoard = new Board(i_GameSettings.BoardSize);
            Player player1 = new Player(
                i_GameSettings.FirstPlayerName,
                checkersLogicBoard.r_PiecesAccordingBoardSizeDictionary[(int)i_GameSettings.BoardSize],
                false,
                Checkers.k_BottomPlayerSoldierSign,
                Checkers.k_BottomPlayerKingSign);
            Player player2 = new Player(
                i_GameSettings.SecondPlayerName,
                checkersLogicBoard.r_PiecesAccordingBoardSizeDictionary[(int)i_GameSettings.BoardSize],
                i_GameSettings.IsPlayerPc,
                Checkers.k_TopPlayerSoldierSign,
                Checkers.k_TopPlayerKingSign);
            r_Checkers = new Checkers(checkersLogicBoard, player1, player2);
            m_CurrentPlayer = r_Checkers.Players[0];
            m_CurrentOpponent = r_Checkers.Players[1];
            m_CurrentPlayerTurn = 0;
            r_SquareSize = SquareButton.SquareSize;
            m_TurnStatus = eTurnStatus.TurnCheck;
            r_ChosenSquareColor = Color.FromArgb(129, 212, 250);
            r_SquareColor = Color.FromArgb(250, 250, 250);
            m_IsComputerTurn = false;
            this.Closing += checkersForm_Closing;
            initializeComponent();
        }

        public void OnBoardChanged()
        {
            updateBoard();
        }

        public void OnComputerTurnEnd(Player i_CurrentPlayer, Player i_CurrentOpponent)
        {
            bool isEndGame = isEndOfTheGame(i_CurrentPlayer, i_CurrentOpponent);
            m_IsComputerTurn = false;
            m_CurrentTurnLabel.Text = setCurrentTurnLabelText(i_CurrentOpponent.Name);
        }

        private static string setPlayerResultsLabelText(Player i_Player)
        {
            return string.Format("{0}: {1}", i_Player.Name, i_Player.CurrentPoints);
        }

        private void checkersForm_Closing(object sender, CancelEventArgs e)
        {
            if(this.DialogResult == DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void gameStart()
        {
            r_Checkers.ReinitializeGame();
            updateBoard();
            m_FirstPlayerResultsLabel.Text = setPlayerResultsLabelText(r_Checkers.Players[0]);
            m_SecondPlayerResultsLabel.Text = setPlayerResultsLabelText(r_Checkers.Players[1]);
            m_CurrentPlayer = r_Checkers.Players[0];
            m_CurrentOpponent = r_Checkers.Players[1];
            m_CurrentPlayerTurn = 0;
            m_TurnStatus = eTurnStatus.TurnCheck;
            m_CurrentTurnLabel.Text = setCurrentTurnLabelText(r_Checkers.Players[0].Name);
        }

        private void initializeComponent()
        {
            this.Icon = Properties.Resources.Checkers_Icon;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.AutoSize = true;
            MaximizeBox = false;
            this.ClientSize = new System.Drawing.Size(((int)r_Checkers.GameBoard.Size * r_SquareSize) + (2 * k_SpacingPixels), ((int)r_Checkers.GameBoard.Size * r_SquareSize) + 50);
            this.Text = "Checkers";
            this.StartPosition = FormStartPosition.CenterScreen;
            initializePlayerResultsTable();
            initializeCurrentTurnTable();
            initializePlayersResultsLabels();
            initializeCurrentTurnLabel();
            initializeBoardSquares();
        }

        private void initializePlayerResultsTable()
        {
            m_CurrentPoint = new Point(5, 10);
            m_PlayerResultsTable = new TableLayoutPanel();
            m_PlayerResultsTable.Location = m_CurrentPoint;
            m_PlayerResultsTable.Height = 15;
            m_PlayerResultsTable.Width = this.ClientSize.Width - 10;
            m_PlayerResultsTable.ColumnCount = 2;
            m_PlayerResultsTable.RowCount = 1;
            m_PlayerResultsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            m_PlayerResultsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            m_PlayerResultsTable.Anchor = AnchorStyles.None;
            this.Controls.Add(m_PlayerResultsTable);
        }

        private void initializeCurrentTurnTable()
        {
            m_CurrentPoint = new Point(50, ((int)r_Checkers.GameBoard.Size * k_SpacingPixels) + 30);
            m_CurrentTurnTable = new TableLayoutPanel();
            m_CurrentTurnTable.Location = m_CurrentPoint;
            m_CurrentTurnTable.Height = 25;
            m_CurrentTurnTable.Width = (int)r_Checkers.GameBoard.Size * k_SpacingPixels;
            m_CurrentTurnTable.ColumnCount = 1;
            m_CurrentTurnTable.RowCount = 1;
            m_CurrentTurnTable.Anchor = AnchorStyles.None;
            this.Controls.Add(m_CurrentTurnTable);
        }

        private void initializePlayersResultsLabels()
        {
            m_FirstPlayerResultsLabel = new Label();
            m_FirstPlayerResultsLabel.Text = setPlayerResultsLabelText(r_Checkers.Players[0]);
            m_FirstPlayerResultsLabel.Dock = DockStyle.Top;
            m_FirstPlayerResultsLabel.Anchor = AnchorStyles.None;
            m_FirstPlayerResultsLabel.TextAlign = ContentAlignment.TopLeft;
            m_FirstPlayerResultsLabel.AutoSize = true;
            m_FirstPlayerResultsLabel.Width = TextRenderer.MeasureText(
                m_FirstPlayerResultsLabel.Text,
                m_FirstPlayerResultsLabel.Font).Width;
            this.m_PlayerResultsTable.Controls.Add(m_FirstPlayerResultsLabel, 0, 0);

            m_SecondPlayerResultsLabel = new Label();
            m_SecondPlayerResultsLabel.AutoSize = true;
            m_SecondPlayerResultsLabel.Dock = DockStyle.Top;
            m_SecondPlayerResultsLabel.Anchor = AnchorStyles.None;
            m_SecondPlayerResultsLabel.TextAlign = ContentAlignment.TopLeft;
            m_SecondPlayerResultsLabel.Width = TextRenderer.MeasureText(
                m_SecondPlayerResultsLabel.Text,
                m_SecondPlayerResultsLabel.Font).Width;
            m_SecondPlayerResultsLabel.Text = setPlayerResultsLabelText(r_Checkers.Players[1]);
            this.m_PlayerResultsTable.Controls.Add(m_SecondPlayerResultsLabel, 1, 0);
        }

        private void initializeCurrentTurnLabel()
        {
            m_CurrentTurnLabel = new Label();
            m_CurrentTurnLabel.Text = setCurrentTurnLabelText(r_Checkers.Players[0].Name);
            m_CurrentTurnLabel.Font = new Font(m_CurrentTurnLabel.Font.FontFamily, 12, FontStyle.Bold);
            m_CurrentTurnLabel.Dock = DockStyle.Top;
            m_CurrentTurnLabel.Anchor = AnchorStyles.None;
            m_CurrentTurnLabel.TextAlign = ContentAlignment.TopLeft;
            m_CurrentTurnLabel.AutoSize = true;
            m_CurrentTurnLabel.Width = TextRenderer.MeasureText(
                m_CurrentTurnLabel.Text,
                m_CurrentTurnLabel.Font).Width;
            this.m_CurrentTurnTable.Controls.Add(m_CurrentTurnLabel, 0, 0);
        }

        private void initializeBoardSquares()
        {
            int boardSize = (int)r_Checkers.GameBoard.Size;
            m_CurrentPoint.Y = 30;

            for (int i = 0; i < boardSize; i++)
            {
                m_CurrentPoint.X = 50;

                for (int j = 0; j < boardSize; j++)
                {
                    r_Board[i, j] = new SquareButton(i, j);
                    r_Board[i, j].Location = new Point(m_CurrentPoint.X, m_CurrentPoint.Y);
                    r_Board[i, j].Sign = r_Checkers.GameBoard.GameBoard[i, j];
                    updateSquareImageBySign(r_Board[i, j]);

                    if ((i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0))
                    {
                        r_Board[i, j].Enabled = true;
                        r_Board[i, j].Click += boardSquare_Click;
                        r_Board[i, j].BackColor = r_SquareColor;
                    }

                    this.Controls.Add(r_Board[i, j]);
                    m_CurrentPoint.X += r_SquareSize;
                }

                m_CurrentPoint.Y += r_SquareSize;
            }
        }

        private string setCurrentTurnLabelText(string i_PlayerName)
        {
            return string.Format("{0}'s turn", i_PlayerName);
        }

        private void boardSquare_Click(object sender, EventArgs e)
        {
            SquareButton currentClickedButton = sender as SquareButton;

            if(!m_IsComputerTurn)
            {
                if(currentClickedButton.Sign == ' ' && m_ChosenSourceSquare == null)
                {
                    showValidityErrorDialog("You must choose a square that contains game piece.");
                }
                else if(currentClickedButton.BackColor != r_ChosenSquareColor)
                {
                    if(currentClickedButton.Sign != ' ' && m_ChosenSourceSquare == null)
                    {
                        currentClickedButton.BackColor = r_ChosenSquareColor;
                        m_ChosenSourceSquare = currentClickedButton;
                    }

                    if(currentClickedButton != m_ChosenSourceSquare)
                    {
                        manageTurnByStatus(currentClickedButton);
                        m_ChosenSourceSquare.BackColor = r_SquareColor;
                        m_ChosenSourceSquare = null;
                    }
                }
                else
                {
                    currentClickedButton.BackColor = r_SquareColor;
                    m_ChosenSourceSquare = null;
                }
            }
        }

        private void manageTurnByStatus(SquareButton i_DestinationSquare)
        {
            switch(m_TurnStatus)
            {
                case eTurnStatus.TurnCheck:
                    {
                        onTurnCheck(i_DestinationSquare);
                        break;
                    }

                case eTurnStatus.SkipMoveStillAvailable:
                    {
                        onTurnCheck(i_DestinationSquare);
                        break;
                    }
            }
        }

        private void onTurnCheck(SquareButton i_DestinationSquare)
        {
            Piece chosenPiece = new Piece(
                m_ChosenSourceSquare.Row,
                m_ChosenSourceSquare.Column,
                r_Checkers.GameBoard.GameBoard[m_ChosenSourceSquare.Row, m_ChosenSourceSquare.Column]);
            bool isMoveValid = C20_Ex02.Move.IsLegalMove(
                chosenPiece,
                i_DestinationSquare.Row,
                i_DestinationSquare.Column,
                r_Checkers.GameBoard);
            C20_Ex02.Move currentMove = new Move(chosenPiece, i_DestinationSquare.Row, i_DestinationSquare.Column);
            bool isMoveLegal = true;

            if(m_CurrentPlayer.SoldierSign == chosenPiece.PieceSign
               || m_CurrentPlayer.KingSign == chosenPiece.PieceSign)
            {
                if(isMoveValid == true)
                {
                    bool isSkipMoveAvailable = m_CurrentPlayer.IsSkipMoveAvailable(r_Checkers.GameBoard);

                    if(isSkipMoveAvailable == true)
                    {
                        if(!currentMove.isSkip)
                        {
                            isMoveLegal = false;
                            showSkipValidityError();
                        }
                    }

                    if(isMoveLegal == true)
                    {
                        onTurnMake(chosenPiece, currentMove);
                    }
                }
                else
                {
                    showValidityErrorDialog("Your move is illegal.");
                }
            }
            else
            {
                showValidityErrorDialog("You chose a piece that does not belong to you.");
            }
        }

        private void showSkipValidityError()
        {
            if (m_TurnStatus == eTurnStatus.TurnCheck)
            {
                showValidityErrorDialog("There is a skip move that you must make.");
            }
            else
            {
                showValidityErrorDialog("You still have a skip moves that you must make with your last chosen piece.");
            }
        }

        private void onTurnMake(Piece i_ChosenPiece, Move i_CurrentMove)
        {
            if(m_TurnStatus == eTurnStatus.TurnCheck)
            {
                onTurnMakeStart(i_ChosenPiece, i_CurrentMove);
            }
            else
            {
                onTurnMakeSkipsAvailable(i_ChosenPiece, i_CurrentMove);
            }
        }

        private void onTurnMakeStart(Piece i_ChosenPiece, Move i_CurrentMove)
        {
            m_CurrentPlayer.MakeMove(r_Checkers.GameBoard, ref i_ChosenPiece, i_CurrentMove.DestinationRow, i_CurrentMove.DestinationColumn, m_CurrentOpponent);
            updateBoard();

            if (i_CurrentMove.isSkip == true)
            {
                bool isSkipMoveStillAvailable = r_Checkers.GameBoard.IsMoveAvailableForPieceInBoard(
                    i_ChosenPiece.PieceSign,
                    i_ChosenPiece.Row,
                    i_ChosenPiece.Column,
                    m_CurrentOpponent.SoldierSign,
                    m_CurrentOpponent.KingSign,
                    true);

                if (isSkipMoveStillAvailable == true)
                {
                    m_LastPieceRow = i_ChosenPiece.Row;
                    m_LastPieceColumn = i_ChosenPiece.Column;
                    m_TurnStatus = eTurnStatus.SkipMoveStillAvailable;
                }
                else
                {
                    onTurnEnd();
                }
            }
            else
            {
                onTurnEnd();
            }
        }

        private void onTurnMakeSkipsAvailable(Piece i_ChosenPiece, Move i_CurrentMove)
        {
            if (i_ChosenPiece.Row == m_LastPieceRow && i_ChosenPiece.Column == m_LastPieceColumn)
            {
                if (i_CurrentMove.isSkip == true)
                {
                    m_CurrentPlayer.MakeMove(r_Checkers.GameBoard, ref i_ChosenPiece, i_CurrentMove.DestinationRow, i_CurrentMove.DestinationColumn, m_CurrentOpponent);
                    updateBoard();
                }
                else
                {
                    showValidityErrorDialog("Your move is illegal.");
                }
            }
         
            bool isSkipMoveStillAvailable = r_Checkers.GameBoard.IsMoveAvailableForPieceInBoard(
                i_ChosenPiece.PieceSign,
                i_ChosenPiece.Row,
                i_ChosenPiece.Column,
                m_CurrentOpponent.SoldierSign,
                m_CurrentOpponent.KingSign,
                true);
            m_LastPieceRow = i_ChosenPiece.Row;
            m_LastPieceColumn = i_ChosenPiece.Column;

            if (!isSkipMoveStillAvailable)
            {
                onTurnEnd();
            }
        }

        private void onTurnEnd()
        {
            bool isEndGame = isEndOfTheGame(m_CurrentPlayer, m_CurrentOpponent);

            if(!isEndGame)
            {
                if(m_CurrentOpponent.IsPlayerPC == true)
                {
                    m_IsComputerTurn = true;
                    m_CurrentTurnLabel.Text = setCurrentTurnLabelText(m_CurrentOpponent.Name);
                    ComputerOptimalMoves computerMoves = new ComputerOptimalMoves();
                    computerMoves.findAllComputerOptimalMoves(r_Checkers.GameBoard);
                    m_CurrentOpponent.MakeComputerMove(computerMoves, r_Checkers.GameBoard, m_CurrentPlayer, this);
                }
                else
                {
                    m_CurrentTurnLabel.Text = setCurrentTurnLabelText(m_CurrentOpponent.Name);
                    m_CurrentPlayerTurn = (m_CurrentPlayerTurn + 1) % 2;
                    m_CurrentPlayer = r_Checkers.Players[m_CurrentPlayerTurn];
                    m_CurrentOpponent = r_Checkers.Players[(m_CurrentPlayerTurn + 1) % 2];
                }

                m_TurnStatus = eTurnStatus.TurnCheck;
            }
        }

        private bool isEndOfTheGame(Player i_CurrentPlayer, Player i_CurrentOpponent)
        {
            bool isEndGame = false;
            bool isCurrentPlayerWin = !i_CurrentOpponent.IsRegularMoveAvailable(r_Checkers.GameBoard)
                                      && !i_CurrentOpponent.IsSkipMoveAvailable(r_Checkers.GameBoard);

            if (r_Checkers.IsTie() == true)
            {
                onTie();
                isEndGame = true;
            }
            else if (isCurrentPlayerWin == true)
            {
                i_CurrentPlayer.CalculatePoints(i_CurrentOpponent.Soldiers, i_CurrentOpponent.Kings);
                onWin(i_CurrentPlayer.Name);
                isEndGame = true;
            }

            return isEndGame;
        }

        private void onTie()
        {
            string messageText = string.Format(
                @"Tie!
Another round?");
            DialogResult tieMessageResult = MessageBox.Show(
                messageText,
                "Checkers",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if(tieMessageResult == DialogResult.Yes)
            {
                gameStart();
            }
            else
            {
                this.Close();
            }
        }

        private void onWin(string i_Winner)
        {
            string messageText = string.Format(
                @"{0} Won!
Another round?",
                i_Winner);
            DialogResult winMessageResult = MessageBox.Show(
                messageText,
                "Checkers",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            this.DialogResult = winMessageResult;

            if (winMessageResult == DialogResult.Yes)
            {
                gameStart();
            }
            else
            {
                this.Close();
            }
        }

        private void updateBoard()
        {
            int boardSize = (int)r_Checkers.GameBoard.Size;

            for(int i = 0; i < boardSize; i++)
            {
                for(int j = 0; j < boardSize; j++)
                {
                    r_Board[i, j].Sign = r_Checkers.GameBoard.GameBoard[i, j];
                    updateSquareImageBySign(r_Board[i, j]);
                }
            }
        }

        private void updateSquareImageBySign(SquareButton i_BoardSquare)
        {
            switch(i_BoardSquare.Sign)
            {
                case Checkers.k_TopPlayerSoldierSign:
                    {
                        i_BoardSquare.Image = Properties.Resources.TopSoldier;
                        break;
                    }

                case Checkers.k_BottomPlayerSoldierSign:
                    {
                        i_BoardSquare.Image = Properties.Resources.BottomSoldier;
                        break;
                    }

                case Checkers.k_TopPlayerKingSign:
                    {
                        i_BoardSquare.Image = Properties.Resources.TopKing;
                        break;
                    }

                case Checkers.k_BottomPlayerKingSign:
                    {
                        i_BoardSquare.Image = Properties.Resources.BottomKing;
                        break;
                    }

                default:
                    {
                        i_BoardSquare.Image = null;
                        break;
                    }
            }
        }

        private void showValidityErrorDialog(string i_ErrorDescription)
        {
            MessageBox.Show(i_ErrorDescription, "Validity Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}