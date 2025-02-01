using Portfolio.Models.Enums;

namespace Portfolio.Models
{
    public class ClientAppConfig
    {
        public string OwnerName { get; set; } = "";
        public CVDesignType CVDesignType { get; set; } = CVDesignType.Default;
    }
}
