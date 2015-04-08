using System.Linq;
using System.Windows.Forms;
using Minesweeper.Properties;

namespace Minesweeper
{
    public partial class HighscoresForm : Form
    {
        public HighscoresForm()
        {
            InitializeComponent();
            ShowHighscores();
        }

        private void ShowHighscores()
        {
            foreach (var entry in Settings.Default.Highscores.OrderBy(e => e.Time))
            {
                this.highscoresListView.Items.Add(new ListViewItem(new[]
                {
                    entry.Nickname,
                    entry.Time.ToString(),
                    entry.BoardSize
                }));
            }
        }
    }
}
