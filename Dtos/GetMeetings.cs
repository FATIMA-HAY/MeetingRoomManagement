using System.ComponentModel.DataAnnotations.Schema;
using MeetingRoomManagement.Entities;

namespace MeetingRoomManagement.Dtos
{
    public class GetMeetings 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Agenda { get; set; }
        public List<AttendeesDto> Attendees { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateOnly DATE { get; set; }
        public string Status { get; set; }
        public int RoomId { get; set; }
        public int CreatedBy { get; set; }

    }
}
