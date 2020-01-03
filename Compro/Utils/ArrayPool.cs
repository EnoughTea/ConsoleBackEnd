using System;
using System.Collections.Generic;

namespace Compro
{
    internal class ArrayPool<T>
    {
        private readonly Dictionary<int, ObjectPool<T[]>> _objects;

        public ArrayPool() => _objects = new Dictionary<int, ObjectPool<T[]>>();

        public T[] Request(int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));

            // ReSharper disable once InconsistentlySynchronizedField
            return _objects.TryGetValue(length, out var arrayPool) ? arrayPool.Request() : new T[length];
        }

        public void Return(T[] list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            Array.Clear(list, 0, list.Length);

            lock (_objects) {
                if (_objects.TryGetValue(list.Length, out var arrayPool)) {
                    arrayPool.Return(list);
                } else {
                    var pool = new ObjectPool<T[]>(() => new T[list.Length]);
                    pool.Return(list);
                    _objects[list.Length] = pool;
                }
            }
        }
    }
}