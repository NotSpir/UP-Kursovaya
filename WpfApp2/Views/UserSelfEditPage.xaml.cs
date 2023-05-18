using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для UserAddEditPage.xaml
    /// </summary>
    public partial class UserSelfEditPage : Page
    {
        private Users people = new Users();
        public UserSelfEditPage(Users selectedUser)
        {
            InitializeComponent();
            if (selectedUser != null)
            {
                people = selectedUser;
            }
            InitializeComponent();
            DataContext = people;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
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
            if (DPDateDel == null)
                errors.AppendLine("Укажите дату рождения");

            var repeatEmail = AppData.db.Users.FirstOrDefault(u => u.email == people.email && u.ID != people.ID);
            if (repeatEmail != null)
                errors.AppendLine("Данная почта занята");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (people.ID == 0)
            {
                AppData.db.Users.Add(people);
            }
            try
            {
                AppData.db.SaveChanges();
                MessageBox.Show("Данные сохранены");
                NavigationService.GoBack(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
