using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Compro.Tests
{
    public class JsonCommandPieceConverterTests
    {
        private JsonConverter _defaultConverter;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _defaultConverter = new JsonConverter();
        }

        [Test]
        public void FloatsShouldBeConverted()
        {
            string floatString = _defaultConverter.ConvertToString(0.5f).Try().IfFail(e => e.Message);

            Assert.AreEqual("0.5", floatString);
        }

        [Test]
        public void DoublesShouldBeConverted()
        {
            string doubleString = _defaultConverter.ConvertToString(0.5).IfFail(e => e.Message);

            Assert.AreEqual("0.5", doubleString);
        }

        [Test]
        public void NansShouldBeConverted()
        {
            string nan = _defaultConverter.ConvertToString(float.NaN).IfFail(e => e.Message);

            Assert.AreEqual("NaN", nan);
        }

        [Test]
        public void NegativeInfinityShouldBeConverted()
        {
            string negInf = _defaultConverter.ConvertToString(float.NegativeInfinity).IfFail(e => e.Message);

            Assert.AreEqual("-Infinity", negInf);
        }

        [Test]
        public void PositiveInfinityShouldBeConverted()
        {
            string posInf = _defaultConverter.ConvertToString(float.PositiveInfinity).IfFail(e => e.Message);

            Assert.AreEqual("Infinity", posInf);
        }

        [Test]
        public void PositiveInfinityStringShouldBeConverted()
        {
            float posInf = (float) _defaultConverter.ConvertFromString("Infinity", typeof(float)).IfFail(0f);

            Assert.AreEqual(float.PositiveInfinity, posInf);
        }
        
        [Test]
        public void SimpleStringShouldBeConverted()
        {
            string target = "\"Simple string\"";
            string simple = (string) _defaultConverter.ConvertFromString(target, typeof(string))
                .IfFail(e => e.Message);

            Assert.AreEqual("Simple string", simple);
        }
        
        [Test]
        public void StringWithNestedQuotesShouldBeConverted()
        {
            string target = "\"Not \\\"so \\\"very\\\" simple\\\" string\"";
            string nestedQuotes = (string) _defaultConverter.ConvertFromString(target,
                typeof(string)).IfFail(e => e.Message);

            Assert.AreEqual("Not \"so \"very\" simple\" string", nestedQuotes);
        }
        
        [Test]
        public void SingleCharShouldBeConverted()
        {
            string degree = _defaultConverter.ConvertToString('\u00f8')
                .IfFail(e => e.Message);

            Assert.AreEqual("\u00f8", degree);
        }
    }
}
