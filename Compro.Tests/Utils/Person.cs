using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Compro.Tests
{
    [JsonObject(IsReference = true)]
    internal class Person
    {
        public string Name { get; set; }

        public Geoposition Position { get; set; }

        public List<Person> Friends { get; } = new List<Person>();
    }

    internal struct Geoposition : IEquatable<Geoposition>
    {
        public double Latitude;
        public double Longitude;

        /// <inheritdoc />
        public bool Equals(Geoposition other) => Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Geoposition other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);

        public static bool operator ==(Geoposition left, Geoposition right) => left.Equals(right);

        public static bool operator !=(Geoposition left, Geoposition right) => !left.Equals(right);
    }
}