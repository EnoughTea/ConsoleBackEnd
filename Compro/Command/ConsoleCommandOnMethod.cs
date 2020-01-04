using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LanguageExt;
using LanguageExt.Common;

namespace Compro
{
    public class ConsoleCommandOnMethod : IConsoleCommand
    {
        public object ExecutedMethodInstance { get; }

        public MethodInfo ExecutedMethodInfo { get; }

        public ConsoleCommandOnMethod(object executedMethodInstance,
                                       MethodInfo executedMethodInfo,
                                       CommandIOPartConverters converters)
        {
            if (converters == null) throw new ArgumentNullException(nameof(converters));

            ExecutedMethodInstance = executedMethodInstance ??
                throw new ArgumentNullException(nameof(executedMethodInstance));
            ExecutedMethodInfo = executedMethodInfo ?? throw new ArgumentNullException(nameof(executedMethodInfo));
            Name = executedMethodInfo.Name;
            Description = executedMethodInfo.GetCustomAttribute<CommandExecutableAttribute>()?.Description ?? "";
            Aliases = executedMethodInfo.GetCustomAttributes<CommandAliasAttribute>()
                .Select(a => a.Alias)
                .ToList()
                .AsReadOnly();
            Result = ExtractResult(executedMethodInfo, converters);
            Parameters = ExtractParameters(executedMethodInfo, converters);
        }

        public string Name { get; }
        
        public IReadOnlyList<string> Aliases { get; }

        public IReadOnlyList<CommandParameterInfo> Parameters { get; }

        public CommandReturnInfo Result { get; }

        public string Description { get; }

        public ICommandCallResult Call(params string[] args)
        {
            var result = from convertedArgs in CommandParameterInfo.ConvertArgs(Parameters, args)
                         from returned in Execute(convertedArgs)
                         select returned;
            return result.Try().Match<ICommandCallResult>(
                returned => Result.HasValue
                    ? new CommandCallSuccess(returned, Result.Converter)
                    : CommandCallSuccess.Void,
                e => new CommandCallFailure(e));
        }

        /// <inheritdoc />
        public bool CanBeCalledBy(string name)
        {
            return string.Equals(Name, name, StringComparison.OrdinalIgnoreCase) ||
                Aliases.Exists(alias => string.Equals(alias, name, StringComparison.OrdinalIgnoreCase));
        }

        public static ConsoleCommandOnMethod[] GatherFromInstance(object instance, CommandIOPartConverters converters)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (converters == null) throw new ArgumentNullException(nameof(converters));

            var commandMethodInfos = instance.GetType().GetMethods().Where(methodInfo =>
                methodInfo.GetCustomAttributes(typeof(CommandExecutableAttribute), false).Length > 0);
            return commandMethodInfos.Select(cmi => new ConsoleCommandOnMethod(instance, cmi, converters))
                .ToArray();
        }

        private Try<object> Execute(object[] convertedArgs)
        {
            Result<object> Try()
            {
                var result = ExecutedMethodInfo.Invoke(ExecutedMethodInstance, convertedArgs);
                ArrayPools<object>.Return(convertedArgs);
                return result;
            }

            return Try;
        }

        private static IReadOnlyList<CommandParameterInfo> ExtractParameters(MethodInfo executedMethodInfo,
                                                                             CommandIOPartConverters converters)
        {
            var parameterInfos = executedMethodInfo.GetParameters();
            var commandParameters = new List<CommandParameterInfo>(parameterInfos.Length);
            commandParameters.AddRange(from info in parameterInfos select ToCommandParameterInfo(info, converters));
            return commandParameters.AsReadOnly();
        }

        private static CommandParameterInfo ToCommandParameterInfo(ParameterInfo parameterInfo,
                                                                   CommandIOPartConverters converters)
        {
            var parameterDoc = parameterInfo.GetCustomAttribute<CommandDocAttribute>();
            var converter = converters.Get(parameterInfo.ParameterType);
            var defaultInfo = parameterInfo.HasDefaultValue
                ? new CommandParameterDefault(parameterInfo.DefaultValue)
                : CommandParameterDefault.None;
            return new CommandParameterInfo(parameterInfo.ParameterType, defaultInfo, converter,
                parameterInfo.Name, parameterDoc?.Description ?? "");
        }

        private static CommandReturnInfo ExtractResult(MethodInfo executedMethodInfo,
                                                       CommandIOPartConverters converters)
        {
            var returnDocAttribute = executedMethodInfo.ReturnTypeCustomAttributes
                .GetCustomAttributes(typeof(CommandDocAttribute), false)
                .FirstOrDefault() as CommandDocAttribute;
            var converter = converters.Get(executedMethodInfo.ReturnType);
            return new CommandReturnInfo(executedMethodInfo.ReturnType, converter, "returns",
                returnDocAttribute?.Description ?? "");
        }
    }
}