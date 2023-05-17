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
    /// Логика взаимодействия для TaskAddEditPage.xaml
    /// </summary>
    public partial class TaskAddEditPage : Page
    {
        private TaskNames tasks = new TaskNames();
        public TaskAddEditPage(TaskNames selectedTask)
        {
            InitializeComponent();
            if (selectedTask != null)
            {
                tasks = selectedTask;
                ComboDisc.SelectedItem = selectedTask.Discipline;
            }
            InitializeComponent();
            DataContext = tasks;
            ComboDisc.ItemsSource = AppData.db.Discipline.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tasks.TaskName))
                errors.AppendLine("Укажите название задания");
            if (tasks.CompletionTime == null)
                errors.AppendLine("Укажите время выполнения");
            if (string.IsNullOrWhiteSpace(tasks.Description))
                errors.AppendLine("Укажите описание");
            if (ComboDisc.SelectedItem == null)
                errors.AppendLine("Укажите дисциплину");

            var repeatName = AppData.db.TaskNames.FirstOrDefault(u => u.TaskName == tasks.TaskName && u.ID != tasks.ID);
            if (repeatName != null)
                errors.AppendLine("Данное название занято");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var currentDisc = ComboDisc.SelectedItem as Discipline;
            if (tasks.ID == 0)
            {
                tasks.Author = AppData.CurrentUser.ID;
                tasks.DisciplineID = currentDisc.ID;
                AppData.db.TaskNames.Add(tasks);
            }
            try
            {
                tasks.Author = AppData.CurrentUser.ID;
                tasks.DisciplineID = currentDisc.ID;
                AppData.db.SaveChanges();
                MessageBox.Show("Данные сохранены");
                AppData.MainFrame.Navigate(new ShopPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
