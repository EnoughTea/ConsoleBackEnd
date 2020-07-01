namespace ConsoleBackEnd
{
    public interface IParsedConsoleCommand
    {
        string Name { get; }

        string[] Args { get; }
    }
}