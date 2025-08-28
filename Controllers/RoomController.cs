using System.Net;
using MeetingRoomManagement.DataBaseContext;
using MeetingRoomManagement.Dtos;
using MeetingRoomManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("GetRoom")]
        public IActionResult GetRoom()
        {
           var rooms= storeDBContext.rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity,
                Location = r.Location,
                Features=r.Feature,
                CreatedBy = r.CreatedBy,
                RoomStatus= r.RoomStatus,
            }).ToList();
            return Ok(new
            {
                success = true,
                rooms
            });
        }
        [HttpPost("AddRoom")]
        [Authorize(Roles = "Admin")]
        public IActionResult PostRoom(int UserID,AddRoomDto room)
        {
            var features = new RoomFeatures
            {
                Projector = room.Features.Projector,
                VideoConference = room.Features.VideoConference,
                WhiteBoard = room.Features.WhiteBoard
            };
            storeDBContext.roomFeatures.Add(features);
            storeDBContext.SaveChanges();
            var Room = new Rooms
            {
                Name = room.Name,
                Capacity = room.Capacity,
                Location = room.Location,
                FeatureId = features.Id,
                CreatedBy = UserID,
                RoomStatus="Available"
            };

            storeDBContext.rooms.Add(Room);
            storeDBContext.SaveChanges();
            return Ok(new
            {
                success = true,
            });
        }
        [HttpPut("UpdateRoom")]
        [Authorize(Roles = "Admin")]
        public HttpStatusCode PutRoom(int RoomId, AddRoomDto room)
        {
            var Room = storeDBContext.rooms.Find(RoomId);
            if (Room == null) return HttpStatusCode.NotFound;
            else
            {
                var features = new RoomFeatures
                {
                    Projector = room.Features.Projector,
                    VideoConference = room.Features.VideoConference,
                    WhiteBoard = room.Features.WhiteBoard
                };
                storeDBContext.roomFeatures.Add(features);
                storeDBContext.SaveChanges();
                var r= new Rooms
                {
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Location = room.Location,
                    FeatureId = features.Id,
                    CreatedBy = room.CreatedBy,
                    RoomStatus = "Available"
                };
                storeDBContext.rooms.Update(Room);
                storeDBContext.SaveChanges();
                return HttpStatusCode.OK;
            }
        }
        [HttpDelete("DeleteRoom")]
        [Authorize(Roles = "Admin")]
        public HttpStatusCode DeleteRoom(int RoomId)
        {
            var room = storeDBContext.rooms.Find(RoomId);
            storeDBContext.rooms.Remove(room);
            return HttpStatusCode.OK;
        }
         [HttpGet("AvailableRoom")]
         public async Task<IActionResult> GetAvailableRooms() {
            var CountAvailableRoom=await storeDBContext.rooms.Where(r=>r.RoomStatus=="Available").CountAsync();
            return Ok(CountAvailableRoom);
     }
    }
}
