using System;

namespace POS.Data.Dto
{
    public class UserClaimDto
    {
        public Guid? UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public Guid ActionId { get; set; }
    }
}
