using LanguageExt;
using Newtonsoft.Json;

namespace ConsoleBackEnd 
{
    public class CommandReturnedObjectJsonConverter : ICommandReturnedObjectConverter
    {
        public static CommandReturnedObjectJsonConverter Default { get; } = new CommandReturnedObjectJsonConverter();

        /// <summary> If true, returned string will be unescaped. </summary>
        public bool Unescape { get; }

        public Formatting Formatting { get; }

        public JsonSerializerSettings? SerializerSettings { get; }

        public CommandReturnedObjectJsonConverter(bool unescape = true,
                                                 Formatting formatting = Formatting.Indented,
                                                 JsonSerializerSettings? serializerSettings = null)
        {
            Unescape = unescape;
            Formatting = formatting;
            SerializerSettings = serializerSettings;
        }

        /// <inheritdoc />
        public Try<string> Convert(object? commandReturnedObject) =>
            JsonConverter.ToString(commandReturnedObject, Unescape, Formatting, SerializerSettings);
    }
}