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
// Word = Microsoft.Office.Interop.Word;

namespace WpfApp2.Views
{
    /// <summary>
    /// Логика взаимодействия для ShopPage.xaml
    /// </summary>
    public partial class ShopPage : Page
    {
        public ShopPage()
        {
            InitializeComponent();

            var allTypes = AppData.db.Discipline.ToList();
            allTypes.Insert(0, new Discipline
            {
                DisciplineName = "Все типы"
            });
            ComboType.ItemsSource = allTypes;
            ComboType.SelectedIndex = 0;
            if (AppData.CurrentUser.Position == 1)
            {
                BtnDelete.Visibility = Visibility.Visible;
                BtnEdit.Visibility = Visibility.Visible;
            }    

            var currentSrevices = AppData.db.TaskNames.ToList();
            LViewTasks.ItemsSource = currentSrevices;
        }
        private void UpdateShop()
        {
            var currentServices = AppData.db.TaskNames.ToList();
            if (ComboType.SelectedIndex != 0)
                currentServices = currentServices.Where(c => c.Discipline.DisciplineName == ComboType.SelectedValue).ToList();

            currentServices = currentServices.Where(c => c.TaskName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            //LViewTasks.ItemsSource = currentServices.OrderBy(p => p.Price).ToList();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateShop();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateShop();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnWord_Click(object sender, RoutedEventArgs e)
        {
            /*    //Report 1 №п/п; Вид сырья; Сумма, внизу Итого:
                object oMissing = System.Reflection.Missing.Value;
                object oEndOfDoc = "\\endofdoc"; // \endofdoc is a predefined bookmark 

                //Start Word and create a new document.
                Word._Application oWord;
                Word._Document oDoc;
                oWord = new Word.Application();
                oWord.Visible = true;
                oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                ref oMissing, ref oMissing);

                //Собственно вставка таблицы, в данном случае  (Кол-во видов сырья) x 3

                Word.Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                int r, c;
                string strText;

            */
        }
    }
    }
