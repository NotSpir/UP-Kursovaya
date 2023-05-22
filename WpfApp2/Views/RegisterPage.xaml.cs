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
            if (!people.email.Contains('@'))
                errors.AppendLine("Укажите настоящую почту!");
            if (string.IsNullOrWhiteSpace(PassBox.Password))
                errors.AppendLine("Укажите пароль");
            else if (!IsCorrectPassword(PassBox.Password))
                errors.AppendLine("Пароль должен быть длинной минимум 8 символов и содержать минимум 1 заглавную букву, 1 строчную и 1 цифру");
            else if (string.IsNullOrWhiteSpace(PassRepeatBox.Password))
                errors.AppendLine("Повторите пароль");
            else if (PassBox.Password != PassRepeatBox.Password)
                errors.AppendLine("Пароли не совпадают!");
            if (DPDateDel == null)
                errors.AppendLine("Укажите дату рождения");

            var repeatEmail = AppData.db.Users.FirstOrDefault(u => u.email == people.email);
            if (repeatEmail != null)
                errors.AppendLine("Данная почта занята");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var currentAccess = 3;
            if (people.ID == 0)
            {
                people.Position = currentAccess;
                AppData.db.Users.Add(people);
            }
            try
            {
                people.Password = PassBox.Password;
                people.Position = currentAccess;
                AppData.db.SaveChanges();
                MessageBox.Show("Вы успешно зарегистрировались!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void GoSingInBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
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

        private void TextBoxSpaceblock(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private bool IsCorrectPassword(string text)
        {
            if ((new Regex("[A-Z]+")).IsMatch(text) && (new Regex("[a-z]+")).IsMatch(text) && (new Regex("[0-9]+")).IsMatch(text) && text.Length >= 8)
                return true;
            return false;
        }
        //Конец функций для проверок ввода
    }
}
