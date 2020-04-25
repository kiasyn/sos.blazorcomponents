using System;

namespace Sos.BlazorComponents
{
    internal static class DateTimeUtilities
    {
        public static DateTime StartOfMonth(this DateTime dt)
        {
            return CreateRepresentableDateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime EndOfMonth(this DateTime dt)
        {
            return CreateRepresentableDateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 - (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(1 * diff).Date;
        }

        public static DateTime StartOfYear(this DateTime dt)
        {
            return CreateRepresentableDateTime(dt.Year, 1, 1);
        }

        public static DateTime EndOfYear(this DateTime dt)
        {
            return CreateRepresentableDateTime(dt.Year + 1, 1, 1).AddTicks(-1);
        }

        public static DateTime StartOfDecade(this DateTime dt)
        {
            return CreateRepresentableDateTime((int)(dt.Year / 10) * 10, 1, 1);
        }

        public static DateTime EndOfDecade(this DateTime dt)
        {
            return CreateRepresentableDateTime(((int)(dt.Year / 10) * 10) + 10, 1, 1).AddTicks(-1);
        }

        public static DateTime StartOfCentury(this DateTime dt)
        {
            return CreateRepresentableDateTime((int)(dt.Year / 100) * 100, 1, 1);
        }

        public static DateTime EndOfCentury(this DateTime dt)
        {
            return CreateRepresentableDateTime(((int)(dt.Year / 100) * 100) + 100, 1, 1).AddTicks(-1);
        }

        public static DateTime StartOfMillenium(this DateTime dt)
        {
            return CreateRepresentableDateTime((int)(dt.Year / 1000) * 1000, 1, 1);
        }

        public static DateTime EndOfMillenium(this DateTime dt)
        {
            return CreateRepresentableDateTime(((int)(dt.Year / 1000) * 1000) + 1000, 1, 1).AddTicks(-1);
        }

        public static DateTime SafeAddYears(this DateTime dt, int years)
        {
            int year = dt.Year + years;
            return CreateRepresentableDateTime(year, dt.Month, dt.Day);
        }

        private static DateTime CreateRepresentableDateTime(int year, int month, int day)
        {
            if (year <= 0)
            {
                year = 1;
            }
            if (year >= 10000)
            {
                year = 9999;
            }
            return new DateTime(year, month, day);
        }
    }
}
