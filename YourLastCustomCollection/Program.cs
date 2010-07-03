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
            var customers = new YourLastCollection<Customer>();
            customers.Add(new Customer("Chris"));
            customers.Add(new Customer("Missy"));
            customers.Add(new Customer("Lincoln"));

            var addresses = new YourLastCollection<Address>();
            addresses.Add(new Address("Mitchell", "SD"));
            addresses.Add(new Address("Ethan", "SD"));
            addresses.Add(new Address("Canistota", "SD"));

            for (int i = 0; i < customers.Count; i++)
            {
                if (customers[i].Name.Contains("s"))
                {
                    Console.WriteLine("Customer {0}'s name is {1}", i, ((Customer) customers[i]).Name);
                }
            }

            Console.WriteLine();

            for (int i = 0; i < addresses.Count; i++)
            {
                Console.WriteLine("Address {0} is {1},{2}", i, addresses[i].City, addresses[i].State);
            }

            Console.ReadLine();
        }
    }

    class YourLastCollection<CLASS> : CollectionBase
    {
        public void Add(CLASS obj)
        {
            InnerList.Add(obj);
        }

        public CLASS this[int index]
        {
            get { return (CLASS) InnerList[index]; }
        }
    }

    class AddressCollection : CollectionBase
    {
        public void Add(Address a)
        {
            InnerList.Add(a);
        }

        public Address this[int index]
        {
            get
            {
                return (Address) InnerList[index];
            }
        }
    }

    class CustomerCollection : System.Collections.CollectionBase
    {
        public void Add(Customer c)
        {
            base.InnerList.Add(c);
        }

        public Customer this[int index]
        {
            get { return (Customer)InnerList[index];  }
        }
    }

    class Address
    {
        public Address(string city, string state)
        {
            City = city;
            State = state;
        }

        public string City { get; set; }
        public string State { get; set; }
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
