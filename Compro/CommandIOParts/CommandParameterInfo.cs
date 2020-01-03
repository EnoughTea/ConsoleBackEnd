using System;
using System.Collections.Generic;
using LanguageExt;

namespace Compro
{
    public class CommandParameterInfo : CommandIOPart
    {
        public CommandParameterDefault Default { get; }

        public CommandParameterInfo(Type type,
                                    CommandParameterDefault defaultValue,
                                    ICommandPieceConverter converter,
                                    string name = "",
                                    string description = "")
            : base(type, converter, name, description) =>
            Default = defaultValue ?? throw new ArgumentNullException(nameof(defaultValue));

        public static Try<object[]> ConvertArgs(IReadOnlyList<CommandParameterInfo> parameterInfos,
                                                string[] args) =>
            () => {
                if (args.Length > parameterInfos.Count) {
                    throw new ArgumentException($"Passed {args.Length} argument(s), but there are " +
                        $"${parameterInfos.Count} command parameters.");
                }

                var convertedArgs = new object[parameterInfos.Count];
                for (int index = 0; index < parameterInfos.Count; index++) {
                    var currentParamInfo = parameterInfos[index];
                    if (args.Length > index) {
                        string arg = args[index];
                        ConvertAndSetArg(currentParamInfo, arg, convertedArgs, index);
                    } else {
                        if (currentParamInfo.Default.HasDefault) {
                            convertedArgs[index] = currentParamInfo.Default.Value;
                        } else {
                            throw new ArgumentException($"Passed {args.Length} argument(s), but it was not enough.");
                        }
                    }
                }

                return convertedArgs;
            };

        private static void ConvertAndSetArg(ICommandIOPart currentParamInfo,
                                             string argRepr,
                                             object[] convertedArgs,
                                             int index)
        {
            var conversionResult = currentParamInfo.Converter.ConvertFromString(argRepr, currentParamInfo.Type).Try();
            conversionResult.Match(r => convertedArgs[index] = r, e => throw new ArgumentException(e.Message, e));
        }
    }
}