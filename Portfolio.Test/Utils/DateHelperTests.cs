using Portfolio.Utils;

namespace Portfolio.Test.Utils
{
    public class DateHelperTests
    {
        [Theory]
        [InlineData(2025, 1, 1, 2020, 1, 1, 0, 0)]
        [InlineData(2020, 1, 5, 2020, 3, 30, 0, 3)]
        [InlineData(2020, 1, 25, 2020, 3, 30, 0, 2)]
        [InlineData(2020, 1, 5, 2024, 3, 30, 4, 3)]
        [InlineData(2020, 1, 25, 2024, 3, 30, 4, 2)]
        [InlineData(2020, 11, 1, 2021, 1, 30, 0, 3)]
        [InlineData(2019, 10, 1, 2021, 9, 15, 2, 0)]
        public void CalculateDiffInYearsAndMonths(int year1, int month1, int day1, int year2, int month2, int day2, int expectedYears, int expectedMonths)
        {
            DateOnly firstDate = new(year1, month1, day1);
            DateOnly secondDate = new(year2, month2, day2);

            (int years, int months) = DateHelper.CalculateDiffInYearsAndMonths(firstDate, secondDate);

            Assert.Equal(expectedYears, years);
            Assert.Equal(expectedMonths, months);
        }
    }
}
