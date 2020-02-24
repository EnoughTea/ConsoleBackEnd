using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LanguageExt;
using LanguageExt.Common;

namespace Compro
{
    public class ConsoleCommand : IConsoleCommand
    {
        public object? ExecutedMethodInstance { get; }

        public MethodInfo ExecutedMethodInfo { get; }

        public string Name { get; }

        public IReadOnlyList<string> Aliases { get; }

        public IReadOnlyList<CommandParameterInfo> Parameters { get; }

        public ICommandReturnInfo Result { get; }

        public string Description { get; }

        public ConsoleCommand(object executedMethodInstance,
                              MethodInfo executedMethodInfo,
                              string nameOverride = "")
        {
            ExecutedMethodInstance = executedMethodInstance;
            ExecutedMethodInfo = executedMethodInfo ?? throw new ArgumentNullException(nameof(executedMethodInfo));
            Name = string.IsNullOrWhiteSpace(nameOverride) ? executedMethodInfo.Name : nameOverride;
            Description = executedMethodInfo.GetCustomAttribute<CommandExecutableAttribute>()?.Description ?? "";
            Aliases = executedMethodInfo.GetCustomAttributes<CommandAliasAttribute>()
                .Select(a => a.Alias)
                .ToList()
                .AsReadOnly();
            Result = ExtractResult(executedMethodInfo);
            Parameters = ExtractParameters(executedMethodInfo);
        }

        public ICommandExecuteResult Execute(params string[] args)
        {
            var result = from convertedArgs in CommandParameterInfo.ConvertArgs(Parameters, args)
                         from returned in Execute(convertedArgs)
                         select returned;
            return result.Try().Match<ICommandExecuteResult>(
                returned => Result.HasValue
                    ? new ConsoleCommandExecuteSuccess(returned)
                    : ConsoleCommandExecuteSuccess.Void,
                e => new ConsoleCommandExecuteFailure(e));
        }

        /// <inheritdoc />
        public bool CanBeCalledBy(string name)
        {
            return string.Equals(Name, name, StringComparison.OrdinalIgnoreCase) ||
                Aliases.Exists(alias => string.Equals(alias, name, StringComparison.OrdinalIgnoreCase));
        }

        private Try<object?> Execute(object[] convertedArgs)
        {
            Result<object?> Try()
            {
                var result = ExecutedMethodInfo.Invoke(ExecutedMethodInstance, convertedArgs);
                ArrayPools<object?>.Return(convertedArgs);
                return result;
            }

            return Try;
        }

        private static IReadOnlyList<CommandParameterInfo> ExtractParameters(MethodInfo executedMethodInfo)
        {
            var parameterInfos = executedMethodInfo.GetParameters();
            var commandParameters = new List<CommandParameterInfo>(parameterInfos.Length);
            commandParameters.AddRange(from info in parameterInfos select ToCommandParameterInfo(info));
            return commandParameters.AsReadOnly();
        }

        private static CommandParameterInfo ToCommandParameterInfo(ParameterInfo parameterInfo)
        {
            var parameterDoc = parameterInfo.GetCustomAttribute<CommandDocAttribute>();
            var defaultInfo = parameterInfo.HasDefaultValue
                ? new CommandParameterDefault(parameterInfo.DefaultValue)
                : CommandParameterDefault.None;
            return new CommandParameterInfo(parameterInfo.ParameterType, defaultInfo, parameterInfo.Name ?? "<unknown>",
                parameterDoc?.Description ?? "");
        }

        private static CommandReturnInfo ExtractResult(MethodInfo executedMethodInfo)
        {
            var returnDocAttribute = executedMethodInfo.ReturnTypeCustomAttributes
                .GetCustomAttributes(typeof(CommandDocAttribute), false)
                .FirstOrDefault() as CommandDocAttribute;
            return new CommandReturnInfo(executedMethodInfo.ReturnType, "returns",
                returnDocAttribute?.Description ?? "");
        }
    }
}