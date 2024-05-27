using kp.Model;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для History.xaml
    /// </summary>
    public partial class History : Window
    {
        public History()
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
                        join payments in dbContext.Payments 
                        on bookings.booking_id equals payments.booking_id
                        where users.users_id == GlobalVariables.ClientID
                        select new
                        {
                            payments.payment_id,
                            destinations.country_name,
                            destinations.city_name,
                            packages.price,
                            touroperators.name,
                            packages.start_date,
                            packages.end_date,
                            payments.payment_date
                        };
                    histinfo.ItemsSource = query.ToList();
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
            MessageBox.Show("Вы уже в данном окне");
        }

        private void BookingButton_Click(object sender, RoutedEventArgs e)
        {
            UserBookings bookings = new UserBookings();
            bookings.Show();
            this.Close();
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow log = new LoginWindow();
            log.Show();
            this.Close();
        }
    }
}
