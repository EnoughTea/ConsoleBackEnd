using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Compro
{
    public class CommandIOPartConverters
    {
        public static CommandIOPartConverters Shared { get; } = new CommandIOPartConverters(); 
        
        private static readonly JsonConverter _JsonConverter =
            new JsonConverter();

        private readonly ConcurrentDictionary<Type, ICommandIOPartConverter> _converters =
            new ConcurrentDictionary<Type, ICommandIOPartConverter>();

        public void Register(Type targetType, ICommandIOPartConverter converter)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));
            if (converter == null) throw new ArgumentNullException(nameof(converter));

            _converters.AddOrUpdate(targetType, _ => converter, (_, __) => converter);
        }

        public void Unregister(Type targetType)
        {
            _converters.TryRemove(targetType, out _);
        }

        public void Clear()
        {
            _converters.Clear();
        }

        public ICommandIOPartConverter Get(Type targetType)
        {
            if (!_converters.TryGetValue(targetType, out var converter)) {
                converter = _JsonConverter;
            }

            return converter;
        }
    }
}