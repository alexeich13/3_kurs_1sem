using Adminkp.Model;
using Adminkp.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Adminkp.View
{
    /// <summary>
    /// Логика взаимодействия для InfoUsers.xaml
    /// </summary>
    public partial class InfoUsers : Page
    {
        private readonly UserRepository userRepository;
        private DispatcherTimer timer;
        public InfoUsers(UserRepository _userRepository)
        {
            InitializeComponent();
            userRepository = _userRepository;
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            LoadDataInGrid();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataInGrid();
        }
        private void LoadDataInGrid()
        {
            using (var dbContext = new Model.ApplicationContext())
            {
                var query =
                    from users in dbContext.Users
                    join roles in dbContext.Roles
                    on users.role_id equals roles.role_id
                    select new
                    {
                        users.users_id,
                        users.first_name,
                        users.last_name,
                        users.login_user,
                        users.password_user,
                        users.email,
                        roles.role_name
                    };
                iform.ItemsSource = query.ToList();
            }
        }
        private void ShowData(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (iform.SelectedItem != null)
            {
                dynamic selectedRow = iform.SelectedItem;
                UserId.Text = GetValueOrDefault(selectedRow, "users_id");
                UserName.Text = GetValueOrDefault(selectedRow, "first_name");
                UserLastName.Text = GetValueOrDefault(selectedRow, "last_name");
                UserLog.Text = GetValueOrDefault(selectedRow, "login_user");
                UserPass.Text = GetValueOrDefault(selectedRow, "password_user");
                UserAddress.Text = GetValueOrDefault(selectedRow, "email");
                UserRole.Text = GetValueOrDefault(selectedRow, "role_name");
            }
        }
        private string GetValueOrDefault(dynamic obj, string propertyName)
        {
            try
            {
                return obj.GetType().GetProperty(propertyName)?.GetValue(obj)?.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (iform.SelectedItem != null)
            {
                dynamic selectedData = iform.SelectedItem;
                int us_id = selectedData.users_id;
                DeleteUser(us_id);
            }
            else
            {
                MessageBox.Show("Выберите пользователя.");
            }
        }
        private void DeleteUser(int userId)
        {
                int current_id = GlobalVariablesAdmin.AdminID;
                if (userId == current_id)
                {
                    MessageBox.Show("Нельзя удалить самого себя");
                    return;
                }
                try
                {
                    userRepository.DelUser(userId);
                    MessageBox.Show("Пользователь успешно удален");
                    LoadDataInGrid(); 
                }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении пользователя, он имеет бронирования: {ex.Message}");
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddUser addUser = new AddUser();
            addUser.ShowDialog();
        }
        private void Dob_Click(object sender, EventArgs e)
        {
            // Не выполняйте никаких дополнительных действий внутри этого обработчика
        }
    }
}
