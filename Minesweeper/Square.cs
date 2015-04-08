using System;

namespace Minesweeper
{
    [Serializable]
    public struct Square
    {
        public bool IsMine { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public int AdjacentMines { get; set; }
    }
}