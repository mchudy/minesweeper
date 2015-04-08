using System;

namespace Minesweeper.Serialization
{
    [Serializable]
    public class GameState
    {
        public Square[,] Board { get; set; }
        public int GameTime { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}