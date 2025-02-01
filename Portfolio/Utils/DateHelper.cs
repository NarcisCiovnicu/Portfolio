namespace Portfolio.Utils
{
    public static class DateHelper
    {
        public static DateOnly Today()
        {
            return DateOnly.FromDateTime(DateTime.Today);
        }

        public static Tuple<int, int> CalculateDiffInYearsAndMonths(DateOnly firstDate, DateOnly secondDate)
        {
            int diffInDays = secondDate.DayNumber - firstDate.DayNumber;

            if (diffInDays < 1)
            {
                return Tuple.Create(0, 0);
            }

            int years = diffInDays / 365;
            diffInDays %= 365;

            int months = diffInDays / 30;
            diffInDays %= 30;

            if (diffInDays > 14)
            {
                months++;
            }

            if (months == 12)
            {
                years++;
                months = 0;
            }

            return new Tuple<int, int>(years, months);
        }

        public static string GetDiffInYearsAndMonthsAsString(DateOnly firstDate, DateOnly secondDate)
        {
            (int years, int months) = CalculateDiffInYearsAndMonths(firstDate, secondDate);

            if (years == 0)
            {
                return MonthsToString(months);
            }
            if (months == 0)
            {
                return YearsToString(years);
            }
            return $"{YearsToString(years)} {MonthsToString(months)}";
        }

        private static string AddPlural(int number)
        {
            return number != 1 ? "s" : "";
        }

        private static string YearsToString(int years)
        {
            return $"{years} yr{AddPlural(years)}";
        }

        private static string MonthsToString(int months)
        {
            return $"{months} mo{AddPlural(months)}";
        }
    }
}
