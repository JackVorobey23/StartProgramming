using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Library
{
    public partial class MainWindow : Window
    {
        private void RadioButton_MouseEnter(object sender, MouseEventArgs e)
        {
            RadioButton rb = e.Source as RadioButton;
            int num = Convert.ToInt32(rb.Name.Split('_')[1]);
            switch (num)
            {
                case 1:
                    Rect_1.Visibility = Visibility.Visible;
                    break;
                case 2:
                    Rect_2.Visibility = Visibility.Visible;
                    break;
                case 3:
                    Rect_3.Visibility = Visibility.Visible;
                    break;
            }
        }
        private void RadioButton_MouseLeave(object sender, MouseEventArgs e)
        {
            RadioButton rb = e.Source as RadioButton;
            int num = Convert.ToInt32(rb.Name.Split('_')[1]);
            switch (num)
            {
                case 1:
                    Rect_1.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    Rect_2.Visibility = Visibility.Hidden;
                    break;
                case 3:
                    Rect_3.Visibility = Visibility.Hidden;
                    break;
            }
        }
        private void ProfileBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            Border bd = sender as Border;
            bd.Background = new SolidColorBrush(Color.FromArgb(255, 70, 80, 152));
        }
        private void ProfileBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            Border bd = sender as Border;
            bd.Background = new SolidColorBrush(Color.FromArgb(255, 53, 59, 112));
        }
        private void RegEducation_MouseEnter(object sender, MouseEventArgs e)
        {
            Border bd = sender as Border;
            if (bd.Name == "RegDegree_Enable" && AcademDegree) return;
            if (bd.Name == "RegDegree_Disable" && !AcademDegree) return;
            bd.Background = new SolidColorBrush(Color.FromArgb(76, 141, 145, 255));
        }
        private void RegEducation_MouseLeave(object sender, MouseEventArgs e)
        {
            Border bd = sender as Border;
            if (bd.Name == "RegDegree_Enable" && AcademDegree) return;
            if (bd.Name == "RegDegree_Disable" && !AcademDegree) return;
            bd.Background = new SolidColorBrush(Color.FromArgb(38, 141, 145, 255));
        }
        private void BackBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle rt = sender as Rectangle;
            rt.Fill = new SolidColorBrush(Color.FromArgb(64, 141, 243, 255));
        }
        private void BackBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle rt = sender as Rectangle;
            rt.Fill = new SolidColorBrush(Color.FromArgb(38, 141, 243, 255));
        }
        private void RegFinish_MouseEnter(object sender, MouseEventArgs e)
        {
            Border bd = sender as Border;
            bd.Background = new SolidColorBrush(Color.FromArgb(64, 106, 233, 255));
        }
        private void RegFinish_MouseLeave(object sender, MouseEventArgs e)
        {
            Border bd = sender as Border;
            bd.Background = new SolidColorBrush(Color.FromArgb(64, 106, 112, 255));
        }
        private void ScrollRectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle bd = sender as Rectangle;
            bd.Fill = new SolidColorBrush(Color.FromArgb(128, 255, 174, 141));
        }
        private void ScrollRectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle bd = sender as Rectangle;
            bd.Fill = new SolidColorBrush(Color.FromArgb(128, 141, 145, 255));
        }
        private void DeleteBook_MouseEnter(object sender, MouseEventArgs e)
        {
            Border bd = sender as Border;
            bd.Background = new SolidColorBrush(Color.FromArgb(64, 255, 106, 106));
        }
        private void DeleteBook_MouseLeave(object sender, MouseEventArgs e)
        {
            Border bd = sender as Border;
            bd.Background = new SolidColorBrush(Color.FromArgb(64, 255, 171, 106));
        }
        private void DeniedAccess_MouseEnter(object sender, MouseEventArgs e)
        {
            Border bd = sender as Border;
            bd.Background = new SolidColorBrush(Color.FromArgb(64, 255, 106, 149));
        }
        private void DeniedAccess_MouseLeave(object sender, MouseEventArgs e)
        {
            Border bd = sender as Border;
            bd.Background = new SolidColorBrush(Color.FromArgb(64, 242, 106, 255));
        }
        private void LoginTicket_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LoginTicket.Text.Length > 0)
            {
                LoginTicket_Label.Content = "";
            }
            else
            {
                LoginTicket_Label.Content = "Номер читацького квитка";
            }
        }
        private void LoginPass_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LoginPass.Text.Length > 0)
                LoginPass_Label.Content = "";
            else
                LoginPass_Label.Content = "Пароль";
        }
        private void RegTeleNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RegTeleNum.Text.Length > 0)
                NumExample_Label.Content = "";
            else NumExample_Label.Content = "+380 XX-XXX-XXX";
        }
        private void RegBD_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length > 0)
            {
                if (tb.Name == "RegBD") RegBD_Label.Content = "";
                if (tb.Name == "RegBM") RegBM_Label.Content = "";
                if (tb.Name == "RegBY") RegBY_Label.Content = "";
            }
            if (tb.Text.Length == 0)
            {
                if (tb.Name == "RegBD") RegBD_Label.Content = "00";
                if (tb.Name == "RegBM") RegBM_Label.Content = "00";
                if (tb.Name == "RegBY") RegBY_Label.Content = "0000";
            }
        }
        private void BookDataSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox Tb = sender as TextBox;
            if (Tb.Name.ToString().Split('_')[0] == "DataSearchTitle")
            {
                DataBookSearchT_Label.Content = "";
                DataSearchSiphre_TextBox.Text = "";
                DataBookSearchS_Label.Content = "Шукати за шифром...";
            }
            else
            {
                DataBookSearchS_Label.Content = "";
                DataSearchTitle_TextBox.Text = "";
                DataBookSearchT_Label.Content = "Шукати за назвою...";
            }//Візуальна частина

        }
        private void EduChange_Click(object sender, MouseButtonEventArgs e)
        {
            Border bd = sender as Border;
            string eduNow = RegEducation.Content.ToString();
            string eduAfter;
            if (bd.Margin.Bottom == 14)
            {
                if (eduNow == "Початкова") eduAfter = "Середня";
                else if (eduNow == "Середня") eduAfter = "Вища";
                else eduAfter = "Початкова";
            }
            else
            {
                if (eduNow == "Початкова") eduAfter = "Вища";
                else if (eduNow == "Середня") eduAfter = "Початкова";
                else eduAfter = "Середня";
            }
            RegEducation.Content = eduAfter;
        }
        private void RegDegree_Click(object sender, MouseButtonEventArgs e)
        {
            Border bd = sender as Border;
            RegDegree_Enable.BorderThickness = RegDegree_Disable.BorderThickness =
            new Thickness(0);
            RegDegree_Enable.Background = RegDegree_Disable.Background =
            new SolidColorBrush(Color.FromArgb(38, 141, 145, 255));

            if (bd.Name.ToString() == "RegDegree_Enable")
                AcademDegree = true;
            else AcademDegree = false;
            bd.BorderThickness = new Thickness(1, 1, 1, 1);
            bd.Background = new SolidColorBrush(Color.FromArgb(38, 255, 113, 221));
        }
        private void ChangeTypeOfSearch_Click(object sender, MouseButtonEventArgs e)
        {
            ChangeSearch_Age.BorderThickness =
            ChangeSearch_Surname.BorderThickness =
            ChangeSearch_Ticket.BorderThickness = new Thickness(0);

            ChangeSearch_Surname.Background = ChangeSearch_Age.Background =
            ChangeSearch_Ticket.Background = new SolidColorBrush(Color.FromArgb(38, 141, 145, 255));
            AgeSearch.Visibility = SurnameSearch.Visibility =
            TicketSearch.Visibility = Visibility.Hidden;
            Border bd = sender as Border;
            switch (bd.Name.ToString().Split('_')[1])
            {
                case "Age":
                    ChangeSearch_Age.BorderThickness = new Thickness(1);
                    ChangeSearch_Age.Background = new SolidColorBrush(Color.FromArgb(38, 255, 213, 221));
                    AgeSearch.Visibility = Visibility.Visible;
                    break;
                case "Surname":
                    ChangeSearch_Surname.BorderThickness = new Thickness(1);
                    ChangeSearch_Surname.Background = new SolidColorBrush(Color.FromArgb(38, 255, 213, 221));
                    SurnameSearch.Visibility = Visibility.Visible;
                    break;
                case "Ticket":
                    ChangeSearch_Ticket.BorderThickness = new Thickness(1);
                    ChangeSearch_Ticket.Background = new SolidColorBrush(Color.FromArgb(38, 255, 213, 221));
                    TicketSearch.Visibility = Visibility.Visible;
                    break;
            }
        }
        private void DelUserSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length > 0)
                UserToDelete.Content = "";
            else
                UserToDelete.Content = "За паспортом...";
        }
        private void DelBookSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length > 0)
                BookToDelete.Content = "";
            else
                BookToDelete.Content = "За шифром...";
        }
        private void ProfileBookDelete_Click(object sender, MouseButtonEventArgs e)
        {
            foreach (Label lb in MyBooks.Children)
            {
                lb.BorderThickness = new Thickness(1);
            }
            BookDelete = true;
        }
    }
}
