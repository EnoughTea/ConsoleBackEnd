using System;
using LanguageExt;
using Newtonsoft.Json;

namespace ConsoleBackEnd
{
    /// <summary> Converts command parameters using JSON. </summary>
    public class CommandParameterConverter : ICommandParameterConverter
    {
        private readonly JsonSerializerSettings? _serializerSettings;
        public static CommandParameterConverter Default { get; } = new CommandParameterConverter();

        public CommandParameterConverter() { }

        public CommandParameterConverter(JsonSerializerSettings serializerSettings)
            : this() =>
            _serializerSettings = serializerSettings ?? throw new ArgumentNullException(nameof(serializerSettings));

        /// <inheritdoc />
        public Try<object?> Convert(ICommandIOPart parameterInfo, string parameter) =>
            JsonConverter.FromString(parameter, parameterInfo.Type, _serializerSettings);
    }
}