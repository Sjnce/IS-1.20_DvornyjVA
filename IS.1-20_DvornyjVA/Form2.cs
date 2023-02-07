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
    public partial class Form2 : Form
    {

        #region Подключение к бд
        // строка подключения к БД
        string connStr = "server=chuc.caseum.ru;port=33333;user=st_1_20_10;database=is_1_20_st10_KURS;password=34088849;"; // chuc.caseum.ru - дома, 10.90.12.110 - в чюке
        //Переменная соединения
        MySqlConnection conn;
        #endregion

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(connStr);
        }

        #region Форма в форме
        private Form activeForm = null;
        private void openNewForm(Form openNewForm)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = openNewForm;
            openNewForm.TopLevel = false;
            openNewForm.FormBorderStyle = FormBorderStyle.None;
            openNewForm.Dock = DockStyle.Fill;
            panel3.Controls.Add(openNewForm);
            panel3.Tag = openNewForm;
            openNewForm.BringToFront();
            openNewForm.Show();
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

        #region Кнопка регистрации
        private void button4_Click(object sender, EventArgs e)
        {
            string fio_employee = textBox5.Text;
            string phone_employee = textBox4.Text;
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
            openNewForm(new Form3());
        }
        #endregion

    }
}
