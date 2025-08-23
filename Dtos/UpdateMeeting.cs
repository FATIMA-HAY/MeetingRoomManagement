using MeetingRoomManagement.Entities;

namespace MeetingRoomManagement.Dtos
{
    public class UpdateMeeting
    {
        public string Title { get; set; }
        public string Agenda { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateOnly Date {  get; set; }
        public int RoomId { get; set; }
        public List<string> AttendeesEmail { get; set; }
    }
}
