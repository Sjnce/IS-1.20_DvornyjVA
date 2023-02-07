using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IS._1_20_DvornyjVA
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            customizeButton();
        }

        #region Кастом формы и кнопок
        private void customizeButton()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
        }
        private void hideVisiblePanel()
        {
            if (panel1.Visible == true)
                panel1.Visible = false;
            if (panel2.Visible == true)
                panel2.Visible = false;
            if (panel3.Visible == true)
                panel3.Visible = false;
            if (panel4.Visible == true)
                panel4.Visible = false;
        }
        private void showVisiblePanel(Panel ItemPanel)
        {
            if (ItemPanel.Visible == false)
            {
                hideVisiblePanel();
                ItemPanel.Visible = true;
            }
            else
            {
                ItemPanel.Visible = false;
            }
        }
        #endregion

        #region Кнопки "Сортировка товара"
        private void button1_Click(object sender, EventArgs e) // Сортировка товара
        {
            showVisiblePanel(panel1);
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            //
            //
            //
            //hideVisiblePanel1();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //
            //
            //
            //hideVisiblePanel1();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //
            //
            //
            //hideVisiblePanel1();
        }
        #endregion 

        #region Кнопки "Сотрудники"
        private void button8_Click(object sender, EventArgs e) // Сотрудники
        {
            showVisiblePanel(panel2);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            openNewForm(new Form4());
        }
        private void button13_Click(object sender, EventArgs e) // Регистрация
        {
            openNewForm(new Form2());
        }
        #endregion 

        #region Кнопки "Поставка"
        private void button12_Click(object sender, EventArgs e) // Поставка товара
        {
            showVisiblePanel(panel3);
        }
        #endregion 

        #region Кнопки "Продажа"
        private void button19_Click(object sender, EventArgs e) // Продажа товара
        {
            showVisiblePanel(panel4);
        }
        #endregion

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
            panel6.Controls.Add(openNewForm);
            panel6.Tag = openNewForm;
            openNewForm.BringToFront();
            openNewForm.Show();
        }
        #endregion

        private void button15_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void button14_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
