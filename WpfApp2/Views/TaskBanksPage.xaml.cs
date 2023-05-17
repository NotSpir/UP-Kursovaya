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
    /// Логика взаимодействия для TaskBanksPage.xaml
    /// </summary>
    public partial class TaskBanksPage : Page
    {
        List<TaskBanks> currentBanks = AppData.db.TaskBanks.ToList();
        public TaskBanksPage()
        {
            InitializeComponent();

            if (AppData.CurrentUser.Position == 1)
            {
                BtnDelete.Visibility = Visibility.Visible;
                BtnAdd.Visibility = Visibility.Visible;
            }

            foreach (var item in currentBanks)
            {
                var newTask = new List<TaskNames>();
                var taskL = AppData.db.TasksIBanks.ToList().Where(u => u.BankID == item.ID);
                foreach (var item1 in taskL)
                {
                    newTask.Add(AppData.db.TaskNames.ToList().Where(u => u.ID == item1.TaskID).FirstOrDefault());
                }
                item.TaskList = newTask;
            }
            
            LViewBanks.ItemsSource = currentBanks;
        }

        private void BtnWord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new TaskBanksAddEditPage(null));
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateShop();
        }

        private void UpdateShop()
        {
            currentBanks = AppData.db.TaskBanks.ToList();

            currentBanks = currentBanks.Where(c => c.BankName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            //LViewTasks.ItemsSource = currentServices.OrderBy(p => p.Price).ToList();

            LViewBanks.ItemsSource = currentBanks;

        }

        private void ListViewItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (AppData.CurrentUser.Position != 3 )
                if (sender != null)
                    NavigationService.Navigate(new TaskBanksAddEditPage((sender as ListViewItem).DataContext as TaskBanks));
        }
    }
}
