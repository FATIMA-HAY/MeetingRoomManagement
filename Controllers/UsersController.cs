using System.Net;
using MeetingRoomManagement.DataBaseContext;
using MeetingRoomManagement.Dtos;
using MeetingRoomManagement.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomManagement.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        [HttpGet]
        public List<UsersDto> Get()
        {
            var context = new StoreDBContext();
            return context.user.Select(c => new UsersDto
            {
                FIRSTNAME = c.FIRSTNAME,
                LASTNAME = c.LASTNAME,
                EMAIL = c.EMAIL
            }).ToList();
        }
        [HttpPost]//creat

        public HttpStatusCode post(AddUsers user)
        {
            var context = new StoreDBContext();
            var usero = new Users
            {
                FIRSTNAME = user.FIRSTNAME,
                LASTNAME = user.LASTNAME,
                EMAIL = user.EMAIL,
                PASSWORD = user.PASSWORD
            };
            context.user.Add(usero);
            context.SaveChanges();
            return HttpStatusCode.OK;

        }
        [HttpPut]//update
        public HttpStatusCode put(UpdateUsers user)
        {
            var context = new StoreDBContext();
            var usero = new Users
            {
                ID = user.ID,
                FIRSTNAME = user.FIRSTNAME,
                LASTNAME = user.LASTNAME,
                EMAIL = user.EMAIL,
            };
            context.user.Add(usero);
            context.SaveChanges();
            return HttpStatusCode.OK;
        }
        [HttpDelete]
        public HttpStatusCode delete(int id)
        {
            var context = new StoreDBContext();
            var usero = context.user.First(usero => usero.ID == id);
            context.user.Remove(usero);
            context.SaveChanges();
            return HttpStatusCode.OK;
        }
    }
}
