using System;
using System.Collections.Generic;

//top level statement (dans le main)
VersionNine.Examples.RecordExample2();

namespace VersionNine
{
    public static class Examples
    {
        //Record
        //Record - concise, immutable by default, value equality, nice ToString, inheritance, deconstruct
        public record Person(string FirstName, string LastName, string[] PhoneNumbers);
        //equivaut à 
        public record Person2
        {
            public string FirstName { get; init; } = default!;
            public string LastName { get; init; } = default!;
        };

        //mutable
        public record MutablePerson
        {
            public string FirstName { get; set; } = default!;
            public string LastName { get; set; } = default!;
        };

        static void RecordExample()
        {
            //equality
            var phoneNumbers = new string[2];
            Person person1 = new("Nancy", "Davolio", phoneNumbers);
            Person person2 = new("Nancy", "Davolio", phoneNumbers);
            Console.WriteLine(person1 == person2); // output: True

            person1.PhoneNumbers[0] = "555-1234";
            Console.WriteLine(person1 == person2); // output: True

            Console.WriteLine(ReferenceEquals(person1, person2)); // output: False

            //mutation using with
            person2 = person1 with { FirstName = "John" };
            Console.WriteLine(person2);
            // output: Person { FirstName = John, LastName = Davolio, PhoneNumbers = System.String[] }
            Console.WriteLine(person1 == person2); // output: False

            person2 = person1 with { PhoneNumbers = new string[1] };
            Console.WriteLine(person2);
            // output: Person { FirstName = Nancy, LastName = Davolio, PhoneNumbers = System.String[] }
            Console.WriteLine(person1 == person2); // output: False

            person2 = person1 with { };
            Console.WriteLine(person1 == person2); // output: True
        }

        //record inheritance
        public abstract record Person3(string FirstName, string LastName);
        public record Teacher(string FirstName, string LastName, int Grade) : Person3(FirstName, LastName);
        public record Student(string FirstName, string LastName, int Grade) : Person3(FirstName, LastName);

        public static void RecordExample2()
        {
            Person3 teacher = new Teacher("Nancy", "Davolio", 3);
            Person3 student = new Student("Nancy", "Davolio", 3);
            Console.WriteLine(teacher == student); // output: False

            Student student2 = new Student("Nancy", "Davolio", 3);
            Console.WriteLine(student2 == student); // output: True
        }

        //init
        public class WeatherObservation
        {
            public DateTime RecordedAt { get; init; }
            public decimal TemperatureInCelsius { get; init; }
            public decimal PressureInMillibars { get; init; }

            public override string ToString() => $"At {RecordedAt:h:mm tt} on {RecordedAt:M/d/yyyy}: Temp = {TemperatureInCelsius}, with {PressureInMillibars} pressure";
        }

        //better pattern matching

        //parenthèses, and, or
        public static bool IsLetterOrSeparator(this char c) => c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or '.' or ',';

        //quick wins

        //new()
        private static List<WeatherObservation> _observations = new() { new() { RecordedAt = DateTime.Now } };

        //less cast
        static int? Conditional()
        {
            var condition = false;
            return condition ? null : 23; //no need to cast (int?)null
        }

        //enumerator pattern
        class PseudoEnumerator
        {
            private List<int> _myCollectionOfStuff;
            public IEnumerator<int> GetEnumerator() => _myCollectionOfStuff.GetEnumerator();
        }

        static void EnumeratorExample()
        {
            foreach (var value in new PseudoEnumerator())
            {

            }
        }
    }
}
