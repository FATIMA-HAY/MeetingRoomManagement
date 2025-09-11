using System.Net;
using System.Threading.Tasks;
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
           var now = DateTime.Now;
           var rooms= storeDBContext.rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity,
                Location = r.Location,
                Features=r.Feature,
                CreatedBy = r.CreatedBy,
                RoomStatus= storeDBContext.meetings.Any(m=>m.RoomId==r.Id && m.StartTime<=now && m.EndTime>=now)?"Booked":"Available",
            }).ToList();
            storeDBContext.SaveChanges();
            return Ok(new
            {
                success = true,
                rooms
            });
        }
        [HttpGet("MostUsedRoom")]
        public async Task<IActionResult> MostUsedRoom()
        {
            var mostusedroom= storeDBContext.meetings.GroupBy(m => m.RoomId).Select(g => new
            {
                RoomId = g.Key,
                UsageCount=g.Count(),
            })
                .OrderByDescending(m => m.UsageCount).FirstOrDefault();
            var room = await storeDBContext.rooms.Where(r => r.Id == mostusedroom.RoomId).Select(r => new RoomDto
            {
                Name = r.Name,
            }).FirstOrDefaultAsync();
            return Ok(new
            {
                success = true,
                room
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
        public async Task<IActionResult> PutRoom(int RoomId, AddRoomDto room)
        {
            var existingRoom = await storeDBContext.rooms.Include(r => r.Feature).FirstOrDefaultAsync(r => r.Id == RoomId);

            if (existingRoom == null)
            {
                // If the room doesn't exist, return a 404 Not Found response.
                return NotFound();
            }

            // Update the existing room properties with the new data from the DTO
            existingRoom.Name = room.Name;
            existingRoom.Capacity = room.Capacity;
            existingRoom.Location = room.Location;

            // Update the existing features
            existingRoom.Feature.Projector = room.Features.Projector;
            existingRoom.Feature.VideoConference = room.Features.VideoConference;
            existingRoom.Feature.WhiteBoard = room.Features.WhiteBoard;

            // Tell the context that the entity has been modified.
            storeDBContext.rooms.Update(existingRoom);

            // Save the changes to the database
            await storeDBContext.SaveChangesAsync();

            // Return a success response
            return Ok(new
            {
                success = true,
            });
        }
        [HttpDelete("DeleteRoom")]
        [Authorize(Roles = "Admin")]
        public HttpStatusCode DeleteRoom(int RoomId)
        {
            var room = storeDBContext.rooms.Find(RoomId);
            if (room == null) { return HttpStatusCode.NotFound; }
            storeDBContext.rooms.Remove(room);
            storeDBContext.SaveChanges();
            return HttpStatusCode.OK;
        }
         [HttpGet("AvailableRoom")]
         public async Task<IActionResult> GetAvailableRooms() {
            var CountAvailableRoom=await storeDBContext.rooms.Where(r=>r.RoomStatus=="Available").CountAsync();
            return Ok(CountAvailableRoom);
     }
    }
}
