using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Sedan : Car
{
    public int SeatCount
    {
        get => seatCount;
        private set
        {
            seatCount = value;
            if (value > 4)
                seatCount = 4;
        }
    }
    public int MaxSpeed { get => maxSpeed; }

    public Sedan(int maxSpeed, int maxCapacity, int seatCount, string model, double petrol, double coef)
        :base(maxCapacity, model,  petrol,  coef)
    {
        this.maxSpeed = maxSpeed;
        this.SeatCount = seatCount;
    }
}

