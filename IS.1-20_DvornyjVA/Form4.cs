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
    public partial class Form4 : Form
    {
        // строка подключения к БД
        string connStr = "server=chuc.caseum.ru;port=33333;user=st_1_20_10;database=is_1_20_st10_KURS;password=34088849;"; // chuc.caseum.ru - дома, 10.90.12.110 - в чюке
        //Переменная соединения
        MySqlConnection conn;
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(connStr);
        }
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

        private void button4_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            //string password = sha256(textBox2.Text);
            string sql = $"DELETE FROM Employee WHERE login = {login}";
            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            Update();
            MessageBox.Show("Сотрудник удалён.");
        }
    }
}
