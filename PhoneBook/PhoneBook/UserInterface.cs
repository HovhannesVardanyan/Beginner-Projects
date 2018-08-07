using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PhoneBook
{
    /// <summary>
    /// This class provides interaction with user.
    /// </summary>
    class UserInterface
    {
        public UserInterface()
        {

        }
        public void Start(string message = "")
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            User myUser = null;
            try
            {
               myUser =  LogIn();
            }
            catch (FailedLogin ex)
            {
                
                this.Start(ex.Message);
            }
           
                this.Actions(myUser);
            
           
        }
        private void GenerateWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        private void AdminType(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        private void Actions(User myUser,string message = "")
        {
            Console.Clear();

            GenerateWarning(message);
            bool exit = false;
            Console.WriteLine();
            AdminType("1 | Display my contacts");
            AdminType("2 | Add a contact");
            AdminType("3 | Remove contact");
            AdminType("4 | Edit contact");
            AdminType("5 | Exit");
            string a = Console.ReadLine();
            try
            {
                switch (a)
                {
                    case "1":
                        PageContacts(myUser);
                        break;
                    case "2":
                        AddContact(myUser);
                        break;
                    case "3":
                        RemoveContact(myUser);
                        break;
                    case "4":
                        EditContact(myUser);
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        throw new InvalidInput("Your action is not valid");
                }
            }
            catch (InvalidInput e)
            {
                Actions(myUser,e.Message);
            }

            if (exit)
                return;
            
            Console.WriteLine();
            AdminType("Press any key to continue | H");
            Console.ReadKey();
            this.Actions(myUser);
        }
        private void PageContacts(User user)
        {
            int i = 1;
            bool cond;
            while (true)
            {
                Console.Clear();
                cond = false;
                Console.WriteLine(user.PhoneBook.GetPage(i));
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (i != 1)
                            i--;
                        break;
                    case ConsoleKey.RightArrow:
                        if ((i) * 5 < user.PhoneBook.Count)
                            i++;
                        break;
                    case ConsoleKey.Enter:
                        cond = true;
                        break;
                }
                if (cond)
                    break;

            }
        }
        private void DisplayContacts(User user)
        {
            int index = 1;
            Console.Clear();
            foreach (var item in user.PhoneBook)
            {
                AdminType(index + " | " + item);
                index++;
            }
        }
        private void AddContact(User user)
        {
            Console.Clear();
            Console.WriteLine();
            AdminType("Enter First Name");
            string first = Console.ReadLine();
            while (first == "")
            {
                Console.SetCursorPosition(0,Console.CursorTop-3);

                GenerateWarning("Your input is empty");
                AdminType("Enter First Name        ");
                first = Console.ReadLine();
            }
            Console.WriteLine();

            AdminType("Enter Last Name");
            string last = Console.ReadLine();
            while (last == "")
            {
                Console.SetCursorPosition(0, Console.CursorTop - 3);

                GenerateWarning("Your input is empty");
                AdminType("Enter First Name        ");
                last = Console.ReadLine();
            }
            Console.WriteLine();
            AdminType("Enter all phone numbers, for ending the list type ");
            List<string> phones = new List<string>();
                AskForSequentialInput(phones);
            Console.WriteLine();
            AdminType("Enter all emails, for ending the list type ");
            List<string> emails = new List<string>();
            AskForSequentialInput(emails);

            user.PhoneBook.AddContact(first,last,phones,emails);
           
        }
        private void AskForSequentialInput(List<string> phones)
        {
            string ans = "";
            Console.WriteLine("");
            while (true)
            {
                
                ans = Console.ReadLine();
                if (ans.Contains('#'))
                    break;
                if (ans == "")
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 2);
                    GenerateWarning("Your input was invalid");
                    continue;
                }
                
                phones.Add(ans);
                Console.WriteLine("");

            }
            
        }
        private void EditContact(User user)
        {
            PageContacts(user);
            int ind = BrowseItem();
            Console.Clear();

            var (first, last,  phone, emails) = user.PhoneBook.GetContact(ind);


            string f = TakeInputForUpdate("This is the First Name you might want to change",first);
            string l = TakeInputForUpdate("This is the Last Name you might want to change", last);

            List<string> p = ManageSequentialData(phone.ToList(), "This is a phone number, that you might want to remove or change");

            List<string> e = ManageSequentialData(emails.ToList(), "This is an email, that you might want to remove or change");
            
            user.PhoneBook.UpdateContact(ind,f, l, p, e);
        }
        private List<string> ManageSequentialData(List<string> col,string message)
        {
            List<string> p = new List<string>();

            string ph = "";
            foreach (string item in col)
            {
                ph = TakeInputForUpdate(message, item);
                if (item != "")
                    p.Add(ph);
            }
            Console.Clear();
            Console.WriteLine("Do you want to add more ? Yes : No");

            string ans = Console.ReadLine();
            while (!ans.ToUpper().Contains('N') && !ans.ToUpper().Contains('Y'))
            {
                Console.Clear();
                Console.WriteLine("Do you want to add more ? Yes : No");
                ans = Console.ReadLine();
            }
            if (ans.ToUpper().Contains('Y'))
            {
                AskForSequentialInput(p);
            }

            return p;
        }
        private string TakeInputForUpdate(string text,string info)
        {
            AdminType(text);
            AdminType(info);
            Console.WriteLine();
            string ans = Console.ReadLine();

            if (ans == "R")
                return "";
            if (ans == "")
                return info;

            while (ans == "")
            {
                Console.Clear();
                AdminType(text);
                AdminType(info);
                ans = Console.ReadLine();
            }
            Console.WriteLine();
            return ans;

        }
        private void RemoveContact(User user)
        {
            PageContacts(user);
            AdminType("Enter the index of the contact you want to remove");
            int a = BrowseItem();
            if (a == -1)
                return;
            try
            {
                user.PhoneBook.RemoveContact(a);
                Console.WriteLine("Item successfully removed!");
            }
            catch (ItemNotFound e)
            {
                GenerateWarning(e.Message);
                Console.ReadKey();
                RemoveContact(user);
            }
            
        }
        private int BrowseItem()
        {
            int a = 0;
            string ans = "";
            Console.WriteLine("");
            while (true)
            {
                Console.WriteLine("");
                ans = Console.ReadLine();

                if (ans.ToLower() == "exit")
                    return -1;

                if (ans == "" || !int.TryParse(ans, out a) || a <= 0)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 3);
                    GenerateWarning("Your input was invalid");
                    continue;
                }
                else
                    break;
            }

            return a;
        }
        private User LogIn()
        {
            AdminType("Login");
            string login = Console.ReadLine();
            Console.WriteLine();
            AdminType("Password");
            string password = Console.ReadLine();

            if (login == "" || password == "")
                throw new FailedLogin("Either Login or Passowrd Fields are empty!!");

            using (StreamReader reader = new StreamReader("users.txt"))
            {
                string line;
                string[] arr;
                while ((line = reader.ReadLine()) != null)
                {
                    arr = line.Split('#');
                    if (arr[0] == login && arr[1] == password)
                        return new User(login, password);
                }
            }
            
            throw new FailedLogin("Username Or/And Password is Incorrect");
        }
    }
}
