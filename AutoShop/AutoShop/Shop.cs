using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum CarType {
    Sedan,
    Bus,
    Truck,
    Invalid
}

class Shop
{
    Mapper[] list;
    public Shop(int NumOfCars)
    {
        list = new Mapper[NumOfCars];
    }

    public bool Add(Car car, double price)
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] == null)
            {
                list[i] = new Mapper(car, price);
                return true;
            }
        }
        Console.WriteLine("Not enough room for storing more cars!");
        return false;
    }
    public void Trade(User user)
    {

        Console.WriteLine("What car are you looking for?  ");
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Your balance: " + user.Money + "$ \n");

            Console.Write("Enter Car type  ");
            string answer = Console.ReadLine();
            
            if (answer.ToUpper() == "SEDAN")
                LookForCar<Sedan>();

            else if (answer.ToUpper() == "TRUCK")
                LookForCar<Truck>();

            else if (answer.ToUpper() == "BUS")
                LookForCar<Bus>();
            else
            {
                Console.WriteLine("We are sorry, but there is no result, based on your search");
                continue;
            }

            if (int.TryParse(Console.ReadLine(), out int i))
                this.SellACar(user, i);
            else
                Console.WriteLine("Your id is not valid, that is why we couldn't complete the transactions");
            
            Console.WriteLine("Looking for other things?");
            if (Console.ReadLine().ToUpper().Contains('N'))
                break;

            Console.Clear();
        }
       
    }


    private void LookForCar<T>()
    {
        for(int i = 0; i < list.Length; i++)
        {
            if (list[i] == null)
                continue;
            if (list[i].car is T b)
            {
                Console.Write($"{i} | ");
                list[i].car.PrintStats();
                Console.WriteLine(list[i].Price + "$");
            }
        }
    }
    private void SellACar(User user,int id)
    {

        if (id > this.list.Length || this.list[id] == null)
        {
            Console.WriteLine("That's not a valid id");
        }

        else if (user.BuyACar(this.list[id].car, this.list[id].Price))
        {
            Console.WriteLine("Congrats on your purchase");
            this.list[id] = null;
            Console.Clear();

            Console.WriteLine("These are the cars that you possess");
            user.DisplayCars();

        }
        else
            Console.WriteLine("You cannot afford to buy this car");

        Console.ReadLine();

        

        

    }

}

