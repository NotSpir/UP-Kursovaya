﻿using System;
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
using System.Text.RegularExpressions;

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

            if (AppData.CurrentUser != null)
            {
                StreamWriter sr = new StreamWriter("..\\LoginData.txt");
                File.WriteAllText(sr.ToString(), "");
                sr.WriteLine(AppData.CurrentUser.email);
                sr.WriteLine(AppData.CurrentUser.Password);
                sr.Close();

                MainWindow newWindow = new MainWindow();
                Application.Current.MainWindow.Close();
                Application.Current.MainWindow = newWindow;
                AppData.MainFrame = newWindow.MainFrame;
                newWindow.Show();

                if (AppData.CurrentUser.Position == 1)
                {
                    NavigationService.Navigate(new TaskSearchPage());
                }
                else
                {
                    NavigationService.Navigate(new TaskSearchPage());
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

        private static readonly Regex _regex = new Regex("[^0-9@A-Z.a-z]+");
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

        private void TextBoxSpaceblock(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }
    }
}
