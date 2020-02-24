using System;
using NUnit.Framework;

namespace Compro.Tests
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
            var multiply = new Func<double, double, double>((a, b) => a * b);
            _commands.Register(new ConsoleCommand(multiply.Target, multiply.Method, "mul"));

            var result = _commands.Execute("mul(0.5,0.5)");
            var converted = result.ConvertOrError();
            
            Assert.AreEqual("0.25", converted);
        }
    }
}