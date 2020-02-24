using System;
using System.Collections.Generic;
using LanguageExt;
using LanguageExt.Common;

namespace Compro
{
    /// <summary> Metadata for a command argument. </summary>
    public class CommandParameterInfo : CommandIOPart, ICommandParameterInfo
    {
        /// <summary>
        /// Gets metadata for an argument's default value, if any. References global
        /// <see cref="CommandParameterDefault.None"/> for an argument without default value.
        /// </summary>
        public ICommandParameterDefault Default { get; }

        public CommandParameterInfo(Type type,
                                    CommandParameterDefault defaultValue,
                                    string name = "",
                                    string description = "")
            : base(type, name, description) =>
            Default = defaultValue ?? throw new ArgumentNullException(nameof(defaultValue));

        /// <summary>
        /// Converts given string arguments to their typed version using passed parameter infos as a 'scheme'.
        /// </summary>
        /// <param name="parameterInfos">Parameter infos serving as a 'scheme' for args.</param>
        /// <param name="args">Args to map onto passed parameter infos.</param>
        /// <returns> Array with converted arguments. It is requested from <see cref="ArrayPools{T}"/>,
        /// so do not forget to return it there when you're done with it.</returns>
        /// <exception cref="ArgumentException">Not enough arguments for passed parameter infos.</exception>
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