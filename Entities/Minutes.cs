using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.Entities
{
    [PrimaryKey("Id")]
    public class Minutes
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        [ForeignKey("MeetingId")]
        public Meetings Meeting { get; set; }
        public string PointOfDisc {  get; set; }
        public DateTime DueDate { get; set; }
        public string Summary { get; set; }

    }
}
