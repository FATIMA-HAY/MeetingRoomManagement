namespace MeetingRoomManagement.Dtos
{
    public class UpdateMeeting
    {
        public string Title { get; set; }
        public string Agenda { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RoomId { get; set; }
    }
}
