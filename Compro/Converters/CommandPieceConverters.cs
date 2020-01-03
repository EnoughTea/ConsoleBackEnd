using System;
using System.Collections.Generic;

namespace Compro
{
    public class CommandPieceConverters
    {
        private static readonly JsonCommandPieceConverter _JsonConverter =
            new JsonCommandPieceConverter();

        private readonly IDictionary<Type, ICommandPieceConverter> _converters =
            new Dictionary<Type, ICommandPieceConverter>();

        public void Register(Type targetType, ICommandPieceConverter converter)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));
            if (converter == null) throw new ArgumentNullException(nameof(converter));

            _converters[targetType] = converter;
        }

        public void Unregister(Type targetType)
        {
            _converters.Remove(targetType);
        }

        public void Clear()
        {
            _converters.Clear();
        }

        public ICommandPieceConverter Get(Type targetType)
        {
            if (!_converters.TryGetValue(targetType, out var converter)) {
                converter = GetDefaultConverter(targetType);
            }

            return converter;
        }

        public static ICommandPieceConverter GetDefaultConverter(Type targetType) => _JsonConverter;
    }
}