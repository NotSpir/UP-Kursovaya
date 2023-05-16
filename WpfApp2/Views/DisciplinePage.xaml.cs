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
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class DisciplinePage : Page
    {
        public DisciplinePage()
        {
            InitializeComponent();
            DGridMenu.ItemsSource = AppData.db.Discipline.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DisciplineAddEditPage(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var dataForRemoving = DGridMenu.SelectedItems.Cast<Discipline>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {dataForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AppData.db.Discipline.RemoveRange((IEnumerable<Discipline>)dataForRemoving);
                    AppData.db.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    DGridMenu.ItemsSource = AppData.db.Discipline.ToList();
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
                NavigationService.Navigate(new DisciplineAddEditPage((sender as DataGridRow).DataContext as Discipline));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                AppData.db.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridMenu.ItemsSource = AppData.db.Discipline.ToList();
            }
        }
    }
}
