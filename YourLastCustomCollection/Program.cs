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

            customers = customers.Filter(c => c.Name.Contains("s")).Sort(x => x.Name);

            int i = 0;
            foreach(var customer in customers)
            {
                Console.WriteLine("Customer {0}'s name is {1}", i,  customer.Name);
                i = i + 1;
            }

            Console.WriteLine();

            addresses = addresses.Sort(x => x.City);

            foreach(var address in addresses)
            {
                Console.WriteLine("Address {0} is {1},{2}", i, address.City, address.State);
            }

            Console.ReadLine();
        }
    }

    class AnythingComparer<CLASS> : IComparer<CLASS>, IComparer
    {
        private readonly Func<CLASS, CLASS, int> _sortFunction;

        public AnythingComparer(Func<CLASS,CLASS,int> sortFunction)
        {
            _sortFunction = sortFunction;
        }

        public int Compare(CLASS x, CLASS y)
        {
            return _sortFunction(x, y);
        }

        public int Compare(object x, object y)
        {
            return Compare((CLASS) x, (CLASS) y);
        }
    }

    class YourLastCollection<CLASS> : CollectionBase, IEnumerable<CLASS>
    {
        private IComparer _currentSort;
        private Func<CLASS, bool> _currentFilter;

        public void Add(CLASS obj)
        {
            InnerList.Add(obj);
            
        }

        public CLASS this[int index]
        {
            get { return (CLASS) InnerList[index]; }
        }


        private YourLastCollection<CLASS> Sort(IComparer comparer)
        {
            var sorted = new YourLastCollection<CLASS>();
            
            foreach(CLASS c in InnerList)
            {
                sorted.Add(c);
            }

            sorted.InnerList.Sort(comparer);
            return sorted;
        }

        public YourLastCollection<CLASS> Sort(Func<CLASS,IComparable> property)
        {

            var comparer = new AnythingComparer<CLASS>((x, y) =>
            {
                return property(x).CompareTo(property(y));
            });

            _currentSort = comparer;
            return this;
        }
        public YourLastCollection<CLASS> Filter(Func<CLASS,bool> criteria)
        {
            _currentFilter = criteria;
            return this;
        }

        public IEnumerator<CLASS> GetEnumerator()
        {
            if(_currentSort != null)
            {
                InnerList.Sort(_currentSort);
            }
           foreach(CLASS c in InnerList)
           {
               if(_currentFilter != null)
               {
                   if(_currentFilter(c) )
                   {
                       yield return c;
                   }
               }
               else
               {
                   yield return c;
               }
           } 
        }
    }

    class AddressComparer : IComparer<Address>, IComparer
    {
        public int Compare(object x, object y)
        {
            return Compare((Address) x, (Address) y);
        }

        public int Compare(Address x, Address y)
        {
            return x.City.CompareTo(y.City);
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
