using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Compro.Tests
{
    public class TerminalCommandOnMethodTests
    {
        private TestCommandProvider _commandProvider;
        private TerminalCommandOnMethod[] _commands;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _commandProvider = new TestCommandProvider();
            _commands = TerminalCommandOnMethod.GatherFromInstance(_commandProvider, CommandIOPartConverters.Shared);
        }

        [Test]
        public void CommandsWithPrimitiveParametersShouldWorkOutOfTheBox()
        {
            var sqrtCommand = _commands.First(c => c.Name == "Sqrt");

            var result = sqrtCommand.Call("0.25");

            Assert.AreEqual("0.5", result.ConvertedValue);
        }

        [Test]
        public void CommandsWithComplexReturnShouldWorkOutOfTheBox()
        {
            var findCommand = _commands.First(c => c.Name == "Find");

            var result = findCommand.Call("Macron");

            Assert.IsTrue(result.ConvertedValue.Contains("Putin"), result.ConvertedValue);
            Assert.AreNotEqual(CommandCallSuccess.Void, result);
        }

        [Test]
        public void CommandsWithVoidReturnAndParamsShouldWorkOutOfTheBox()
        {
            var printCommand = _commands.First(c => c.Name == "PrintLocalTime");

            var result = printCommand.Call();

            Assert.AreEqual("", result.ConvertedValue);
            Assert.AreEqual(CommandCallSuccess.Void, result);
        }

        internal class TestCommandProvider
        {
            private static readonly Person putin = new Person {
                Name = "Putin",
                Position = new Geoposition {
                    Latitude = 55.7558,
                    Longitude = 37.6173
                }
            };

            private static readonly Person macron = new Person {
                Name = "Macron",
                Position = new Geoposition {
                    Latitude = 48.8566,
                    Longitude = 2.3522
                },
                Friends = { putin }
            };

            private readonly List<Person> _persons = new List<Person> { putin, macron };

            [CommandExecutable]
            public float Sqrt(float value) => (float) Math.Sqrt(value);

            [CommandExecutable]
            public void PrintLocalTime() => Console.WriteLine(DateTime.Now.ToLongTimeString());

            [CommandExecutable("Finds a person by name in the predefined person database.")]
            [return: CommandDoc("Found person or null.")]
            public Person Find([CommandDoc("Person's name.")] string name) =>
                _persons.Find(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}