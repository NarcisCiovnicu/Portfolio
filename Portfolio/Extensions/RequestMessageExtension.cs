namespace Portfolio.Extensions
{
    public static class RequestMessageExtension
    {
        public static string GetRequestInstance(this HttpRequestMessage requestMessage)
        {
            return $"{requestMessage.Method} {requestMessage.RequestUri?.AbsolutePath}";
        }
    }
}
