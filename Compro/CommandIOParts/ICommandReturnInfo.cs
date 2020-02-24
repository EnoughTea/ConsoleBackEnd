namespace Compro
{
    /// <summary>
    /// Metadata for a command return value.
    /// </summary>
    public interface ICommandReturnInfo : ICommandIOPart
    {
        /// <summary>
        /// Gets the value indicating whether the command returns something or not (returns void).
        /// </summary>
        bool HasValue { get; }
    }
}