using Adminkp.View;
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
using Adminkp.Model;
using Adminkp.Repository;

namespace Adminkp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            LogWindow logWindow = new LogWindow();
            logWindow.Show();
            this.Close();
        }
        private void ShowData(Page page)
        {
            show.Navigate(page);
        }
        private void ShowUsers_Click(object sender, RoutedEventArgs e)
        {
            UserRepository userRepository = new UserRepository();
            ShowData(new InfoUsers(userRepository));
        }
        private void ShowBookings_Click(object sender, RoutedEventArgs e)
        {
            ShowData(new InfoBookings());
        }
        private void ShowTours_Click(object sender, RoutedEventArgs e)
        {
            PackageRepository packageRepository = new PackageRepository();
            ShowData(new InfoTours(packageRepository));
        }
    }
}
