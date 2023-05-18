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
            string currentMail = "";
            string currentPassword = ""; 
            try {
                currentMail = File.ReadLines("..\\LoginData.txt").First();
                currentPassword = File.ReadLines("..\\LoginData.txt").Last();
            } catch { }
            if (AppData.db.Users.FirstOrDefault(u => u.email == currentMail && u.Password == currentPassword) != null)
            {
                AppData.CurrentUser = AppData.db.Users.Where(p => p.email == currentMail && p.Password == currentPassword)
                .AsEnumerable()
                .FirstOrDefault(p => p.email == currentMail && p.Password == currentPassword);
                if (AppData.CurrentUser.Position == 1)
                {
                    MenuPersonal.Visibility = Visibility.Visible;
                    MenuEdit.Visibility = Visibility.Visible;
                }
                if (AppData.CurrentUser.Position == 2)
                {
                    MenuPersonal.Visibility = Visibility.Visible;
                    MenuEdit.Visibility = Visibility.Visible;
                    AdminItem1.Visibility = Visibility.Collapsed;
                    AdminItem2.Visibility = Visibility.Collapsed;
                    AdminItem3.Visibility = Visibility.Collapsed;
                    AdminItem4.Visibility = Visibility.Collapsed;
                }
                if (AppData.CurrentUser.Position == 3)
                {
                    MenuPersonal.Visibility = Visibility.Visible;
                    MenuEdit.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MainFrame.Navigate(new SignInPage());
                MenuPersonal.Visibility = Visibility.Hidden;
                return;
            }
            
            AppData.MainFrame = MainFrame;
            AppData.MainFrame.Navigate(new ShopPage());
        }

        private void MainFrame_ContentLoaded(object sender, EventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                GoBackButton.Visibility = Visibility.Visible;
            }
            else
            {
                GoBackButton.Visibility = Visibility.Hidden;
            }
            
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.GoBack();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter sr = new StreamWriter("..\\LoginData.txt");
            File.WriteAllText(sr.ToString(), "");
            sr.Close();
            MainWindow newWindow = new MainWindow();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = newWindow;
            AppData.MainFrame = newWindow.MainFrame;
            newWindow.Show();
            
            AppData.MainFrame.Navigate(new SignInPage());
        }

        private void CheckUsers(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new UserPage());
        }

        private void CheckDisciplines(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new DisciplinePage());
        }

        private void CheckAllTasks(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new ShopPage());
        }

        private void CheckAllTaskBases(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new TaskBanksPage());
        }

        private void OpenPersonalCabinet(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new PersonalCabinetPage());
        }

        private void GoToMainPage(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new ShopPage());
        }
    }
}
