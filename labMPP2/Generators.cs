using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labMPP2
{
    public static class TestGenerator
    {
        public static void Register(Faker faker)
        {
            faker.RegisterGenerator<Test>(() =>
            {
                var uri = faker.CreateValue(typeof(Uri));
                var dateTime = faker.CreateValue(typeof(DateTime));
                var args = faker.GenerateList<string>(5);
                return new Test((Uri)uri, (DateTime)dateTime, args);
            });
        }
    }

    public static class PersonGenerator
    {
        private static readonly string[] FirstNames = { "John", "Jane", "Mike", "Emily" };
        private static readonly string[] LastNames = { "Doe", "Smith", "Johnson", "Williams" };

        public static void Register(Faker faker)
        {
            faker.RegisterGenerator<Person>(() =>
            {
                var firstName = faker.PickRandom(FirstNames);
                var lastName = faker.PickRandom(LastNames);
                var age = faker.CreateValue(typeof(int));
                var address = faker.Create<Address>();

                return new Person(firstName, lastName, (int)age, address);
            });
        }
    }

    public static class AddressGenerator
    {
        private static readonly string[] Streets = { "Main St", "Oak St", "Maple Ave", "Cedar Rd" };
        private static readonly string[] Cities = { "New York", "Los Angeles", "Chicago", "Houston" };
        private static readonly string[] States = { "NY", "CA", "IL", "TX" };

        public static void Register(Faker faker)
        {
            faker.RegisterGenerator<Address>(() =>
            {
                var street = faker.PickRandom(Streets);
                var city = faker.PickRandom(Cities);
                var state = faker.PickRandom(States);
                var zipCode = faker.CreateValue(typeof(string));

                return new Address(street, city, state, (string)zipCode);
            });
        }
    }
}
