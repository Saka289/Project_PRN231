using WebApp.Models.Dtos;

namespace WebApp.ViewModels
{
    public class MyAccountViewModel
    {
        public MemberDto Member { get; set; }
        public ChangePasswordDto ChangePassword { get; set; }
        public IEnumerable<OrderDto> Order { get; set; }
    }
}
