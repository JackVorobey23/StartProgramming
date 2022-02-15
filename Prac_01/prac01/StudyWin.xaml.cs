using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using System.Windows;
using System.Windows.Controls;

namespace prac01
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
        public static Random rnd = new Random();
        public List<List<double>> intervals = new List<List<double>>();
        public List<List<double>> dispersions = new List<List<double>>();
        public double interval = 0;
        public int counter = 3;
        public int intercount = 0;
        public (double, double) CountDispersions(List<double> dispersions)
        {
            int lenght = dispersions.Count;
            double M = 0, S = 0;

            for(int i = 0; i < lenght; i++)
                M += dispersions[i];
            M /= lenght;
            for(int i = 0; i < lenght; i++)
                S += Math.Pow(dispersions[i] - M, 2);
            S /= lenght - 1;
            S = Math.Sqrt(S);
            return (S,M);
        }
        public static bool CountExept(List<double> arr, double yi)
        {

            arr.Remove(yi);

            double Mi = 0;
            for (int k = 0; k < arr.Count; k++)
                Mi += arr[k];
            Mi /= arr.Count;

            double Si = 0;
            for (int k = 0; k < arr.Count; k++)
                Si += Math.Pow(arr[k] - Mi, 2);
            Si /= arr.Count - 1;
            Si = Math.Sqrt(Si);

            double Tp = Math.Abs((yi - Mi) / (Si / arr.Count));

            Chart chart = new Chart();
            StatisticFormula f = chart.DataManipulator.Statistics;
            double Tt = f.InverseTDistribution(0.05, arr.Count - 1);

            arr.Add(yi);

            if (Tp > Tt) return true;

            else return false;
        }
        private void Stop_Study_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < intervals.Count; i++)
            {
                for(int j = 0; j < intervals[i].Count; j++)
                {
                    if (CountExept(intervals[i], intervals[i][j]))
                    {
                        intervals[i].Remove(intervals[i][j]);
                    }
                }
            }
            StreamWriter file = new StreamWriter("IntervalData.txt", true);
            int k = 1;
            foreach (List<double> arr in intervals)
            {
                foreach (double elem in arr)
                {
                    file.Write(elem + ";");
                }
                file.Write($" Disp = {Math.Round(CountDispersions(arr).Item1, 3)} " +
                    $"MatExpect = {Math.Round(CountDispersions(arr).Item2, 3)} - {k++} att.\n");
            }
            file.Close();
            ExamWin ew = new ExamWin();
            Hide();
            ew.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }

        private void Accept_Code_Word(object sender, RoutedEventArgs e)
        {
            CodeWord.Text = InputCodeWord.Text;
            StreamWriter file = new StreamWriter("IntervalData.txt");
            file.Write(InputCodeWord.Text + "- CodeWord;\n");
            file.Close();
            ICWBack.Visibility = ICWBack2.Visibility = ICWText.Visibility =
            ICWButton.Visibility = InputCodeWord.Visibility = Visibility.Hidden;
        }

        private void ChangeAttempt(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(Attempt.Content) != 7)
                Attempt.Content = Convert.ToInt32(Attempt.Content) + 1;
            else
                Attempt.Content = 3;
            counter = Convert.ToInt32(Attempt.Content);
        }

        private void NextAtt(object sender, RoutedEventArgs e)
        {
            Attempt.IsEnabled = false;
            InputCode.Text = "";
            intercount++;
            InputCode.IsEnabled = true;
            counter--;
            if (counter == 1)
            {
                NextAttempt.IsEnabled = false;
                StreamWriter file = new StreamWriter("IntervalData.txt", true);
                file.Write(Attempt.Content.ToString() + "- Attempt;\n");
                file.Close();
            }
        }
        private void ChangedInput(object sender, TextChangedEventArgs e)
        {
            SymbolsCount.Text = InputCode.Text.Length.ToString();
            if(InputCode.Text.Length == 0)
            {
                return;
            }
            int pos = InputCode.Text.Length - 1;
            if (InputCode.Text[pos] == CodeWord.Text[pos])
            {
                if (interval != 0)
                {
                    intervals[intercount].Add(Math.Round(Convert.ToDouble(Convert.ToInt64(DateTime.Now.Ticks) / 10000) / 1000 - interval, 3));
                    interval = Convert.ToDouble(Convert.ToInt64(DateTime.Now.Ticks) / 10000) / 1000;
                }
                else
                {
                    intervals.Add(new List<double>());
                    interval = Convert.ToDouble(Convert.ToInt64(DateTime.Now.Ticks) / 10000) / 1000;
                }
                if (InputCode.Text == CodeWord.Text)
                {
                    interval = 0;
                    InputCode.IsEnabled = false;
                }
            } else
            {
                InputCode.Text = InputCode.Text.Remove(pos);
            }
        }
    }
}
