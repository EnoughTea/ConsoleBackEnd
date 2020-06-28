using System;
using Newtonsoft.Json;

namespace Compro.Demo
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
            var list = new Func<string>(() => consoleCommands.ToString());
            consoleCommands.Register(new ConsoleCommand(list.Target!, list.Method, "List",
                "Shows a list of available commands.", new[] { "list", "help" }));
            var quit = new Action(() => Environment.Exit(0));
            consoleCommands.Register(new ConsoleCommand(quit.Target!, quit.Method, "Quit",
                "Quits the program.", new[] { "q" }));

            // Simple REPL:
            while (true) {
                string commandRepr = Console.ReadLine();
                string evaluated = consoleCommands.Execute(commandRepr).ConvertOrError(jsonSerializerSettings);
                Console.WriteLine(evaluated);
            }
        }
    }
}