using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Compro.Tests
{
    public class TerminalCommandOnMethodTests
    {
        private TestCommandProvider _commandProvider;
        private ConsoleCommandOnMethod[] _commands;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _commandProvider = new TestCommandProvider();
            _commands = ConsoleCommands.GatherFromInstance(_commandProvider);
        }

        [Test]
        public void CommandsWithPrimitiveParametersShouldWorkOutOfTheBox()
        {
            var sqrtCommand = _commands.First(c => c.Name == "Sqrt");

            var result = sqrtCommand.Execute("0.25");
            var converted = result.ConvertOrError();
            
            Assert.AreEqual("0.5", converted);
        }

        [Test]
        public void CommandsWithComplexReturnShouldWorkOutOfTheBox()
        {
            var findCommand = _commands.First(c => c.Name == "Find");

            var result = findCommand.Execute("\"Macron\"");
            var converted = result.ConvertOrError();

            Assert.AreNotEqual(CommandExecuteSuccess.Void, result);
            Assert.IsTrue(converted.Contains("Putin"));
        }

        [Test]
        public void CommandsWithVoidReturnAndParamsShouldWorkOutOfTheBox()
        {
            var printCommand = _commands.First(c => c.Name == "PrintLocalTime");

            var result = printCommand.Execute();

            Assert.AreEqual("", result.ConvertOrError());
            Assert.AreEqual(CommandExecuteSuccess.Void, result);
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