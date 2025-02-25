namespace Shared.Test
{
    public class ProblemDetailsHelperTests
    {
        [Theory]
        [InlineData(500, "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1")]
        [InlineData(530, "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6")]
        [InlineData(599, "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6")]
        public void GetProblemDetailsType_ShouldReturnProblemTypeBasedOnStatusCode(int statusCode, string expected)
        {
            // setup & execute
            string problemType = ProblemDetailsHelper.GetProblemDetailsType(statusCode);

            // assert
            Assert.Equal(expected, problemType);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-300)]
        [InlineData(99)]
        [InlineData(999)]
        [InlineData(123123)]
        [InlineData(600)]
        public void GetProblemDetailsType_ShouldReturnDefaultProblemType_WhenStatusCodeIsOutOfNormalRange(int statusCode)
        {
            // setup & execute
            string problemType = ProblemDetailsHelper.GetProblemDetailsType(statusCode);

            // assert
            Assert.Equal("about:blank", problemType);
        }
    }
}