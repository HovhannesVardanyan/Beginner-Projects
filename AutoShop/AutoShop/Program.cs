using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        User me = new User("Hovo",200);
        Bus b = new Bus(20,50,10,"Hundai",50,0.01);
        Truck t = new Truck(50, 50, 10, "Furz", 50, 0.01);
        Sedan s = new Sedan(50, 50, 10, "Ferrari",  50, 0.01);

        Shop sh = new Shop(20);
        sh.Add(b,50);
        sh.Add(t,60);
        sh.Add(s, 100);

        sh.Add(b, 50);
        sh.Add(t, 60);
        sh.Add(s, 100);

        sh.Add(b, 50);
        sh.Add(t, 60);
        sh.Add(s, 100);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Where do you want to go ? \n Shop  ?  Garage ? Exit ?");
            string ans = Console.ReadLine();

            Console.Clear();
            if (ans.ToUpper() == "SHOP")
                sh.Trade(me);
            else if (ans.ToUpper() == "GARAGE")
            {
                me.DisplayCars();
                Console.ReadKey();
            }
            else if (ans.ToUpper() == "EXIT")
                break;
        }


    }
} 