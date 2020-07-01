using System;
using NUnit.Framework;

namespace ConsoleBackEnd.Tests
{
    public class ConsoleCommandsTests
    {
        private ConsoleCommands _commands;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _commands = new ConsoleCommands();
        }

        [TearDown]
        public void TearDown()
        {
            _commands.UnregisterAll();
        }

        [Test]
        public void RegisteredLambdaCommandShouldWorkOutOfTheBox()
        {
            var mul = new Func<double, double, double>((a, b) => a * b);
            _commands.Register(new ConsoleCommand(mul.Target!, mul.Method, "mul"));

            var result = _commands.Execute("mul(0.5,0.5)");
            string converted = result.ConvertOrError();

            Assert.AreEqual("0.25", converted);
        }

        [Test]
        public void RegisterAllCommandsFromInstanceWithCommandMethodsShouldWorkOutOfTheBox()
        {
            _commands.RegisterAllFromInstance(new TestCommandProvider());

            var result = _commands.Execute("Find", @"{""Latitude"": ""55.7558"", ""Longitude"": ""37.6173""}");
            string converted = result.ConvertOrError();

            Assert.IsTrue(converted.Contains("Tchaikovsky"));
        }
    }
}