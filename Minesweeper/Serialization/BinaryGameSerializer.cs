using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Minesweeper.Serialization
{
    public class BinaryGameSerializer : IGameSerializer
    {
        public void Serialize(Board board, string path)
        {
            GameState gameState = Board.SaveState(board);
            BinaryFormatter formatter = new BinaryFormatter();
            using (var stream = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(stream, gameState);
            }
        }

        public Board Deserialize(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                var gameState = (GameState)formatter.Deserialize(stream);
                return Board.LoadState(gameState);
            }
        }
    }
}