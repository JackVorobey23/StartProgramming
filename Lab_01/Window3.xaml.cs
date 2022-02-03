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
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        static bool check = false;
        static string FirstPart = "";
        static string SecondPart = "";
        static string Action = "";
        public Window3()
        {
            InitializeComponent();
        }

        private void Exit_Btn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }

        private void Clean(object sender, RoutedEventArgs e)
        {
            MainRect.Content = "";
        }

        private void NumBtn(object sender, RoutedEventArgs e)
        {
            if (check)
            {
                MainRect.Content = "";
                check = false;
            }
            string num = sender.ToString().Split(' ')[1];
            MainRect.Content += num;
        }

        private void Rem(object sender, RoutedEventArgs e)
        {
            string content = MainRect.Content.ToString();
            if (content.Length > 0) MainRect.Content = content.Remove(content.Length - 1);
        }

        private void ActionFunc(object sender, RoutedEventArgs e)
        {
            if (FirstPart != "")
            {
                SecondPart = MainRect.Content.ToString();
                switch (Action)
                {
                    case "÷":
                        MainRect.Content = Convert.ToDouble(FirstPart) / Convert.ToDouble(SecondPart);
                        break;
                    case "x":
                        MainRect.Content = Convert.ToDouble(FirstPart) * Convert.ToDouble(SecondPart);
                        break;
                    case "-":
                        MainRect.Content = Convert.ToDouble(FirstPart) - Convert.ToDouble(SecondPart); 
                        break;
                    case "+":
                        MainRect.Content = Convert.ToDouble(FirstPart) + Convert.ToDouble(SecondPart); 
                        break;
                }
                Action = sender.ToString().Split(' ')[1];
                FirstPart = MainRect.Content.ToString();
            }
            else
            {
                Action = sender.ToString().Split(' ')[1];
                FirstPart = MainRect.Content.ToString();
            }
            check = true;
        }
    }
}
