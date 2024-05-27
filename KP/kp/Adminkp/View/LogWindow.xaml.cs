using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Adminkp.View
{
    /// <summary>
    /// Логика взаимодействия для LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();
        }
        private void SwitchStackPanel_Click(object sender, RoutedEventArgs e)
        {
            // Переключение видимости StackPanel при нажатии кнопки
            stackPanelLogin.Visibility = stackPanelLogin.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            stackPanelRegistration.Visibility = stackPanelRegistration.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername2.Text;
            string password = txtPassword2.Password;
            string firstname = txtFirstName.Text;
            string lastname = txtLastName.Text;
            string address = txtAddress.Text;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname) || string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
            }
            else if (!Regex.IsMatch(username, @"^[\w]+$"))
            {
                MessageBox.Show("Логин может содержать только буквы, цифры и символ '_'");
            }
            else if (!IsAllLetters(firstname))
            {
                MessageBox.Show("Неверное значение имени");
            }
            else if (firstname.Contains(" "))
            {
                MessageBox.Show("Имя не может содержать пробелы");
            }
            else if (!IsAllLetters(lastname))
            {
                MessageBox.Show("Неверное значение фамилии");
            }
            else if (lastname.Contains(" "))
            {
                MessageBox.Show("Фамилия не может содержать пробелы");
            }
            else if (!IsValidEmail(address))
            {
                MessageBox.Show("Неверный формат адреса электронной почты");
            }
            else if (!Regex.IsMatch(password, @"^[\w\-.]+$"))
            {
                MessageBox.Show("Пароль может содержать только буквы, цифры, символы '_', '-' и '.'");
            }
            else if (password.Contains(" "))
            {
                MessageBox.Show("Пароль не может содержать пробелы");
            }
            else if (password.Length < 5)
            {
                MessageBox.Show("Пароль должен состоять из не менее, чем 6 символов");
            }
            else if (password.Length > 20)
            {
                MessageBox.Show("Пароль не может содержать более 20 символов");
            }
            else
            {
                if (RegisterUser(username, password, firstname, lastname, address))
                {
                   MainWindow mainWindow = new MainWindow();
                   mainWindow.Show();
                   this.Close();
                }
                else
                {
                    MessageBox.Show("Такой пользователь уже существует.");
                }
            }
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password))
            {
                // Проверка на пустые поля
                MessageBox.Show("Введите логин и пароль.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(username))
            {
                // Проверка на пустые поля
                MessageBox.Show("Введите логин.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                // Проверка на пустые поля
                MessageBox.Show("Введите пароль.");
                return;
            }
            else
            {
                if (AuthenticateUser(username, password))
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
        }
        public bool AuthenticateUser(string username, string password)
        {
            using (var dbContext = new Model.ApplicationContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.login_user == username);

                if (user != null && user.password_user == password)
                {
                    // Проверяем роль пользователя
                    if (user.Roles != null && user.Roles.role_name == "Администратор")
                    {
                        // Пользователь авторизован и имеет роль "Пользователь"
                        GlobalVariablesAdmin.AdminID = user.users_id;
                        return true;
                    }
                }
                return false;
            }
        }
        private bool RegisterUser(string username, string password, string firstname, string lastname, string address)
        {
            using (var dbContext = new Model.ApplicationContext())
            {
                try
                {
                    if (dbContext.Users.Any(u => u.login_user == username))
                    {
                        return false;
                    }

                    var newUser = new Users
                    {
                        login_user = username,
                        password_user = password,
                        first_name = firstname,
                        last_name = lastname,
                        email = address,
                        role_id = dbContext.Roles.FirstOrDefault(r => r.role_name == "Администратор")?.role_id
                    };
                    dbContext.Users.Add(newUser);
                    dbContext.SaveChanges();
                    GlobalVariablesAdmin.AdminID = newUser.users_id;
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении данных тура: {ex.Message}");
                    return false;
                }
            }
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
