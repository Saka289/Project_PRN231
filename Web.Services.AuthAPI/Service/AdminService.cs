using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Enums;
using Web.Services.AuthAPI.Data;
using Web.Services.AuthAPI.Models;
using Web.Services.AuthAPI.Models.Dto;
using Web.Services.AuthAPI.Service.IService;

namespace Web.Services.AuthAPI.Service
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public AdminService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        public async Task<ResponseDto> AddEditMember(MemberAddEditDto model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserId))
                {
                    ApplicationUser user = new()
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        NormalizedEmail = model.Email.ToLower(),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        EmailConfirmed = true
                    };
                    var addUser = await _userManager.CreateAsync(user, model.Password);
                    if (addUser.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, SD.RoleCustomer);
                    }
                    else
                    {
                        _response.Result = false;
                        _response.IsSuccess = false;
                        _response.Message = addUser.Errors.FirstOrDefault().Description.ToString();
                        return _response;
                    }
                }
                else
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(model.UserId);
                    if (user == null)
                    {
                        _response.Result = false;
                        _response.IsSuccess = false;
                        _response.Message = "Not found !!!";
                        return _response;
                    }
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        await _userManager.RemovePasswordAsync(user);
                        var result = await _userManager.AddPasswordAsync(user, model.Password);
                        if (!result.Succeeded)
                        {
                            _response.Result = false;
                            _response.IsSuccess = false;
                            _response.Message = result.Errors.FirstOrDefault().Description.ToString();
                            return _response;
                        }
                    }
                }
                _response.IsSuccess = true;
                _response.Result = true;
                _response.Message = "Create or Update Successfully !!!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        public async Task<ResponseDto> DeleteMember(string userId)
        {
            try
            {
                var member = await _userManager.Users
                    .Where(u => u.Id.Equals(userId))
                    .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                    .Join(_context.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
                    .Where(r => r.r.Name.ToLower().Equals(SD.RoleCustomer))
                    .Select(m => m.ur.u)
                    .FirstOrDefaultAsync();
                if (member == null)
                {
                    _response.Result = false;
                    _response.IsSuccess = false;
                    _response.Message = "Not found !!!";
                    return _response;
                }
                await _userManager.DeleteAsync(member);
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        public async Task<ResponseDto> GetApplicationRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
                _response.Result = _mapper.Map<string[]>(roles);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        public async Task<ResponseDto> GetMemberByUserID(string userId)
        {
            try
            {
                var member = await _userManager.Users
                    .Where(u => u.Id.Equals(userId))
                    .Select(m => m.ToMemberDto(_context))
                    .FirstOrDefaultAsync();
                _response.Result = _mapper.Map<MemberDto>(member);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        public async Task<ResponseDto> GetMemberByEmail(string email)
        {
            try
            {
                var member = await _userManager.Users
                    .Where(u => u.Email.Equals(email))
                    .Select(m => m.ToMemberDto(_context))
                    .FirstOrDefaultAsync();
                _response.Result = _mapper.Map<MemberDto>(member);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        public async Task<ResponseDto> GetMembers()
        {
            try
            {
                var members = await _userManager.Users
                    .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                    .Join(_context.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
                    .Where(r => r.r.Name.ToLower().Equals(SD.RoleCustomer))
                    .Select(m => m.ur.u.ToMemberDto(_context))
                    .ToListAsync();
                _response.Result = members;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        public async Task<ResponseDto> LockMember(string userId)
        {
            try
            {
                var member = await _userManager.Users
                  .Where(u => u.Id.Equals(userId))
                  .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                  .Join(_context.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
                  .Where(r => r.r.Name.ToLower().Equals(SD.RoleCustomer))
                  .Select(m => m.ur.u)
                  .FirstOrDefaultAsync();
                if (member == null)
                {
                    _response.Result = false;
                    _response.IsSuccess = false;
                    _response.Message = "Not found !!!";
                    return _response;
                }
                await _userManager.SetLockoutEndDateAsync(member, DateTime.UtcNow.AddDays(5));
                _response.Result = true;
                _response.Message = "LockMember Successfully !!!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        public async Task<ResponseDto> UnlockMember(string userId)
        {
            try
            {
                var member = await _userManager.Users
                  .Where(u => u.Id.Equals(userId))
                  .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                  .Join(_context.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
                  .Where(r => r.r.Name.ToLower().Equals(SD.RoleCustomer))
                  .Select(m => m.ur.u)
                  .FirstOrDefaultAsync();
                if (member == null)
                {
                    _response.Result = false;
                    _response.IsSuccess = false;
                    _response.Message = "Not found !!!";
                    return _response;
                }
                await _userManager.SetLockoutEndDateAsync(member, null);
                _response.Message = "UnlockMember Successfully !!!";
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
