using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Adminkp.Model;
using Adminkp.Repository;

namespace Adminkp.View
{
    /// <summary>
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void CancButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка и парсинг значений из TextBox'ов
                string userName = UserName.Text;
                if (string.IsNullOrWhiteSpace(userName))
                {
                    MessageBox.Show("Имя пользователя не может быть пустым");
                    return;
                }
                else if (!IsAllLetters(userName))
                {
                    MessageBox.Show("Неверное значение имени");
                    return;
                }

                string userLastName = UserLastName.Text;
                if (string.IsNullOrWhiteSpace(userLastName))
                {
                    MessageBox.Show("Фамилия пользователя не может быть пустой");
                    return;
                }
                else if (!IsAllLetters(userLastName))
                {
                    MessageBox.Show("Неверное значение фамилии");
                    return;
                }

                string userLog = UserLog.Text;
                if (string.IsNullOrWhiteSpace(userLog))
                {
                    MessageBox.Show("Логин пользователя не может быть пустым");
                    return;
                }
                else if (!Regex.IsMatch(userLog, @"^[\w]+$"))
                {
                    MessageBox.Show("Логин может содержать только буквы, цифры и символ '_'");
                    return;
                }
                using (var dbContext = new Model.ApplicationContext())
                {
                    if (dbContext.Users.Any(u => u.login_user == userLog))
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует");
                        return;
                    }
                }

                    string userPass = UserPass.Text;
                if (string.IsNullOrWhiteSpace(userPass))
                {
                    MessageBox.Show("Пароль пользователя не может быть пустым");
                    return;
                }
                else if (!Regex.IsMatch(userPass, @"^[\w\-.]+$"))
                {
                    MessageBox.Show("Пароль может содержать только буквы, цифры, символы '_', '-' и '.'");
                    return;
                }
                else if (userPass.Length < 5)
                {
                    MessageBox.Show("Пароль должен состоять из не менее, чем 6 символов");
                    return;
                }
                else if (userPass.Length > 20)
                {
                    MessageBox.Show("Пароль не может содержать более 20 символов");
                    return;
                }

                string userAddress = UserAddress.Text;
                if (string.IsNullOrWhiteSpace(userAddress))
                {
                    MessageBox.Show("Электронный адрес не может быть пустым");
                    return;
                }
                else if (!IsValidEmail(userAddress))
                {
                    MessageBox.Show("Неверный формат адреса электронной почты");
                    return;
                }

                if (!int.TryParse(UserRoleId.Text, out int userRoleId))
                {
                    MessageBox.Show("Некорректное значение для идентификатора роли");
                    return;
                }
                UserRepository userRepository = new UserRepository();
                userRepository.AddUser(userName, userLastName, userLog, userPass, userAddress, userRoleId);
                MessageBox.Show("Пользователь успешно создан и добавлен в базу данных");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании и добавлении пользователя: {ex.Message}");
            }
            this.Close();
        }
        public bool IsAllLetters(string input)
        {
            return input.All(char.IsLetter);
        }
        private bool IsValidEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == email && email.IndexOf('@') > 0;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
