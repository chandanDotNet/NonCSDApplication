using Microsoft.AspNetCore.Identity;
using System;

namespace POS.Data
{
    public class UserToken : IdentityUserToken<Guid>
    {
        public virtual User User { get; set; } = null;
    }
}
