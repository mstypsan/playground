using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MyApi
{
    public class PasswordlessTokenProvider : DataProtectorTokenProvider<User>
    {
        public const string ProviderName = nameof(PasswordlessTokenProvider);

        public PasswordlessTokenProvider(IDataProtectionProvider dataProtectionProvider,
            IOptions<PasswordlessTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<User>> logger)
            : base(dataProtectionProvider, options, logger)
        {
        }
    }


    public class PasswordlessTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public PasswordlessTokenProviderOptions()
        {
            Name = "PasswordlessTokenProvider";
            TokenLifespan = TimeSpan.FromMinutes(12);
        }
    }
}