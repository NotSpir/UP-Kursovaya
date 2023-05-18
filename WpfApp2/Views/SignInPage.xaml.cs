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
using WpfApp2.Model;
using System.IO;

namespace WpfApp2.Views
{
    /// <summary>
    /// Логика взаимодействия для SignInPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
        public SignInPage()
        {
            InitializeComponent();
        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(TxbMail.Text))
                errors.AppendLine("Укажите почту");
            if (string.IsNullOrWhiteSpace(TxbPassword.Password))
                errors.AppendLine("Укажите пароль");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            AppData.CurrentUser = AppData.db.Users.Where(p => p.email == TxbMail.Text && p.Password == TxbPassword.Password)
                .AsEnumerable()
                .FirstOrDefault(p => p.email == TxbMail.Text && p.Password == TxbPassword.Password);

            if(AppData.CurrentUser != null)
            {
                StreamWriter sr = new StreamWriter("..\\LoginData.txt");
                File.WriteAllText(sr.ToString(), "");
                sr.WriteLine(AppData.CurrentUser.email);
                sr.WriteLine(AppData.CurrentUser.Password);
                sr.Close();

                MainWindow newWindow = new MainWindow();
                Application.Current.MainWindow.Close();
                Application.Current.MainWindow = newWindow;
                newWindow.Show();

                if (AppData.CurrentUser.Position == 1)
                {
                    NavigationService.Navigate(new ShopPage());
                }
                else
                {
                    NavigationService.Navigate(new ShopPage());
                }
                
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }
    }
}
