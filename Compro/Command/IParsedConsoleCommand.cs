namespace Compro
{
    public interface IParsedConsoleCommand
    {
        string Name { get; }

        string[] Args { get; }
    }
}