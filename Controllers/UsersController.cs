using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using MeetingRoomManagement.DataBaseContext;
using MeetingRoomManagement.Dtos;
using MeetingRoomManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MeetingRoomManagement.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly JwtSettings _jwtSettings;
        private readonly StoreDBContext _storeDBContext;
        
        public UsersController(StoreDBContext storeDBContext,IOptions<JwtSettings> jwtsetting)
        {
            _jwtSettings = jwtsetting.Value;
            _storeDBContext = storeDBContext;
        }//constructor mnst5dmo la nmare2 nas5a men l object  l men3uzoun la 2e2dar est3mlo aw bya3tine error
        [HttpGet]
        public List<UsersDto> Get()
        {
            return _storeDBContext.user.Select(c => new UsersDto
            {
                FIRSTNAME = c.FIRSTNAME,
                LASTNAME = c.LASTNAME,
                EMAIL = c.EMAIL
            }).ToList();
        }
        [HttpPost]//creat

        public HttpStatusCode post(AddUsers user)
        {
            var usero = new Users
            {
                FIRSTNAME = user.FIRSTNAME,
                LASTNAME = user.LASTNAME,
                EMAIL = user.EMAIL,
                PASSWORD = user.PASSWORD,
                ROLEID=user.ROLEID

            };
            _storeDBContext.user.Add(usero);
            _storeDBContext.SaveChanges();
            return HttpStatusCode.OK;

        }
        [HttpPut]//update
        public HttpStatusCode put(UpdateUsers user)
        {
            var usero = new Users
            {
                ID = user.ID,
                FIRSTNAME = user.FIRSTNAME,
                LASTNAME = user.LASTNAME,
                EMAIL = user.EMAIL,
            };
            _storeDBContext.user.Add(usero);
            _storeDBContext.SaveChanges();
            return HttpStatusCode.OK;
        }
        [HttpDelete]
        public HttpStatusCode delete(int id)
        {
            var usero = _storeDBContext.user.First(usero => usero.ID == id);
            if (usero == null) return HttpStatusCode.NotFound;
            _storeDBContext.user.Remove(usero);
            _storeDBContext.SaveChanges();
            return HttpStatusCode.OK;
        }
    }
}
