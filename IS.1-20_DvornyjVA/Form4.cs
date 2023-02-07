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

        #region Подключение к бд
        // строка подключения к БД
        string connStr = "server=chuc.caseum.ru;port=33333;user=st_1_20_10;database=is_1_20_st10_KURS;password=34088849;"; // chuc.caseum.ru - дома, 10.90.12.110 - в чюке
        //Переменная соединения
        MySqlConnection conn;
        #endregion

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
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
        private void button4_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                DeleteRow(textBox1.Text);
            }  
            openNewForm(new Form3());
        }
        #endregion

    }
}
