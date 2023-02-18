using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace IS._1_20_DvornyjVA
{
    public partial class EmpForm : Form
    {

        #region Подключение к бд
        // строка подключения к БД
        string connStr = "server=10.90.12.110;port=33333;user=st_1_20_10;database=is_1_20_st10_KURS;password=34088849;"; // chuc.caseum.ru - дома, 10.90.12.110 - в чюке
        //Переменная соединения
        MySqlConnection conn;
        #endregion

        public EmpForm()
        {
            InitializeComponent();
            select();
            dataGridView1.AllowUserToAddRows = false; // скрытие нижней строки в гриде
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(connStr);
            dataGridView1.RowHeadersVisible = false;
        }

        #region dataGridView1
        public void select() //заполнение DataGridView
        {
            DataSet ds;
            ds = new DataSet();
            string connStr = "server=10.90.12.110;port=33333;user=st_1_20_10;database=is_1_20_st10_KURS;password=34088849;";
            MySqlConnection connection = new MySqlConnection(connStr);

            MySqlCommand command = new MySqlCommand();
            string commandString = "SELECT * FROM Employee;";
            command.CommandText = commandString;
            command.Connection = connection;
            MySqlDataReader reader;
            try
            {
                command.Connection.Open();
                reader = command.ExecuteReader();
                this.dataGridView1.Columns.Add("id_employee", "id");
                this.dataGridView1.Columns.Add("fio_employee", "Ф.И.О.");
                this.dataGridView1.Columns.Add("phone_employee", "Номер телефона");
                this.dataGridView1.Columns.Add("login", "Логин");
                this.dataGridView1.Columns.Add("password", "Пароль");
                this.dataGridView1.Columns.Add("post", "Должность");
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(reader["id_employee"].ToString(), reader["fio_employee"].ToString(), 
                        reader["phone_employee"].ToString(), reader["login"].ToString(),
                        reader["password"].ToString(), reader["post"].ToString());
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: \r\n{0}", ex.ToString());
            }
            finally
            {
                command.Connection.Close();
            }
        }

        void readnig() //метод получения записей, который вновь заполнит таблицу
        {
            DataSet ds;
            ds = new DataSet();
            string connStr = "server=chuc.caseum.ru;port=33333;user=st_1_20_10;database=is_1_20_st10_KURS;password=34088849;";
            MySqlConnection connection = new MySqlConnection(connStr);

            MySqlCommand command = new MySqlCommand();
            string commandString = "SELECT * FROM Employee;";
            command.CommandText = commandString;
            command.Connection = connection;
            MySqlDataReader reader;
            try
            {
                command.Connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(reader["id_employee"].ToString(), reader["fio_employee"].ToString(), 
                        reader["phone_employee"].ToString(), reader["login"].ToString(),
                        reader["password"].ToString(), reader["post"].ToString());
                }
                reader.Close();
            }
            catch
            {
                command.Connection.Close();
            }
        }
        #endregion

        #region Хеширование
        static string sha256(string randomString)
        {
            //Тут происходит криптографическая магия. Смысл данного метода заключается в том, что строка залетает в метод
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        #endregion

        #region удаление строчки в СУБД
        private void DeleteRow(string login)
        {
            using (MySqlConnection con = new MySqlConnection(connStr))
            {
                try
                {
                    //параметризованный запрос
                    string sql = "DELETE FROM Employee " +
                    "WHERE login = @login";
                    //открываем соединение с базой данных
                    con.Open();
                    //создаём команду
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    //создаем параметр и добавляем его в коллекцию
                    cmd.Parameters.AddWithValue("@login", login);
                    //выполняем sql запрос
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                DeleteRow(textBox1.Text); // Удаление по id
            }

            dataGridView1.Rows.Clear(); // Очистка
            readnig(); // Обновление грита
        }
        #endregion

        #region Регистрация
        private void button4_Click_1(object sender, EventArgs e)
        {
            string fio_employee = textBox5.Text;
            string phone_employee = maskedTextBox1.Text;
            string login = textBox1.Text;
            string password = sha256(textBox2.Text);
            string post = textBox6.Text;
            string sql = $"INSERT INTO Employee(fio_employee, phone_employee, login, password, post)"
                + $"VALUES ('{fio_employee}', '{phone_employee}', '{login}', '{password}', '{post}')";
            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            Update();

            dataGridView1.Rows.Clear(); // Очистка
            readnig(); // Обновление грита
        }
        #endregion

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
