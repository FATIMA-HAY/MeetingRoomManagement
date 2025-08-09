using System.Net;
using MeetingRoomManagement.DataBaseContext;
using MeetingRoomManagement.Dtos;
using MeetingRoomManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomController : Controller
    {
      public readonly StoreDBContext storeDBContext;
      public RoomController(StoreDBContext storeDBContext)
        {
            this.storeDBContext = storeDBContext;
        }

        [HttpGet]
        public List<RoomDto> GetRoom()
        {
            return storeDBContext.rooms.Select(r => new RoomDto
            {
                Capacity = r.Capacity,
                Location = r.Location,
                FeatureId = r.FeatureId,
                CreatedBy = r.CreatedBy,

            }).ToList();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public HttpStatusCode PostRoom(RoomDto room)
        {
            var Room = new Rooms
            {
                Capacity = room.Capacity,
                Location = room.Location,
                FeatureId = room.FeatureId,
                CreatedBy = room.CreatedBy,
            };
            storeDBContext.rooms.Add(Room);
            storeDBContext.SaveChanges();
            return HttpStatusCode.OK;
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public HttpStatusCode PutRoom(int RoomId, RoomDto room)
        {
           var Room=storeDBContext.rooms.Find(RoomId);
           if (Room == null) return HttpStatusCode.NotFound;
           else
            {
                var r = new Rooms
                {
                    Id=RoomId,
                    Capacity = room.Capacity,
                    Location = room.Location,
                    FeatureId = room.FeatureId,
                    CreatedBy= room.CreatedBy,
                };
                storeDBContext.rooms.Update(Room);
                storeDBContext.SaveChanges();
                return HttpStatusCode.OK;
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public HttpStatusCode DeleteRoom(int RoomId)
        {
            var room = storeDBContext.rooms.Find(RoomId);
            storeDBContext.rooms.Remove(room);
            return HttpStatusCode.OK;
        }
    }
}
