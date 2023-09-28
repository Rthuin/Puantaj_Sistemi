using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SekerProje_v2
{
    public partial class LoginPage : Form
    {
        public LoginPage()
        {
            InitializeComponent();
            txtBoxLogin.KeyDown += textBoxCardId_KeyDown;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlClass sqlClass = new SqlClass();
            
            this.Hide();
            sqlClass.insertToWorklog(sqlClass.selectFromPersonelInfo(txtBoxLogin.Text));

            Thread.Sleep(1000);
            txtBoxLogin.Text = string.Empty;
            this.Show();
        }
        private void textBoxCardId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnLogin.PerformClick();
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            SignUpPage form1 = new SignUpPage();
            this.Hide();
            form1.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SettingsPage form1 = new SettingsPage();    
            form1.ShowDialog();
        }
    }
}
