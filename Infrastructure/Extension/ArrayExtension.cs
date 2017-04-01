namespace Infrastructure.Extensions
{
    public static class ArrayExtension
    {
        public static T Last<T>(this T[] array)
        {
            var length = array.Length;
            return array[length - 1];
        }
    }
}
