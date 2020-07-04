using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleBackEnd
{
    /// <summary>
    /// Parses given user input containing a command like "CommandName(param1Json, paramNJson)"
    /// into a command name with array of args.
    /// </summary>
    public class ConsoleCommandParser : IConsoleCommandParser
    {
        public static ConsoleCommandParser Default { get; } = new ConsoleCommandParser();

        public IConsoleCommandParseResult Parse(string commandRepr)
        {
            var failure = InitialValidation(commandRepr, out string trimmed);
            if (failure != null) {
                return failure;
            }

            var failurOrSuccess = ParameterListValidation(trimmed, out int parametersStart);
            if (failurOrSuccess != null) {
                return failurOrSuccess;
            }

            (string commandName, string parametersPart) = ExtractNameAndParams(trimmed, parametersStart);
            if (string.IsNullOrWhiteSpace(parametersPart)) {
                return new ConsoleCommandParseSuccess(commandName);
            }

            JArray jsonArgs;
            try {
                jsonArgs = JArray.Parse("[" + parametersPart + "]");
            } catch (Exception e) {
                return new ConsoleCommandParseFailure(e);
            }
            
            var args = jsonArgs.Select(element => element.ToString(Formatting.None)).ToArray();
            return new ConsoleCommandParseSuccess(commandName, args);
        }

        private static (string CommandName, string ParametersPart) ExtractNameAndParams(string trimmed,
                                                                                        int parametersStart)
        {
            string commandName = trimmed.Substring(0, parametersStart).Trim();
            int parametersEnd = trimmed.Length - 1;
            string parametersPart = trimmed
                .Substring(parametersStart + 1, parametersEnd - parametersStart - 1)
                .Trim();
            return (commandName, parametersPart);
        }

        private static ConsoleCommandParseFailure? InitialValidation(string commandRepr, out string trimmed)
        {
            if (string.IsNullOrWhiteSpace(commandRepr)) {
                trimmed = "";
                return ConsoleCommandParseFailure.Fail("Input is empty.");
            }

            trimmed = commandRepr.Trim();
            return trimmed.Length == 0 ? ConsoleCommandParseFailure.Fail("Input is whitespace.") : null;
        }

        private static IConsoleCommandParseResult? ParameterListValidation(string commandRepr, out int parametersStart)
        {
            parametersStart = commandRepr.IndexOf("(", StringComparison.Ordinal);
            if (parametersStart < 0) {
                return new ConsoleCommandParseSuccess(commandRepr);
            }

            if (parametersStart == 0) {
                return ConsoleCommandParseFailure.Fail("No command name is present.");
            }

            return !commandRepr.EndsWith(")")
                ? ConsoleCommandParseFailure.Fail("Command with params should end with ')'.")
                : null;
        }
    }
}