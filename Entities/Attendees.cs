using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MeetingRoomManagement.Entities
{
    [PrimaryKey("UserId")]
    public class Attendees
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public Users Users { get; set; }
        public string Email { get; set; }
        public int MeetingId { get; set; }
        [ForeignKey("MeetingId")]
        public Meetings Meeting { get; set; }
       
        public bool IsPresent { get; set; }
        public ICollection<Assignements> assignements { get; set; }

    }
}
