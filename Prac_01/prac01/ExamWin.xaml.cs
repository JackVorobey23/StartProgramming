using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace prac01
{
    /// <summary>
    /// Логика взаимодействия для ExamWin.xaml
    /// </summary>
    public partial class ExamWin : Window
    {
        public int counterAu = 0;
        public List<double> dispersStudy = new List<double>();
        public List<double> MatExpectStudy = new List<double>();
        public double Ft = 0;
        public ExamWin()
        {
            InitializeComponent();
            StreamReader file = new StreamReader("IntervalData.txt");
            while (!file.EndOfStream)
            {
                string temp = file.ReadLine();
                if (temp.Contains("att."))
                {
                    dispersStudy.Add(Convert.ToDouble(temp.Split(' ')[3]));
                    MatExpectStudy.Add(Convert.ToDouble(temp.Split(' ')[6]));
                }
                if (temp.Contains("CodeWord"))
                    CodeWord.Text = temp.Split('-')[0];
                if (temp.Contains("Attempt"))
                    Attempts.Text = temp.Split('-')[0];
                counterAu = Convert.ToInt32(Attempts.Text);
            }
            file.Close();
        }

        public double intervalAu = 0;
        public List<List<double>> intervalsAu = new List<List<double>>();
        public List<double> dispersAu = new List<double>();
        public List<double> MatExpectAu = new List<double>();
        public int intercountAu = 0;
        public int[] r;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 sw = new Window1();
            Hide();
            sw.Show();
        }
        public void MainCount()
        {
            foreach (List<double> list in intervalsAu)
            {
                dispersAu.Add(CountDispersions(list).Item1);
                MatExpectAu.Add(CountDispersions(list).Item2);
            }
            Ft = FisherCount(CodeWord.Text.Length);
            Dispers.Text = DisperseCount();
            Pident.Text = P_identCount();
            FirstKindErr.Text = FirstKindErrC();
            SecKindErr.Text = SecondKindErrC();
        }
        public string DisperseCount()
        {
            int countRes = 0;
            foreach(double dispersion in dispersAu)
            {
                foreach(double dispersionSt in dispersStudy)
                {
                    double Fp = Math.Pow(Math.Max(dispersion, dispersionSt), 2) /
                    Math.Pow(Math.Min(dispersion, dispersionSt), 2);
                    if (Fp > Ft) countRes++;
                    else countRes--;
                }
            }
            if (countRes > 0)
                return "дисперсії неоднорідні";
            else
                return "дисперсії рівні";
        }
        public string P_identCount()
        {
            r = new int[dispersAu.Count];
            double n = CodeWord.Text.Length - 2;
            double[] P_arr = new double[dispersAu.Count];
            for(int i = 0; i < dispersAu.Count; i++)
            {
                for(int j = 0; j < dispersStudy.Count; j++)
                {
                    double dispAu = dispersAu[i], dispSt = dispersStudy[j];
                    double matExAu = MatExpectAu[i], matExSt = MatExpectStudy[j];
                    double S = Math.Sqrt(((Math.Pow(dispAu, 2) + Math.Pow(dispSt, 2)) * (n - 1)) / (2 * n - 1));
                    double Tp = Math.Abs(matExAu - matExSt) / (S * Math.Sqrt(2 / n));
                    Chart chart = new Chart();
                    StatisticFormula f = chart.DataManipulator.Statistics;
                    double Tt = f.InverseTDistribution(0.05, Convert.ToInt32(n - 1)) / 7.0;
                    if (Tt >= Tp) r[i]++;
                }
                P_arr[i] = r[i] / n;
            }
            return Math.Round(Avarage(P_arr), 2).ToString();
        }
        public string FirstKindErrC()
        {
            int att = Convert.ToInt32(Attempts.Text);
            double[] P1_arr = new double[att];
            for(int i = 0; i < P1_arr.Length; i++)
                P1_arr[i] = (att - r[i]) / att / 1.0;

            return Math.Round(Avarage(P1_arr), 2).ToString();
        }
        public string SecondKindErrC()
        {
            int att = Convert.ToInt32(Attempts.Text);
            double[] P2_arr = new double[att];
            for (int i = 0; i < P2_arr.Length; i++)
                P2_arr[i] = r[i] / att / 1.0;

            return Math.Round(Avarage(P2_arr), 2).ToString();
        }
        public double FisherCount(int num)
        {
            double[] arr = { 161, 19, 9.28, 6.39, 5.05, 4.28, 3.79, 3.44, 3.18, 2.98, 2.72 };
            return arr[num + 1];
        }
        public (double, double) CountDispersions(List<double> dispersions)
        {
            int lenght = dispersions.Count;
            double M = 0, S = 0;

            for (int i = 0; i < lenght; i++)
                M += dispersions[i];
            M /= lenght;
            for (int i = 0; i < lenght; i++)
                S += Math.Pow(dispersions[i] - M, 2);
            S /= lenght - 1;
            S = Math.Sqrt(S);
            return (S, M);
        }

        public double Avarage(double[] arr)
        {
            double res = 0;
            for (int i = 0; i < arr.Length; i++)
                res += arr[i];
            res /= arr.Length;
            return res;
        }
        private void ChangedInput(object sender, TextChangedEventArgs e)
        {
            CountSym.Text = InputCode.Text.Length.ToString();
            if (InputCode.Text.Length == 0)
            {
                return;
            }
            int pos = InputCode.Text.Length - 1;
            if (InputCode.Text[pos] == CodeWord.Text[pos])
            {
                if (intervalAu != 0)
                {
                    intervalsAu[intercountAu].Add(Math.Round(Convert.ToDouble(Convert.ToInt64(DateTime.Now.Ticks) / 10000) / 1000 - intervalAu, 3));
                    intervalAu = Convert.ToDouble(Convert.ToInt64(DateTime.Now.Ticks) / 10000) / 1000;
                }
                else
                {
                    intervalsAu.Add(new List<double>());
                    intervalAu = Convert.ToDouble(Convert.ToInt64(DateTime.Now.Ticks) / 10000) / 1000;
                }
                if (InputCode.Text == CodeWord.Text)
                {
                    intervalAu = 0;
                    InputCode.IsEnabled = false;
                    if(counterAu == 1)
                    {
                        MainCount();
                    }
                }
            }
            else
            {
                InputCode.Text = InputCode.Text.Remove(pos);
            }
        }
        private void NextAttempt_Click(object sender, RoutedEventArgs e)
        {
            InputCode.Text = "";
            intercountAu++;
            InputCode.IsEnabled = true;
            counterAu--;
            if (counterAu == 1)
            {
                NextAttempt.IsEnabled = false;
            }
        }
    }
}
