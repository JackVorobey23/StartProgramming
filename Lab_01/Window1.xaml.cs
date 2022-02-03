using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Lab_01
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            
        }

        private void Exit_Btn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }

        private void Write_Btn(object sender, RoutedEventArgs e)
        {
            string creditBook = CreditBook.Text;
            string fullName = FullName.Text;
            string data = Data.Text;
            StreamReader file = new StreamReader("StudentData.txt");
            string dataFromFile = file.ReadToEnd();
            file.Close();
            StreamWriter MyFileA = new StreamWriter("StudentData.txt");
            MyFileA.Write(dataFromFile);
            MyFileA.Write($"№Залікової: {creditBook}; ПІП: {fullName}; Особисті дані:{data}\n");
            MyFileA.Close();
            CreditBook.Text = FullName.Text = Data.Text = "";
        }

        private void Delete_Btn(object sender, RoutedEventArgs e)
        {
            string creditBookDlt = CreditBookDlt.Text;
            CreditBookDlt.Text = "";
            StreamReader fileRem = new StreamReader("StudentData.txt");
            string dataFromFile = fileRem.ReadToEnd();
            fileRem.Close();
            StreamReader file = new StreamReader("StudentData.txt");
            while (!file.EndOfStream)
            {
                string DeleteStr = file.ReadLine();

                if (creditBookDlt == DeleteStr.Split(' ')[1].Trim(';'))
                {
                    file.Close();
                    StreamWriter fileDlt = new StreamWriter("StudentData.txt");
                    fileDlt.Write(dataFromFile.Replace("\n" + DeleteStr, ""));
                    fileDlt.Close();
                    break;
                }
            }
        }
    }
}
