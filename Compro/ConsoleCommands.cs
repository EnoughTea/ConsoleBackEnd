using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(nameof(Compro) + ".Tests")]

namespace Compro
{
    public class ConsoleCommands : IConsoleCommands
    {
        private readonly ConcurrentDictionary<string, IConsoleCommand> _commands =
            new ConcurrentDictionary<string, IConsoleCommand>(StringComparer.OrdinalIgnoreCase);

        private readonly IConsoleCommandParser _parser;

        /// <summary> Uses default console command parser. </summary>
        public ConsoleCommands() : this(ConsoleCommandParser.Default) { }

        public ConsoleCommands(IConsoleCommandParser parser)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public ICommandExecuteResult Execute(string commandName, params string[] commandArgs)
        {
            if (_commands.TryGetValue(commandName, out var command)) {
                return command.Execute(commandArgs);
            }

            return new ConsoleCommandExecuteFailure(new ArgumentException($"Command '{commandName}' was not found",
                nameof(commandName)));
        }

        public ICommandExecuteResult Execute(string commandRepr)
        {
            var parseResult = _parser.Parse(commandRepr);
            return parseResult.Get().Match(parsed => Execute(parsed.Name, parsed.Args),
                e => new ConsoleCommandExecuteFailure(e));
        }

        public void Register(IConsoleCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            _commands.AddOrUpdate(command.Name, _ => command, (_, __) => command);
            foreach (string alias in command.Aliases) {
                _commands.AddOrUpdate(alias, _ => command, (_, __) => command);
            }
        }

        public bool Unregister(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            return _commands.TryRemove(name, out _);
        }

        public void UnregisterAll()
        {
            _commands.Clear();
        }

        public void RegisterAllFromInstance(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var commands = GatherFromInstance(instance);
            foreach (var command in commands) {
                Register(command);
            }
        }

        public bool IsRegistered(string commandName)
        {
            if (commandName == null) throw new ArgumentNullException(nameof(commandName));

            return _commands.ContainsKey(commandName);
        }

        /// <inheritdoc />
        public override string ToString() => string.Join(Environment.NewLine + Environment.NewLine, this);

        /// <inheritdoc />
        public IEnumerator<IConsoleCommand> GetEnumerator() =>
            _commands.Values.Distinct().OrderBy(command => command.Name).GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal static ConsoleCommand[] GatherFromInstance(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var commandMethodInfos = instance.GetType().GetMethods().Where(methodInfo =>
                methodInfo.GetCustomAttributes(typeof(CommandExecutableAttribute), false).Length > 0);
            return commandMethodInfos.Select(cmi => new ConsoleCommand(instance, cmi))
                .ToArray();
        }
    }
}