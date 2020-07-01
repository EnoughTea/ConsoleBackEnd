using System;
using System.Collections.Concurrent;

namespace ConsoleBackEnd
{
    internal class ObjectPool<T>
    {
        private readonly Func<T> _objectGenerator;
        private readonly ConcurrentBag<T> _objects;

        public ObjectPool(Func<T> objectGenerator)
        {
            _objects = new ConcurrentBag<T>();
            _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
        }

        public T Request() => _objects.TryTake(out var item) ? item : _objectGenerator();

        public void Return(T item)
        {
            _objects.Add(item);
        }
    }
}