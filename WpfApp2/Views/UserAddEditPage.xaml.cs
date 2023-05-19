using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.Model;

namespace WpfApp2.Views
{
    /// <summary>
    /// Логика взаимодействия для UserAddEditPage.xaml
    /// </summary>
    public partial class UserAddEditPage : Page
    {
        private Users people = new Users();
        public UserAddEditPage(Users selectedUser)
        {
            InitializeComponent();
            if (selectedUser != null)
            {
                people = selectedUser;
                ComboRoles.SelectedItem = selectedUser.Positions;
            }
            InitializeComponent();
            DataContext = people;
            ComboRoles.ItemsSource = AppData.db.Positions.ToList();
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
            if (ComboRoles.SelectedItem == null)
                errors.AppendLine("Укажите уровень доступа");
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
            var currentAccess = ComboRoles.SelectedItem as Positions;
            if (people.ID == 0)
            {
                people.Position = currentAccess.ID;
                AppData.db.Users.Add(people);
            }
            try
            {
                people.Position = currentAccess.ID;
                AppData.db.SaveChanges();
                MessageBox.Show("Данные сохранены");
                NavigationService.GoBack(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //Начало функций для проверок ввода
        private static readonly Regex _regex = new Regex("[^0-9@A-Z.a-z]+");
        private static readonly Regex regexRu = new Regex("[^А-Яа-я]+");
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            TextValidError.Text = "";
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                    TextValidError.Text = "Разрешены только символы английского алфавита, числа и @ и точка";
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void CharValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            TextValidError.Text = "";
            e.Handled = _regex.IsMatch(e.Text);
            if (_regex.IsMatch(e.Text))
            TextValidError.Text = "Разрешены только символы английского алфавита, числа и @ и точка";
        }

        private void TextBoxRuPasting(object sender, DataObjectPastingEventArgs e)
        {
            TextValidError.Text = "";
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (regexRu.IsMatch(text))
                {
                    e.CancelCommand();

                }
            }
            else
            {
                e.CancelCommand();
            }
        }
        private void CharValidationRuTextBox(object sender, TextCompositionEventArgs e)
        {
            TextValidError.Text = "";
            e.Handled = regexRu.IsMatch(e.Text);
            if (regexRu.IsMatch(e.Text))
                TextValidError.Text = "Разрешены только русские символы";
        }
        //Конец функций для проверок ввода
    }
}
