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

namespace WpfApp2.Views
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private Users people = new Users();
        public RegisterPage()
        {
            InitializeComponent();
            DataContext = people;
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(people.FirstName))
                errors.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(people.Surname))
                errors.AppendLine("Укажите фамилию");
            if (string.IsNullOrWhiteSpace(people.Patronymic))
                errors.AppendLine("Укажите отчество");
            if (string.IsNullOrWhiteSpace(people.email))
                errors.AppendLine("Укажите почту");
            if (string.IsNullOrWhiteSpace(people.Password))
                errors.AppendLine("Укажите пароль");  

            var repeatEmail = AppData.db.Users.FirstOrDefault(u => u.email == people.email);
            if (repeatEmail != null)
                errors.AppendLine("Данная почта занята");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (people.ID == 0)
                people.Position = 2;
                AppData.db.Users.Add(people);
            try 
            { 
                AppData.db.SaveChanges();
                MessageBox.Show("Вы успешно зарегистрировались!");
                NavigationService.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void GoSingInBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
