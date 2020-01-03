using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Compro.Tests
{
    public class JsonCommandPieceConverterTests
    {
        private JsonCommandPieceConverter _defaultConverter;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _defaultConverter = new JsonCommandPieceConverter();
            var arr = ArrayPool<object>.Shared.Rent(10);
            ArrayPool<object>.Shared.Return(arr, true);
        }

        [Test]
        public void FloatsShouldBeConverted()
        {
            var floatString = _defaultConverter.ConvertToString(0.5f).Try().IfFail("");

            Assert.AreEqual("0.5", floatString);
        }

        [Test]
        public void DoublesShouldBeConverted()
        {
            var doubleString = _defaultConverter.ConvertToString(0.5).IfFail("");

            Assert.AreEqual("0.5", doubleString);
        }

        [Test]
        public void NansShouldBeConverted()
        {
            var nan = _defaultConverter.ConvertToString(float.NaN).IfFail("");

            Assert.AreEqual("NaN", nan);
        }

        [Test]
        public void NegativeInfinityShouldBeConverted()
        {
            var negInf = _defaultConverter.ConvertToString(float.NegativeInfinity).IfFail("");

            Assert.AreEqual("-Infinity", negInf);
        }

        [Test]
        public void PositiveInfinityShouldBeConverted()
        {
            var posInf = _defaultConverter.ConvertToString(float.PositiveInfinity).IfFail("");

            Assert.AreEqual("Infinity", posInf);
        }

        [Test]
        public void PositiveInfinityStringShouldBeConverted()
        {
            var posInf = (float) _defaultConverter.ConvertFromString("Infinity", typeof(float)).IfFail(0f);

            Assert.AreEqual(float.PositiveInfinity, posInf);
        }
    }
}
