using System;
using Web.Services.AuthAPI.Data;

namespace Web.Services.AuthAPI.Models.Dto
{
    public static class ModelMappingExtensions
    {
        public static MemberDto ToMemberDto(this ApplicationUser user, AppDbContext context)
        {
            return new MemberDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                Roles = context.UserRoles
                        .Where(ur => ur.UserId == user.Id)
                        .Join(context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                        .ToList()
            };
        }
    }
}
