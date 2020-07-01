namespace ConsoleBackEnd
{
    /// <summary> Metadata for a command argument. </summary>
    public interface ICommandParameterInfo : ICommandIOPart
    {
        /// <summary>
        ///     Gets metadata for an argument's default value, if any.
        ///     Use some kind of global 'None' value for arguments without default value.
        /// </summary>
        ICommandParameterDefault Default { get; }

        /// <summary>
        ///     <c>true</c> if parameter is optional; <c>false</c> otherwise.
        /// </summary>
        bool IsOptional { get; }
    }
}