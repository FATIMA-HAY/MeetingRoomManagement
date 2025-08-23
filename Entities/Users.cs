using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.Entities
{
    [PrimaryKey("ID")]
    public class Users
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//3achen ye5ud l columns as  identity column
        public int ID { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string EMAIL { get; set; }
        public string PASSWORD { get; set; }

        public int ROLEID { get; set; }
        [ForeignKey("ROLEID")]
        public Role ROLE { get; set; }//navigation property
        public ICollection<Attendees> Attendee {  get; set; }
    }
}
