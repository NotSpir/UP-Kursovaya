using System;
using System.Collections.Generic;
using System.IO;
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
using WpfApp2.Model;
using WpfApp2.Views;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string currentMail = File.ReadLines("..\\LoginData.txt").First();
            string currentPassword = File.ReadLines("..\\LoginData.txt").Last();
            if (AppData.db.Users.FirstOrDefault(u => u.email == currentMail && u.Password == currentPassword) != null)
            {
                AppData.CurrentUser = AppData.db.Users.FirstOrDefault(u => u.email == currentMail && u.Password == currentPassword);
                if (AppData.CurrentUser.Position == 1)
                MainFrame.Navigate(new AdminPage());
                if (AppData.CurrentUser.Position == 2)
                    MainFrame.Navigate(new ShopPage());
                if (AppData.CurrentUser.Position == 3)
                    MainFrame.Navigate(new ShopPage());
            }
            else MainFrame.Navigate(new SignInPage());

            AppData.MainFrame = MainFrame;
        }

        private void MainFrame_ContentLoaded(object sender, EventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                GoBackButton.Visibility = Visibility.Visible;
                LogOutButton.Visibility = Visibility.Visible;
            }
            else
            {
                GoBackButton.Visibility = Visibility.Hidden;
                LogOutButton.Visibility = Visibility.Hidden;
            }
            
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.GoBack();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SignInPage());

        }
    }
}
