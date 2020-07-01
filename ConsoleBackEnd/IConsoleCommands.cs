using System.Collections.Generic;

namespace ConsoleBackEnd
{
    /// <summary> Interface for a console part responsible for registering and execution of commands. </summary>
    public interface IConsoleCommands : IEnumerable<IConsoleCommand>
    {
        /// <summary>
        ///     Find registered command with the given name and executes it with the specified parameters.
        /// </summary>
        /// <param name="commandName">Command name.</param>
        /// <param name="commandArgs">Command parameters.</param>
        /// <returns>Execution result.</returns>
        ICommandExecuteResult Execute(string commandName, params string[] commandArgs);

        /// <summary>
        ///     Parses given command representation into command name and params, then executes found command.
        /// </summary>
        /// <param name="commandRepr">
        ///     Command representation, depending on the parser used.
        ///     Default parser expects string like <code>IConsoleCommands.Execute("command(1, \"test\")")</code>
        /// </param>
        /// <returns>Execution result.</returns>
        ICommandExecuteResult Execute(string commandRepr);

        /// <summary> Registers the specified console command. </summary>
        /// <param name="command">Command to register.</param>
        void Register(IConsoleCommand command);

        /// <summary>
        ///     Finds all methods with <see cref="CommandExecutableAttribute" /> on the given instance,
        ///     and registers them as commands.
        /// </summary>
        /// <param name="instance">Instance whose methods will be used.</param>
        void RegisterAllFromInstance(object instance);

        bool Unregister(string name);

        void UnregisterAll();

        /// <summary> Checks if a command with the given name was already registered. </summary>
        /// <param name="commandName">Command name to check.</param>
        /// <returns>
        ///     <c>true</c> if command with the given name was already registered;
        ///     <c>false</c> otherwise.
        /// </returns>
        bool IsRegistered(string commandName);
    }
}