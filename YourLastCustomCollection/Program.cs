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

            var filteredAndSortedCustomers = customers.Filter(c => c.Name.Contains("s")).Sort(x => x.Name);

            int i = 0;
            foreach(var customer in filteredAndSortedCustomers)
            {
                Console.WriteLine("Customer {0}'s name is {1}", i,  customer.Name);
                i = i + 1;
            }

            Console.WriteLine();

            var sortedAddresses = addresses.Sort(x => x.City);

            foreach(var address in sortedAddresses)
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

    static class CollectionExtensions
    {
        public static IEnumerable<CLASS> Filter<CLASS>(this IEnumerable<CLASS> collection, Func<CLASS,bool> criteria)
        {
            return new FilteredEnumerable<CLASS>(collection, criteria);
        }

        public static IEnumerable<CLASS> Sort<CLASS>(this IEnumerable<CLASS> collection, Func<CLASS,IComparable> property)
        {
            return new OrderedEnumerable<CLASS>(collection, property);
        }

        class OrderedEnumerable<CLASS> : IEnumerable<CLASS>
        {
            private IEnumerable<CLASS> _mainList;
            private Func<CLASS, IComparable> _sortProperty;

            public OrderedEnumerable(IEnumerable<CLASS> mainList, Func<CLASS, IComparable> sortProperty)
            {
                _mainList = mainList;
                _sortProperty = sortProperty;
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            /// <filterpriority>1</filterpriority>
            public IEnumerator<CLASS> GetEnumerator()
            {
                var comparer = new AnythingComparer<CLASS>((x, y) =>
                {
                    return _sortProperty(x).CompareTo(_sortProperty(y));
                });

                var sortedList = new List<CLASS>(_mainList);
                sortedList.Sort(comparer);

                foreach(var item in sortedList)
                {
                    yield return item;
                }
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        class FilteredEnumerable<CLASS> : IEnumerable<CLASS>
        {
            private Func<CLASS, bool> _criteria;
            private IEnumerable<CLASS> _mainList;

            public FilteredEnumerable(IEnumerable<CLASS> mainList, Func<CLASS, bool> criteria)
            {
                _criteria = criteria;
                _mainList = mainList;
            }

            public IEnumerator<CLASS> GetEnumerator()
            {
                foreach(var c in _mainList)
                {
                    if(_criteria(c))
                    {
                        yield return c;
                    }
                }
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
    class YourLastCollection<CLASS> : CollectionBase, IEnumerable<CLASS>
    {
        
        public void Add(CLASS obj)
        {
            InnerList.Add(obj);
            
        }

        public CLASS this[int index]
        {
            get { return (CLASS) InnerList[index]; }
        }

        public IEnumerator<CLASS> GetEnumerator()
        {

            foreach(CLASS c in InnerList)
            {
                yield return c;
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
