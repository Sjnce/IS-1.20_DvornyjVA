﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;

namespace IS._1_20_DvornyjVA
{
    public partial class InForm : Form
    {

        #region подключение к бд
        // строка подключения к БД
        string connStr = "server=10.90.12.110;port=33333;user=st_1_20_10;database=is_1_20_st10_KURS;password=34088849;"; // chuc.caseum.ru - дома, 10.90.12.110 - в чюке
        //Переменная соединения
        MySqlConnection conn;
        //Логин и пароль к данной форме Вы сможете посмотреть в БД db_test таблице t_user

        //Вычисление хэша строки и возрат его из метода
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
        public void GetUserInfo(string login_user)
        {
            //Объявлем переменную для запроса в БД
            string selected_id_stud = textBox1.Text;
            // устанавливаем соединение с БД
            conn.Open();
            // запрос
            string sql = $"SELECT fio_employee, id_employee, login From Employee WHERE login='{login_user}'";
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(sql, conn);
            // объект для чтения ответа сервера
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            while (reader.Read())
            {
                // элементы массива [] - это значения столбцов из запроса SELECT
                Auth.auth_id = reader[1].ToString();
                Auth.auth_fio = reader[0].ToString();
                //Auth.auth_role = Convert.ToInt32(reader[4].ToString());
            }
            reader.Close(); // закрываем reader
            // закрываем соединение с БД
            conn.Close();
        }
#endregion

        public InForm()
        {
            InitializeComponent();
            textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None; //скрытие рамок у textbox
            textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None; //скрытие рамок у textbox
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(connStr);
        }

        #region Вход в главную форму
        public delegate void ThreadStart();
        void Vhod()
        {
            this.Invoke(new Action(() => { this.Hide(); })); // Новый поток
            LoadingForm loa = new LoadingForm();
            loa.Show();
            //Запрос в БД на предмет того, если ли строка с подходящим логином, паролем
            string sql = "SELECT * FROM Employee WHERE login = @un and password= @up";
            //Открытие соединения
            conn.Open();
            //Объявляем таблицу
            DataTable table = new DataTable();
            //Объявляем адаптер
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            //Объявляем команду
            MySqlCommand command = new MySqlCommand(sql, conn);
            //Определяем параметры
            command.Parameters.Add("@un", MySqlDbType.VarChar, 25);
            command.Parameters.Add("@up", MySqlDbType.VarChar, 25);
            //Присваиваем параметрам значение
            command.Parameters["@un"].Value = textBox1.Text;
            command.Parameters["@up"].Value = sha256(textBox2.Text);
            //Заносим команду в адаптер
            adapter.SelectCommand = command;
            //Заполняем таблицу
            adapter.Fill(table);
            //Закрываем соединение
            conn.Close();
            //Если вернулась больше 0 строк, значит такой пользователь существует
            if (table.Rows.Count > 0)
            {
                //Присваеваем глобальный признак авторизации
                Auth.auth = true;
                //Достаем данные пользователя в случае успеха
                GetUserInfo(textBox1.Text);

                Thread.Sleep(5000); // Новый поток
                textBox1.Invoke(new Action(() => { this.Close(); })); // Новый поток
            }
            else
            {
                //Отобразить сообщение о том, что авторизаия неуспешна
                labelError1.Text = "Неверные данные авторизации!";
                labelError2.Text = "Неверные данные авторизации!";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            Thread myThread1 = new Thread(Vhod); // Создаем новый поток
            myThread1.Start();
        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
