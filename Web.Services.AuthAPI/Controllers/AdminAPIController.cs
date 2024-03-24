using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Web.Services.AuthAPI.Models;
using Web.Services.AuthAPI.Models.Dto;
using Web.Services.AuthAPI.Service.IService;

namespace Web.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAPIController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly UserManager<ApplicationUser> _userManager;
        protected ResponseDto _response;

        public AdminAPIController(IAdminService adminService, UserManager<ApplicationUser> userManager)
        {
            _adminService = adminService;
            _userManager = userManager;
            _response = new ResponseDto();
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMembers()
        {
            var result = await _adminService.GetMembers();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost("UpdateRoleMember")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateRoleMember([FromBody] UpdateRoleDto updateRoleDto)
        {
            var result = await _adminService.UpdateRoleMemeber(updateRoleDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet("GetMemberByUserID/{userId}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMemberByUserID([Required] string userId)
        {
            var result = await _adminService.GetMemberByUserID(userId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("GetMemberByEmail/{email}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMemberByEmail([Required] string email)
        {
            var result = await _adminService.GetMemberByEmail(email);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddEditMember([FromBody] MemberAddEditDto model)
        {
            if (await CheckEmailExistsAsync(model.Email, model.UserId))
            {
                _response.IsSuccess = false;
                _response.Message = $"An existing account is using {model.Email}, email addres. Please try with another email address";
                return BadRequest(_response);
            }
            var result = await _adminService.AddEditMember(model);
            if (result.IsSuccess == true)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("LockMember")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> LockMember([FromBody] string userId)
        {
            var result = await _adminService.LockMember(userId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost("UnlockMember")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UnlockMember([FromBody] string userId)
        {
            var result = await _adminService.UnlockMember(userId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpDelete("DeleteMember/{userId}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteMember([Required] string userId)
        {
            var result = await _adminService.DeleteMember(userId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("GetRoles")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _adminService.GetApplicationRoles();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        private async Task<bool> CheckEmailExistsAsync(string email, string userId)
        {
            return await _userManager.Users.AnyAsync(x => x.Email.Equals(email) && x.Id != userId);
        }
    }
}
