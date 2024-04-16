using System;

namespace labMPP2
{
    public class Program
    {
        static void Main(string[] args)
        {
            var faker = new Faker();
            PersonGenerator.Register(faker);
            AddressGenerator.Register(faker);
            TestGenerator.Register(faker);

            Person person = faker.Create<Person>();
            Address address = faker.Create<Address>();
            Test test = faker.Create<Test>();

            ObjectInfoPrinter.PrintObjectInfo(test);
            Console.WriteLine();
            ObjectInfoPrinter.PrintObjectInfo(person);
            Console.WriteLine();
            ObjectInfoPrinter.PrintObjectInfo(address);
        }
    }
}