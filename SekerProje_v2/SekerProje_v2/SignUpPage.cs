using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SekerProje_v2
{
    public partial class SignUpPage : Form
    {

        public SignUpPage()
        {
            InitializeComponent();
        }
        private string PrsnlCardId;
        public string prsnlCardId
        {
            get { return PrsnlCardId; }
            set { PrsnlCardId = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlClass sqlClass = new SqlClass(); 
            
            string name = textName.Text;
            string surname = textSurname.Text;
            string tcNo = textIdNo.Text;
            string department = textDepartment.Text;


            SqlConnection conn = sqlClass.connect2Database();
            //CardReadingPage readedCard = new CardReadingPage();
            LoginPage lgnPage = new LoginPage();
            try
            {
                conn.Open();

                string checkIfEmployeeExistsQuery = "SELECT CardID FROM [PersonelInfo] WHERE [TCno] = @TCno";
                SqlCommand checkCmd = new SqlCommand(checkIfEmployeeExistsQuery, conn);
                checkCmd.Parameters.AddWithValue("@TCno", tcNo);
                object existingEmployeeID = checkCmd.ExecuteScalar();

                if (existingEmployeeID != null)
                {
                   
                    string a = sqlClass.insertToPersonelLog(conn, name, surname, tcNo, department).ToString();
                     
                    MessageBox.Show(a);

                    string insertCardQuery = "UPDATE [CardTable] SET [SecondCard] = @SecondCard WHERE [ID_NO] = @CalisanID AND [FirstCard] = @FirstCard ";
                    string updatePersonelValidity = "UPDATE [PersonelInfo] SET [VALID_CARD] = @Validity WHERE [TCno] = @CalisanID AND [CardID] = @FirstCard ";
                    SqlCommand updatePersoneCmd = new SqlCommand(updatePersonelValidity, conn);
                    SqlCommand insertCardCmd = new SqlCommand(insertCardQuery, conn);
                    
                    insertCardCmd.Parameters.AddWithValue("@CalisanID", tcNo);
                    insertCardCmd.Parameters.AddWithValue("@FirstCard", (string)existingEmployeeID);
                    insertCardCmd.Parameters.AddWithValue("@SecondCard", a);
                    insertCardCmd.ExecuteNonQuery();

                    updatePersoneCmd.Parameters.AddWithValue("@Validity", 0);
                    updatePersoneCmd.Parameters.AddWithValue("@FirstCard", (string)existingEmployeeID); 
                    updatePersoneCmd.Parameters.AddWithValue("@CalisanID", tcNo);
                    updatePersoneCmd.ExecuteNonQuery();
                    MessageBox.Show("Kayıt başarıyla eklendi if.");
                    this.Hide();
                    lgnPage.Show();
                    Thread.Sleep(1000);



                }
                else
                {
                    
                    sqlClass.insertToPersonelLog(conn, name, surname, tcNo, department);
                    this.Show();
                    MessageBox.Show("Kayıt başarıyla eklendi else.");
                    this.Hide();
                    lgnPage.Show();
                    Thread.Sleep(1000);

                    this.Close();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: qweqw  " + ex.Message);

                lgnPage.Close();

            }
            finally
            {
                conn.Close();
            }
        }
    }
}

