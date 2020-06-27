using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Compro.Tests
{
    public class ConsoleCommandOnMethodTests
    {
        private TestCommandProvider _commandProvider;
        private ConsoleCommand[] _commands;

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
            var success = result as ConsoleCommandExecuteSuccess;
            Assert.NotNull(success);
            Assert.True(Equals(success.ReturnedValue, 0.5f));
        }

        [Test]
        public void LambdaCommandShouldWorkOutOfTheBox()
        {
            var plusOneToString = (Func<int, string>) (i => (i + 1).ToString());
            var command = new ConsoleCommand(plusOneToString.Target!, plusOneToString.Method);

            var result = command.Execute("1");
            var converted = result.ConvertOrError();

            Assert.AreEqual("\"2\"", converted);
        }

        [Test]
        public void CommandsWithComplexReturnShouldWorkOutOfTheBox()
        {
            var findCommand = _commands.First(c => c.Name == "Find");

            var result = findCommand.Execute("\"Debussy\"");
            var converted = result.ConvertOrError();

            Assert.AreNotEqual(ConsoleCommandExecuteSuccess.Void, result);
            Assert.IsTrue(converted.Contains("Tchaikovsky"));
        }

        [Test]
        public void CommandsWithVoidReturnAndParamsShouldWorkOutOfTheBox()
        {
            var printCommand = _commands.First(c => c.Name == "PrintLocalTime");

            var result = printCommand.Execute();

            Assert.AreEqual("", result.ConvertOrError());
            Assert.AreEqual(ConsoleCommandExecuteSuccess.Void, result);
        }

        [Test]
        public void CommandShouldReturnExceptionStringOnError()
        {
            var sqrtCommand = _commands.First(c => c.Name == "Sqrt");

            var result = sqrtCommand.Execute("\"not a float\"");
            var failure = result as ConsoleCommandExecuteFailure;

            Assert.NotNull(failure);
            Assert.That(failure.Exception is ArgumentException);
            var converted = result.ConvertOrError();
            Assert.That(converted.StartsWith("System.ArgumentException: Could not convert string to double"));
        }

        [Test]
        public void CommandsWithCollectionReturnShouldWorkOutOfTheBox()
        {
            var allCommand = _commands.First(c => c.Name == "All");

            var result = (ConsoleCommandExecuteSuccess) allCommand.Execute();
            var returnedValue = (IReadOnlyList<Person>) result.ReturnedValue;

            Assert.That(!ReferenceEquals(_commandProvider.Persons, returnedValue));
            CollectionAssert.AreEqual(_commandProvider.Persons, returnedValue);
        }
    }

    internal class TestCommandProvider
    {
        private static readonly Person _Tchaikovsky = new Person {
            Name = "Tchaikovsky",
            Position = new Geoposition {
                Latitude = 55.7558,
                Longitude = 37.6173
            }
        };

        private static readonly Person _Debussy = new Person {
            Name = "Debussy",
            Position = new Geoposition {
                Latitude = 48.8566,
                Longitude = 2.3522
            },
            Friends = { _Tchaikovsky }
        };

        public List<Person> Persons { get; } = new List<Person> { _Tchaikovsky, _Debussy };

        [CommandExecutable]
        public float Sqrt(float value) => (float) Math.Sqrt(value);

        [CommandExecutable]
        public void PrintLocalTime() => Console.WriteLine(DateTime.Now.ToLongTimeString());

        [CommandExecutable("Finds a person by name in the predefined person database.")]
        [return: CommandDoc("Found person or null.")]
        public Person Find([CommandDoc("Person's name.")] string name) =>
            Persons.Find(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

        [CommandExecutable]
        public Person Find(Geoposition geoPos) => Persons.Find(p => p.Position == geoPos);

        [CommandExecutable]
        public IReadOnlyList<Person> All() => Persons.AsReadOnly();
    }
}