using System;
using System.Linq;
using NUnit.Framework;

namespace Compro.Tests
{
    public class JsonCommandPieceConverterTests
    {
        [Test]
        public void FloatsShouldBeConverted()
        {
            string floatString = JsonConverter.ToString(0.5f).Try().IfFail(e => e.Message);

            Assert.AreEqual("0.5", floatString);
        }

        [Test]
        public void DoublesShouldBeConverted()
        {
            string doubleString = JsonConverter.ToString(0.5).IfFail(e => e.Message);

            Assert.AreEqual("0.5", doubleString);
        }

        [Test]
        public void NansShouldBeConverted()
        {
            string nan = JsonConverter.ToString(float.NaN).IfFail(e => e.Message);

            Assert.AreEqual("NaN", nan);
        }

        [Test]
        public void NegativeInfinityShouldBeConverted()
        {
            string negInf = JsonConverter.ToString(float.NegativeInfinity).IfFail(e => e.Message);

            Assert.AreEqual("-Infinity", negInf);
        }

        [Test]
        public void PositiveInfinityShouldBeConverted()
        {
            string posInf = JsonConverter.ToString(float.PositiveInfinity).IfFail(e => e.Message);

            Assert.AreEqual("Infinity", posInf);
        }

        [Test]
        public void PositiveInfinityStringShouldBeConverted()
        {
            float posInf = (float) JsonConverter.FromString("Infinity", typeof(float)).IfFail(0f);

            Assert.AreEqual(float.PositiveInfinity, posInf);
        }

        [Test]
        public void SimpleStringShouldBeConverted()
        {
            string target = "\"Simple string\"";
            string simple = (string) JsonConverter.FromString(target, typeof(string))
                .IfFail(e => e.Message);

            Assert.AreEqual("Simple string", simple);
        }

        [Test]
        public void StringWithNestedQuotesShouldBeConverted()
        {
            string target = "\"Not \\\"so \\\"very\\\" simple\\\" string\"";
            string nestedQuotes = (string) JsonConverter.FromString(target,
                typeof(string)).IfFail(e => e.Message);

            Assert.AreEqual("Not \"so \"very\" simple\" string", nestedQuotes);
        }

        [Test]
        public void SingleCharShouldBeConverted()
        {
            string degree = JsonConverter.ToString('\u00f8')
                .IfFail(e => e.Message);

            Assert.AreEqual("\u00f8", degree);
        }

        [Test]
        public void ReferensesShouldBeDeserialized()
        {
            string personsRepr = @"[
{""$id"":""1"",""Name"":""Tchaikovsky"",""Position"":{""Latitude"":55.7558,""Longitude"":37.6173},""Friends"":[]},
{""$id"":""2"",""Name"":""Debussy"",""Position"":{""Latitude"":48.8566,""Longitude"":2.3522},""Friends"":[{""$ref"":""1""}]}
]";
            var persons = (Person[]) JsonConverter.FromString(personsRepr, typeof(Person[]))
                .IfFail(Array.Empty<Person>());

            Assert.That(persons.Length == 2);
            Assert.That(persons[1].Friends.First() == persons.First());
        }
    }
}