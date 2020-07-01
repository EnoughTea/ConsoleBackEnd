using System;
using Newtonsoft.Json;

namespace ConsoleBackEnd.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var exchangeRatesApi = new ExchangeRatesApi();
            var jsonSerializerSettings = new JsonSerializerSettings { DateFormatString = "yyyy'-'MM'-'dd" };

            var consoleCommands = new ConsoleCommands();

            // Methods marked with [CommandExecutable] will be added from the given object instance:
            consoleCommands.RegisterAllFromInstance(exchangeRatesApi);

            // Example of manually created commands:
            var help = new Func<string>(() => {
                Console.WriteLine($"Available commands:{Environment.NewLine}");
                return consoleCommands.ToString();
            });
            consoleCommands.Register(new ConsoleCommand(help.Target!, help.Method, "Help",
                "Prints available commands.", new[] { "help", "commands" }));
            
            var clearScreen = new Action(Console.Clear);
            consoleCommands.Register(new ConsoleCommand(clearScreen.Target!, clearScreen.Method, "ClearScreen",
                "Clears console.", new[] { "cls" }));
            
            var quit = new Action(() => Environment.Exit(0));
            consoleCommands.Register(new ConsoleCommand(quit.Target!, quit.Method, "Quit",
                "Quits the program.", new[] { "q" }));
            
            Console.WriteLine("Input \"help\" without quotes to see available commands.");
            
            // Simple REPL:
            while (true) {
                Console.Write("Command prompt: ");
                string commandRepr = Console.ReadLine();
                string evaluated = consoleCommands.Execute(commandRepr).ConvertOrError(jsonSerializerSettings);
                Console.WriteLine(evaluated);
            }
        }
    }
}