using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.Entities
{
    [PrimaryKey("Id")]
    public class Attendees
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public Users Users { get; set; }
        public string Email { get; set; }
        public int MeetingId { get; set; }
        [ForeignKey("MeetingId")]
        public Meetings Meeting { get; set; }
       
        public bool IsPresent { get; set; }

    }
}
