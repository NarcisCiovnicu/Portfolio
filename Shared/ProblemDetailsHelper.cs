namespace Shared
{
    public static class ProblemDetailsHelper
    {
        private static readonly Dictionary<int, string> _problemTypes = new()
        {
            [0] = "about:blank",

            [1] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.2",
            [100] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.2.1",

            [2] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.3",
            [200] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.3.1",

            [3] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.4",
            [300] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.4.1",
            [301] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.4.2",
            [302] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.4.3",
            [303] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.4.4",
            [304] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.4.5",
            [305] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.4.6",
            [306] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.4.7",
            [307] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.4.8",
            [308] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.4.9",

            [4] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5",
            [400] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            [401] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.2",
            [402] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.3",
            [403] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.4",
            [404] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
            [405] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.6",
            [406] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.7",
            [407] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.8",
            [408] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.9",

            [5] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6",
            [500] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
            [501] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.2",
            [502] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.3",
            [503] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.4",
            [504] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.5",
            [505] = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.6",
        };

        public static string GetProblemDetailsType(int statusCode)
        {
            string? problemType = _problemTypes.GetValueOrDefault(statusCode);
            if (problemType is not null)
            {
                return problemType;
            }

            return _problemTypes[GetBaseCode(statusCode)];
        }

        private static int GetBaseCode(int statusCode)
        {
            if (statusCode >= 100 && statusCode <= 599)
            {
                return (int)(Math.Floor(statusCode / 100.0));
            }
            else
            {
                return 0;
            }
        }
    }
}
