using Microsoft.AspNetCore.Components;
using Portfolio.Models;
using Portfolio.Utils;
using System.Globalization;

namespace Portfolio.Components.CVDesigns.One
{
    public partial class WorkExperienceHeader
    {
        [Parameter, EditorRequired]
        public required WorkExperience WorkExperience { get; init; }

        private readonly CultureInfo _dateCultureInfo = new("en-US");
        private string StartDateText { get { return WorkExperience.StartDate.ToString(Constants.MonthAndYearDateFormat, _dateCultureInfo); } }
        private string EndDateText { get { return WorkExperience.EndDate?.ToString(Constants.MonthAndYearDateFormat, _dateCultureInfo) ?? "Present"; } }

        private string GetWorkPeriod()
        {
            var startDate = WorkExperience.StartDate;
            DateOnly endDate = WorkExperience.EndDate ?? DateHelper.Today();
            return DateHelper.GetDiffInYearsAndMonthsAsString(startDate, endDate);
        }
    }
}
