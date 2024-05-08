using Microsoft.AspNetCore.Identity;

namespace LoanEnquiryApi.Entity
{
    public class UserEntity : IdentityUser
    {
        public string FullName { get; set; }
        public string UserRole { get; set; }
    }
}
