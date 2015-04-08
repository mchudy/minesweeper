using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Minesweeper.Properties;

namespace Minesweeper
{
    public partial class SettingsForm : Form
    {
        private const string ImageDialogFiler = 
            "Image files (*.bmp, *.jpg, *.jpeg, *.png, *.gif)|*.bmp;*.jpg;*.jpeg;*.png;*.gif)";

        private Color mineColor = Settings.Default.MineColor;
        private Color flagColor = Settings.Default.FlagColor;
        private Color revealedColor = Settings.Default.RevealedColor;
        private Color unrevealedColor = Settings.Default.UnrevealedColor;
        private string mineImagePath = Settings.Default.MineImagePath;
        private string flagImagePath = Settings.Default.FlagImagePath;

        public SettingsForm()
        {
            InitializeComponent();
            if (File.Exists(mineImagePath))
            {
                this.mineImageButton.BackgroundImage = Image.FromFile(mineImagePath);
            }
            if (File.Exists(flagImagePath))
            {
                this.flagImageButton.BackgroundImage = Image.FromFile(flagImagePath);
            }
        }

        private void saveSettingsButton_Click(object sender, EventArgs e)
        {
            Settings.Default.MineProbability = this.probabilityTrackBar.Value * 10;
            Settings.Default.Rows = (int)this.rowsUpDown.Value;
            Settings.Default.Columns = (int)this.columnsUpDown.Value;
            Settings.Default.MineColor = this.mineColor;
            Settings.Default.FlagColor = this.flagColor;
            Settings.Default.UnrevealedColor = this.unrevealedColor;
            Settings.Default.RevealedColor = this.revealedColor;
            Settings.Default.MineImagePath = this.mineImagePath;
            Settings.Default.FlagImagePath = this.flagImagePath;
            Settings.Default.Save();
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void mineColorButton_Click(object sender, EventArgs e)
        {
            Color color;
            if (GetColorFromUser(out color))
            {
                this.mineColor = color;
            }
        }

        private void unrevealedColorButton_Click(object sender, EventArgs e)
        {
            Color color;
            if (GetColorFromUser(out color))
            {
                this.unrevealedColor = color;
            }
        }

        private void revealedColorButton_Click(object sender, EventArgs e)
        {
            Color color;
            if (GetColorFromUser(out color))
            {
                this.revealedColor = color;
            }
        }

        private void flagColorButton_Click(object sender, EventArgs e)
        {
            Color color;
            if (GetColorFromUser(out color))
            {
                this.flagColor = color;
            }
        }
        private static bool GetColorFromUser(out Color selectedColor)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    selectedColor = dialog.Color;
                    return true;
                }
            }
            selectedColor = Color.Empty;
            return false;
        }

        private void mineImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = ImageDialogFiler;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.mineImagePath = dialog.FileName;
                    this.mineImageButton.BackgroundImage = Image.FromFile(dialog.FileName);
                }
            }
        }

        private void flagImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = ImageDialogFiler;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.flagImagePath = dialog.FileName;
                    this.flagImageButton.BackgroundImage = Image.FromFile(dialog.FileName);
                }
            }
        }
    }
}
