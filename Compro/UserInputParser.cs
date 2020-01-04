
using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Compro
{
    public interface IUserInputParseResult
    {
        public string CommandName { get; }
        
        public string[] CommandArgs { get; }

        Exception Exception { get; }

        bool IsSuccess { get; }
    }

    public class UserInputParseSuccess : IUserInputParseResult
    {
        internal static readonly string[] ZeroArgs = new string[0];
        
        /// <inheritdoc />
        public string CommandName { get; }

        /// <inheritdoc />
        public string[] CommandArgs { get; }

        /// <inheritdoc />
        public Exception Exception { get; private set; }

        /// <inheritdoc />
        public bool IsSuccess => true;

        public UserInputParseSuccess(string commandName, string[] commandArgs)
        {
            CommandName = commandName ?? throw new ArgumentNullException(nameof(commandName));
            CommandArgs = commandArgs ?? throw new ArgumentNullException(nameof(commandArgs));
        }

        public UserInputParseSuccess(string commandName) : this(commandName, ZeroArgs) { }
    }

    public class UserInputParseFailure : IUserInputParseResult
    {
        /// <inheritdoc />
        public string CommandName { get; }

        /// <inheritdoc />
        public string[] CommandArgs { get; }

        /// <inheritdoc />
        public Exception Exception { get; }

        /// <inheritdoc />
        public bool IsSuccess { get; }

        public UserInputParseFailure(Exception e)
        {
            Exception = e ?? throw new ArgumentNullException(nameof(e));
            CommandName = "";
            CommandArgs = UserInputParseSuccess.ZeroArgs;
        }

        public static UserInputParseFailure Fail(string message = "") =>
            new UserInputParseFailure(new ArgumentException(message));
    }
    
    public class UserInputParser
    {
        public IUserInputParseResult Parse(string commandRepr)
        {
            var failure = InitialValidation(commandRepr, out string trimmed);
            if (failure != null) {
                return failure;
            }

            var result = ParameterListValidation(trimmed, out int parametersStart);
            if (result != null) {
                return result;
            }
            
            var (commandName, parametersPart) = ExtractNameAndParams(trimmed, parametersStart);
            if (string.IsNullOrWhiteSpace(parametersPart)) {
                return new UserInputParseSuccess(commandName);
            }

            var jsonArgs = JArray.Parse("[" + parametersPart + "]");
            string[] args = jsonArgs.Select(element => element.ToString(Formatting.None)).ToArray();
            return new UserInputParseSuccess(commandName, args);
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

        private UserInputParseFailure InitialValidation(string commandRepr, out string trimmed)
        {
            if (string.IsNullOrWhiteSpace(commandRepr)) {
                trimmed = "";
                return UserInputParseFailure.Fail("Input is empty.");
            }
            
            trimmed = commandRepr.Trim();
            return trimmed.Length == 0 ? UserInputParseFailure.Fail("Input is whitespace.") : null;
        }

        private IUserInputParseResult ParameterListValidation(string commandRepr, out int parametersStart)
        {
            parametersStart = commandRepr.IndexOf("(", StringComparison.Ordinal);
            if (parametersStart < 0) {
                return new UserInputParseSuccess(commandRepr);
            }

            if (parametersStart == 0) {
                return UserInputParseFailure.Fail("No command name is present.");
            }

            return !commandRepr.EndsWith(")")
                ? UserInputParseFailure.Fail("Command with params should end with ')'.")
                : null;
        }
    }
}