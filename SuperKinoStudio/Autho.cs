using SuperKinoStudio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperKinoStudio
{
    public partial class Autho : Form
    {
        KinoStudioEntities entities = new KinoStudioEntities();
        Users users = new Users();
        public Autho()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Registration().ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Проверка наличия пользователя с указанным логином в базе данных
            var user = entities.Users.FirstOrDefault(u => u.Login == username);

            if (user != null)
            {
                // Проверка соответствия пароля
                if (user.Password == password)
                {
                    // Пользователь найден, перенаправляем на соответствующую форму в зависимости от его роли
                    if (user.RoleId == 1)
                    {
                        new Client().Show();
                    }
                    else if (user.RoleId == 2)
                    {
                        new Employee(user.UserId).Show();
                    }
                    else if (user.RoleId == 3)
                    {
                        new President().Show();
                    }
                }
                else
                {
                    // Вывод сообщения об ошибке - неправильный пароль
                    MessageBox.Show("Неправильный пароль");
                }
            }
            else
            {
                // Вывод сообщения об ошибке - пользователь с указанным логином не найден
                MessageBox.Show("Пользователь с указанным логином не найден");
            }

        }
    }
}
