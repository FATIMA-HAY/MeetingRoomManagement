using MeetingRoomManagement.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingRoomManagement.Dtos
{
    public class AddMinutesDto
    {
        public int MeetingId { get; set; }
        public string PointOfDisc { get; set; }
        public string Summary { get; set; }
        //public DateOnly DueDate { get; set; }
        public List<AssignmentDto> Assignments { get; set; }
        public List<int> PresentUserIds { get; set; }
    }
}
