using Adminkp.Repository;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Adminkp.View
{
    /// <summary>
    /// Логика взаимодействия для InfoTours.xaml
    /// </summary>
    public partial class InfoTours : Page
    {
        private readonly PackageRepository packageRepository;
        private DispatcherTimer timer;
        public InfoTours(PackageRepository _packageRepository)
        {
            InitializeComponent();
            packageRepository = _packageRepository;
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
                    from packages in dbContext.Packages
                    join destinations in dbContext.Destinations
                    on packages.destination_id equals destinations.destination_id
                    join touroperators in dbContext.TourOperators
                    on packages.tour_operator_id equals touroperators.tour_operator_id
                    select new
                    {
                        packages.package_id,
                        touroperators.name,
                        packages.description,
                        packages.start_date,
                        packages.end_date,
                        packages.price,
                        destinations.country_name,
                        destinations.city_name
                    };
                iform.ItemsSource = query.ToList();
            }
        }
        private void ShowData(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (iform.SelectedItem != null)
            {
                dynamic selectedRow = iform.SelectedItem;
                TourId.Text = GetValueOrDefault(selectedRow, "package_id");
                TourOperator.Text = GetValueOrDefault(selectedRow, "name");
                TourName.Text = GetValueOrDefault(selectedRow, "description");
                StartDate.Text = GetValueOrDefault(selectedRow, "start_date");
                EndDate.Text = GetValueOrDefault(selectedRow, "end_date");
                TourPrice.Text = GetValueOrDefault(selectedRow, "price");
                TourCountry.Text = GetValueOrDefault(selectedRow, "country_name");
                TourCity.Text = GetValueOrDefault(selectedRow, "city_name");
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
                // Получаем выбранный объект из DataGrid
                dynamic selectedData = iform.SelectedItem;
                int pack_id = selectedData.package_id;
                DeletePackage(pack_id);
            }
            else
            {
                MessageBox.Show("Выберите путевку.");
            }
        }
        private void DeletePackage(int packageId)
        {
            try
            {    
                packageRepository.DeletePackage(packageId);
                MessageBox.Show("Путевка успешно удалена");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении путевки, так как она уже заказана: {ex.Message}");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddTour addTour = new AddTour();
            addTour.ShowDialog();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if(iform.SelectedItem != null)
            {
                dynamic selectedData = iform.SelectedItem;
                int pack_id = selectedData.package_id;
                EditTour editTour = new EditTour(pack_id);
                editTour.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите путевку.");
            }
        }
        private void Dob_Click(object sender, EventArgs e)
        {
            // Не выполняйте никаких дополнительных действий внутри этого обработчика
        }
    }
}
