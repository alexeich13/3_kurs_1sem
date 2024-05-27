using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using Adminkp.Model;
using Adminkp.Repository;

namespace Adminkp.View
{
    /// <summary>
    /// Логика взаимодействия для InfoBookings.xaml
    /// </summary>
    public partial class InfoBookings : Page
    {
        private DispatcherTimer timer;
        public InfoBookings()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
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
                    from packages in dbContext.Packages
                    join bookings in dbContext.Bookings
                    on packages.package_id equals bookings.package_id
                    join users in dbContext.Users
                    on bookings.users_id equals users.users_id
                    select new
                    {
                        bookings.booking_id,
                        bookings.booking_date,
                        users.login_user,
                        packages.description,
                        packages.start_date,
                        packages.end_date,
                        bookings.payment_status
                    };
                iform.ItemsSource = query.ToList();
            }
        }
        private void ShowData(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(iform.SelectedItem != null)
            {
                dynamic selectedRow = iform.SelectedItem;
                BookId.Text = GetValueOrDefault(selectedRow, "booking_id");
                BookDate.Text = GetValueOrDefault(selectedRow, "booking_date");
                UserLog.Text = GetValueOrDefault(selectedRow, "login_user");
                TourName.Text = GetValueOrDefault(selectedRow, "description");
                StartDate.Text = GetValueOrDefault(selectedRow, "start_date");
                EndDate.Text = GetValueOrDefault(selectedRow, "end_date");
                BookStatus.Text = GetValueOrDefault(selectedRow, "payment_status");
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (iform.SelectedItem != null)
            {
                // Получаем выбранный объект из DataGrid
                dynamic selectedData = iform.SelectedItem;
                int book_id = selectedData.booking_id;
                try
                {
                    using (var dbContext = new Model.ApplicationContext())
                    {
                        // Создаем экземпляр класса, содержащего метод CancelBooking
                        var bookingRepository = new BookingRepository();

                        // Вызываем метод CancelBooking
                        bookingRepository.CancelBooking(book_id);

                        MessageBox.Show("Бронирование успешно отменено");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите бронирование.");
            }
        }
        private void Dob_Click(object sender, EventArgs e)
        {
            // Не выполняйте никаких дополнительных действий внутри этого обработчика
        }
    }
}
