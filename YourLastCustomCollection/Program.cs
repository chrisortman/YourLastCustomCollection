using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YourLastCustomCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            ArrayList customers = new ArrayList();
            customers.Add(new Customer("Chris"));
            customers.Add(new Customer("Missy"));
            customers.Add(new Customer("Lincoln"));

            
            for(int i=0; i<customers.Count; i++)
            {
                Console.WriteLine("Customer {0}'s name is {1}",i,((Customer)customers[i]).Name);
            }

            Console.ReadLine();
        }
    }

    class Customer
    {
        public Customer(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
