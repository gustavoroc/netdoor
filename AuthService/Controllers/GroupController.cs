using AuthService.DTOs;
using AuthService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        public GroupService GroupService { get; set; }

        public GroupController(GroupService groupService)
        {
            GroupService = groupService;
        }

        [HttpPost("CreateGroup")]
        public async Task<IActionResult> CreateGroup(GroupDto dto)
        {
            await GroupService.CreateGroup(dto);
            return Ok();
        }


        [HttpPost("AddUserToGroup/{groupId}/{userId}")]
        public async Task<IActionResult> AddUserToGroup(int groupId, string userId)
        {
            await GroupService.AddUserToGroup(groupId, userId);
            return Ok();
        }
    }
}
