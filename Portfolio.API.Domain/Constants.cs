namespace Portfolio.API.Domain
{
    public static class Constants
    {
        public static class Config
        {
            public const string WebClientOrigin = "WebClientOrigin";

            public static class RateLimit
            {
                public const string BucketTokenLimit = "BucketRateLimit:TokenLimit";
                public const string BucketQueueLimit = "BucketRateLimit:QueueLimit";
                public const string BucketReplenishmentPeriod = "BucketRateLimit:ReplenishmentPeriod";
                public const string BucketTokensPerPeriod = "BucketRateLimit:TokensPerPeriod";
            }
        }
    }
}
