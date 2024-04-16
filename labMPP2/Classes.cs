using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labMPP2
{
    public class Test
    {
        public Uri Uri { get; }
        public DateTime DateTime { get; }
        public List<string> Args { get; }

        public Test(Uri uri, DateTime dateTime, List<string> args)
        {
            Uri = uri;
            DateTime = dateTime;
            Args = args;
        }
    }

    public class Person
    {
        public string FirstName { get; }
        public string LastName { get; }
        public int Age { get; }
        public Address Address { get; }

        public Person(string firstName, string lastName, int age, Address address)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Address = address;
        }
    }

    public class Address
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }

        public Address(string street, string city, string state, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }
    }
}
