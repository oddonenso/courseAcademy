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
using System.IO;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

namespace SuperKinoStudio
{
    public partial class Registration : Form
    {
        KinoStudioEntities entities = new KinoStudioEntities();
        Users users = new Users();
        OpenFileDialog openFileDialog1 = new OpenFileDialog();

        public Registration()
        {
            InitializeComponent();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            string surname = textBox2.Text.Trim();
            string midname = textBox3.Text.Trim();
            string login = textBox4.Text.Trim();
            string password = textBox5.Text;
            string email = textBox6.Text.Trim();
            string phone = textBox7.Text.Trim();
            string role = RoleBox.Text;
            int age = (int)numericUpDown1.Value;
            string gender = genderBox.Text;

            if (!Regex.IsMatch(name, @"^[A-Za-zА-Яа-я]+$"))
            {
                MessageBox.Show("Имя должно содержать только буквы.");
                return;
            }

            if (!Regex.IsMatch(surname, @"^[A-Za-zА-Яа-я]+$"))
            {
                MessageBox.Show("Фамилия должна содержать только буквы.");
                return;
            }

            if (!Regex.IsMatch(midname, @"^[A-Za-zА-Яа-я]+$"))
            {
                MessageBox.Show("Отчество должно содержать только буквы.");
                return;
            }
            var existingUser = entities.Users.FirstOrDefault(u => u.Login == login);
            if (existingUser != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует.");
                return;
            }

            if (!Regex.IsMatch(phone, @"^\+7\(\d{3}\)-\d{3}-\d{2}-\d{2}$"))
            {
                MessageBox.Show("Неправильный формат номера телефона. Используйте формат +7(999)-999-99-99.");
                return;
            }

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Неправильный формат адреса электронной почты.");
                return;
            }

            users.Name = name;
            users.Surname = surname;
            users.Midname = midname;
            users.Login = login;
            users.Password = password;
            users.Email = email;
            users.PhoneNumber = phone;
            users.RoleId = GetRoleId(role);
            users.GenderId = GetGenderId(gender);
            users.Age = age;

            entities.Users.Add(users);
            entities.SaveChanges();

            MessageBox.Show("Пользователь создан.");

            // Clear fields after saving
            ClearFields(); 



        }
        private void ClearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            RoleBox.SelectedIndex = -1;
            genderBox.SelectedIndex = -1;
            numericUpDown1.Value = 0;
            pictureBox1.Image = null;
        }
        private int GetRoleId(string roleName)
        {
            var role = entities.Role.FirstOrDefault(r => r.RoleName == roleName);
            if (role != null)
            {
                return role.RoleId;
            }
            else
            {
                // Можно бросить исключение или вернуть значение по умолчанию
                throw new InvalidOperationException($"Пол '{roleName}' не найден.");
            }
        }

        private int GetGenderId(string genderName)
        {
            var gender = entities.Gender.FirstOrDefault(g => g.GenderName == genderName);
            if (gender != null)
            {
                return gender.GenderId;
            }
            else
            {
                throw new InvalidOperationException($"Пол '{genderName}' не найден.");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.gif,*.png)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG";
            openFileDialog1.Title = "Select Image File";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Read image to byte array
                byte[] imageBytes = File.ReadAllBytes(openFileDialog1.FileName);
                users.Image = imageBytes;

                // Отображаем изображение в PictureBox
                pictureBox1.Image = Image.FromStream(new MemoryStream(imageBytes));
                // Зумируем изображение
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
