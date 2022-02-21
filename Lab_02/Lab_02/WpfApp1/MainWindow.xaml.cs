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
namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_Btn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateStudentWin sw = new CreateStudentWin();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CreateTicTacToeWin mw = new CreateTicTacToeWin();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            CreateCalcalatorWin mw = new CreateCalcalatorWin();
            Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            CreateAboutDevWin mw = new CreateAboutDevWin();
            Close();
        }
    }
}