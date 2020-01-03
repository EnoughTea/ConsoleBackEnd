using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LanguageExt;

namespace Compro
{
    public class TerminalCommandOnMethod : ITerminalCommand
    {
        public object ExecutedMethodInstance { get; }

        public MethodInfo ExecutedMethodInfo { get; }

        public TerminalCommandOnMethod(object executedMethodInstance, MethodInfo executedMethodInfo)
        {
            ExecutedMethodInstance = executedMethodInstance ??
                throw new ArgumentNullException(nameof(executedMethodInstance));
            ExecutedMethodInfo = executedMethodInfo ?? throw new ArgumentNullException(nameof(executedMethodInfo));
            Name = executedMethodInfo.Name;
            Description = executedMethodInfo.GetCustomAttribute<CommandExecutableAttribute>()?.Description ?? "";
            Result = ExtractResult(executedMethodInfo);
            Parameters = ExtractParameters(executedMethodInfo);
        }

        public string Name { get; }

        public IReadOnlyList<CommandParameterInfo> Parameters { get; }

        public CommandReturnInfo Result { get; }

        public string Description { get; }

        public CommandCallResult Call(params string[] args)
        {
            var result = from convertedArgs in CommandParameterInfo.ConvertArgs(Parameters, args)
                         from returned in Execute(convertedArgs)
                         from returnedConverted in ConvertIfNotVoid(returned)
                         select returnedConverted;
            return result.Try().Match(returned =>
                    Result.HasValue ? new CommandCallResult(returned) : CommandCallResult.Void,
                e => new CommandCallResult(e));
        }

        private Try<string> ConvertIfNotVoid(object returned) =>
            Result.HasValue ? Result.Converter.ConvertToString(returned) : () => "";

        public static TerminalCommandOnMethod[] GatherFromInstance<T>(T instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var commandMethodInfos = instance.GetType().GetMethods().Where(methodInfo =>
                methodInfo.GetCustomAttributes(typeof(CommandExecutableAttribute), false).Length > 0);
            return commandMethodInfos.Select(cmi => new TerminalCommandOnMethod(instance, cmi))
                .ToArray();
        }

        private Try<object> Execute(object[] convertedArgs) =>
            () => ExecutedMethodInfo.Invoke(ExecutedMethodInstance, convertedArgs);

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
            var converter = CommandPieceConverters.GetDefaultConverter(parameterInfo.ParameterType);
            var defaultInfo = parameterInfo.HasDefaultValue
                ? new CommandParameterDefault(parameterInfo.DefaultValue)
                : CommandParameterDefault.None;
            return new CommandParameterInfo(parameterInfo.ParameterType, defaultInfo, converter,
                parameterInfo.Name, parameterDoc?.Description ?? "");
        }

        private static CommandReturnInfo ExtractResult(MethodInfo executedMethodInfo)
        {
            var returnDocAttribute = executedMethodInfo.ReturnTypeCustomAttributes
                .GetCustomAttributes(typeof(CommandDocAttribute), false)
                .FirstOrDefault() as CommandDocAttribute;
            var converter = CommandPieceConverters.GetDefaultConverter(executedMethodInfo.ReturnType);
            return new CommandReturnInfo(executedMethodInfo.ReturnType, converter, "returns",
                returnDocAttribute?.Description ?? "");
        }
    }
}