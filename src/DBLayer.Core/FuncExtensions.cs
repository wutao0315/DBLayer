namespace DBLayer.Core
{
    public static class FuncExtensions
    {
        public static bool In<T>(this T obj, T[] array)
        {
            return true;
        }
        public static bool NotIn<T>(this T obj, T[] array)
        {
            return true;
        }
        public static bool InFunc<T>(this T obj, T[] array)
        {
            return true;
        }
        public static bool NotInFunc<T>(this T obj, T[] array)
        {
            return true;
        }
        public static bool Like(this string str, string likeStr)
        {
            return true;
        }
        public static bool NotLike(this string str, string likeStr)
        {
            return true;
        }
        public static bool Less(this string str, string less)
        {
            return true;
        }
        public static bool LessEqual(this string str, string lessEqual)
        {
            return true;
        }
        public static bool Greater(this string str, string greater)
        {
            return true;
        }
        public static bool GreaterEqual(this string str, string greaterEqual)
        {
            return true;
        }
    }
}
