using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SekerProje_v2
{
    internal class SqlClass
    {
        public SqlConnection connect2Database()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=AYSE;Initial Catalog=SekerDB;Integrated Security=True");
            return conn;
        }
        private void createPersonelInfoTable()
        {
            string tableName = "PersonelInfo";
            using (SqlConnection connection = connect2Database())
            {
                connection.Open();
                string createTableQuery = $@"CREATE TABLE {tableName} (
                                            KisiID INT PRIMARY KEY IDENTITY(1,1),
                                            Isim NVARCHAR(50) NOT NULL,
                                            Soyisim NVARCHAR(50) NOT NULL,
                                            TCno NVARCHAR(11) NOT NULL,
                                            Departman NVARCHAR(50),
                                            CardID NVARCHAR(50),
                                            VALID_CARD BIT NOT NULL
                                            )";
                using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Tablo oluşturma hatası: {ex.Message}");
                    }
                }
            }
        }

        private void createWorkLogTable()
        {
            string tableName = "WorkLog";
            using (SqlConnection connection = connect2Database())
            {
                connection.Open();
                string createTableQuery = $@"CREATE TABLE {tableName} (
                                            WorkLogID INT PRIMARY KEY IDENTITY(1,1),
                                            CardID NVARCHAR(50) NOT NULL,
                                            EmployeeID NVARCHAR(50) NOT NULL,
                                            EntranceTime DATETIME,
                                            ExitTime DATETIME NULL,
                                            IsPresent BIT
                                            )";
                using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Tablo oluşturma hatası: {ex.Message}");
                    }
                }
            }
        }
        private void createCardTable()
        {
            string tableName = "CardTable";
            using (SqlConnection conn = connect2Database())
            {
                conn.Open();
                string createTableQuery = $@"CREATE TABLE {tableName} (
                                            CardTableID INT PRIMARY KEY IDENTITY(1,1),
                                            ID_NO NVARCHAR(12) ,
                                            FIRST_NAME NVARCHAR(50),
                                            LAST_NAME NVARCHAR(50),
                                            DEPARTMENT NVARCHAR(50),
                                            FirstCard NVARCHAR(50),
                                            SecondCard NVARCHAR(50) NULL, 
                                            ThirdCard NVARCHAR(50) NULL 
                                            )";
                using (SqlCommand cmd = new SqlCommand(createTableQuery, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show($"error {ex.Message}");
                    }
                }
            }
        }
        public void CreateAllTables()
        {
            createPersonelInfoTable();
            createWorkLogTable();
            createCardTable();
        }
        public void insertToWorkLog(string cardID, string employeeID, DateTime entranceTime, bool isPresent)
        {
            using (SqlConnection connection = connect2Database())
            {
                connection.Open();
                string insertQuery = @"INSERT INTO WorkLog (CardID, EmployeeID, EntranceTime, IsPresent) 
                                        VALUES (@CardID, @EmployeeID, @EntranceTime, @IsPresent)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@CardID", cardID);
                    command.Parameters.AddWithValue("@EmployeeID", employeeID);
                    command.Parameters.AddWithValue("@EntranceTime", entranceTime);
                    command.Parameters.AddWithValue("@IsPresent", isPresent);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Veri başarıyla eklendi.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Veri ekleme hatası: {ex.Message}");
                    }
                }
            }
        }
        public string insertToPersonelLog(SqlConnection conn, string name, string surname, string tcNo, string department)
        {

            SignUpPage signUpPage = new SignUpPage();

            string sqlQuery = "INSERT INTO [PersonelInfo] ([Isim], [Soyisim], [TCno], [Departman], [CardID], [VALID_CARD]) VALUES (@Isim, @Soyisim, @TCno, @Departman, @CardID, @Validity)";
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            cmd.Parameters.AddWithValue("@Isim", name);
            cmd.Parameters.AddWithValue("@Soyisim", surname);
            cmd.Parameters.AddWithValue("@TCno", tcNo);
            cmd.Parameters.AddWithValue("@Departman", department);
            cmd.Parameters.AddWithValue("@Validity", 1);
            CardReadingPage readedCard = new CardReadingPage();
            signUpPage.Hide();
            readedCard.ShowDialog();

            cmd.Parameters.AddWithValue("@CardID", readedCard.prsnlCardId);
            cmd.ExecuteNonQuery();

            string message = "Durum: " + isEmployeeIDExists(tcNo).ToString();
            MessageBox.Show(message);
            if (!isEmployeeIDExists(tcNo))
            {
                string insertCardQuery = "INSERT INTO [CardTable] ([ID_NO], [FIRST_NAME], [LAST_NAME], [DEPARTMENT], [FirstCard]) VALUES (@CalisanID, @Name, @Surname, @Department, @FirstCard)";
                SqlCommand insertCardCmd = new SqlCommand(insertCardQuery, conn);
                insertCardCmd.Parameters.AddWithValue("@CalisanID", tcNo);
                insertCardCmd.Parameters.AddWithValue("@Name", name);
                insertCardCmd.Parameters.AddWithValue("@Surname", surname);
                insertCardCmd.Parameters.AddWithValue("@Department", department);
                insertCardCmd.Parameters.AddWithValue("@FirstCard", readedCard.prsnlCardId);
                insertCardCmd.ExecuteNonQuery();
            }
            return readedCard.prsnlCardId;
        }
        public bool isEmployeeIDExists(string id)
        {
            using (SqlConnection conn = connect2Database())
            {
                conn.Open();
                string selectQuery = "SELECT COUNT(*) FROM [CardTable] WHERE [ID_NO] = @IDNo";
                SqlCommand cmd = new SqlCommand(selectQuery, conn);
                cmd.Parameters.AddWithValue("@IDno", id);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        private void insertTo(Node node, SqlConnection conn)
        {
            string insertQuery = $@"INSERT INTO [WorkLog] ([CardID], [EmployeeID],  [EntranceTime], [ExitTime], [IsPresent])
                                 VALUES (@CardID, @EmployeeID,  @EntranceTime, @ExitTime, @IsPresent)";

            using (SqlCommand command = new SqlCommand(insertQuery, conn))
            {
                command.Parameters.AddWithValue("@CardID", node.prsnlCardId);
                command.Parameters.AddWithValue("@EmployeeID", node.prsnlId);

                command.Parameters.AddWithValue("@EntranceTime", DateTime.Now);
                command.Parameters.AddWithValue("@ExitTime", DBNull.Value);
                command.Parameters.AddWithValue("@IsPresent", 1);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"{rowsAffected} kayıt başarıyla eklendi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Kayıt ekleme hatası: {ex.Message}");
                }
            }
        }
        private void updateTo(Node node , SqlConnection conn)
        {
            string updateQuery = "UPDATE [WorkLog] SET [ExitTime] = @ExitTime, [IsPresent] = 0 WHERE [CardID] = @CardID AND [IsPresent] = 1 ";
            using (SqlCommand updateCommand = new SqlCommand(updateQuery, conn))
            {
                updateCommand.Parameters.AddWithValue("@ExitTime", DateTime.Now);
                updateCommand.Parameters.AddWithValue("@CardID", node.prsnlCardId);
                int rowsUpdated = updateCommand.ExecuteNonQuery();
                MessageBox.Show($"{rowsUpdated} kayıt başarıyla güncellendi.");
            }
        }
        public void insertToWorklog(Node node)
        {
            SqlConnection conn = connect2Database();
            conn.Open();
            if (node != null)
            {
                string checkIfEmployeeExistsQuery = "SELECT IsPresent FROM [WorkLog] WHERE [CardID] = @CardID ORDER BY EntranceTime DESC";
                SqlCommand checkCmd = new SqlCommand(checkIfEmployeeExistsQuery, conn);
                checkCmd.Parameters.AddWithValue("@CardID", node.prsnlCardId);
                object existingEmployeeID = checkCmd.ExecuteScalar();

                string checkValidCardQuery = "SELECT CardID FROM [PersonelInfo] WHERE [VALID_CARD] = 0";
                SqlCommand checkValidityCmd = new SqlCommand(checkValidCardQuery, conn);
                object validCardID = checkValidityCmd.ExecuteScalar();
                int isPresent = Convert.ToInt32(existingEmployeeID);
                MessageBox.Show(validCardID.ToString());
                if (validCardID != null)
                {
                    if (existingEmployeeID != null)
                    {
                        if (isPresent == 1)
                        {
                            if (node.prsnlCardId == validCardID.ToString())
                            {
                                MessageBox.Show("kaybolan karti girdiniz. size tahsis edilen yeni karti kullaniiniz");
                            }
                            else
                            {
                                updateTo(node, conn);
                               
                            }
                        }
                        else
                        {
                            if (isPresent == 1)
                            {
                                updateTo(node, conn);
                            }
                            else
                                insertTo(node, conn);
                        }


                    }
                    else
                    {


                        if (node.prsnlCardId == validCardID.ToString())
                        {
                            MessageBox.Show("kaybolan karti girdiniz. size tahsis edilen yeni karti kullaniiniz");
                        }

                        else
                        {
                            insertTo(node, conn);
                        }

                    }
                }
                else 
                {
                    if (isPresent == 1)
                    {
                        updateTo(node, conn);
                    }
                    else
                        insertTo(node, conn);
                }
                
            }
            else 
            {
                MessageBox.Show("Belirtilen ID numarasına sahip personel bulunamadı.");
            }
            conn.Close();
        }
        public Node selectFromPersonelInfo(string cardId)
        {
            SqlConnection conn = connect2Database();
            Node node = new Node();

            using (conn)
            {
                try
                {
                    conn.Open();
                    string selectQuery = "SELECT * FROM [PersonelInfo] WHERE TCno = (SELECT ID_NO FROM [CardTable] WHERE [FirstCard] = @CardID OR [SecondCard] = @CardID)";
                    SqlCommand cmd = new SqlCommand(selectQuery, conn);
                    cmd.Parameters.AddWithValue("@CardID", cardId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            node.prsnlCardId = cardId;
                            node.prsnlName = reader["Isim"].ToString();
                            node.prsnlSurname = reader["Soyisim"].ToString();
                            node.prsnlId = reader["TCno"].ToString();
                            node.prsnlDepartment = reader["Departman"].ToString();

                        }
                        else
                        {
                            MessageBox.Show("Belirtilen ID numarasına sahip personel bulunamadı.");

                        }
                    }
                    return node;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata:  qweqw " + ex.Message);
                    return null;
                }
            }


        }
    }
}
