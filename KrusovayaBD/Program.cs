using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KrusovayaBD
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Login login = new Login();
            login.StartPosition = FormStartPosition.CenterScreen;
            login.ShowDialog();

            if (login.авторизован)
            {
                Application.Run(new Main());
            }
            else MessageBox.Show("Неправильный логин или пароль.");
        }
    }
}
