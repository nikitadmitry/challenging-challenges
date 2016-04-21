// ReSharper disable once CheckNamespace
namespace System
{
    public static class RoundHelper
    {
        public static decimal Round(this decimal number)
        {
            return Math.Round(number, MidpointRounding.AwayFromZero);
        }

        public static double Round(this double number)
        {
            return Math.Round(number, MidpointRounding.AwayFromZero);
        }

        public static int RoundToInt(this decimal number)
        {
            return Convert.ToInt32(Math.Round(number, MidpointRounding.AwayFromZero));
        }

        public static double Round(this double number, int numberOfSignsAfterZero)
        {
            return Math.Round(number, numberOfSignsAfterZero);
        }

        public static decimal Round(this decimal number, int numberOfSignsAfterZero)
        {
            return Math.Round(number, numberOfSignsAfterZero);
        }

        public static int RoundToInt(this decimal? number)
        {
            return number == null ? 0 : Convert.ToInt32(Math.Round(number.Value, MidpointRounding.AwayFromZero));
        }
    }
}