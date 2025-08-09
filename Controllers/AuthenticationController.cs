using MeetingRoomManagement.DataBaseContext;
using MeetingRoomManagement.Dtos;
using MeetingRoomManagement.Entities;
using MeetingRoomManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly StoreDBContext _dbContext;
        private readonly TokenServices _tokenServices;
        public AuthenticationController(StoreDBContext dbContext, TokenServices tokenservices) {
            _dbContext = dbContext;
            _tokenServices = tokenservices;
        }
        [HttpPost("login")]
        [AllowAnonymous]//drure 7ata ma ye7tej token
        public async Task<ActionResult<UserWithToken>> Login([FromBody] LoginUsers login)// fromBody la2en l parameter info men body l httpRequest
        {
            //check eza l email w l password sa7
            var user = await _dbContext.user.FirstOrDefaultAsync(u => u.EMAIL == login.email);
            UserWithToken userWithToken = null;
            if (user == null || user.PASSWORD ==login.password)
                return Unauthorized("Invalid Credentials");

            //create lal token
            var User= _dbContext.user.Include(u=>u.ROLE).FirstOrDefault(u=> u.ID==user.ID);
            userWithToken.AccessToken = _tokenServices.GenerateToken(user.ID,User.ROLE.USERROLE);
            return userWithToken;

        }
    }
}
