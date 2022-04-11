using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HideAll();
        }
        static string connString = "Data Source=EYGEH;Initial Catalog=Library;Integrated Security=True";
        public static SqlConnection conn = new SqlConnection(connString);
        string BackToTarget = "Book_Find";
        bool AcademDegree = true, AddBook = false, BookDelete = false, LibrarianAccessFlag = false;
        static User LogInUser = null;
        static Book TempBookInfo = null;
        private void TitleSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = TitleSearch.Text;
            if (search.Length > 0)
            {
                TitleSearch_Label.Content = "";
                CiphreSearch.Text = "";
            }
            else
            {
                TitleSearch_Label.Content = "За назвою...";
                return;
            }
            Border ToRem = new Border();
            for (int j = 0; j < 9; j++)
            {
                foreach (Border bd in BF_Grid.Children.OfType<Border>())
                {
                    if (bd.Name.Contains("BookInSearch_"))
                    {
                        ToRem = bd;
                        break;
                    }
                }
                BF_Grid.Children.Remove(ToRem);
            }
            List<Book> tempBooks = new List<Book>();
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM Books WHERE Title LIKE '%{search}%'", conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Book book = new Book
                    {
                        Cipher = Convert.ToInt32(reader.GetValue(0)),
                        Title = reader.GetValue(1).ToString(),
                        Autor = reader.GetValue(2).ToString(),
                        Year = Convert.ToInt32(reader.GetValue(3)),
                        Hall_ID = Convert.ToInt32(reader.GetValue(4)),
                        Book_Left = Convert.ToInt32(reader.GetValue(5)),
                        Book_Amount = Convert.ToInt32(reader.GetValue(6))
                    };
                    tempBooks.Add(book);
                }
                reader.Close();
                conn.Close();
            }
            catch
            {
                conn.Close();
                return;
            }
            int i = 0;
            foreach (Book book in tempBooks)
            {
                if (i > 8) break;
                Border bd = new Border
                {
                    Margin = new Thickness(18, 160 + i * 37, 18, 312 - i * 37),
                    Background = new SolidColorBrush(Color.FromArgb(38, 141, 145, 255)),
                    CornerRadius = new CornerRadius(4),
                    Name = $"BookInSearch_{i}",
                };
                Label lb = new Label
                {
                    Content = $"{book.Title}({book.Book_Left} лишилось)",
                    FontSize = 24,
                    Padding = new Thickness(0),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 203, 215, 255)),
                };
                bd.Child = lb;
                if (AddBook) bd.MouseLeftButtonDown += AddBookFunc;
                BF_Grid.Children.Add(bd);
                i++;
            }
        }
        private void CiphreSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CiphreSearch.Text.Length > 0)
            {
                CiphreSearch_Label.Content = "";
                TitleSearch.Text = "";
            }
            else
            {
                CiphreSearch_Label.Content = "За шифром...";
                return;
            }
            Border ToRem = new Border();
            int i = 0;
            for (int j = 0; j < 9; j++)
            {
                foreach (Border bd in BF_Grid.Children.OfType<Border>())
                {
                    if (bd.Name.Contains("BookInSearch_"))
                    {
                        ToRem = bd;
                        break;
                    }
                }
                BF_Grid.Children.Remove(ToRem);
            }
            string search = CiphreSearch.Text;
            List<Book> tempBooks = new List<Book>();
            conn.Open();
            try
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Books WHERE Cipher = {search}", conn);
                SqlDataReader reader = command.ExecuteReader(); while (reader.Read())
                {
                    Book book = new Book
                    {
                        Cipher = Convert.ToInt32(reader.GetValue(0)),
                        Title = reader.GetValue(1).ToString(),
                        Autor = reader.GetValue(2).ToString(),
                        Year = Convert.ToInt32(reader.GetValue(3)),
                        Hall_ID = Convert.ToInt32(reader.GetValue(4)),
                        Book_Left = Convert.ToInt32(reader.GetValue(5)),
                        Book_Amount = Convert.ToInt32(reader.GetValue(6))
                    };
                    tempBooks.Add(book);
                }
                reader.Close();
                conn.Close();
            }
            catch
            {
                conn.Close();
                return;
            }
            foreach (Book book in tempBooks)
            {
                if (i > 8) break;
                if (book.Cipher.ToString() == search)
                {
                    Border bd = new Border
                    {
                        Margin = new Thickness(18, 160 + i * 37, 18, 312 - i * 37),
                        Background = new SolidColorBrush(Color.FromArgb(38, 141, 145, 255)),
                        CornerRadius = new CornerRadius(4),
                        Name = $"BookInSearch_{i}",
                    };
                    Label lb = new Label
                    {
                        Content = $"{book.Title}({book.Book_Left} лишилось)",
                        FontSize = 24,
                        Padding = new Thickness(0),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Foreground = new SolidColorBrush(Color.FromArgb(255, 203, 215, 255)),
                    };
                    bd.Child = lb;
                    bd.MouseLeftButtonDown += AddBookFunc;
                    BF_Grid.Children.Add(bd);
                    i++;
                }
            }

        }
        private void AddBookFunc(object sender, MouseButtonEventArgs e)
        {
            if (!AddBook) return;
            HideAll();
            conn.Open();
            Border bd = sender as Border;
            Label lb = bd.Child as Label;
            string Title = lb.Content.ToString().Split('(')[0];
            int Cipher = 0;
            DateTime DT = DateTime.Now;
            string comm = $"SELECT * FROM Books WHERE Title = '{Title}'";
            SqlCommand command = new SqlCommand(comm, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Cipher = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            comm = $"INSERT INTO Book_User VALUES ({LogInUser.Ticket_Num},{Cipher},'{DT.Year}-{DT.Month}-{DT.Day}')";
            command = new SqlCommand(comm, conn);
            command.ExecuteNonQuery();

            comm = $"UPDATE Books SET Book_Left = Book_Left - 1 WHERE Cipher = {Cipher}";
            command = new SqlCommand(comm, conn);
            command.ExecuteNonQuery();

            Label newBookLabel = new Label
            {
                Content = Title,
                Background = new SolidColorBrush(Color.FromArgb(38, 141, 145, 255)),
                Padding = new Thickness(10, 0, 0, 0),
                FontSize = 25,
                Foreground = new SolidColorBrush(Color.FromArgb(100, 168, 186, 255)),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 616,
                Height = 34,
                FontWeight = FontWeights.Thin,
                FontFamily = new FontFamily("Century Gothic"),
                Margin = new Thickness(0, MyBooks.Children.Count * 39, 0, 0),
                BorderBrush = new SolidColorBrush(Color.FromArgb(64, 255, 171, 106))
            };
            newBookLabel.MouseLeftButtonDown += DelBookFromProfile;
            MyBooks.Children.Add(newBookLabel);

            foreach (Label tlb in MyBooks.Children)
            {
                if (Convert.ToInt32(tlb.Margin.Top) < 0 ||
                   Convert.ToInt32(tlb.Margin.Top) > 39 * 3)
                    tlb.Visibility = Visibility.Hidden;
                else tlb.Visibility = Visibility.Visible;
            }
            ShowPage("Profile_About");
            conn.Close();
        }
        private void DelBookFromProfile(object sender, MouseButtonEventArgs e)
        {
            if (!BookDelete) return;
            BookDelete = false;
            conn.Open();
            Label lb = sender as Label;
            int marg = Convert.ToInt32(lb.Margin.Top);
            foreach (Label tlb in MyBooks.Children)
            {
                if (tlb.Margin.Top > marg)
                {
                    tlb.Margin = new Thickness(0, tlb.Margin.Top - 39, 0, 0);
                }
            }
            int Cipher = 0;
            string comm = $"SELECT * FROM Books WHERE Title = '{lb.Content}'";
            SqlCommand command = new SqlCommand(comm, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Cipher = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            comm = $"DELETE FROM Book_User WHERE (Cipher = {Cipher}) AND (Ticket_Num = {LogInUser.Ticket_Num})";
            command = new SqlCommand(comm, conn);
            command.ExecuteNonQuery();

            comm = $"UPDATE Books SET Book_Left = Book_Left + 1 WHERE Title = '{lb.Content}'";
            command = new SqlCommand(comm, conn);
            command.ExecuteNonQuery();

            conn.Close();
            foreach (Label tlb in MyBooks.Children)
            {
                tlb.BorderThickness = new Thickness(0);
            }
            MyBooks.Children.Remove(lb);

        }
        private void Login_Button_Click(object sender, MouseButtonEventArgs e)
        {
            int Log = 0;
            try
            {
                Log = Convert.ToInt32(LoginTicket.Text);
            }
            catch
            {
                return;
            }
            HideAll();
            if (!(Log > 0 && LoginPass.Text.Contains(Log.ToString() + "pass")))
                return;

            conn.Open();
            SqlCommand GetUser = new SqlCommand($"SELECT * FROM Users WHERE Passport = {Log};", conn);
            SqlDataReader reader = GetUser.ExecuteReader();
            while (reader.Read())
            {
                LogInUser = new User
                {
                    Passport = Convert.ToInt32(reader.GetValue(0)),
                    Surname = reader.GetValue(1).ToString(),
                    Ticket_Num = Convert.ToInt32(reader.GetValue(2)),
                    TeleNum = reader.GetValue(3).ToString(),
                    BirthDay = Convert.ToDateTime(reader.GetValue(4)),
                    Education = Convert.ToInt32(reader.GetValue(5)),
                    AcademDegree = Convert.ToBoolean(reader.GetValue(6)),
                };
            }
            reader.Close();
            SqlCommand GetHallUser = new SqlCommand($"SELECT * FROM User_Hall WHERE Ticket_Num = {LogInUser.Ticket_Num};", conn);
            reader = GetHallUser.ExecuteReader();
            reader.Read();
            int Hall = Convert.ToInt32(reader.GetValue(0));
            reader.Close();

            SqlCommand GetBookCiphres = new SqlCommand($"USE Library SELECT * FROM Book_User WHERE Ticket_Num = {LogInUser.Ticket_Num};", conn);
            reader = GetBookCiphres.ExecuteReader();
            List<int> Ciphres = new List<int>();
            while (reader.Read())
            {
                Ciphres.Add(Convert.ToInt32(reader.GetValue(1)));
            }
            reader.Close();
            string cont = "";
            for (int j = 0; j < Ciphres.Count; j++)
            {
                SqlCommand GetBookTitles = new SqlCommand($"USE Library SELECT * FROM Books WHERE Cipher = {Ciphres[j]};", conn);
                reader = GetBookTitles.ExecuteReader();
                while (reader.Read())
                {
                    cont = reader.GetValue(1).ToString();
                }
                reader.Close();
                Label lb = new Label
                {
                    Content = cont,
                    Background = new SolidColorBrush(Color.FromArgb(38, 141, 145, 255)),
                    BorderBrush = new SolidColorBrush(Color.FromArgb(64, 255, 171, 106)),
                    Padding = new Thickness(10, 0, 0, 0),
                    FontSize = 25,
                    Foreground = new SolidColorBrush(Color.FromArgb(100, 168, 186, 255)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 616,
                    Height = 34,
                    FontWeight = FontWeights.Thin,
                    FontFamily = new FontFamily("Century Gothic"),
                    Margin = new Thickness(0, j * 39, 0, 0)
                };
                lb.MouseLeftButtonDown += DelBookFromProfile;
                MyBooks.Children.Add(lb);
            }
            SurnameProfile_Label.Content = LogInUser.Surname;
            Hall_Label.Content = Hall;
            conn.Close();
            LoginPass.Text = LoginTicket.Text = "";
            ShowPage("Profile_About");
        }
        private void HallChange_Click(object sender, MouseButtonEventArgs e)
        {
            Label lb = sender as Label;
            string flag = lb.Content.ToString().Split('(')[0];

            conn.Open();

            SqlCommand CheckCapacity = new SqlCommand
            ($"SELECT * FROM Halls WHERE Hall_ID = {flag}", conn);
            SqlDataReader reader = CheckCapacity.ExecuteReader();
            reader.Read();
            int Capacity = Convert.ToInt32(reader.GetValue(2));
            reader.Close();

            if (Capacity == 0)
            {
                HallChange_Grid.Visibility = Visibility.Hidden;
                DelBookButton.Visibility = Visibility.Visible;
                conn.Close();
                return;
            }

            SqlCommand deleteHallUser = new SqlCommand
            ($"DELETE FROM User_Hall WHERE (Hall_ID = {Hall_Label.Content}) AND (Ticket_Num = {LogInUser.Ticket_Num})", conn);
            deleteHallUser.ExecuteNonQuery();

            SqlCommand ChangeCapacity = new SqlCommand
            ($"UPDATE Halls SET Capacity = Capacity + 1 WHERE Hall_ID = {Hall_Label.Content};" +
             $"UPDATE Halls SET Capacity = Capacity - 1 WHERE Hall_ID = {flag}", conn);
            ChangeCapacity.ExecuteNonQuery();

            Hall_Label.Content = flag;
            HallChange_Grid.Visibility = Visibility.Hidden;
            DelBookButton.Visibility = Visibility.Visible;
            SqlCommand AddHallUser = new SqlCommand
            ($"INSERT INTO User_Hall VALUES ({Hall_Label.Content},{LogInUser.Ticket_Num})", conn);
            AddHallUser.ExecuteNonQuery();

            List<Hall> tempHalls = new List<Hall>();
            string comm = $"SELECT * FROM Halls";
            SqlCommand UpdateHalls = new SqlCommand(comm, conn);
            reader = UpdateHalls.ExecuteReader();

            while (reader.Read())
            {
                Hall hall = new Hall
                {
                    Hall_ID = Convert.ToInt32(reader.GetValue(0)),
                    Title = reader.GetValue(1).ToString(),
                    Capacity = Convert.ToInt32(reader.GetValue(2))
                };
                tempHalls.Add(hall);
            }
            reader.Close();
            conn.Close();
            LabelHall_1.Content = $"1({tempHalls[0].Capacity} м.)";
            LabelHall_2.Content = $"2({tempHalls[1].Capacity} м.)";
            LabelHall_3.Content = $"3({tempHalls[2].Capacity} м.)";
            LabelHall_4.Content = $"4({tempHalls[3].Capacity} м.)";
        }
        private void ProfileScroll_Click(object sender, MouseButtonEventArgs e)
        {
            Rectangle rt = sender as Rectangle;
            bool flag = false;
            if (rt.Name == "ProfileScroll_Up")
            {
                foreach (Label lb in MyBooks.Children)
                {
                    if (lb.Margin.Top < 0 && lb.Visibility == Visibility.Hidden)
                    {
                        flag = true; break;
                    }
                }
                if (!flag) return;
                foreach (Label lb in MyBooks.Children)
                {
                    int marg = Convert.ToInt32(lb.Margin.Top);
                    lb.Margin = new Thickness(0, marg + 39, 0, 0);
                }
            }
            else
            {
                foreach (Label lb in MyBooks.Children)
                {
                    if (lb.Margin.Top > 39 * 3 && lb.Visibility == Visibility.Hidden)
                    {
                        flag = true; break;
                    }
                }
                if (!flag) return;
                foreach (Label lb in MyBooks.Children)
                {
                    int marg = Convert.ToInt32(lb.Margin.Top);
                    lb.Margin = new Thickness(0, marg - 39, 0, 0);
                }
            }
            foreach (Label lb in MyBooks.Children)
            {
                if (Convert.ToInt32(lb.Margin.Top) < 0 ||
                   Convert.ToInt32(lb.Margin.Top) > 39 * 3)
                    lb.Visibility = Visibility.Hidden;
                else lb.Visibility = Visibility.Visible;
            }
        }
        private void FoundUsers_Click(object sender, MouseButtonEventArgs e)
        {
            List<string> Surname_Ticket = new List<string>();
            Searched_Users.Children.Clear();
            conn.Open();
            string comm = "";
            if (AgeSearch.Visibility == Visibility.Visible)
            {
                int MaxD = Convert.ToInt32(AgeSearch_MaxAge.Text);
                int MinD = Convert.ToInt32(AgeSearch_MinAge.Text);
                comm = $"SELECT * FROM Users WHERE (Birthday > '{2022 - MaxD}') AND (Birthday < '{2022 - MinD}')";
            }
            else if (SurnameSearch.Visibility == Visibility.Visible)
                comm = $"SELECT * FROM Users WHERE Surname LIKE '%{SurnameSearch_TextBox.Text}%'";

            else if (TicketSearch.Visibility == Visibility.Visible)
            {
                int TNum = Convert.ToInt32(TicketSearch_TextBox.Text);
                comm = $"SELECT * FROM Users WHERE Ticket_Num = {TNum}";
            }

            SqlCommand command = new SqlCommand(comm, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Surname_Ticket.Add($"{reader.GetValue(1)}_{reader.GetValue(2)}");
            reader.Close();
            conn.Close();
            for (int i = 0; i < Surname_Ticket.Count; i++)
            {
                string Sname = Surname_Ticket[i].Split('_')[0];
                string Ticket = Surname_Ticket[i].Split('_')[1];
                Label Lb = new Label
                {
                    Content = $"Прізвище: '{Sname}' Квиток: '{Ticket}'",
                    Background = new SolidColorBrush(Color.FromArgb(38, 114, 145, 255)),
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 111, 125, 198)),
                    Padding = new Thickness(10, 0, 0, 0),
                    FontSize = 18,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    FontWeight = FontWeights.Thin,
                    Width = 440,
                    Height = 26,
                    Margin = new Thickness(0, 31 * i, 0, 0),
                };
                Searched_Users.Children.Add(Lb);
            }
            foreach (Label lb in Searched_Users.Children)
            {
                if (lb.Margin.Top > 93 || lb.Margin.Top < 0)
                    lb.Visibility = Visibility.Hidden;

                else lb.Visibility = Visibility.Visible;
            }
        }
        private void UserScroll_Click(object sender, MouseButtonEventArgs e)
        {
            Rectangle rt = sender as Rectangle;
            bool flag = false;
            if (rt.Name == "UserScroll_Up")
            {
                foreach (Label lb in Searched_Users.Children)
                {
                    if (lb.Margin.Top < 0 && lb.Visibility == Visibility.Hidden)
                    {
                        flag = true; break;
                    }
                }
                if (!flag) return;
                foreach (Label lb in Searched_Users.Children)
                {
                    int marg = Convert.ToInt32(lb.Margin.Top);
                    lb.Margin = new Thickness(0, marg + 31, 0, 0);
                }
            }
            else
            {
                foreach (Label lb in Searched_Users.Children)
                {
                    if (lb.Margin.Top > 31 * 3 && lb.Visibility == Visibility.Hidden)
                    {
                        flag = true; break;
                    }
                }
                if (!flag) return;
                foreach (Label lb in Searched_Users.Children)
                {
                    int marg = Convert.ToInt32(lb.Margin.Top);
                    lb.Margin = new Thickness(0, marg - 31, 0, 0);
                }
            }
            foreach (Label lb in Searched_Users.Children)
            {
                if (Convert.ToInt32(lb.Margin.Top) < 0 ||
                   Convert.ToInt32(lb.Margin.Top) > 31 * 3)
                    lb.Visibility = Visibility.Hidden;
                else lb.Visibility = Visibility.Visible;
            }
        }
        private void BookSearch_Click(object sender, MouseButtonEventArgs e)
        {
            SearchedBooks.Children.Clear();
            conn.Open();
            string comm = "";
            List<string> BookTitles = new List<string>();
            if (DataSearchTitle_TextBox.Text.Length > 0)
            {
                comm = $"SELECT * FROM Books WHERE Title LIKE '%{DataSearchTitle_TextBox.Text}%'";
            }
            else if (DataSearchSiphre_TextBox.Text.Length > 0)
            {
                comm = $"SELECT * FROM Books WHERE Cipher = {DataSearchSiphre_TextBox.Text}";
            }
            else { conn.Close(); return; }

            SqlCommand command = new SqlCommand(comm, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read() && BookTitles.Count < 4)
            {
                BookTitles.Add(reader.GetString(1));
            }
            reader.Close();
            for (int i = 0; i < BookTitles.Count; i++)
            {
                Border bd = new Border
                {
                    Name = $"SearchedBook_{i}",
                    Background = new SolidColorBrush(Color.FromArgb(38, 141, 145, 255)),
                    CornerRadius = new CornerRadius(4),
                    Margin = new Thickness(0, i * 32, 0, 96 - i * 32),
                };
                Label lb = new Label
                {
                    Content = BookTitles[i],
                    Padding = new Thickness(10, 0, 0, 0),
                    FontSize = 18,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 111, 125, 198)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 440,
                    Height = 26,
                    FontWeight = FontWeights.Thin

                };
                lb.MouseLeftButtonDown += SearchBookInfo_Click;
                bd.Child = lb;
                SearchedBooks.Children.Add(bd);
            }
            conn.Close();
            return;
        }
        private void SearchBookInfo_Click(object sender, MouseButtonEventArgs e)
        {
            HideAll();
            Label label = sender as Label;

            conn.Open();
            string cmm = $"SELECT * FROM Books WHERE Title = '{label.Content}'";
            SqlCommand command = new SqlCommand(cmm, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                TempBookInfo = new Book
                {
                    Cipher = Convert.ToInt32(reader.GetValue(0)),
                    Title = reader.GetValue(1).ToString(),
                    Autor = reader.GetValue(2).ToString(),
                    Year = Convert.ToInt32(reader.GetValue(3)),
                    Hall_ID = Convert.ToInt32(reader.GetValue(4)),
                    Book_Left = Convert.ToInt32(reader.GetValue(5)),
                    Book_Amount = Convert.ToInt32(reader.GetValue(6))
                };
            }
            reader.Close();
            conn.Close();
            (SBA_Cipher.Content, SBA_Title.Content, SBA_Autor.Content,
             SBA_Year.Content, SBA_Hall.Content, SBA_Left.Content,
             SBA_Amount.Content) = (TempBookInfo.Cipher, TempBookInfo.Title,
             TempBookInfo.Autor, TempBookInfo.Year, TempBookInfo.Hall_ID,
             TempBookInfo.Book_Left, TempBookInfo.Book_Amount);
            ShowPage("Book_About");
        }
        private void RegisterProfile_Click(object sender, MouseButtonEventArgs e)
        {
            int[] CoD = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            string Adress = "", Surname = "";
            int AD = 0;
            int Education = -1, Pasport = -1, BD = -1, BM = -1, BY = -1;
            long TeleNum = -1;
            try
            {
                Pasport = Convert.ToInt32(RegPassport.Text);
                if (RegEducation.Content.ToString() == "Початкова")
                    Education = 1;
                else if (RegEducation.Content.ToString() == "Середня")
                    Education = 2;
                else Education = 3;
                TeleNum = Convert.ToInt64(RegTeleNum.Text);
                BD = Convert.ToInt32(RegBD.Text);
                BM = Convert.ToInt32(RegBM.Text);
                BY = Convert.ToInt32(RegBY.Text);
                if (AcademDegree) AD = 1;
            }
            catch
            {
                return;
            }
            Adress += RegAddress.Text;
            Surname += RegSurname.Text;
            if (TeleNum.ToString().Length != 12 || BM > 12 || BM < 1 ||
                BD > CoD[BM - 1] || BD < 1 || Adress == "" ||
                Surname == "" || Education == -1 || Pasport == -1 ||
                BY < 1900 || BY > 2022)
                return;

            conn.Open();
            string comm = $"SELECT MAX(Ticket_Num) FROM Users";
            SqlCommand command = new SqlCommand(comm, conn);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int TicketNum = Convert.ToInt32(reader.GetValue(0));
            reader.Close();

            comm = $"INSERT INTO Users VALUES({Pasport}, '{Surname}', {TicketNum + 1}, {TeleNum}, '{BY}-{BM}-{BD}', {Education}, {AD})";
            command = new SqlCommand(comm, conn);
            command.ExecuteNonQuery();
            conn.Close();
            HideAll();
            ShowPage("RegMessageBox");
        }
        private void DelUser_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                conn.Open();
                User UserToDel = new User();
                int Passport = Convert.ToInt32(DelUserSearch.Text);
                string comm = $"DELETE FROM Users WHERE Passport = {Passport}";
                SqlCommand command = new SqlCommand(comm, conn);
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                return;
                conn.Close();
            }
        }
        private void DelBook_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                conn.Open();
                int Cipher = Convert.ToInt32(DelBookSearch.Text);
                string comm = $"DELETE FROM Books WHERE Ciphre = {Cipher}";
                SqlCommand command = new SqlCommand(comm, conn);
                command.ExecuteNonQuery();
                conn.Close();
                HideAll();
                ShowPage("Librarian_Access");
            }
            catch
            {
                conn.Close();
                return;
            }
        }
        private void AddBook_Click(object sender, MouseButtonEventArgs e)
        {
            string Title = "", Autor = "";
            int Year = -1, Amount = -1, Hall = -1;
            try
            {
                Year = Convert.ToInt32(AddBook_Year.Text);
                Amount = Convert.ToInt32(AddBook_Amount.Text);
                Hall = Convert.ToInt32(AddBook_Hall.Text);
            }
            catch
            {
                return;
            }
            Title += AddBook_Title.Text;
            Autor += AddBook_Surname.Text;
            if (Title == "" || Autor == "" || Year < 0 || Year > 2022 ||
                Amount < 0 || Hall > 4 || Hall < 1)
                return;

            conn.Open();
            string comm = $"SELECT MAX(Cipher) FROM Books";
            SqlCommand command = new SqlCommand(comm, conn);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int Cipher = Convert.ToInt32(reader.GetValue(0));
            reader.Close();

            Book book = new Book
            {
                Title = Title,
                Autor = Autor,
                Year = Year,
                Book_Amount = Amount,
                Book_Left = Amount,
                Hall_ID = Hall,
                Cipher = Cipher + 1,
            };
            comm = $"INSERT INTO Books VALUES({book.Cipher}, '{Title}', '{Autor}', {Year}, {Hall}, {Amount}, {Amount})";
            command = new SqlCommand(comm, conn);
            command.ExecuteNonQuery();


            conn.Close();
            HideAll();
            ShowPage("RegMessageBox");
        }
        private void Statistic_Click(object sender, MouseButtonEventArgs e)
        {
            HideAll();
            ShowPage("Statistic");
            UsersDidntReturn.Children.Clear();
            conn.Open();
            string comm = "";
            List<string> UsersTicketNums = new List<string>();
            DateTime tempDate = DateTime.Now.AddDays(-30);
            comm = $"SELECT * FROM Book_User WHERE GetData < '{tempDate.Year}-{tempDate.Month}-{tempDate.Day}'";
            SqlCommand command = new SqlCommand(comm, conn);
            SqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                UsersTicketNums.Add(reader.GetValue(0).ToString());
            }
            reader.Close();
            List<string> UserSur_TN = new List<string>();
            for (int i = 0; i < UsersTicketNums.Count; i++)
            {
                comm = $"SELECT * FROM Users WHERE Ticket_Num = {UsersTicketNums[i]}";
                command = new SqlCommand(comm, conn);
                reader = command.ExecuteReader();
                while (reader.Read())
                    UserSur_TN.Add($"{reader.GetValue(1)}_{reader.GetValue(0)}");

                reader.Close();
            }
            for (int i = 0; i < UserSur_TN.Count; i++)
            {
                Border bd = new Border
                {
                    Name = $"UserMonth_{i}",
                    Background = new SolidColorBrush(Color.FromArgb(38, 141, 145, 255)),
                    CornerRadius = new CornerRadius(4),
                    Margin = new Thickness(10, 10 + i * 36, 51, 136 - i * 36),
                };
                Label lb = new Label
                {
                    Content = $"{UserSur_TN[i].Split('_')[0]}(Pas: {UserSur_TN[i].Split('_')[1]})",
                    Padding = new Thickness(10, 0, 0, 0),
                    FontSize = 21,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 129, 207, 199)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 280,
                    Height = 31,
                    FontWeight = FontWeights.Normal
                };
                bd.Child = lb;
                UsersDidntReturn.Children.Add(bd);
            }

            foreach (Border bd in UsersDidntReturn.Children.OfType<Border>())
            {
                if (Convert.ToInt32(bd.Margin.Top) < 0 ||
                   Convert.ToInt32(bd.Margin.Top) > 118)
                    bd.Visibility = Visibility.Hidden;
                else bd.Visibility = Visibility.Visible;
            }
            double J = 0, M = 0, S = 0;
            command = new SqlCommand($"SELECT * FROM Users WHERE Education = 1", conn);
            reader = command.ExecuteReader();
            while (reader.Read())
                J++;
            reader.Close();

            command = new SqlCommand($"SELECT * FROM Users WHERE Education = 2", conn);
            reader = command.ExecuteReader();
            while (reader.Read())
                M++;
            reader.Close();

            command = new SqlCommand($"SELECT * FROM Users WHERE Education = 3", conn);
            reader = command.ExecuteReader();
            while (reader.Read())
                S++;
            PercentOfJunEdu.Content = Math.Round((J / (J + S + M)) * 100);
            PercentOfMidEdu.Content = Math.Round((M / (J + S + M)) * 100);
            PercentOfHightEdu.Content = Math.Round((S / (J + S + M)) * 100);

            reader.Close();
            conn.Close();
            return;
        }
        private void UserWRB_TextChanged(object sender, TextChangedEventArgs e)
        {
            int MaxAmount;
            try
            {
                MaxAmount = Convert.ToInt32(UserWRB_TB.Text);
            }
            catch { return; }
            conn.Open();
            List<string> RareBooks = new List<string>();
            SqlCommand command = new SqlCommand
            ($"SELECT * FROM Books WHERE Book_Amount < {MaxAmount}", conn);

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                RareBooks.Add($"{reader.GetValue(0)}");
            reader.Close();
            List<int> UsersWRB_TN = new List<int>();
            for (int i = 0; i < RareBooks.Count; i++)
            {
                command = new SqlCommand($"SELECT * FROM Book_User WHERE (Cipher = {RareBooks[i]})", conn);
                reader = command.ExecuteReader();
                while (reader.Read())
                    UsersWRB_TN.Add(Convert.ToInt32(reader.GetValue(0)));
                reader.Close();
            }
            UsersWRB_TN.Distinct();
            UsersWRB_TN.Sort();
            List<string> UsersWRB = new List<string>();
            for (int i = 0; i < UsersWRB_TN.Count; i++)
            {
                command = new SqlCommand($"SELECT * FROM Users WHERE (Ticket_Num = {UsersWRB_TN[i]})", conn);
                reader = command.ExecuteReader(); 
                reader.Read();
                UsersWRB.Add($"{reader.GetValue(1)}_{reader.GetValue(0)}");
                reader.Close();
            }
            if (LibrarianAccessFlag)
                UsersWRBGrid.Children.Clear();

            for (int i = 0; i < UsersWRB.Count; i++)
            {
                Border bd = new Border
                {
                    Name = $"UserMonth_{i}",
                    Background = new SolidColorBrush(Color.FromArgb(38, 141, 145, 255)),
                    CornerRadius = new CornerRadius(4),
                    Margin = new Thickness(10, 10 + i * 36, 51, 136 - i * 36),
                };
                Label lb = new Label
                {
                    Content = $"{UsersWRB[i].Split('_')[0]}(Pas: {UsersWRB[i].Split('_')[1]})",
                    Padding = new Thickness(10, 0, 0, 0),
                    FontSize = 21,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 129, 207, 199)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 280,
                    Height = 31,
                    FontWeight = FontWeights.Normal
                };
                bd.Child = lb;
                UsersWRBGrid.Children.Add(bd);
                if (Convert.ToInt32(bd.Margin.Top) < 0 ||
                   Convert.ToInt32(bd.Margin.Top) > 118)
                    bd.Visibility = Visibility.Hidden;
                else bd.Visibility = Visibility.Visible;
            }
            conn.Close();
            return;
        }
        private void UserWRBScroll_Click(object sender, MouseButtonEventArgs e)
        {
            Rectangle rt = sender as Rectangle;
            bool flag = false;
            if (rt.Name == "UserWRBScroll_UP")
            {
                foreach (Border bd in UsersWRBGrid.Children)
                {
                    if (bd.Margin.Top < 0 && bd.Visibility == Visibility.Hidden)
                    {
                        flag = true; break;
                    }
                }
                if (!flag) return;
                foreach (Border bd in UsersWRBGrid.Children)
                {
                    int marg = Convert.ToInt32(bd.Margin.Top);
                    bd.Margin = new Thickness(10, marg + 36, 51, 146 - (marg + 36));
                }
            }
            else
            {
                foreach (Border bd in UsersWRBGrid.Children)
                {
                    if (bd.Margin.Top > 118 && bd.Visibility == Visibility.Hidden)
                    {
                        flag = true; break;
                    }
                }
                if (!flag) return;
                foreach (Border bd in UsersWRBGrid.Children)
                {
                    int marg = Convert.ToInt32(bd.Margin.Top);
                    bd.Margin = new Thickness(10, marg - 36, 51, 146 - (marg - 36));
                }
            }
            foreach (Border bd in UsersWRBGrid.Children)
            {
                if (Convert.ToInt32(bd.Margin.Top) < 0 ||
                   Convert.ToInt32(bd.Margin.Top) > 118)
                    bd.Visibility = Visibility.Hidden;
                else bd.Visibility = Visibility.Visible;
            }
        }
        private void UserMonthScroll_Click(object sender, MouseButtonEventArgs e)
        {
            Rectangle rt = sender as Rectangle;
            bool flag = false;
            if (rt.Name == "UserMonthScroll_Up")
            {
                foreach (Border bd in UsersDidntReturn.Children)
                {
                    if (bd.Margin.Top < 0 && bd.Visibility == Visibility.Hidden)
                    {
                        flag = true; break;
                    }
                }
                if (!flag) return;
                foreach (Border bd in UsersDidntReturn.Children)
                {
                    int marg = Convert.ToInt32(bd.Margin.Top);
                    bd.Margin = new Thickness(10, marg + 36, 51, 146 - (marg + 36));
                }
            }
            else
            {
                foreach (Border bd in UsersDidntReturn.Children)
                {
                    if (bd.Margin.Top > 118 && bd.Visibility == Visibility.Hidden)
                    {
                        flag = true; break;
                    }
                }
                if (!flag) return;
                foreach (Border bd in UsersDidntReturn.Children)
                {
                    int marg = Convert.ToInt32(bd.Margin.Top);
                    bd.Margin = new Thickness(10, marg - 36, 51, 146 - (marg - 36));
                }
            }
            foreach (Border bd in UsersDidntReturn.Children)
            {
                if (Convert.ToInt32(bd.Margin.Top) < 0 ||
                   Convert.ToInt32(bd.Margin.Top) > 118)
                    bd.Visibility = Visibility.Hidden;
                else bd.Visibility = Visibility.Visible;
            }
        }
        private void HallChoose_Click(object sender, MouseButtonEventArgs e)
        {
            if (HallChange_Grid.Visibility == Visibility.Visible)
            {
                HallChange_Grid.Visibility = Visibility.Hidden;
                DelBookButton.Visibility = Visibility.Visible;
                return;
            }
            DelBookButton.Visibility = Visibility.Hidden;
            HallChange_Grid.Visibility = Visibility.Visible;
            conn.Open();
            List<Hall> tempHalls = new List<Hall>();

            string comm = $"SELECT * FROM Halls";
            SqlCommand command = new SqlCommand(comm, conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Hall hall = new Hall
                {
                    Hall_ID = Convert.ToInt32(reader.GetValue(0)),
                    Title = reader.GetValue(1).ToString(),
                    Capacity = Convert.ToInt32(reader.GetValue(2))
                };
                tempHalls.Add(hall);
            }
            reader.Close();
            conn.Close();
            LabelHall_1.Content = $"1({tempHalls[0].Capacity} м.)";
            LabelHall_2.Content = $"2({tempHalls[1].Capacity} м.)";
            LabelHall_3.Content = $"3({tempHalls[2].Capacity} м.)";
            LabelHall_4.Content = $"4({tempHalls[3].Capacity} м.)";
        }
    }
}
