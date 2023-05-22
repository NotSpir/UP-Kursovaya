using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
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
using WpfApp2.Model; //s
namespace WpfApp2.Views
{
    /// <summary>
    /// Логика взаимодействия для PersonalCabinetPage.xaml
    /// </summary>
    public partial class PersonalCabinetPage : Page
    {
        public PersonalCabinetPage()
        {
            InitializeComponent();

            if (AppData.CurrentUser.Position == 3)
            {
                List<TaskNames> currentTasks = new List<TaskNames>();
                foreach (var item in AppData.db.CompletedTaskUser.ToList().Where(c => c.UserID == AppData.CurrentUser.ID).ToList())
                    currentTasks.Add(AppData.db.TaskNames.ToList().Where(c => c.ID == item.CompletedTaskID && item.UserID == AppData.CurrentUser.ID).First());
                LBMyTasks.ItemsSource = currentTasks;
            }
            else
                LBMyTasks.ItemsSource = AppData.db.TaskNames.ToList().Where(c => c.Author == AppData.CurrentUser.ID).ToList();

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new UserSelfEditPage(AppData.CurrentUser));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                AppData.db.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                TBName.Text = AppData.CurrentUser.FirstName;
                TBSurname.Text = AppData.CurrentUser.Surname;
                TBPatronymic.Text = AppData.CurrentUser.Patronymic;
                TBMail.Text = AppData.CurrentUser.email;
                TBPassword.Text = AppData.CurrentUser.Password;
                TBRole.Text = AppData.CurrentUser.Positions.PositionName;
            }
        }
    }
}
