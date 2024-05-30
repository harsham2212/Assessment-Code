using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingAssessment.Refactor
{
    public class People
    {
        private static readonly DateTimeOffset Under16 = DateTimeOffset.Now.AddYears(-15);
        public string Name { get; private set; }
        public DateTimeOffset DOB { get; private set; }

        public People(string name) : this(name, Under16.Date)
        {
        }

        public People(string name, DateTime dob)
        {
            Name = name;
            DOB = dob;
        }
    }

    public class BirthingUnit
    {
        private List<People> _people;
        private readonly Random _random; // Single Random instance for reuse

        public BirthingUnit()
        {
            _people = new List<People>();
            _random = new Random();  // Initialize Random once
        }

        public List<People> GetPeople(int count) //Fixed i to count for proper naming
        {
            string[] prefixes = { "Bob", "Betty" };
            string[] suffixes = { "Stark", "Root", "Johnson", "Williams", "Jackson", "Head", "Miller", "Cummins", "Taylor", "Warner" };

            for (int j = 0; j < count; j++)
            {
                try
                {
                    string randomPrefix = prefixes[_random.Next(prefixes.Length)];
                    string randomSuffix = suffixes[_random.Next(suffixes.Length)];
                    string name = randomPrefix + " " + randomSuffix;
                    DateTime dob = DateTime.UtcNow.AddDays(-_random.Next(18 * 365, 85 * 365)); // Generate random birth date
                    _people.Add(new People(name, dob)); ;
                }
                catch (Exception)
                {
                    throw new Exception("Failed to create user. Please try again later.");
                }
            }
            return _people;
        }

        public void GetBob()
        {
            var bobsOlderThan30 = _people.Where(p => p.Name.StartsWith("Bob") && p.DOB < DateTimeOffset.Now.AddYears(-30)).ToList();

            if (bobsOlderThan30.Any())
            {
                Console.WriteLine("\nPerson having First Name as Bob Older Than 30:");
                foreach (var bob in bobsOlderThan30)
                {
                    Console.WriteLine($"Name: {bob.Name}, Date of Birth: {bob.DOB}");
                }
            }
            else
            {
                Console.WriteLine("\nNo Bobs found older than 30.");
            }
        }

        public string GetMarried(People p, string lastName)
        {
            if (lastName.Contains("test"))
                return p.Name;
            string fullName = $"{p.Name} {lastName}"; // Used string interpolation for clarity
            return fullName.Length > 255 ? fullName.Substring(0, 255) : fullName;
        }

        public class Program
        {
            public static void Main(string[] args)
            {
                var birthingUnit = new BirthingUnit();

                // Generate 10 random people
                var people = birthingUnit.GetPeople(5);

                // Print the details of all people
                Console.WriteLine("All People:");
                foreach (var person in people)
                {
                    Console.WriteLine($"Name: {person.Name}, DOB: {person.DOB}");
                }
                birthingUnit.GetBob();

                // Test GetMarried method
                var personToMarry = new People("Lucy", DateTime.UtcNow.AddYears(-25));
                string marriedName = birthingUnit.GetMarried(personToMarry, "Jhonshan");

                Console.WriteLine($"\n{personToMarry.Name} married and changed name to: {marriedName}");
            }

        }
    }
}