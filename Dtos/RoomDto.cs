using MeetingRoomManagement.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingRoomManagement.Dtos
{
    public class RoomDto
    {
        public int Capacity { get; set; }
        public int Location { get; set; }
        public int FeatureId { get; set; }
        public int CreatedBy { get; set; }
    }
}
