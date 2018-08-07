using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace PhoneBook
{
    class PhoneBookFiles : IEnumerable<string>, IDisposable // Phonebook class that uses mere .txt files for storing information
    {
        private StreamReader reader;
        private StreamWriter writer;
        private FileStream file;
        public long Count { get; private set; }
        private string path;
        private bool disposed = false;
        
        public PhoneBookFiles(string filePath) 
        {
            file = new FileStream(filePath + ".txt",FileMode.OpenOrCreate,FileAccess.ReadWrite, FileShare.None);
            reader = new StreamReader(file);
            writer = new StreamWriter(file);
            this.path = filePath;
            GetCount();
        } 

        private void GetCount()
        {
            Reset();
            string line;
            int i = 0;
            while ((line = reader.ReadLine()) != null)
            {
                i++;
            }
            Count = i;
            Reset();
        } 

        private void Reset()
        {
            file.Position = 0;
            reader.DiscardBufferedData();
            writer.Flush();
        } 

        public void AddContact(string first, string last, IList<string> PhoneNumbers, IList<string> Emails)
        {
            Validate(first, last, PhoneNumbers, Emails);

            AddLine((new Contact(first, last, PhoneNumbers, Emails)).Encocde());
            Count++;
        } 

        private void AddLine(string contact) 
        {
            Reset();
            string line;
            while ((line = reader.ReadLine()) != null)
            {

            }
            writer.WriteLine(contact);
            writer.Flush();
        } 

        public void RemoveContact(int index)
        {
            ValidateIndex(index);

            EditLine(index,"");
            
            Count--;
        } 

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<string>).GetEnumerator();
        } 

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            Reset();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                //if (line[0] == '@')
                 //   continue;
                yield return new Contact(line).ToString();
            }
        } 

        public (string first, string last, IList<string> phone, IList<string> email) GetContact(int index) 
        {
            ValidateIndex(index);

            Reset();
            
            Contact myContact = null;

            myContact = new Contact(GetLine(index));

            return (myContact.FirstName, myContact.LastName, myContact.PhoneNumbers, myContact.Emails);
        } 

        public string GetPage(int index, int pageSize)
        {
            StringBuilder sb = new StringBuilder();
            Reset();
            long start = (index - 1) * pageSize;

            

            long ind = 1;

            long end = index * pageSize;


            if (start > Count || end < 1)
                return null;

            string line;
            int i = 0;
            while ((line = reader.ReadLine()) != null)
            {
                i++;
                if (i > start && i <= end)
                    sb.AppendLine(ind + " | " + (new Contact(line)).ToString() );
                ind++;
            }
            return sb.ToString();
        }

        private string GetLine(int index) 
        {
            string l;
            int i = 0;
            while ((l = reader.ReadLine()) != null)
            {
                i++;
                if (i == index)
                    return l;
            }
            throw new InvalidInput("Could not found an element with such index!");
        } 

        public void UpdateContact(int index, string first, string last, IList<string> phone, IList<string> email) 
        {
            Validate(first, last, phone, email);

            Contact c = new Contact(first, last, phone, email);
            
            EditLine(index, c.Encocde());

        } 

        private void EditLine(int index,string info) 
        {
            Reset();
            StreamWriter sw = new StreamWriter($"{path}(1).txt");
            string line;
            int i = 0;
            while ((line = reader.ReadLine()) != null)
            {
                i++;
                if (index == i)
                {
                    if(info == "")
                        continue;
                    line = info;
                }
                sw.WriteLine(line);
            }
            sw.Flush();
            sw.Close();
            file.Close();
            File.Delete($"{path}.txt");
            File.Move($"{path}(1).txt", $"{path}.txt");
            file = new FileStream(path + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            reader = new StreamReader(file);
            writer = new StreamWriter(file);
        } 
        private void ValidateIndex(int index)
        {
            if (index <= 0)
                throw new InvalidInput("Not valid index given");
            if (index > Count)
                throw new ItemNotFound("Item with such index does not exist in this phonebook!");
        }

        private void Validate(string first, string last)
        {
            if (first == "" || last == "")
                throw new InvalidInput("First Name and/or Last Name is/are empty!");
        }

        private void Validate(IList<string> PhoneNumbers, IList<string> Emails)
        {
            if (PhoneNumbers == null || Emails == null)
                throw new InvalidInput("Phone Numbers or Emails collections are empty!");
        }

        private void Validate(string first, string last, IList<string> PhoneNumbers, IList<string> Emails)
        {
            Validate(first, last);
            Validate(PhoneNumbers, Emails);
        }

        private void Validate(string number)
        {
            foreach (char item in number)
            {
                if (!Char.IsDigit(item) && item != ' ')
                    throw new InvalidInput("The phone number cannot consist of anything other than numbers, spaces");
            }
        }
        
        public void Dispose()
        {
            if (!disposed)
            {
                writer.Dispose();
                reader.Dispose();
                file.Dispose();
                disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        private class Contact 
        {
            private IList<string> phones;
            private IList<string> emails;
            public IList<string> PhoneNumbers { get { return Clone(phones); } set { phones = Clone(value); } }
            public IList<string> Emails { get { return Clone(emails); } set { emails = Clone(value); } }
            public string FirstName { get; private set; }
            public string LastName { get; private set; }
            public Contact(string first,string last,IList<string> phones,IList<string> emails)
            {
                this.FirstName = first;
                this.LastName = last;
                this.Emails = emails;
                this.PhoneNumbers = phones;
            }
            public Contact(string line)
            {
                string[] args = line.Split('#');

                this.FirstName = args[0].Trim();
                this.LastName = args[1].Trim();

                if (2 < args.Length)
                    phones = args[2].Split('*');
                else
                    phones = new List<string>();

                if(3 < args.Length)
                    emails = args[3].Split('*');
                else
                    emails = new List<string>();
            }
            private IList<string> Clone(IList<string> t)
            {
                List<string> k = new List<string>();

                foreach (string i in t)
                    k.Add(i);
                return k;
            }

            public override string ToString()
            {
                StringBuilder ans = new StringBuilder();
                ans.Append(FirstName + "  " + LastName + "  ");

                foreach (string item in phones)
                    ans.Append(item + "  ");
                
                foreach (string item in emails)
                    ans.Append(item + "  ");
                
                return ans.ToString();
            }
            public string Encocde()
            {
                StringBuilder ans = new StringBuilder();
                ans.Append(FirstName + "#" + LastName + "#");

                foreach (string item in phones)
                    ans.Append(item + "*");

                ans[ans.Length - 1] = '#';
                foreach (string item in emails)
                    ans.Append(item + "*");

                ans[ans.Length - 1] = ' ';

                return ans.ToString();
            }
        }
    }
}
