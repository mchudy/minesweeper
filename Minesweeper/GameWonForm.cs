using System;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class GameWonForm : Form
    {
        public GameWonForm()
        {
            InitializeComponent();
        }

        public string Nickname { get; private set; }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Nickname = this.nickTextBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
