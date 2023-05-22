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
using Word = Microsoft.Office.Interop.Word;
using System.IO;
using WpfApp2.Model;
using System.Runtime.InteropServices;

namespace WpfApp2.Views
{
    /// <summary>
    /// Логика взаимодействия для TaskBanksPage.xaml
    /// </summary>
    public partial class TaskBanksPage : Page
    {
        private int PagesCount;
        private int NumberOfPage = 0;
        private int maxItemShow = 5;
        List<TaskBanks> currentBanks = AppData.db.TaskBanks.ToList();
        public TaskBanksPage()
        {
            InitializeComponent();

        }

        private void BtnWord_Click(object sender, RoutedEventArgs e)
        {
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
            var dataForWord = (sender as Button).DataContext as TaskBanks;

            Word.Paragraph oPar1;
            oPar1 = oDoc.Content.Paragraphs.Add(ref oMissing);
            oPar1.Range.Text = dataForWord.BankName;
            oPar1.Range.Font.Bold = 1;
            oPar1.Range.Font.Name = "Times New Roman";
            oPar1.Range.Font.Size = 36;
            oPar1.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oPar1.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
            oPar1.Range.InsertParagraphAfter();

            foreach (var item in dataForWord.TaskList)
            {
                if (item.WordVersion != null)
                {
                    if (File.Exists(item.WordVersion))
                    {
                        FileInfo f = new FileInfo(item.WordVersion);
                        string fullFilePath = f.FullName;

                        var sourceDoc = oWord.Documents.Open(fullFilePath);
                        sourceDoc.ActiveWindow.Selection.WholeStory();
                        sourceDoc.ActiveWindow.Selection.Copy();

                        Word.Paragraph oPar0;
                        oPar0 = oDoc.Content.Paragraphs.Add(ref oMissing);
                        oPar0.Range.Paste();

                        sourceDoc.Close();
                        Marshal.ReleaseComObject(sourceDoc);

                        
                        oPar0.Range.InsertBreak();
                        continue;

                     }
                }
                string textName = item.TaskName;
                var author = AppData.db.Users.ToList().Where(u => u.ID == item.Author).FirstOrDefault();
                var discipline = AppData.db.Discipline.ToList().Where(u => u.ID == item.DisciplineID).FirstOrDefault();
                string textMadeBy = $"Задание создал(а): {author.Surname} {author.FirstName} {author.Patronymic}";
                string textDiscipline = $"Дисциплина: {discipline.DisciplineName}";
                string textCompleteTime = $"Время выполнения задания: {item.CompletionTime.ToString()} мин.";
                string textTask = item.Description;

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
                oPara5.Range.InsertBreak();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var tasksForRemoving = LViewBanks.SelectedItems.Cast<TaskBanks>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {tasksForRemoving.Count()} банков заданий?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AppData.db.TaskBanks.RemoveRange((IEnumerable<TaskBanks>)tasksForRemoving);
                    AppData.db.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    AppData.MainFrame.Navigate(new TaskBanksPage());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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

            PagesCount = Convert.ToInt16(Math.Floor(((double)currentBanks.Count / maxItemShow) - 0.0000001));
            CheckPages();
            LViewBanks.ItemsSource = currentBanks.Skip(maxItemShow * NumberOfPage).Take(maxItemShow).ToList();

        }

        private void ListViewItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (AppData.CurrentUser.Position == 1 || AppData.CurrentUser.Position == 2)
                if (sender != null)
                    NavigationService.Navigate(new TaskBanksAddEditPage((sender as ListViewItem).DataContext as TaskBanks));
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

                if (AppData.CurrentUser.Position == 1 || AppData.CurrentUser.Position == 2)
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
                PagesCount = Convert.ToInt16(Math.Floor(((double)currentBanks.Count / maxItemShow) - 0.0000001));
                CheckPages();
                LViewBanks.ItemsSource = currentBanks.Skip(maxItemShow * NumberOfPage).Take(maxItemShow).ToList();
            }
        }

    }
}
