using Adminkp.Repository;
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
using System.Text.RegularExpressions;

namespace Adminkp.View
{
    /// <summary>
    /// Логика взаимодействия для EditTour.xaml
    /// </summary>
    public partial class EditTour : Window
    {
        public EditTour(int pack_id)
        {
            InitializeComponent();
            using (var dbContext = new Model.ApplicationContext())
            {
                if (pack_id != 0)
                {
                    var query =
                        from packages in dbContext.Packages
                        where packages.package_id == pack_id
                        select new
                        {
                            packages.tour_operator_id,
                            packages.destination_id,
                            packages.description,
                            packages.package_id,
                            packages.price,
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
                        int p_id = (int)result.package_id;
                        string packag_id = p_id.ToString();
                        int tourop = (int)result.tour_operator_id;
                        string to_id = tourop.ToString();
                        int destin = (int)result.destination_id;
                        string destout = destin.ToString();
                        TourNumber.Text = packag_id;
                        TourOperatorId.Text = to_id;
                        DestId.Text = destout;
                        Price.Text = price;
                        StartDate.Text = strt;
                        EndDate.Text = nd;
                        Name.Text = result.description;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: Данные отсутствуют.");
                    }
                }
            }
        }

        private void CancButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка и парсинг значений из TextBox'ов
                int tourNumber;
                if (!int.TryParse(TourNumber.Text, out tourNumber))
                {
                    MessageBox.Show("Некорректное значение для номера тура");
                    return;
                }

                int tourOperatorId;
                if (!int.TryParse(TourOperatorId.Text, out tourOperatorId))
                {
                    MessageBox.Show("Некорректное значение для ID туроператора");
                    return;
                }

                int destId;
                if (!int.TryParse(DestId.Text, out destId))
                {
                    MessageBox.Show("Некорректное значение для ID местоположения");
                    return;
                }

                string name = Name.Text;
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Поле 'Название' не может быть пустым");
                    return;
                }
                else if (!Regex.IsMatch(name, @"^[a-zA-Z0-9_\s]+$"))
                {
                    MessageBox.Show("Название может содержать только буквы, цифры и пробел");
                    return;
                }

                decimal price;
                if (!decimal.TryParse(Price.Text, out price)|| price < 0)
                {
                    MessageBox.Show("Некорректное значение для цены");
                    return;
                }

                DateTime startDate;
                if (!DateTime.TryParse(StartDate.Text, out startDate) || startDate < DateTime.Today)
                {
                    MessageBox.Show("Некорректное значение для даты начала");
                    return;
                }

                DateTime endDate;
                if (!DateTime.TryParse(EndDate.Text, out endDate) || endDate < DateTime.Today)
                {
                    MessageBox.Show("Некорректное значение для даты окончания");
                    return;
                }
                PackageRepository packageRepository = new PackageRepository();
                packageRepository.UpdatePackage(tourNumber, tourOperatorId, destId, name, price, startDate, endDate);
                MessageBox.Show("Данные успешно обновлены");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных тура: {ex.Message}");
            }
            this.Close();
        }
    }
}
