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
    /// Логика взаимодействия для TaskBanksAddEditPage.xaml
    /// </summary>
    public partial class TaskBanksAddEditPage : Page
    {
        List<TaskNames> currentTasks = AppData.db.TaskNames.ToList();
        private TaskBanks banks = new TaskBanks();
        public TaskBanksAddEditPage(TaskBanks selectedBank)
        {
            InitializeComponent();

            if (selectedBank != null)
            {
                banks = selectedBank;

                var allTypes = AppData.db.Discipline.ToList();
                allTypes.Insert(0, new Discipline
                {
                    DisciplineName = "Любая дисциплина"
                });
                ComboType.ItemsSource = allTypes;
                ComboType.SelectedIndex = 0;
                LViewTasks.ItemsSource = currentTasks;

                var newTask = new List<TaskNames>();
                var taskL = AppData.db.TasksIBanks.ToList().Where(u => u.BankID == banks.ID);
                foreach (var item1 in taskL)
                {
                    newTask.Add(AppData.db.TaskNames.ToList().Where(u => u.ID == item1.TaskID).FirstOrDefault());
                }
                banks.TaskList = newTask;
            } else
            {
                TBoxSearch.IsReadOnly = true;
                ComboType.IsReadOnly = true;
            }
            InitializeComponent();
            DataContext = banks;
        }

        private void UpdateShop()
        {
            currentTasks = AppData.db.TaskNames.ToList();
            if (ComboType.SelectedIndex != 0)
                currentTasks = currentTasks.Where(c => c.Discipline == ComboType.SelectedValue).ToList();

            currentTasks = currentTasks.Where(c => c.TaskName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            LViewTasks.ItemsSource = currentTasks;

        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateShop();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateShop();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(banks.BankName))
                errors.AppendLine("Укажите название банка");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (banks.ID == 0)
            {
                AppData.db.TaskBanks.Add(banks);
            }
            try
            {
                AppData.db.SaveChanges();
                MessageBox.Show("Данные сохранены");
                AppData.MainFrame.Navigate(new TaskBanksPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ListViewItem_LeftMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                var newTask = new TasksIBanks();
                newTask.BankID = banks.ID;
                newTask.TaskID = ((sender as ListViewItem).DataContext as TaskNames).ID;
                newTask.Variant = 1;
                newTask.VarDescription = "0";
                var oldComplete = AppData.db.TasksIBanks.FirstOrDefault(u => u.TaskID == newTask.TaskID && u.BankID == newTask.BankID);
                if (oldComplete != null)
                {
                    AppData.db.TasksIBanks.Remove(oldComplete);
                    AppData.db.SaveChanges();
                    (sender as ListViewItem).Background = Brushes.White;
                    MessageBox.Show("Задание удалено из банка");
                    NavigationService.Navigate(new TaskBanksAddEditPage(banks));
                    return;
                }
                else
                {
                    AppData.db.TasksIBanks.Add(newTask);
                    AppData.db.SaveChanges();
                    (sender as ListViewItem).Background = Brushes.White;
                    MessageBox.Show("Задание добавлено в банк");
                    NavigationService.Navigate(new TaskBanksAddEditPage(banks));
                }
            }
        }
    }
}
