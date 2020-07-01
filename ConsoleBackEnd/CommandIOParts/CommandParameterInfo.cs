using System;
using System.Collections.Generic;
using ConsoleBackEnd.Extensions;
using LanguageExt;
using LanguageExt.Common;
using static ConsoleBackEnd.TryHelper;

namespace ConsoleBackEnd
{
    /// <summary> Metadata for a command argument. </summary>
    public class CommandParameterInfo : CommandIOPart, ICommandParameterInfo
    {
        /// <summary>
        ///     Gets metadata for an argument's default value, if any. References global
        ///     <see cref="CommandParameterDefault.None" /> for an argument without default value.
        /// </summary>
        public ICommandParameterDefault Default { get; }

        /// <inheritdoc />
        public bool IsOptional { get; }

        public CommandParameterInfo(Type type,
                                    CommandParameterDefault defaultValue,
                                    string name = "",
                                    string description = "",
                                    bool isOptional = false)
            : base(type, name, description)
        {
            Default = defaultValue ?? throw new ArgumentNullException(nameof(defaultValue));
            IsOptional = isOptional;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string optionalPart = IsOptional ? " [Optional]" : "";
            return string.IsNullOrWhiteSpace(Description)
                ? $"{Name}: {Type.UnwrappedName()}{optionalPart}"
                : $"{Name}: {Type.UnwrappedName()}{optionalPart} — {Description}";
        }

        /// <summary>
        ///     Converts given string arguments to their typed version using passed parameter infos as a 'scheme'.
        /// </summary>
        /// <param name="parameterInfos">Parameter infos serving as a 'scheme' for args.</param>
        /// <param name="args">Args to map onto passed parameter infos.</param>
        /// <param name="parameterConverter">
        ///     Parameter converter used to convert string representation
        ///     of a parameter into actual parameter.
        /// </param>
        /// <returns>
        ///     Array with converted arguments. It is requested from <see cref="ArrayPools{T}" />,
        ///     so do not forget to return it there when you're done with it.
        /// </returns>
        /// <exception cref="ArgumentException">Not enough arguments for passed parameter infos.</exception>
        internal static Try<object?[]> ConvertArgs(IReadOnlyList<CommandParameterInfo> parameterInfos,
                                                   string[] args,
                                                   ICommandParameterConverter parameterConverter)
        {
            if (args.Length > parameterInfos.Count) {
                return Fail<object?[]>(new ArgumentException($"Passed {args.Length} argument(s), but there are " +
                    $"${parameterInfos.Count} command parameters."));
            }

            var convertedArgs = ArrayPools<object?>.Request(parameterInfos.Count);
            for (int index = 0; index < parameterInfos.Count; index++) {
                var currentParamInfo = parameterInfos[index];
                if (args.Length > index) {
                    string arg = args[index];
                    var conversionResult = parameterConverter.Convert(currentParamInfo, arg);
                    Exception? failure = null;
                    conversionResult.Match(convertedArg => convertedArgs[index] = convertedArg,
                        e => failure = e);
                    if (failure != null) return Fail<object?[]>(failure);
                } else {
                    if (currentParamInfo.Default.HasDefault) {
                        convertedArgs[index] = currentParamInfo.Default.Value;
                    } else {
                        return Fail<object?[]>(new ArgumentException(
                            $"Passed {args.Length} argument(s), but it was not enough."));
                    }
                }
            }

            Result<object?[]> InnerTry() => convertedArgs;
            return InnerTry;
        }
    }
}