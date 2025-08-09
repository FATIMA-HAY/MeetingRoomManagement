using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.Entities
{
    [PrimaryKey("Id")]
    public class Meetings
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public Users User { get; set; }
        public string Title { get; set; }
        public string Agenda { get; set; }
        public int AttendeesNumber { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Rooms Rooms { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }

    }
}
