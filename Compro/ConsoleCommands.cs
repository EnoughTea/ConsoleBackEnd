using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Compro
{
    public class ConsoleCommands
    {
        private readonly ConcurrentDictionary<string, IConsoleCommand> _commands =
            new ConcurrentDictionary<string, IConsoleCommand>(StringComparer.OrdinalIgnoreCase);

        private readonly ConsoleCommandParser _parser = new ConsoleCommandParser();

        public ICommandExecuteResult Execute(string commandName, params string[] commandArgs)
        {
            if (_commands.TryGetValue(commandName, out var command)) {
                return command.Execute(commandArgs);
            }

            return new CommandExecuteFailure(new ArgumentException($"Command '{commandName}' was not found",
                nameof(commandName)));
        }

        public ICommandExecuteResult Execute(string commandRepr)
        {
            var parseResult = _parser.Parse(commandRepr);
            return parseResult.Get().Match(parsed => Execute(parsed.Name, parsed.Args),
                e => new CommandExecuteFailure(e));
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

        public void Unregister(IEnumerable<string> names)
        {
            foreach (var name in names) {
                Unregister(name);
            }
        }
        
        public void RegisterCommandsFromInstance(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            foreach (var command in GatherFromInstance(instance)) {
                Register(command);
            }
        }

        internal static ConsoleCommandOnMethod[] GatherFromInstance(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var commandMethodInfos = instance.GetType().GetMethods().Where(methodInfo =>
                methodInfo.GetCustomAttributes(typeof(CommandExecutableAttribute), false).Length > 0);
            return commandMethodInfos.Select(cmi => new ConsoleCommandOnMethod(instance, cmi))
                .ToArray();
        }
    }
}