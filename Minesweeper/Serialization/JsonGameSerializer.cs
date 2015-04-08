using System.IO;
using Newtonsoft.Json;

namespace Minesweeper.Serialization
{
    public class JsonGameSerializer : IGameSerializer
    {

        public void Serialize(Board board, string path)
        {
            GameState gameState = Board.SaveState(board);
            string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
            using (var stream = new StreamWriter(path))
            {
                stream.Write(json);
            }
        }

        public Board Deserialize(string path)
        {
            using (var stream = new StreamReader(path))
            {
                string json = stream.ReadToEnd();
                GameState gameState = JsonConvert.DeserializeObject<GameState>(json);
                return Board.LoadState(gameState);
            }
        }
    }
}