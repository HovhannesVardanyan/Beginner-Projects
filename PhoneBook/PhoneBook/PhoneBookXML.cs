using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
namespace PhoneBook
{
    public class PhoneBook : IEnumerable<string> // PhoneBook class that uses XML files for storing information
    {
        public long Count { get; private set; }

        const int PageSize = 6;

        private string path;

        public PhoneBook(string filePath)
        {
            this.path = filePath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

                using (FileStream fs = new FileStream(this.path + "\\1.xml", FileMode.Create, FileAccess.Write))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(List<Contact>));
                    xml.Serialize(fs, new List<Contact>() { });
                }

                using (FileStream fs = new FileStream(this.path + $"\\{this.path}.txt", FileMode.Create, FileAccess.Write))
                using (BinaryWriter br = new BinaryWriter(fs))
                {
                    br.Write((long)0);
                }
            }
            else
                GetCount();
        } // done

        private void GetCount()
        {
            int i = 1;
            while (File.Exists(Path(i)))
            {
                i++;
            }

            i--;

            Count = (i-1) * PageSize + GetList(i).Count;
        }  // done
            
        public void AddContact(string first, string last, List<string> PhoneNumbers, List<string> Emails)
        {
            Validate(first, last, PhoneNumbers, Emails);

            XmlSerializer xml = new XmlSerializer(typeof(List<Contact>));
            for (int i = 1; ; i++)
            {
                string p = path + $"\\{i}" + ".xml";
                
                using (FileStream fs = new FileStream(p, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    List<Contact> t;
                    if (fs.Length == 0)
                    {
                        t = new List<Contact>() { new Contact(first, last, PhoneNumbers, Emails) };
                        xml.Serialize(fs, t);
                        break;
                    }
                    else
                    {
                        t = (List<Contact>)xml.Deserialize(fs);
                        if (t.Count < PageSize)
                        {
                            t.Add(new Contact(first, last, PhoneNumbers, Emails));
                            fs.SetLength(0);
                            xml.Serialize(fs, t);
                            break;
                        }
                        
                    }

                }
            }

            Count++;
        }  // done

        public void RemoveContact(int index)
        {
            ValidateIndex(index);

            Count--;

            int c = 0;
            XmlSerializer xml = new XmlSerializer(typeof(List<Contact>));
            int i = 1;
            for (; ; i++)
            {
                string p = path + $"\\{i}" + ".xml";

                using (FileStream fs = new FileStream(p, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    if (PageSize > (index - c - 1))
                    {
                        List<Contact> t = (List<Contact>)xml.Deserialize(fs);
                        t.RemoveAt(index - c - 1);
                        fs.SetLength(0);
                        xml.Serialize(fs, t);
                        break;
                    }
                    c += PageSize;
                }


            }

            int k = i;
            while (File.Exists(Path(++i)))
            {

            }

            i--;

            if (k == i)
                return;

            Contact cr;

            using (FileStream fs = new FileStream(Path(i), FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                List<Contact> t = (List<Contact>)xml.Deserialize(fs);
                cr = t[t.Count - 1];

                t.RemoveAt(t.Count - 1);
                fs.SetLength(0);
                xml.Serialize(fs, t);
            }

            this.AddContact(cr.FirstName, cr.LastName, cr.PhoneNumbers, cr.Emails);

            
        } // done

        public string GetPage(int index)
        {
            StringBuilder sb = new StringBuilder();

            //if (Count == 0)
              //  return "";

            string p = Path(index);

            if (!File.Exists(p))
                return null;
            
            XmlSerializer xml = new XmlSerializer(typeof(List<Contact>));
            
            List<Contact> t = GetList(index);
            
            int s = (index - 1) * PageSize  +  1;

            foreach (Contact item in t)
            {
                sb.AppendLine($"{s} | {item}");
                s++;
            }
            
            return sb.ToString();
        } // done

        private string Path(int index)
        {
            return path + $"\\{index}" + ".xml";
        } // done

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<string>).GetEnumerator();
        }  // done

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            for (int i = 1; File.Exists(Path(i)); i++)
            {
                yield return GetPage(i);
            }
        }  // done
        
        private List<Contact> GetList(int index = 1)
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Contact>));
            List<Contact> t;
            using (FileStream fs = new FileStream(Path(index), FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                 t = (List<Contact>)xml.Deserialize(fs);
            }
            return t;
        } // Done
        
        public (string first, string last, List<string> phone, List<string> email) GetContact(int index)
        {
            ValidateIndex(index);

            var (a, b) = GetCords(index);

            Contact myContact = GetList( a )[b];

            return (myContact.FirstName, myContact.LastName, myContact.PhoneNumbers, myContact.Emails);
        } // done

        private (int list, int row) GetCords(int index)
            => ((index / PageSize) + 1, index % PageSize - 1); // done
        
        public void UpdateContact(int index, string first, string last, List<string> phone, List<string> email)
        {
            ValidateIndex(index);
            var (list, row) = GetCords(index);

            List<Contact> c = GetList(list);
            c[row] = new Contact(first, last, phone, email);

            Replace(c, list);
        } // done

        private void Replace(List<Contact> t, int index)
        {
            string p = Path(index);

            using (FileStream fs = new FileStream(p,FileMode.Open,FileAccess.ReadWrite,FileShare.None))
            {
                fs.SetLength(0);

                XmlSerializer xml = new XmlSerializer(typeof(List<Contact>));

                xml.Serialize(fs, t);
            }
        } // done
        
        private void ClearFile(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                fs.SetLength(0);
            }
        } // done

        private void ValidateIndex(int index)
        {
            if (index <= 0)
                throw new InvalidInput("Not valid index given");
            if (index > Count)
                throw new ItemNotFound("Item with such index does not exist in this phonebook!");
        } // done

        private void Validate(string first, string last)
        {
            if (first == "" || last == "")
                throw new InvalidInput("First Name and/or Last Name is/are empty!");
        } // done

        private void Validate(IList<string> PhoneNumbers, IList<string> Emails)
        {
            if (PhoneNumbers == null || Emails == null)
                throw new InvalidInput("Phone Numbers or Emails collections are empty!");
        } // done

        private void Validate(string first, string last, IList<string> PhoneNumbers, IList<string> Emails)
        {
            Validate(first, last);
            Validate(PhoneNumbers, Emails);
        } // done

        private void Validate(string number)
        {
            foreach (char item in number)
            {
                if (!Char.IsDigit(item) && item != ' ')
                    throw new InvalidInput("The phone number cannot consist of anything other than numbers, spaces");
            }
        } // done
    }
    public class Contact
    {
        public List<string> PhoneNumbers { get; set; }
        public List<string> Emails { get; set; }
    

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Contact(string first, string last, List<string> phones, List<string> emails)
        {
            this.FirstName = first;
            this.LastName = last;
            this.Emails = emails;
            this.PhoneNumbers = phones;
        }
        public Contact()
        {

        }
        public override string ToString()
        {
            StringBuilder ans = new StringBuilder();
            ans.Append(FirstName + "  " + LastName + "  ");

            foreach (string item in PhoneNumbers)
                ans.Append(item + "  ");

            foreach (string item in Emails)
                ans.Append(item + "  ");

            return ans.ToString();
        }
    }

}


