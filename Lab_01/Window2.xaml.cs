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
using System.Windows.Shapes;

namespace Lab_01
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();

        }
        private void Exit_Btn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }
        private void FuncAfterClose(object sender, SelectionChangedEventArgs e)
        {
            int[,] area = {{ CB1.SelectedIndex, CB2.SelectedIndex, CB3.SelectedIndex, CB4.SelectedIndex, CB5.SelectedIndex },
                           { CB6.SelectedIndex, CB7.SelectedIndex, CB8.SelectedIndex, CB9.SelectedIndex, CB10.SelectedIndex },
                           { CB11.SelectedIndex, CB12.SelectedIndex, CB13.SelectedIndex, CB14.SelectedIndex, CB15.SelectedIndex },
                           { CB16.SelectedIndex, CB17.SelectedIndex, CB18.SelectedIndex, CB19.SelectedIndex, CB20.SelectedIndex },
                           { CB21.SelectedIndex, CB22.SelectedIndex, CB23.SelectedIndex, CB24.SelectedIndex, CB25.SelectedIndex } 
            };
            Winner.Content = LinesCheck(area);
            Turn.Content = ChangeTurn(Turn.Content.ToString());
            if (Winner.Content.ToString() != "")
                foreach (ComboBox b in this.myGrid.Children.OfType<ComboBox>())
                    b.IsEnabled = false;
        }
        static string ChangeTurn(string input)
        {
            if (input == "X") return "O";
            else return "X";
        }
        static string LinesCheck(int[,] area)
        {
            int counterx = 0;
            int countero = 0;
            for (int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if (area[i, j] == 0) counterx++;
                    if (area[i, j] == 1) countero++;
                }
                if (countero >= 4) return "O";
                if (counterx >= 4) return "X";
                counterx = countero = 0;
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (area[j, i] == 0) counterx++;
                    if (area[j, i] == 1) countero++;
                }
                if (countero >= 4) return "O";
                if (counterx >= 4) return "X";
                counterx = countero = 0;
            }
            for(int i = 0; i < 5; i++)
            {
                if (area[i, i] == 0) counterx++; if (area[i, i] == 1) countero++;
                if (countero >= 4) return "O"; if (counterx >= 4) return "X";
            }
            countero = counterx = 0;
            for(int i = 0; i < 5; i++)
            {
                if (area[i, 4 - i] == 0) counterx++; if (area[i, 4 - i] == 1) countero++;
                if (countero >= 4) return "O"; if (counterx >= 4) return "X";
            }
            countero = counterx = 0;
            for (int i = 1; i < 5; i++)
            {
                if (area[i, i - 1] == 0) counterx++; if (area[i, i - 1] == 1) countero++;
                if (countero >= 4) return "O"; if (counterx >= 4) return "X";
            }
            countero = counterx = 0;
            for (int i = 1; i < 5; i++)
            {
                if (area[i - 1, i] == 0) counterx++; if (area[i - 1, i] == 1) countero++;
                if (countero >= 4) return "O"; if (counterx >= 4) return "X";
            }
            countero = counterx = 0;
            for (int i = 0; i < 4; i++)
            {
                if (area[i, 3 - i] == 0) counterx++; if (area[i, 3 - i] == 1) countero++;
                if (countero >= 4) return "O"; if (counterx >= 4) return "X";
            }
            countero = counterx = 0;
            for (int i = 1; i < 5; i++)
            {
                if (area[i, 5 - i] == 0) counterx++; if (area[i, 5 - i] == 1) countero++;
                if (countero >= 4) return "O"; if (counterx >= 4) return "X";
            }
            return "";

        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            Window2 w2 = new Window2();
            Hide();
            w2.Show();
        }
    }
}
