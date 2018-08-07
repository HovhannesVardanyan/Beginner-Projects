using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Mapper
{
    public readonly Car car;
    public double Price { get; private set; }
    public Mapper(Car car, double price)
    {
        this.Price = price;
        this.car = car;
    }
}

