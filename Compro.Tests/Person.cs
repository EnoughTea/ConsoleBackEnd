using System.Collections.Generic;

namespace Compro.Tests {
    internal class Person
    {
        public string Name { get; set; }

        public Geoposition Position { get; set; }

        public List<Person> Friends { get; } = new List<Person>();
    }

    internal struct Geoposition
    {
        public double Latitude;
        public double Longitude;
    }
}