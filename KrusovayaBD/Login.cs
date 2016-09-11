using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace KrusovayaBD
{
    public partial class Login : Form
    {
        DataTableAdapters.Таблица_Пользователи пользователи = new DataTableAdapters.Таблица_Пользователи();

        public bool авторизован = false;
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings[1].ConnectionString = "Data Source = " + textBox3.Text + "; Initial Catalog = KursovayaBD; Integrated Security = True";
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");

            try {
                if ((int)пользователи.авторизация(textBox1.Text, textBox2.Text) > 0)
                {
                    авторизован = true;
                }
                this.Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
