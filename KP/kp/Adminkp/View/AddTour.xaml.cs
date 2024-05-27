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
using Adminkp.Model;
using Adminkp.Repository;
using System.Text.RegularExpressions;

namespace Adminkp.View
{
    /// <summary>
    /// Логика взаимодействия для AddTour.xaml
    /// </summary>
    public partial class AddTour : Window
    {
        public AddTour()
        {
            InitializeComponent();
        }

        private void CancButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(TourOperatorId.Text, out int tourOperatorId))
                {
                    MessageBox.Show("Некорректное значение для ID туроператора");
                    return;
                }

                if (!int.TryParse(DestId.Text, out int destId))
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
                }

                if (!decimal.TryParse(Price.Text, out decimal price) || price < 0)
                {
                    MessageBox.Show("Некорректное значение для цены");
                    return;
                }

                if (!DateTime.TryParse(StartDate.Text, out DateTime startDate) || startDate < DateTime.Today)
                {
                    MessageBox.Show("Некорректное значение для даты начала");
                    return;
                }

                if (!DateTime.TryParse(EndDate.Text, out DateTime endDate) || endDate < DateTime.Today)
                {
                    MessageBox.Show("Некорректное значение для даты окончания");
                    return;
                }
                PackageRepository packageRepository = new PackageRepository();
                packageRepository.AddPackage(tourOperatorId, destId, name, price, startDate, endDate);
                MessageBox.Show("Путевка успешно создана и добавлена в базу данных");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании и добавлении путевки: {ex.Message}");
            }
            this.Close();
        }
    }
}
