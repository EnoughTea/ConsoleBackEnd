namespace ConsoleBackEnd
{
    internal static class ArrayPools<T>
    {
        private static readonly ArrayPool<T> _Pool = new ArrayPool<T>();

        public static T[] Request(int length) => _Pool.Request(length);

        public static void Return(T[] array) => _Pool.Return(array);
    }
}