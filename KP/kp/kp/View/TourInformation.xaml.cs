using kp.Model;
using kp.Repository;
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
    /// Логика взаимодействия для TourInformation.xaml
    /// </summary>
    public partial class TourInformation : Window
    {
        public TourInformation(int pack_id)
        {
            InitializeComponent();
            using (var dbContext = new Model.AppContext())
            {
                if (pack_id != 0)
                {
                    var query =
                        from packages in dbContext.Packages
                        join destinations in dbContext.Destinations
                        on packages.destination_id equals destinations.destination_id
                        join touroperators in dbContext.TourOperators
                        on packages.tour_operator_id equals touroperators.tour_operator_id
                        where packages.package_id == pack_id
                        select new
                        {
                            packages.description,
                            packages.package_id,
                            packages.price,
                            destinations.country_name,
                            destinations.city_name,
                            touroperators.name,
                            packages.start_date,
                            packages.end_date
                        };
                    var result = query.FirstOrDefault();
                    if (result != null)
                    {
                        decimal pr = result.price;
                        string price = pr.ToString();
                        System.DateTime start = result.start_date;
                        System.DateTime end = result.end_date;
                        string strt = start.ToString();
                        string nd = end.ToString();
                        int p_id = result.package_id;
                        string packag_id = p_id.ToString();
                        txtTourNumber.Text = packag_id;
                        txtTourOperator.Text = result.name;
                        txtCountry.Text = result.country_name;
                        txtCity.Text = result.city_name;
                        txtPrice.Text = price;
                        txtStartDate.Text = strt;
                        txtEndDate.Text = nd;
                        txtDescription.Text = result.description;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: Данные отсутствуют.");
                    }
                }            
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
        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BookingRepository bookingRepository = new BookingRepository();
                if (int.TryParse(txtTourNumber.Text, out int tour_number))
                {
                    bookingRepository.AddBooking(GlobalVariables.ClientID, tour_number);
                    MessageBox.Show("Бронирование произведено успешно");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}");
            }
        }
    }
}
