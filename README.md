# Simple game console back-end

This thing parses a command from user input, which should look like `commandNameOrAlias(param1Json, paramNJson)`.
Then parsed command is executed and either a command result or an exception message is returned.  

Commands are strongly typed with tons of metadata available.
It is possible to either create commands manually or register all instance methods marked with `CommandExecutable` attribute.

```c#
var consoleCommands = new ConsoleCommands();    // Contains commands for execution

var square = new Func<double, double>(x => x * x);  // Manually created command
 // Register created command with name `square`, give it a description and alias `sqr`:
consoleCommands.Register(new ConsoleCommand(square.Target!, square.Method, "square",
    "Returns square of the given number.", new[] { "sqr" }));

// Now try to execute it with various arguments.
// Parameters are parsed into actual typed argument via JSON deserialization.
// So you can easily pass arrays as '["something", "somethingElse"]' and objects as '{"property":"value"}'.
// If any exception happens inside a command method or argument parsing process,
// its message will be returned instead of a command result.
consoleCommands.Execute("sqr(2)").ConvertOrError();         // "4.0"
consoleCommands.Execute("sqr(\"1.25\")").ConvertOrError();  // "1.5625"
consoleCommands.Execute("sqr(\"NaN\")").ConvertOrError();   // "NaN"
consoleCommands.Execute("sqr(\"N\")").ConvertOrError();     // Could not convert string to double: N. Path '', line 1, position 3.

```

For a slightly more complex example involving metadata attributes and not-so-simple command arguments see `ConsoleBackEnd.Demo`.
Here is its `Program.cs` just to entice you:

```c#
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
var consoleCommands = new ConsoleCommands(ConsoleCommandParser.Default, ConsoleCommandParameterConverter.Default);
// Argument JSON handling can be customized by passing 'new ConsoleCommandParameterConverter(yourJsonDeserializerSettings)' here.

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
```