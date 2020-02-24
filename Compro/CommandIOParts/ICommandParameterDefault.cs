namespace Compro
{
    /// <summary> Metadata for a default value of an argument. </summary>
    public interface ICommandParameterDefault
    {
        /// <summary> Gets a default value for the argument.
        /// Always null when argument does not have a default value. </summary>
        public object? Value { get; }

        /// <summary> Gets a value indicating whether the argument has a default value or not. </summary>
        public bool HasDefault { get; }
    }
}