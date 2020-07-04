using System;
using Newtonsoft.Json;

namespace ConsoleBackEnd.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // For this demo I've written ExchangeRatesApi class, which provides several methods
            // representing requests to https://exchangeratesapi.io/.
            // These return various forex currency rates at the given day.
            // So let's allow user to call them from console via text input,
            // passing function parameters as JSON objects:

            // First, create instance containing several commands which call ExchangeRates API synchronously.
            var exchangeRatesApi = new ExchangeRatesApi();

            // Slightly customize 'command result to string' converter,
            // since exchangeratesapi.io returns ISO dates without time part.
            var commandReturnsConverter = new CommandReturnedObjectJsonConverter(true, Formatting.Indented,
                new JsonSerializerSettings { DateFormatString = "yyyy'-'MM'-'dd" });

            // Create command manager. It will contain a set of commands, parse given user input into a proper command,
            // convert its parameters from a string to typed objects, execute the command with converted parameters
            // and return a result.
            var consoleCommands = new ConsoleCommands();

            // Methods marked with [CommandExecutable] will be added from the given exchangeRatesApi instance:
            consoleCommands.RegisterAllFromInstance(exchangeRatesApi);

            // Example of the manually created commands:
            var help = new Func<string>(() => {
                Console.WriteLine($"Available commands:{Environment.NewLine}");
                return consoleCommands.ToString();
            });
            consoleCommands.Register(new ConsoleCommand(help.Target!, help.Method, "help",
                "Prints available commands.", new[] { "commands" }));

            var clearScreen = new Action(Console.Clear);
            consoleCommands.Register(new ConsoleCommand(clearScreen.Target!, clearScreen.Method, "clear",
                "Clears console.", new[] { "cls" }));

            var quit = new Action(() => Environment.Exit(0));
            consoleCommands.Register(new ConsoleCommand(quit.Target!, quit.Method, "quit",
                "Quits the program.", new[] { "q" }));

            Console.WriteLine("Input \"help\" without quotes to see available commands. Pass command parameters as " +
                "JSON, for example: d(\"2019-12-31\", \"USD\", [\"EUR\", \"RUB\"])");

            // Continue to execute user imput until the 'quit' command:
            while (true) {
                Console.Write("Command prompt: ");
                string commandRepr = Console.ReadLine();
                string evaluated = consoleCommands.Execute(commandRepr).ConvertOrError(commandReturnsConverter);
                Console.WriteLine(evaluated);
            }
        }
    }
}