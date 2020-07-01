namespace ConsoleBackEnd
{
    /// <inheritdoc />
    public class CommandParameterDefault : ICommandParameterDefault
    {
        public static CommandParameterDefault None { get; } = new CommandParameterDefault();

        public object? Value { get; }

        public bool HasDefault { get; }

        public CommandParameterDefault(object? defaultValue)
        {
            Value = defaultValue;
            HasDefault = true;
        }

        public CommandParameterDefault() { }
    }
}