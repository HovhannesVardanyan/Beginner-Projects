using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class User
{
    private Garage garage;
    public string Name { get; private set; }
    public double Money { get;  private set; }
    
    public User(string name, double money, int NumOfCars = 1)
    {
        this.Name = name;
        this.Money = money;
        garage = new Garage(NumOfCars);
    }
    
    public bool BuyACar(Car car, double price)
    {
        if (this.Money < price)
            return false;

        if (!garage.Add(car))
        {
            garage.Extend(garage.Size + 1);
            garage.Add(car);
            
            Console.WriteLine(this.Money);
        }
        this.Money -= price;
        return true;
    }
    public void DisplayCars()
    {
        garage.Display();
    }
}

class Garage {
    private Car[] garage;
    public int Size { get => garage.Length; }
    public Garage(int numOfCars)
    {
        garage = new Car[numOfCars];
    }
    public bool Add(Car car)
    {
        for (int i = 0; i < garage.Length; i++)
        {
            if (garage[i] == null)
            {
                garage[i] = car;
                return true;
            }
        }
        return false;
    }
    public void Extend(int newSize)
    {
        if (newSize < garage.Length)
            return;

        Car[] hold = garage;
        this.garage = new Car[newSize];

        for (int i = 0; i < hold.Length; i++)
        {
            garage[i] = hold[i];
           
        }

    }
    public void Display()
    {
        
        foreach (var item in garage)
        {
            if (item != null)
                item.PrintStats();
        }
    }
}


// User name, money, car

// car - petrol, model, engine power, seatCount, velocity readonly ,Ride -> Petrol--
//Move() FillPetrol()
// Sedan, bus, truck 

// Shop cars, buy

// Mapper