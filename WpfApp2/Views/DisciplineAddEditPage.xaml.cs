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
    /// Логика взаимодействия для ServiceAddEditPage.xaml
    /// </summary>
    public partial class DisciplineAddEditPage : Page
    {
        private Discipline dataElement = new Discipline();
        public DisciplineAddEditPage(Discipline selectedData)
        {
            InitializeComponent();
            if (selectedData != null)
            {
                dataElement = selectedData;
            }
            DataContext = dataElement;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(dataElement.DisciplineName))
                errors.AppendLine("Укажите название");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (dataElement.ID == 0)
            {
                AppData.db.Discipline.Add(dataElement);
            }
            try
            {
                AppData.db.SaveChanges();
                MessageBox.Show("Данные сохранены");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
