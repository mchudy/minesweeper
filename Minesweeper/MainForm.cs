using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Minesweeper.Properties;
using Minesweeper.Serialization;

namespace Minesweeper
{
    public partial class MainForm : Form, IMainView
    {
        private readonly IMainPresenter presenter;
        private readonly Color[] numberOfMinesColors = 
        {
            Color.Empty, Color.Blue, Color.Green, Color.Red, Color.DarkBlue, 
            Color.Maroon, Color.Cyan, Color.Black, Color.Gray
        };

        public MainForm()
        {
            InitializeComponent();
            presenter = new MainPresenter(this, new BinaryGameSerializer());
        }

        public int Rows { get; set; }
        public int Columns { get; set; }

        public void ShowBoard()
        {
            ResetTableStyles();
            CreateButtons();
        }

        public void ShowFlag(int row, int col)
        {
            var button = (Button)boardPanel.GetControlFromPosition(col, row);
            string imagePath = Settings.Default.FlagImagePath;
            if (File.Exists(imagePath))
            {
                button.Text = String.Empty;
                button.BackgroundImage = Image.FromFile(imagePath);
            }
            else
            {
                button.BackColor = Settings.Default.FlagColor;
                button.Text = "!";
            }
        }

        public void ShowUnrevealed(int row, int col)
        {
            var button = (Button)boardPanel.GetControlFromPosition(col, row);
            button.BackColor = Settings.Default.UnrevealedColor;
            button.BackgroundImage = null;
            button.Text = String.Empty;
        }

        public void ShowMine(int row, int col)
        {
            var button = (Button)boardPanel.GetControlFromPosition(col, row);
            string imagePath = Settings.Default.MineImagePath;
            if (File.Exists(imagePath))
            {
                button.BackgroundImage = Image.FromFile(imagePath);
                button.Text = String.Empty;
            }
            else
            {
                button.BackColor = Settings.Default.MineColor;
                button.Text = "KABUM!";
            }
        }

        public void ShowRevealed(int row, int col, int adjacentMines)
        {
            var button = (Button)boardPanel.GetControlFromPosition(col, row);
            button.BackColor = Settings.Default.RevealedColor;
            if (adjacentMines != 0)
            {
                button.Font = new Font(button.Font.Name, button.Font.Size, FontStyle.Bold);
                button.ForeColor = numberOfMinesColors[adjacentMines];
                button.Text = adjacentMines.ToString();
            }
        }

        private void ResetTableStyles()
        {
            boardPanel.Controls.Clear();
            boardPanel.RowStyles.Clear();
            boardPanel.ColumnStyles.Clear();

            boardPanel.RowCount = Rows;
            boardPanel.ColumnCount = Columns;

            for (int i = 0; i < Rows; i++)
            {
                boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            }
            for (int j = 0; j < Columns; j++)
            {
                boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            }
        }

        private void CreateButtons()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    var button = new Button
                    {
                        BackColor = Settings.Default.UnrevealedColor,
                        Dock = DockStyle.Fill,
                        Margin = Padding.Empty,
                        Tag = new Point(i, j),
                        BackgroundImageLayout = ImageLayout.Stretch
                    };
                    button.MouseDown += button_MouseDown;
                    boardPanel.Controls.Add(button, j, i);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Czy jesteś pewien, że chcesz zakończyć?", "Potwierdzenie",
                MessageBoxButtons.OKCancel);
            if (result == DialogResult.Cancel)
                e.Cancel = true;
        }

        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            Point location = (Point)button.Tag;
            if (e.Button == MouseButtons.Left)
            {
                presenter.RevealSquare(location.X, location.Y);
            }
            else if (e.Button == MouseButtons.Right)
            {
                presenter.FlagSquare(location.X, location.Y);
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.NewGame();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.ShowSettings();
        }

        private void highscoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.ShowHighscores();
        }

        private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.SaveGame();
        }

        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.LoadGame();
        }
    }
}
