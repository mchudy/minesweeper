namespace Minesweeper
{
    public interface IMainView
    {
        int Rows { get; set; }
        int Columns { get; set; }

        void ShowBoard();
        void ShowRevealed(int row, int col, int adjacentMines);
        void ShowUnrevealed(int row, int col);
        void ShowMine(int row, int col);
        void ShowFlag(int row, int col);
    }
}