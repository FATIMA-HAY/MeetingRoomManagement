using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.Entities
{    
    [PrimaryKey("Id")]
    public class Assignements
    {
        public int Id { get; set; }
        public string ActionItems { get; set; }
        public DateTime DueDate { get; set; }
        public int MomId { get; set; }
        [ForeignKey("MomId")]
        public Minutes Minute { get; set; }
        public int AssignedTo { get; set; }
        [ForeignKey("AssignedTo")]
        public Attendees Attendee { get; set; }
    }
}
