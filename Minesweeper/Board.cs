using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using Minesweeper.Properties;
using Minesweeper.Serialization;

namespace Minesweeper
{
    public class Board
    {
        private readonly Random random = new Random();
        private readonly Timer timer = new Timer(1000);

        private Square[,] board;
        private readonly int rows;
        private readonly int columns;
        private int gameTime;
        private int minesCount;
        private int correctlyFlaggedMines;
        private int flaggedSquares;
        private bool gameOver;

        public Board(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            timer.Elapsed += (s, e) => gameTime++;
            timer.Start();
        }

        public event EventHandler<GameWonEventArgs> GameWon;
        public event EventHandler GameLost;
        public event EventHandler<SquareEventArgs> SafeSquareRevealed;
        public event EventHandler<SquareEventArgs> MineRevealed;
        public event EventHandler<SquareEventArgs> SquareFlagged;
        public event EventHandler<SquareEventArgs> SquareUnflagged;

        public int Rows
        {
            get { return this.rows; }
        }

        public int Columns
        {
            get { return this.columns; }
        }

        public static GameState SaveState(Board board)
        {
            return new GameState
            {
                Board = board.board,
                Rows = board.rows,
                Columns = board.columns,
                GameTime = board.gameTime
            };
        }

        public static Board LoadState(GameState state)
        {
            Board newBoard = new Board(state.Rows, state.Columns) { board = state.Board, gameTime = state.GameTime };
            for (int i = 0; i < state.Rows; i++)
            {
                for (int j = 0; j < state.Columns; j++)
                {
                    if (newBoard.board[i, j].IsFlagged)
                    {
                        newBoard.flaggedSquares++;
                        if (newBoard.board[i, j].IsMine)
                        {
                            newBoard.minesCount++;
                            newBoard.correctlyFlaggedMines++;
                        }
                    }
                    else if (newBoard.board[i, j].IsMine)
                    {
                        newBoard.minesCount++;
                    }
                    if (newBoard.board[i, j].IsMine && newBoard.board[i, j].IsRevealed)
                        newBoard.gameOver = true;
                }
            }
            if (newBoard.CheckGameWon())
                newBoard.gameOver = true;
            return newBoard;
        }

        public void Initialize()
        {
            board = new Square[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (random.Next(100) < Settings.Default.MineProbability)
                    {
                        board[i, j].IsMine = true;
                        minesCount++;
                    }
                }
            }
            CountAdjacentMines();
            if (minesCount == 0) OnGameWon();
        }

        public void RecreateBoard()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (board[i, j].IsFlagged)
                    {
                        SquareFlagged.Raise(this, new SquareEventArgs(i, j));
                    }
                    else if (board[i, j].IsRevealed && !board[i, j].IsMine)
                    {
                        SafeSquareRevealed.Raise(this, new SquareEventArgs(i, j, board[i, j].AdjacentMines));
                    }
                    else if (board[i, j].IsRevealed)
                    {
                        MineRevealed.Raise(this, new SquareEventArgs(i, j));
                    }
                }
            }
        }

        public void RevealSquare(int row, int col)
        {
            if (gameOver || board[row, col].IsRevealed) return;
            board[row, col].IsRevealed = true;
            if (board[row, col].IsFlagged)
            {
                UnflagSquare(row, col);
            }
            if (board[row, col].IsMine)
            {
                MineRevealed.Raise(this, new SquareEventArgs(row, col));
                gameOver = true;
                timer.Stop();
                GameLost.Raise(this, EventArgs.Empty);
            }
            else
            {
                SafeSquareRevealed.Raise(this, new SquareEventArgs(row, col, board[row, col].AdjacentMines));
                if (board[row, col].AdjacentMines == 0)
                    RevealEmptySquares(row, col);
            }
            if (CheckGameWon()) OnGameWon();
        }

        public void FlagSquare(int row, int col)
        {
            if (gameOver) return;
            if (!board[row, col].IsFlagged && !board[row, col].IsRevealed)
            {
                board[row, col].IsFlagged = true;
                SquareFlagged.Raise(this, new SquareEventArgs(row, col));
                flaggedSquares++;
                if (board[row, col].IsMine)
                {
                    correctlyFlaggedMines++;
                }
                if (CheckGameWon()) OnGameWon();
            }
            else if (board[row, col].IsFlagged)
            {
                UnflagSquare(row, col);
            }
        }

        private void UnflagSquare(int row, int col)
        {
            board[row, col].IsFlagged = false;
            if (board[row, col].IsMine)
            {
                correctlyFlaggedMines--;
            }
            flaggedSquares--;
            SquareUnflagged.Raise(this, new SquareEventArgs(row, col));
        }

        private bool CheckGameWon()
        {
            return correctlyFlaggedMines == minesCount && correctlyFlaggedMines == flaggedSquares;
        }

        private void OnGameWon()
        {
            gameOver = true;
            timer.Stop();
            GameWon.Raise(this, new GameWonEventArgs(gameTime));
        }

        private void RevealEmptySquares(int row, int col)
        {
            foreach (var square in GetAdjacentSquares(row, col))
            {
                if (RevealEmptySquare(square.X, square.Y))
                    RevealEmptySquares(square.X, square.Y);
            }
        }

        private bool RevealEmptySquare(int row, int col)
        {
            if (board[row, col].IsRevealed) return false;
            board[row, col].IsRevealed = true;
            if (board[row, col].IsFlagged)
            {
                board[row, col].IsFlagged = false;
                SquareUnflagged.Raise(this, new SquareEventArgs(row, col));
            }
            SafeSquareRevealed.Raise(this, new SquareEventArgs(row, col, board[row, col].AdjacentMines));
            if (board[row, col].AdjacentMines == 0)
                return true;
            return false;
        }

        private void CountAdjacentMines()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    board[i, j].AdjacentMines = GetAdjacentSquares(i, j)
                        .Count(p => board[p.X, p.Y].IsMine);
                }
            }
        }

        private IEnumerable<Point> GetAdjacentSquares(int row, int col)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (!(i == 0 && j == 0) && CheckBounds(row + i, col + j))
                        yield return new Point(row + i, col + j);
                }
            }
        }

        private bool CheckBounds(int row, int col)
        {
            return row >= 0 && row < rows && col >= 0 && col < columns;
        }
    }
}