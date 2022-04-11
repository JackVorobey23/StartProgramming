
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal class User
    {
        public int Passport;
        public string Surname;
        public int Ticket_Num;
        public string TeleNum;
        public DateTime BirthDay;
        public int Education;
        public bool AcademDegree;
    }
    internal class Book
    {
        public int Cipher;
        public string Title;
        public string Autor;
        public int Year;
        public int Hall_ID;
        public int Book_Left;
        public int Book_Amount;
        public void RegBook()
        {

        }
        public void DeleteBook()
        {

        }
    }
    internal class Hall
    {
        public int Hall_ID;
        public string Title;
        public int Capacity;
    }
    internal class User_Hall
    {
        public int Ticket_Num;
        public int Hall_ID;
    }
    internal class Book_User
    {
        public int Ticket_Num;
        public int Cipher;
    }
}
