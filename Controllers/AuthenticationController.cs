using System.Net;
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
        public AuthenticationController(StoreDBContext dbContext, TokenServices tokenservices)
        {
            _dbContext = dbContext;
            _tokenServices = tokenservices;
        }
        [AllowAnonymous] 
        [HttpPost("login")]
        //drure 7ata ma ye7tej token
        public async Task<ActionResult<UserWithToken>> Login([FromBody] LoginUsers login)// fromBody la2en l parameter info men body l httpRequest
        {
            //check eza l email w l password sa7
            var user = await _dbContext.user.FirstOrDefaultAsync(u => u.EMAIL == login.email);
            if (user == null)
            {
                return Ok(
                    new
                    {
                        success = false,
                        message = "User not found"
                    });
            }
            if (!PasswordHelper.VerifyPassword(login.password, user.PASSWORD))
            {
                return Ok(
                   new
                   {
                       success = false,
                       message = "Incorrect Credentials"
                   });
            }
            //create lal token
            var User = _dbContext.user.Include(u => u.ROLE).FirstOrDefault(u => u.ID == user.ID);
            var token = _tokenServices.GenerateToken(user.ID, User.ROLE.USERROLE);

            /* var userWithToken = new UserWithToken(user)
             {
                 Token = token
             };*/

            return Ok(
                new
                {
                    success = true,
                    // userWithToken
                    token
                });

        }
        [AllowAnonymous]
        [HttpPut("ForgotPassword")]
        public ActionResult ResetPassword(string Email, string password,string ConfirmPassword)
        {
            var user = _dbContext.user.FirstOrDefault(u => u.EMAIL == Email);
            if (user == null) return NotFound();
            if (password!=ConfirmPassword)
            {
                return Ok(
                   new
                   {
                       success = false,
                       message = "Incorrect Credentials"
                   });
            }
            else
            {
                var HashedPassword = PasswordHelper.HashPasswordSHA1(password);
                user.PASSWORD = HashedPassword;
            }
            _dbContext.user.Update(user);
            _dbContext.SaveChanges();
            return Ok(new { success = true });

        }
    }
}
