using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Truck:Car
{
    public int MaxSpeed
    {
        get => maxSpeed;
        private set
        {
            maxSpeed = value;
            if (value > 50)
                maxSpeed = 50;

        }
    }
    public int SeatCount
    {
        get => seatCount;
        private set
        {
            seatCount = value;
            if (value > 2)
                seatCount = 2;
        }
    }

    public Truck(int maxSpeed, int maxCapacity, int seatCount, string model, double petrol, double coef)
        :base(maxCapacity, model,  petrol,  coef)
    {
        this.MaxSpeed = maxSpeed;
        this.SeatCount = seatCount;
    }
}

