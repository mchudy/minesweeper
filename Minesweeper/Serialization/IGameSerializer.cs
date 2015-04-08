namespace Minesweeper.Serialization
{
    public interface IGameSerializer
    {
        void Serialize(Board board, string path);
        Board Deserialize(string path);
    }
}