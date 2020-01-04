using System;
using System.Collections.Generic;
using LanguageExt;
using LanguageExt.Common;

namespace Compro
{
    public class CommandParameterInfo : CommandIOPart
    {
        public CommandParameterDefault Default { get; }

        public CommandParameterInfo(Type type,
                                    CommandParameterDefault defaultValue,
                                    string name = "",
                                    string description = "")
            : base(type, name, description) =>
            Default = defaultValue ?? throw new ArgumentNullException(nameof(defaultValue));

        internal static Try<object?[]> ConvertArgs(IReadOnlyList<CommandParameterInfo> parameterInfos,
                                                   string[] args)
        {
            Result<object?[]> InnerTry()
            {
                if (args.Length > parameterInfos.Count) {
                    throw new ArgumentException($"Passed {args.Length} argument(s), but there are " +
                        $"${parameterInfos.Count} command parameters.");
                }

                var convertedArgs = ArrayPools<object?>.Request(parameterInfos.Count);
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
            }

            return InnerTry;
        }

        private static void ConvertAndSetArg(ICommandIOPart currentParamInfo,
                                             string argRepr,
                                             object?[] convertedArgs,
                                             int index)
        {
            var conversionResult = JsonConverter.FromString(argRepr, currentParamInfo.Type).Try();
            if (conversionResult.IsSuccess) {
                convertedArgs[index] = conversionResult.IfFail(null);
            } else {
                conversionResult.IfFail(e => throw new ArgumentException(e.Message, e));
            }
        }
    }
}