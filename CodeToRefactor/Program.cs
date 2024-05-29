using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingAssessment.Refactor
{
    public class People
    {
        private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15);
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
            for (int j = 0; j < count; j++)
            {
                try
                { // Fixed the random generation logic to alternate correctly
                    string name = _random.Next(2) == 0 ? "Bob" : "Betty";
                    // Changed from subtracting TimeSpan to adding days in the past
                    DateTime dob = DateTime.UtcNow.AddDays(-_random.Next(18 * 365, 85 * 365));
                    _people.Add(new People(name, dob));
                }
                catch (Exception)
                {
                    // Dont think this should ever happen
                    throw new Exception("Failed to create user. Please try again later.");
                }
            }
            return _people;
        }

        private IEnumerable<People> GetBob(bool olderThan30)
        {
            DateTime thresholdDate = DateTime.UtcNow.AddYears(-30); // Fixed the date calculation
            return olderThan30
                ? _people.Where(x => x.Name == "Bob" && x.DOB < thresholdDate)
                : _people.Where(x => x.Name == "Bob");
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
                var people = birthingUnit.GetPeople(10);

                // Print the details of all people
                Console.WriteLine("All People:");
                foreach (var person in people)
                {
                    Console.WriteLine($"Name: {person.Name}, DOB: {person.DOB}");
                }

                // Get all Bobs older than 30
                var bobsOlderThan30 = birthingUnit.GetBob(true);

                Console.WriteLine("\nBobs Older Than 30:");
                foreach (var bob in bobsOlderThan30)
                {
                    Console.WriteLine($"Name: {bob.Name}, DOB: {bob.DOB}");
                }

                // Test GetMarried method
                var personToMarry = new People("Lucy", DateTime.UtcNow.AddYears(-25));
                string marriedName = birthingUnit.GetMarried(personToMarry, "Jhonshan");

                Console.WriteLine($"\n{personToMarry.Name} married and changed name to: {marriedName}");
            }

        }
    }
}