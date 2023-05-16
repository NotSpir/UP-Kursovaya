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
using WpfApp2.Model;
// Word = Microsoft.Office.Interop.Word;

namespace WpfApp2.Views
{
    /// <summary>
    /// Логика взаимодействия для ShopPage.xaml
    /// </summary>
    public partial class ShopPage : Page
    {
        List<TaskNames> currentTasks = AppData.db.TaskNames.ToList();
        public ShopPage()
        {
            InitializeComponent();

            var allTypes = AppData.db.Discipline.ToList();
            allTypes.Insert(0, new Discipline
            {
                DisciplineName = "Любая дисциплина"
            });
            ComboType.ItemsSource = allTypes;
            ComboType.SelectedIndex = 0;

            if (AppData.CurrentUser.Position == 1)
            {
                BtnDelete.Visibility = Visibility.Visible;
                BtnAdd.Visibility = Visibility.Visible;
            }

            LViewTasks.ItemsSource = currentTasks;
        }
        private void UpdateShop()
        {
            currentTasks = AppData.db.TaskNames.ToList();
            if (ComboType.SelectedIndex != 0)
                currentTasks = currentTasks.Where(c => c.Discipline == ComboType.SelectedValue).ToList();

            currentTasks = currentTasks.Where(c => c.TaskName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            //LViewTasks.ItemsSource = currentServices.OrderBy(p => p.Price).ToList();

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

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var tasksForRemoving = LViewTasks.SelectedItems.Cast<TaskNames>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {tasksForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AppData.db.TaskNames.RemoveRange((IEnumerable<TaskNames>)tasksForRemoving);
                    AppData.db.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    LViewTasks.ItemsSource = AppData.db.TaskNames.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new TaskAddEditPage(null));
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

        private void ListViewItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (AppData.CurrentUser.Position == 1)
            if (sender != null)
                NavigationService.Navigate(new TaskAddEditPage((sender as ListViewItem).DataContext as TaskNames));
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Двойное ЛКМ будет помечать задание выполненым или отменять выполнение
            if (sender != null)
            {
                var newComplete = new CompletedTaskUser();
                newComplete.UserID = AppData.CurrentUser.ID;
                newComplete.CompletedTaskID = ((sender as ListViewItem).DataContext as TaskNames).ID;
                var oldComplete = AppData.db.CompletedTaskUser.FirstOrDefault(u => u.UserID == newComplete.UserID && u.CompletedTaskID == newComplete.CompletedTaskID);
                if (oldComplete != null)
                {
                    AppData.db.CompletedTaskUser.Remove(oldComplete);
                    AppData.db.SaveChanges();
                    (sender as ListViewItem).Background = Brushes.White;
                    return;
                }
                else
                {
                    AppData.db.CompletedTaskUser.Add(newComplete);
                    AppData.db.SaveChanges();
                    (sender as ListViewItem).Background = Brushes.LimeGreen;
                }
            }
        }

    }
    }
