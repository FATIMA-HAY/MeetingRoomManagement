using MeetingRoomManagement.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingRoomManagement.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Location { get; set; }
        public RoomFeatures Features { get; set; }
        public int CreatedBy { get; set; }
        public string RoomStatus { get; set; }
    }
}
