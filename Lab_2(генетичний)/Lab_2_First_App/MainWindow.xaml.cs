using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Lab_2_First_App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static DispatcherTimer dT;
        bool flag = true;
        static int Radius = 30;
        static int PointCount = 5;
        static Polygon myPolygon = new Polygon();
        static List<Ellipse> EllipseArray = new List <Ellipse>();
        static PointCollection pC = new PointCollection();
        static int ParentsCount = 10;
        static Random rnd = new Random();
        List<List<Point>> firstPopulation;

        public MainWindow()
        {
            dT = new DispatcherTimer();

            InitializeComponent();
            InitPoints();
            firstPopulation = FirstGenerate();
            GenerateToPc(firstPopulation);
            InitPolygon();
            dT = new DispatcherTimer();
            dT.Tick += new EventHandler(OneStep);
            dT.Interval = new TimeSpan(0, 0, 0, 0, 1000);
        }

        private void InitPoints()
        {
            Random rnd = new Random();
            pC.Clear();
            EllipseArray.Clear();

            for (int i = 0; i < PointCount; i++)
            {
                Point p = new Point();

                p.X = rnd.Next(Radius, (int)(0.75*MainWin.Width)-3*Radius);
                p.Y = rnd.Next(Radius, (int)(0.90*MainWin.Height-3*Radius));                
                pC.Add(p);
            }
            
            for (int i = 0; i < PointCount; i++)
            { 
                Ellipse el = new Ellipse();

                el.StrokeThickness = 2;
                el.Height = el.Width = Radius;
                el.Stroke = Brushes.Black;
                el.Fill = Brushes.LightBlue;
                EllipseArray.Add(el); 
            }            
        }

        private void InitPolygon()
        {
            myPolygon.Stroke = System.Windows.Media.Brushes.Black;            
            myPolygon.StrokeThickness = 2;            
        }

        private void PlotPoints()
        {            
            for (int i=0; i<PointCount; i++)
            {
                Canvas.SetLeft(EllipseArray[i], pC[i].X - Radius/2);
                Canvas.SetTop(EllipseArray[i], pC[i].Y - Radius/2);
                MyCanvas.Children.Add(EllipseArray[i]);
            }
        }


        private void PlotWay(PointCollection Points)
        {
            myPolygon.Points = Points;
            MyCanvas.Children.Add(myPolygon);
        }

        private void VelCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)e.Source;
            ListBoxItem item = (ListBoxItem)CB.SelectedItem;
                        
            dT.Interval = new TimeSpan(0,0,0,0,Convert.ToInt16(item.Content));
        }

        private void StopStart_Click(object sender, RoutedEventArgs e)
        {
            if (dT.IsEnabled)
            {
                dT.Stop();
                NumElemCB.IsEnabled = true;
            }
            else
            {
                if (flag)
                {
                    firstPopulation = FirstGenerate();
                    flag = false;
                }
                if(Convert.ToInt32(NumElemCB.SelectionBoxItem) == firstPopulation[0].Count)
                {
                    dT.Start();
                    return;
                }
                NumElemCB.IsEnabled = false;
                InitPoints();
                firstPopulation = FirstGenerate();
                GenerateToPc(firstPopulation);
                InitPolygon();
                dT.Start();
            }
        }

        private void NumElemCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)e.Source;
            ListBoxItem item = (ListBoxItem)CB.SelectedItem;
            PointCount = Convert.ToInt32(item.Content);
            InitPoints();
            InitPolygon();
        }

        private void OneStep(object sender, EventArgs e)
        {
            MyCanvas.Children.Clear();
            //InitPoints();
            PlotPoints();
            PlotWay(pC);
            firstPopulation = GenerateNextPopulation(firstPopulation);
            GenerateToPc(firstPopulation);
        }
        public void GenerateToPc(List<List<Point>> Population)
        {
            int shortest = 0;
            for(int i = 1; i < Population.Count; i++)
            {
                if(CalcLenght(Population[i]) < CalcLenght(Population[shortest]))
                {
                    shortest = i;
                }
            }
            for(int i = 0; i < pC.Count; i++)
            {
                pC[i] = Population[shortest][i];
            }
        }
        public List<List<Point>> FirstGenerate()
        {
            List<List<Point>> arr = new List<List<Point>>();
            arr.Add(new List<Point>());

            for (int i = 0; i < PointCount; i++)
                arr[0].Add(pC[i]);

            for (int i = 1; i < ParentsCount; i++)
                arr.Add(GenRandomArr(arr[i-1]));

            return arr;
        }
        public List<Point> GenRandomArr(List<Point> m)
        {
            List<Point> arr = new List<Point>();
            for(int i = 0; i < m.Count; i++)
            {
                arr.Add(m[i]);
            }
            for(int i = 0; i < rnd.Next(3,8); i++)
            {
                int pos1 = rnd.Next(0, arr.Count);
                int pos2 = rnd.Next(0, arr.Count);
                Point temp = arr[pos1];
                arr[pos1] = arr[pos2];
                arr[pos2] = temp;
            }
            return arr;
        }// генерация случайного маршрута
        public List<List<Point>> GenerateNextPopulation(List<List<Point>> presPop)
        {
            List<Point> firstPar = presPop[rnd.Next(ParentsCount)];
            List<Point> secondPar = presPop[rnd.Next(ParentsCount)];

            while(secondPar == firstPar)
                secondPar = presPop[rnd.Next(ParentsCount)];

            (List<Point> fChildren, List<Point> sChildren) = GenerateChildren(firstPar,secondPar);
            presPop.Add(fChildren); presPop.Add(sChildren);
            List<List<Point>> nextPop = FindLongest(presPop);

            return nextPop;
        }// генерация следующей популяции
        public (List<Point>,List<Point>) GenerateChildren(List<Point> fPar, List<Point> sPar) 
        {
            int crossPoint = rnd.Next(1, fPar.Count - 1);
            List<Point> fChildren = GenerateChild(fPar, sPar, crossPoint);
            List<Point> sChildren = GenerateChild(sPar, fPar, crossPoint);
            Mutation(fChildren, sChildren);
            return (fChildren, sChildren);
        }// генерация "детей"
        public void Mutation(List<Point> fChildren, List<Point> sChildren)
        {
            if(rnd.Next(100) > 1)
            {
                int pos1 = rnd.Next(fChildren.Count);
                int pos2 = rnd.Next(fChildren.Count);
                Point temp = fChildren[pos1];
                fChildren[pos1] = fChildren[pos2];
                fChildren[pos2] = temp;
            }
        }// генерация мутаций для "детей"
        public List<Point> GenerateChild(List<Point> fPar, List<Point> sPar, int crossPoint)
        {
            List<Point> Children = new List<Point>();
            List<Point> uslessGen = new List<Point>();
            for (int i = 0; i < crossPoint; i++)
            {
                Children.Add(fPar[i]);
                uslessGen.Add(fPar[i]);
            }
            for (int i = crossPoint; i < sPar.Count; i++)
            {
                if (!uslessGen.Contains(sPar[i]))
                {
                    Children.Add(sPar[i]);
                    uslessGen.Add(sPar[i]);
                }
            }
            while (Children.Count != fPar.Count)
            {
                for (int i = 0; i < fPar.Count; i++)
                    if (!uslessGen.Contains(fPar[i]))
                        Children.Add(fPar[i]);
            }
            return Children;
        }// геренация отдельного ребёнка относительно выбранных родителей
        static List<List<Point>> FindLongest(List<List<Point>> Population)
        {
            List<double> ways = new List<double>();

            for(int i = 0; i < Population.Count; i++)
                ways.Add(CalcLenght(Population[i]));

            Population.RemoveAt(FindMax(ways)); ways.RemoveAt(FindMax(ways));
            Population.RemoveAt(FindMax(ways)); ways.RemoveAt(FindMax(ways));

            return Population;
        }// поиск и удаление двух наибольших(менее эффективных) путей
        static double CalcLenght(List<Point> arr)
        {
            double lenght = 0;
            for(int i = 0; i < arr.Count - 1; i++)
            {
                double coord1 = arr[i].X; double coord2 = arr[i].Y;
                double coord3 = arr[i+1].X; double coord4 = arr[i+1].Y;

                double tempLen = Math.Pow((coord3 - coord1),2) + Math.Pow((coord4 - coord2),2);
                tempLen = Math.Sqrt(tempLen);
                tempLen = Math.Round(tempLen,2);
                lenght += tempLen;
            }
            return lenght;
        }// подсчет длинны маршрута
        static int FindMax(List<double> arr)
        {
            double temp = arr[0];
            int res = 0;
            for (int i = 1; i < arr.Count; i++)
            {
                if (arr[i] > temp)
                {
                    temp = arr[i];
                    res = i;
                }
            }
            return res;
        }// нахождение индексов максимальных элементов в массиве
    }
}