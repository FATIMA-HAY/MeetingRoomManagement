namespace MeetingRoomManagement.Dtos
{
    public class AddMeeting
    {
        public string Title { get; set; }
        public string Agenda { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RoomId { get; set; }
        public int CreatedBy { get; set; }
    }
}
