using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для PersonalCabinetPage.xaml
    /// </summary>
    public partial class PersonalCabinetPage : Page
    {
        public PersonalCabinetPage()
        {
            InitializeComponent();
            TBName.Text = AppData.CurrentUser.FirstName;
            TBSurname.Text = AppData.CurrentUser.Surname;
            TBPatronymic.Text = AppData.CurrentUser.Patronymic;
            TBMail.Text = AppData.CurrentUser.email;
            TBPassword.Text = AppData.CurrentUser.Password;
            TBRole.Text = AppData.CurrentUser.Positions.PositionName;

            LBMyTasks.ItemsSource = AppData.db.TaskNames.ToList().Where(c => c.Author == AppData.CurrentUser.ID).ToList();
        }
    }
}
