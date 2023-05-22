using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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
using Word = Microsoft.Office.Interop.Word;

namespace WpfApp2.Views
{
    /// <summary>
    /// Логика взаимодействия для ShopPage.xaml
    /// </summary>
    public partial class TaskSearchPage : Page
    {
        private int PagesCount;
        private int NumberOfPage = 0;
        private int maxItemShow = 5;
        List<TaskNames> currentTasks = AppData.db.TaskNames.ToList();
        Discipline discVal = new Discipline();
        public TaskSearchPage()
        {
            InitializeComponent();
        }
        private void UpdateShop()
        {
            currentTasks = AppData.db.TaskNames.ToList();
            if (discVal.ID != 0)
                currentTasks = currentTasks.Where(c => c.DisciplineID == discVal.ID).ToList();

            currentTasks = currentTasks.Where(c => c.TaskName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            //LViewTasks.ItemsSource = currentServices.OrderBy(p => p.Price).ToList();

            PagesCount = Convert.ToInt16(Math.Floor(((double)currentTasks.Count / maxItemShow)-0.0000001));
            CheckPages();
            LViewTasks.ItemsSource = currentTasks.Skip(maxItemShow * NumberOfPage).Take(maxItemShow).ToList();
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
                    foreach (var task in tasksForRemoving)
                    {
                        if (task.WordVersion != null)
                            File.Delete(task.WordVersion);
                    }
                    AppData.db.TaskNames.RemoveRange((IEnumerable<TaskNames>)tasksForRemoving);
                    AppData.db.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    AppData.MainFrame.Navigate(new TaskSearchPage());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new TaskAddEditPage(null));
        }

        private void BtnWord_Click(object sender, RoutedEventArgs e)
        {
            if (((sender as Button).DataContext as TaskNames).WordVersion != null)
            {
                if (File.Exists(((sender as Button).DataContext as TaskNames).WordVersion))
                {
                    Process wordProcess = new Process();
                    wordProcess.StartInfo.FileName = ((sender as Button).DataContext as TaskNames).WordVersion;
                    wordProcess.StartInfo.UseShellExecute = true;
                    wordProcess.Start();
                    return;
                }

                else if ((MessageBox.Show("Документ, привязанный к данному заданию не найден. Сгенерировать документ автоматически?", "Ошибка", MessageBoxButton.YesNo, MessageBoxImage.Error)) == MessageBoxResult.No)
                    return;
            }
                    object oMissing = System.Reflection.Missing.Value;
                    object oEndOfDoc = "\\endofdoc"; // \endofdoc is a predefined bookmark 

                    //Start Word and create a new document.
                    Word._Application oWord;
                    Word._Document oDoc;
                    oWord = new Word.Application();
                    oWord.Visible = true;
                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing);

                    //Собственно вставка данных в .docx документ

                    Word.Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                    var dataForWord = (sender as Button).DataContext as TaskNames;
                    string textName = dataForWord.TaskName;
                    var author = AppData.db.Users.ToList().Where(u => u.ID == dataForWord.Author).FirstOrDefault();
                    var discipline = AppData.db.Discipline.ToList().Where(u => u.ID == dataForWord.DisciplineID).FirstOrDefault();
                    string textMadeBy = $"Задание создал(а): {author.Surname} {author.FirstName} {author.Patronymic}";
                    string textDiscipline = $"Дисциплина: {discipline.DisciplineName}";
                    string textCompleteTime = $"Время выполнения задания: {dataForWord.CompletionTime.ToString()} мин.";
                    string textTask = dataForWord.Description;

                    Word.Paragraph oPara1;
                    oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);
                    oPara1.Range.Text = textName;
                    oPara1.Range.Font.Bold = 1;
                    oPara1.Range.Font.Size = 36;
                    oPara1.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara1.Format.SpaceAfter = 16;    //24 pt spacing after paragraph.
                    oPara1.Range.InsertParagraphAfter();

                    Word.Paragraph oPara2;
                    oPara2 = oDoc.Content.Paragraphs.Add();
                    oPara2.Range.Text = textMadeBy;
                    oPara2.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                    oPara2.Range.Font.Bold = 1;
                    oPara2.Range.Font.Italic = 1;
                    oPara2.Range.Font.Size = 14;
                    oPara2.Format.SpaceAfter = 12;
                    oPara2.Range.InsertParagraphAfter();

                    Word.Paragraph oPara3;
                    oPara3 = oDoc.Content.Paragraphs.Add();
                    oPara3.Range.Text = textDiscipline;
                    oPara3.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                    oPara3.Range.Font.Bold = 1;
                    oPara3.Range.Font.Size = 14;
                    oPara3.Format.SpaceAfter = 12;
                    oPara3.Range.InsertParagraphAfter();

                    Word.Paragraph oPara4;
                    oPara4 = oDoc.Content.Paragraphs.Add();
                    oPara4.Range.Text = textCompleteTime;
                    oPara4.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                    oPara4.Range.Font.Bold = 1;
                    oPara4.Range.Font.Size = 14;
                    oPara4.Format.SpaceAfter = 24;
                    oPara4.Range.InsertParagraphAfter();

                    Word.Paragraph oPar;
                    oPar = oDoc.Content.Paragraphs.Add(ref oMissing);
                    oPar.Range.Text = "Краткое описание работы";
                    oPar.Range.Font.Bold = 1;
                    oPar.Range.Font.Italic = 0;
                    oPar.Range.Font.Size = 24;
                    oPar.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPar.Format.SpaceAfter = 16;    //24 pt spacing after paragraph.
                    oPar.Range.InsertParagraphAfter();

                    Word.Paragraph oPara5;
                    oPara5 = oDoc.Content.Paragraphs.Add();
                    oPara5.Range.Text = textTask;
                    oPara5.Range.Font.Size = 14;
                    oPara5.Range.Font.Bold = 0;
                    oPara5.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                    oPara5.Format.SpaceAfter = 24;
                    oPara5.Range.InsertParagraphAfter();
                
           
        }

        private void ListViewItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (AppData.CurrentUser.Position == 1 || AppData.CurrentUser.Position == 2)
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

        private void ListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var disc = (sender as ListBoxItem).DataContext as Discipline;
            discVal = disc;
            UpdateShop();
        }

        private void BtnPagePrev_Click(object sender, RoutedEventArgs e)
        {
            if (NumberOfPage > 0)
            {
                NumberOfPage--;
                TBCurrentPage.Text = (NumberOfPage + 1).ToString();
                CheckPages();
                UpdateShop();
            }
        }

        private void BtnPageNext_Click(object sender, RoutedEventArgs e)
        {
            if (NumberOfPage < PagesCount)
            {
                NumberOfPage++;
                TBCurrentPage.Text = (NumberOfPage + 1).ToString();
                CheckPages();
                UpdateShop();
            }
        }

        private void CheckPages()
        {
            if (NumberOfPage > 0)
            {
                TBPrevPage.Text = (NumberOfPage).ToString();
                TBPrevPage.Visibility = Visibility.Visible;
                BtnPagePrev.BorderBrush = Brushes.Yellow;
            }
            else
            {
                TBPrevPage.Visibility = Visibility.Collapsed;
                BtnPagePrev.BorderBrush = Brushes.LightGray;
            }
            if (NumberOfPage < PagesCount)
            {
                TBNextPage.Text = (NumberOfPage + 2).ToString();
                TBNextPage.Visibility = Visibility.Visible;
                BtnPageNext.BorderBrush = Brushes.Yellow;
            }
            else
            {
                TBNextPage.Visibility = Visibility.Collapsed;
                BtnPageNext.BorderBrush = Brushes.LightGray;
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                AppData.db.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());


                var allTypes = AppData.db.Discipline.ToList();
                allTypes.Insert(0, new Discipline
                {
                    DisciplineName = "Любая дисциплина"
                });

                LBDisciplines.ItemsSource = allTypes;
                LBDisciplines.SelectedIndex = 0;

                if (AppData.CurrentUser.Position == 1 || AppData.CurrentUser.Position == 2)
                {
                    BtnDelete.Visibility = Visibility.Visible;
                    BtnAdd.Visibility = Visibility.Visible;
                }

                PagesCount = Convert.ToInt16(Math.Floor(((double)currentTasks.Count / maxItemShow) - 0.0000001));
                CheckPages();
                LViewTasks.ItemsSource = currentTasks.Skip(maxItemShow * NumberOfPage).Take(maxItemShow).ToList();
            }
        }
    }
    }
