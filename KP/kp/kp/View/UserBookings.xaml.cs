using kp.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace kp.View
{
    /// <summary>
    /// Логика взаимодействия для UserBookings.xaml
    /// </summary>
    public partial class UserBookings : Window
    {
        public UserBookings()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataIntoDataGrid();
        }
        private void LoadDataIntoDataGrid()
        {
            try
            {
                using (var dbContext = new Model.AppContext())
                {
                    var query =
                        from packages in dbContext.Packages
                        join destinations in dbContext.Destinations
                        on packages.destination_id equals destinations.destination_id
                        join touroperators in dbContext.TourOperators
                        on packages.tour_operator_id equals touroperators.tour_operator_id
                        join bookings in dbContext.Bookings
                        on packages.package_id equals bookings.package_id
                        join users in dbContext.Users
                        on bookings.users_id equals users.users_id
                        where users.users_id == GlobalVariables.ClientID
                        select new
                        {
                            bookings.booking_id,
                            destinations.country_name,
                            destinations.city_name,
                            packages.price,
                            touroperators.name,
                            packages.start_date,
                            packages.end_date,
                            bookings.booking_date,
                            bookings.payment_status
                        };
                    bookinfo.ItemsSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }
        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            Choose choose = new Choose();
            choose.Show();
            this.Close();
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            History history = new History();
            history.Show();
            this.Close();
        }

        private void BookingButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы уже в данном окне");
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
        private void PayButton_Click(Object sender, RoutedEventArgs e)
        {
            if (bookinfo.SelectedItem != null)
            {
                // Получаем выбранный объект из DataGrid
                dynamic selectedData = bookinfo.SelectedItem;
                int book_id = selectedData.booking_id;
                // Создаем новое окно
                AddPayment addPayment = new AddPayment(book_id);
                addPayment.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Выберите путевку.");
            }
        }
        private DataGridRow GetSelectedRow()
        {
            var row = (DataGridRow)bookinfo.ItemContainerGenerator.ContainerFromItem(bookinfo.SelectedItem);
            if (row != null && row.IsMouseOver)
            {
                return row;
            }
            return null;
        }
    }
}
