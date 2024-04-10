using System.ComponentModel.DataAnnotations;

namespace Web.Services.AuthAPI.Models.Dto
{
    public class MemberDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTimeOffset? LockOutEnd { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
