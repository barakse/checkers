using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C20_Ex02
{
    public class Checkers
    {
        private readonly List<Player> r_Players;
        private Board m_Board;
        private int m_CurrentPlayerTurn;
        public const char k_TopPlayerSoldierSign = 'O';
        public const char k_TopPlayerKingSign = 'U';
        public const char k_BottomPlayerSoldierSign = 'X';
        public const char k_BottomPlayerKingSign = 'K';

        public Checkers(Board i_Board, Player i_Player1, Player i_Player2)
        {
            r_Players = new List<Player>(2);
            r_Players.Add(i_Player1);
            r_Players.Add(i_Player2);
            m_Board = i_Board;
            m_CurrentPlayerTurn = 0;
        }

        public List<Player> Players
        {
            get
            {
                return r_Players;
            }
        }

        public Board GameBoard
        {
            get
            {
                return m_Board;
            }
        }

        public int CurrentPlayerTurn
        {
            get
            {
                return m_CurrentPlayerTurn;
            }

            set
            {
                m_CurrentPlayerTurn = value;
            }
        }

        public void ReinitializeGame()
        {
            m_Board.BoardInitialize();
            m_CurrentPlayerTurn = 0;

            foreach(Player currentPlayer in r_Players)
            {
                currentPlayer.CurrentPieces = m_Board.r_PiecesAccordingBoardSizeDictionary[(int)m_Board.Size];
                currentPlayer.Soldiers = m_Board.r_PiecesAccordingBoardSizeDictionary[(int)m_Board.Size];
                currentPlayer.Kings = 0;
            }
        }

        public bool IsTie()
        {
            bool isSkipToCheck = true;
            bool legalMovesForFirstPlayer = m_Board.IsMoveInBoardForPlayer(r_Players[0], isSkipToCheck)
                                            || m_Board.IsMoveInBoardForPlayer(r_Players[0], !isSkipToCheck);
            bool legalMovesForSecondPlayer = m_Board.IsMoveInBoardForPlayer(r_Players[1], isSkipToCheck)
                                             || m_Board.IsMoveInBoardForPlayer(r_Players[1], !isSkipToCheck);

            return !legalMovesForFirstPlayer && !legalMovesForSecondPlayer;
        }
    }

    public enum eGameMode
    {
        TwoPlayers = 1,
        Pc
    }
}