using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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

namespace Prac_03
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string connection = "Data Source=EYGEH;Initial Catalog=Prac_03_DB;Integrated Security=True";
        static SqlConnection conn = new SqlConnection(connection);
        static List<User> Users = new User().GetUsers(conn);
        User CurrentUser = null; static string AdminPass = "superSafelyPass1";
        int passChangeCounter = 0;
        public MainWindow()
        {
            InitializeComponent();
            HideAll();
            ShowPage("MainWin");

        }
        string BackToTarget = "MainWin";
        private void HideAll()
        {
            foreach (Grid grid in Window.Children)
            {
                grid.Margin = new Thickness(-371, 0, 371, 0);
            }
        }
        private void ShowPage(string page)
        {
            foreach (Grid grid in Window.Children)
            {
                if(grid.Name == page)
                    grid.Margin = new Thickness(0);
            }
        }
        private void CloseBtn_Click(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        /*--------------------------ANIMATIONS---------------------------*/
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Border btn = (Border)sender;
            btn.BorderThickness = new Thickness(3);
            btn.Background = new SolidColorBrush(Colors.White);
            Label lb = btn.Child as Label;
            lb.Foreground = new SolidColorBrush(Color.FromRgb(255, 139, 55));
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Border btn = (Border)sender;
            btn.BorderThickness = new Thickness(0);
            btn.Background = new SolidColorBrush(Color.FromRgb(255,139,55));
            Label lb = btn.Child as Label;
            lb.Foreground = new SolidColorBrush(Colors.White);
        }
        private void CloseBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            Border btn = (Border)sender;
            btn.BorderThickness = new Thickness(3);
            btn.Background = new SolidColorBrush(Colors.White);
            Label lb = btn.Child as Label;
            lb.Foreground = new SolidColorBrush(Color.FromRgb(255, 74, 74));
        }
        private void CloseBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            Border btn = (Border)sender;
            btn.BorderThickness = new Thickness(0);
            btn.Background = new SolidColorBrush(Color.FromRgb(255, 74, 74));
            Label lb = btn.Child as Label;
            lb.Foreground = new SolidColorBrush(Colors.White);
        }
        private void BackToBtn_Click(object sender, MouseButtonEventArgs e)
        {
            passChangeCounter = 0;
            HideAll();
            ShowPage(BackToTarget);
            if (BackToTarget == "UserLog" || BackToTarget == "Admin_Page")
                BackToTarget = "MainWin";
        }

        private void MainUserBtn_Click(object sender, MouseButtonEventArgs e)
        {
            HideAll();
            if (CurrentUser == null)
                ShowPage("UserLog");
            else
                ShowPage("Profile_About");
        }

        private void CreateAcc_Click(object sender, MouseButtonEventArgs e)
        {
            HideAll();
            ShowPage("UserReg");
            BackToTarget = "UserLog";
        }

        private void ProfileEnter_Click(object sender, MouseEventArgs e)
        {
            string login = UserLog_Login.Text;
            string pass = UserLog_Pass.Text;
            foreach(User user in Users)
            {
                if(user.Login == login)
                {
                    if(user.Pass == pass)
                    {
                        if (!user.Disabled)
                        {
                            HideAll();
                            ShowPage("Profile_About");
                            CurrentUser = user;
                            (Prof_Name.Text, Prof_Surname.Text, Prof_Login.Text) =
                            (CurrentUser.Name, CurrentUser.Surname, CurrentUser.Login);
                            UserLog_Login.Text = UserLog_Pass.Text = "";
                            UserLog_Exception.Content = "";
                            return;
                        }
                        UserLog_Exception.Content = "*Profile is inactive";
                        return;
                    }
                    UserLog_Exception.Content = "*Password entered incorrectly";
                    return;
                }
            }
            UserLog_Exception.Content = "*Login entered incorrectly";
        }

        private void ChangeProfile_Click(object sender, MouseButtonEventArgs e)
        {
            UserReg_Exception.Content = "";
            string login = Prof_Login.Text;
            string name = Prof_Name.Text;
            string surname = Prof_Surname.Text;
            string error = "";
            if (login != CurrentUser.Login)
            {
                foreach (User user in Users)
                {
                    if (user.Login == login)
                    {
                        error += "\n*This login already exists";
                        break;
                    }
                }
            }

            if (name == "") error += "\n*Name field cannot be empty";
            if (surname == "") error += "\n*Surname field cannot be empty";
            if (name.Length > 20) error += "\n*Name cannot be longer than 20 characters";
            if (surname.Length > 20) error += "\n*Surname cannot be longer than 20 characters";

            int NameCounter = 0;
            foreach (char c in name)
                for (int i = 0; i < 10; i++)
                    if (Convert.ToChar(i) == c)
                        NameCounter++;

            if (NameCounter != 0) error += "\n*Name cannot contain a number";

            int SurnameCounter = 0;
            foreach (char c in surname)
                for (int i = 0; i < 10; i++)
                    if (Convert.ToChar(i) == c)
                        SurnameCounter++;

            if (SurnameCounter != 0) error += "\n*Surname cannot contain a number";

            if (error != "")
            {
                UserReg_Exception.Content = error;
                return;
            }
            conn.Open();
            string comm = $"UPDATE MainTable SET Login = '{login}', Surname = '{surname}', Name = '{name}' WHERE Login = '{CurrentUser.Login}'";
            SqlCommand command = new SqlCommand(comm, conn);
            command.ExecuteNonQuery();
            conn.Close();
            for (int i = 0; i < Users.Count; i++)
            {
                if(Users[i].Login == CurrentUser.Login)
                {
                    (Users[i].Login, Users[i].Name, Users[i].Surname) =
                        (login, name, surname);
                    break;
                }
            }
            (CurrentUser.Login, CurrentUser.Name, CurrentUser.Surname) =
                (login, name, surname);
            UserReg_Exception.Content = "Success!";
        }
        private void RegEnter_Click(object sender, MouseEventArgs e)
        {
            UserReg_Exception.Content = "";
            string login = UserReg_Login.Text;
            string pass = UserReg_Pass.Text;
            string name = UserReg_Name.Text;
            string surname = UserReg_Surname.Text;
            string error = "";
            foreach (User user in Users)
            {
                if(user.Login == login)
                {
                    error += "\n*This login already exists";
                    break;
                }
            }
            error += CheckPass(pass);
            if (name == "") error += "\n*Name field cannot be empty";
            if (surname == "") error += "\n*Surname field cannot be empty";
            if (name.Length > 20) error += "\n*Name cannot be longer than 20 characters";
            if (surname.Length > 20) error += "\n*Surname cannot be longer than 20 characters";

            int NameCounter = 0;
            foreach (char c in name)
                for (int i = 0; i < 10; i++)
                    if (Convert.ToChar(i) == c)
                        NameCounter++;

            if (NameCounter != 0) error += "\n*Name cannot contain a number";

            int SurnameCounter = 0;
            foreach (char c in surname)
                for (int i = 0; i < 10; i++)
                    if (Convert.ToChar(i) == c)
                        SurnameCounter++;

            if (SurnameCounter != 0) error += "\n*Surname cannot contain a number";

            if (error != "")
            {
                UserReg_Exception.Content = error;
                return;
            }
            UserReg_Exception.Content = "";

            conn.Open();
            string comm = $"INSERT INTO MainTable VALUES('{login}','{pass}','{name}','{surname}', 0)";
            SqlCommand command = new SqlCommand(comm, conn);
            command.ExecuteNonQuery();
            conn.Close();
            CurrentUser = new User
            {
                Name = name,
                Surname = surname,
                Login = login,
                Pass = pass,
                Disabled = false
            };
            Users.Add(CurrentUser);
            HideAll();
            Prof_Name.Text = name;
            Prof_Surname.Text = surname;
            Prof_Login.Text = login;
            ShowPage("Profile_About");
        }

        private void AdminBtn_Click(object sender, MouseButtonEventArgs e)
        {
            AdminUsers.Children.Clear();
            conn.Open();
            string comm = "SELECT * FROM MainTable";
            SqlCommand command = new SqlCommand(comm, conn);
            SqlDataReader reader = command.ExecuteReader();
            int height = 0;
            while (reader.Read())
            {
                Grid tempGrid = new Grid 
                { 
                    Height = 30,
                };
                Label lb = new Label
                {
                    Content = reader.GetValue(0), Margin = new Thickness(0,0,30,0)
                };
                CheckBox cb = new CheckBox
                {
                    Name = lb.Content + "_CB", Margin = new Thickness(260, 0, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center, Height = 15,
                    IsChecked = false,
                };
                if (reader.GetBoolean(4)) cb.IsChecked = true;
                cb.Checked += CB_IsCheckedChanged;
                cb.Unchecked += CB_IsCheckedChanged;
                tempGrid.Children.Add(lb); 
                tempGrid.Children.Add(cb);
                AdminUsers.Children.Add(tempGrid);
                height += 30;
            }
            AdminUsers.Height = height;
            reader.Close();
            conn.Close();
            HideAll();
            ShowPage("Admin_Page");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text != "") tb.Background = new SolidColorBrush(Colors.White);
            else tb.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void LogOut_Click(object sender, MouseButtonEventArgs e)
        {
            HideAll();
            ShowPage("MainWin");
            CurrentUser = null;
        }

        private void Prof_ChangePass_Click(object sender, MouseButtonEventArgs e)
        {
            passChangeCounter++;
            string error = "";
            string OldPass = Prof_OldPass.Text;
            string newPass = Prof_NewPass.Text;
            if (OldPass != CurrentUser.Pass)
                error += "\n*Incorrect password!";
            if (Prof_NewPass.Text != Prof_NewPassR.Text)
                error += "\n*Confirm the password by entering the same";
            if (OldPass == newPass)
                error += "\n*New password is the same as the old one";
            error += CheckPass(newPass);

            if (error != "")
            {
                if (passChangeCounter == 3)
                {
                    CurrentUser = null;
                    ShowPage("MainWin");
                }
                Prof_Exception.Content = error;
                return;
            }
            passChangeCounter = 0;
            conn.Open();
            string comm = $"UPDATE MainTable SET Pass = '{newPass}' WHERE Login = '{CurrentUser.Login}'";
            SqlCommand command = new SqlCommand(comm, conn);
            command.ExecuteNonQuery();
            conn.Close();
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Login == CurrentUser.Login)
                {
                    Users[i].Pass = newPass;
                    break;
                }
            }
            CurrentUser.Pass = newPass;
            Prof_Exception.Content = "Success!";
        }
        private void Admin_ChangePass_Click(object sender, MouseButtonEventArgs e)
        {
            passChangeCounter++;
            string error = "";
            string OldPass = Admin_OldPass.Text;
            string newPass = Admin_NewPass.Text;
            if (OldPass != CurrentUser.Pass)
                error += "\n*Incorrect password!";
            if (Admin_NewPass.Text != Admin_NewPassR.Text)
                error += "\n*Confirm the password by entering the same";
            if (OldPass == newPass)
                error += "\n*New password is the same as the old one";
            error += CheckPass(newPass);

            if (error != "")
            {
                Admin_Exception.Content = error;
                return;
            }
            passChangeCounter = 0;
            AdminPass = newPass;
            Admin_Exception.Content = "Success!";
        }
        public string CheckPass(string pass)
        {
            string error = "";
            if (pass.Length < 8 || pass.Length > 20)
                error += "\n*Password length must be between 8 and 20";
            if (pass.ToLowerInvariant() == pass)
                error += "\n*Use uppercase letters in your password";
            if (pass.ToUpperInvariant() == pass)
                error += "\n*Use lower case letters in your password";

            int PassCounter = 0;
            foreach (char c in pass)
                for (int i = 0; i < 10; i++)
                    if (Convert.ToString(i) == Convert.ToString(c))
                        PassCounter++;

            if (PassCounter == 0) error += "\n*Use numbers in your password";

            return error;
        }
        private void CB_IsCheckedChanged(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            string login = checkBox.Name.Split('_')[0];
            for(int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Login == login)
                {
                    Users[i].Disabled = Convert.ToBoolean(checkBox.IsChecked);
                    int disable = 0;
                    if (Users[i].Disabled) disable = 1;
                    conn.Open();
                    string comm = $"UPDATE MainTable SET Disable = {disable} WHERE Login = '{login}'";
                    SqlCommand command = new SqlCommand(comm, conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                    break;
                }
            }
        }

        private void AddUser_Click(object sender, MouseButtonEventArgs e)
        {
            string login = AddUser_Login.Text;
            string pass = AddUser_Pass.Text;
            string error = "";
            foreach (User tempUser in Users)
            {
                if (tempUser.Login == login)
                {
                    error += "\n*This login already exists";
                    break;
                }
            }
            error += CheckPass(pass);
            if (login == "") error += "\n*Login field cannot be empty";
            if (error != "")
            {
                AddUser_Exception.Content = error;
                return;
            }
            User user = new User
            {
                Login = login,
                Pass = pass,
                Disabled = false
            };
            Users.Add(user);
            conn.Open();
            string comm = $"INSERT INTO MainTable VALUES('{login}','{pass}', null, null, 0)";
            SqlCommand command = new SqlCommand(comm, conn);
            command.ExecuteNonQuery();
            conn.Close();
            AddUser_Exception.Content = "";
            AdminBtn_Click(sender, e);
        }
        private void Admin_AddUser_Click(object sender, MouseButtonEventArgs e)
        {
            HideAll();
            ShowPage("AddUserPage");
        }
        private void Label_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void About_Click(object sender, MouseButtonEventArgs e)
        {
            HideAll();
            ShowPage("About");
        }

        private void GH_MouseEnter(object sender, MouseEventArgs e)
        {
            Border btn = (Border)sender;
            btn.BorderThickness = new Thickness(3);
            btn.Background = new SolidColorBrush(Colors.White);
            GH_Link.Foreground = new SolidColorBrush(Color.FromRgb(255, 139, 55));
        }
        private void GH_MouseLeave(object sender, MouseEventArgs e)
        {
            Border btn = (Border)sender;
            btn.BorderThickness = new Thickness(0);
            btn.Background = new SolidColorBrush(Color.FromRgb(255, 139, 55));
            GH_Link.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }
}
