namespace ENA
{
    public static partial class StringExtensions
    {
        public static float AsFloat(this string self)
        {
            return float.Parse(self);
        }

        public static string RemoveBlankChars(this string self)
        {
            string result = self.Replace(" ", string.Empty);
            result = result.Replace("\n", string.Empty);
            result = result.Replace("\r", string.Empty);
            return result;
        }
    }
}