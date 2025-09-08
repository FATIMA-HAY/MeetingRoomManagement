using MeetingRoomManagement.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingRoomManagement.Dtos
{
    public class AssignmentDto
    {
        public string ActionItems { get; set; }
        public DateTime DueDate { get; set; }
        public int AssignedTo { get; set; }
    }
}
