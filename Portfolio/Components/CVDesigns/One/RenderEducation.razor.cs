using Microsoft.AspNetCore.Components;
using Portfolio.Models;
using System.Globalization;

namespace Portfolio.Components.CVDesigns.One
{
    public partial class RenderEducation
    {
        [Parameter, EditorRequired]
        public required IList<Education> EducationHistory { get; init; }

        private readonly CultureInfo _dateCultureInfo = new("en-US");

        private string DateToString(DateOnly date)
        {
            return date.ToString(Constants.MonthAndYearDateFormat, _dateCultureInfo);
        }
    }
}