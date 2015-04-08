using System;

namespace Minesweeper
{
    public class SquareEventArgs : EventArgs
    {
        public SquareEventArgs(int row, int column, int adjacentMines = -1)
        {
            this.Row = row;
            this.Column = column;
            this.AdjacentMines = adjacentMines;
        }

        public int Row { get; private set; }
        public int Column { get; private set; }
        public int AdjacentMines { get; private set; }
    }

    public class GameWonEventArgs : EventArgs
    {
        public GameWonEventArgs(int time)
        {
            this.Time = time;
        }

        public int Time { get; private set; }
    }
}