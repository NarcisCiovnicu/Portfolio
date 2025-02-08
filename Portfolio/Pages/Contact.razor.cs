using Microsoft.Extensions.Options;
using Portfolio.Models;

namespace Portfolio.Pages
{
    public partial class Contact(IOptions<ClientAppConfig> options)
    {
        protected readonly string EmailAddress = options.Value.EmailContact;
    }
}
