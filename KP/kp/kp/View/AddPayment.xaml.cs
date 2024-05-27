using kp.Model;
using kp.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для AddPayment.xaml
    /// </summary>
    public partial class AddPayment : Window
    {
        public AddPayment(int book_id)
        {
            InitializeComponent();
            using (var dbContext = new Model.AppContext())
            {
                if (book_id != 0)
                {
                    var query =
                        from packages in dbContext.Packages
                        join destinations in dbContext.Destinations
                        on packages.destination_id equals destinations.destination_id
                        join touroperators in dbContext.TourOperators
                        on packages.tour_operator_id equals touroperators.tour_operator_id
                        join bookings in dbContext.Bookings
                        on packages.package_id equals bookings.package_id
                        where bookings.booking_id == book_id
                        select new
                        {
                            bookings.booking_id,
                            packages.price,
                            destinations.country_name,
                            destinations.city_name,
                            touroperators.name,
                            packages.start_date,
                            packages.end_date,
                            bookings.booking_date
                        };
                    var result = query.FirstOrDefault();
                    if (result != null)
                    {
                        decimal pr = result.price;
                        string price = pr.ToString();
                        System.DateTime start = result.start_date;
                        System.DateTime end = result.end_date;
                        System.DateTime book = result.booking_date;
                        string strt = start.ToString();
                        string nd = end.ToString();
                        string bd = book.ToString();
                        int b_id = result.booking_id;
                        string packag_id = b_id.ToString();
                        txtBookNumber.Text = packag_id;
                        txtTourOperator.Text = result.name;
                        txtCountry.Text = result.country_name;
                        txtCity.Text = result.city_name;
                        txtPrice.Text = price;
                        txtStartDate.Text = strt;
                        txtEndDate.Text = nd;
                        txtBookDate.Text = bd;
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

        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            decimal.TryParse(txtPrice.Text, out  decimal price);
            int.TryParse(txtBookNumber.Text, out int booking_number);
            try
            {
                PaymentRepository paymentRepository = new PaymentRepository();
                paymentRepository.MakePayment(booking_number, price);
                MessageBox.Show("Оплата произведена успешно");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}");
            }
        }
    }
}
