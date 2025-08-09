using MeetingRoomManagement.Entities;

namespace MeetingRoomManagement.Dtos
{
    public class GetMeetings 
    {
        public string Title { get; set; }
        public string Agenda { get; set; }
        public int AttendeesNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public int RoomId { get; set; }
        public int CreatedBy { get; set; }

    }
}
