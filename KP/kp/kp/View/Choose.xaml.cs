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

using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Configuration;
using System.Data;
using kp.Model;

namespace kp.View
{
    /// <summary>
    /// Логика взаимодействия для Choose.xaml
    /// </summary>
    public partial class Choose : Window
    {
        public Choose()
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
                            select new
                            {
                                packages.package_id,
                                packages.price,
                                destinations.country_name,
                                destinations.city_name,
                                touroperators.name,
                                packages.start_date,
                                packages.end_date
                            };
                        info.ItemsSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }
        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы уже находитесь в этом окне");
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            History history = new History();
            history.Show();
            this.Close();
        }

        private void BookingsButton_Click(object sender, RoutedEventArgs e)
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

        private void LookButton_Click(object sender, RoutedEventArgs e)
        {
            if (info.SelectedItem != null)
            {
                // Получаем выбранный объект из DataGrid
                dynamic selectedData = info.SelectedItem;
                int pack_id = selectedData.package_id;
                // Создаем новое окно
                TourInformation tourInformation = new TourInformation(pack_id);
                tourInformation.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Выберите путевку.");
            }
        }

        private void UseFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение значений из DatePicker и TextBox
            DateTime startDate = datePickerFirst.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = datePickerSecond.SelectedDate ?? DateTime.MaxValue;
            decimal maxPrice = (decimal)priceSlider.Value;
            string locationFilter = locationTextBox.Text;
            locationFilter = string.IsNullOrWhiteSpace(locationFilter) ? null : locationFilter;
            // Применение фильтров к DataGrid
            FilterPackages(startDate, endDate, maxPrice, locationFilter);
        }
        private void FilterPackages(DateTime startDate, DateTime endDate, decimal maxPrice, string locationFilter)
        {
            using (var dbContext = new Model.AppContext())
            {
                var filteredLocation = string.IsNullOrWhiteSpace(locationFilter) ? null : locationFilter;
                var query =
                           from packages in dbContext.Packages
                           join destinations in dbContext.Destinations
                           on packages.destination_id equals destinations.destination_id
                           join touroperators in dbContext.TourOperators
                           on packages.tour_operator_id equals touroperators.tour_operator_id
                           where (startDate == default || packages.start_date >= startDate)
                           && (endDate == default || packages.end_date <= endDate)
                           && (maxPrice <= 0 || packages.price <= maxPrice)
                           && (filteredLocation == null || destinations.city_name.Contains(filteredLocation))
                           select new
                           {
                               packages.package_id,
                               packages.price,
                               destinations.country_name,
                               destinations.city_name,
                               touroperators.name,
                               packages.start_date,
                               packages.end_date
                           };
                info.ItemsSource = query.ToList();
            }
        }
        private void DropFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            // Очистка значений в полях фильтров
            datePickerFirst.SelectedDate = null;
            datePickerSecond.SelectedDate = null;
            priceSlider.Value = 0;
            locationTextBox.Text = "";
        }

        private void ShowPrevButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new Model.AppContext())
            {
                var query =
                    from packages in dbContext.Packages
                    join destinations in dbContext.Destinations
                    on packages.destination_id equals destinations.destination_id
                    join touroperators in dbContext.TourOperators
                    on packages.tour_operator_id equals touroperators.tour_operator_id
                    select new
                    {
                        packages.package_id,
                        packages.price,
                        destinations.country_name,
                        destinations.city_name,
                        touroperators.name,
                        packages.start_date,
                        packages.end_date
                    };
                info.ItemsSource = query.ToList();
            }
            
        }
        private void Dob_Click(object sender, EventArgs e)
        {
            // Не выполняйте никаких дополнительных действий внутри этого обработчика
        }
    }
}
