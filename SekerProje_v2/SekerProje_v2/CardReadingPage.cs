using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SekerProje_v2
{
    public partial class CardReadingPage : Form
    {
        private string PrsnlCardId;
        public string prsnlCardId
        {
            get { return PrsnlCardId; }
            set { PrsnlCardId = value; }
        }
        public CardReadingPage()
        {
            InitializeComponent();
            textCardId.KeyDown += textBoxCardId_KeyDown;
        }
        private void textBoxCardId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                button1.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            prsnlCardId = textCardId.Text;
            this.Close();
        }
    }
}
