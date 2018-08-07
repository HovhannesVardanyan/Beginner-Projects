using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook
{
    class User 
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public PhoneBook PhoneBook { get; }

        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
            this.PhoneBook = new PhoneBook(username);
        }
    }
    
}
