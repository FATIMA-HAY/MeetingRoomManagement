using MeetingRoomManagement.DataBaseContext;
using MeetingRoomManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MeetingRoomManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomFeaturesController : Controller
    {
        public readonly StoreDBContext storeDBContext;
        public RoomFeaturesController(StoreDBContext storeDBContext)
        {
            this.storeDBContext = storeDBContext;
        }
        [HttpGet]
        public List<RoomFeatures> GetRoomFeatures()
        {
            return storeDBContext.roomFeatures.Select(c => new RoomFeatures
            {
                Id = c.Id,
                Projector = c.Projector,
                VideoConference = c.VideoConference,
                WhiteBoard = c.WhiteBoard,
            }).ToList();
        }
    }
}
