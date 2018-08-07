using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// car - petrol, model, engine power, seatCount, velocity readonly ,Ride -> Petrol--
//Move() FillPetrol()


class Car
{
    // fields
    protected  int  maxSpeed;
    protected int seatCount;

    public  readonly int maxCapacity;
    public readonly string model;
    public readonly double coef;
    private double petrol;
  
    //constructors
    public Car(int maxSpeed,int maxCapacity,int seatCount,string model, double petrol, double coef)
    {
        this.maxSpeed = maxSpeed;
        this.maxCapacity = maxCapacity;
        this.seatCount = seatCount;
        this.model = model;
        this.Petrol = petrol;
        this.coef = coef; 
    }
    public Car(int maxCapacity, string model, double coef, double petrol ) : this ( 100,  maxCapacity,  4,  model,  petrol,  0.1)
    {

    }

    //properties
    public double Petrol {
        get  => petrol ;
        private set
        {
            petrol = value;
            if (value > maxCapacity)
                petrol = maxCapacity;
            
        }

    }


    //methods
    public void FillPetrol(double amount)
    {
        if (amount > 0)
            Petrol += amount;
    }

    public void Ride(int velocity)
    {
        if (velocity < 0 || velocity > maxSpeed)
        {
            Console.WriteLine("Enter Something valid");
            return;
        }
        if(this.Petrol < velocity*coef)
        {
            Console.WriteLine("Your petrol is not enough");
            return;
        }

        Petrol -= velocity * coef;
    }


    public void PrintStats()
    {
        
        Console.WriteLine($"Model - {model}, Seats - {seatCount}, Max Speed {maxSpeed}, Max Capacity {maxCapacity}, Coef {coef}");
    }
    
}

