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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserAddEditPage(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var usersForRemoving = DGridMenu.SelectedItems.Cast<Users>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {usersForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AppData.db.Users.RemoveRange((IEnumerable<Users>)usersForRemoving);
                    AppData.db.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    DGridMenu.ItemsSource = AppData.db.Users.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void SelectRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
                NavigationService.Navigate(new UserAddEditPage((sender as DataGridRow).DataContext as Users));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                AppData.db.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridMenu.ItemsSource = AppData.db.Users.ToList();
            }
        }
    }
}
