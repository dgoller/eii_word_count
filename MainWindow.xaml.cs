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
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WordCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class WordCount
        {
            public string Word { get; set; }
            public int Count { get; set; }

            public WordCount(string word, int count) {
                Word = word;
                Count = count;
            }
        }
        private ObservableCollection<WordCount> wordCountList;
        public ObservableCollection<WordCount> WordCountList {
            get
            {
                return wordCountList;
            }
            set
            {
                wordCountList = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            wordCountList =  new ObservableCollection<WordCount>();
            WordCountList = new ObservableCollection<WordCount>();
        }

        private void btnFileOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Text files (*.txt)|*.txt";
            if(fileDialog.ShowDialog() == true)
            {
                fileName.Text = fileDialog.FileName;
                fileText.Text = File.ReadAllText(fileDialog.FileName);
                getWordCount(fileText.Text);
            }
        }

        private void getWordCount(string fileText)
        {
            string lowText = fileText.ToLower();
            string[] words = lowText.Split(new char[] { '.', '?', '!', ' ', ':', ',', '\r', '\n', ';' }, StringSplitOptions.RemoveEmptyEntries);
            ObservableCollection<WordCount> tempWordCountList = new ObservableCollection<WordCount>();

            foreach (string word in words)
            {
                if (tempWordCountList == null)
                {
                    var tempWordCount = new WordCount(word, 1);
                    tempWordCountList = new ObservableCollection<WordCount>((IEnumerable<WordCount>)tempWordCount);
                }
                else
                { 
                    var found = tempWordCountList.FirstOrDefault(x => x.Word == word);
                    if (found != null)
                    {
                        int index = tempWordCountList.IndexOf(found);
                        found.Count++;
                        tempWordCountList[index] = found;
                    }
                    else
                    {
                        tempWordCountList.Add(new WordCount(word, 1));
                    }
                }
            }

            WordCountList = tempWordCountList;
            wordList.ItemsSource = WordCountList;
            wordList.IsEnabled = true;
        }
    }
}
