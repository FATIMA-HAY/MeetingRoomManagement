using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.Entities
{
    [PrimaryKey("Id")] 
    
    public class RoomFeatures
    {
        public int Id { get; set; }
        public bool Projector { get; set; }
        public bool VideoConference { get; set; }
        public bool WhiteBoard { get; set; }

    }
}
