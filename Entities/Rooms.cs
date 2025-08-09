using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.Entities
{
    [PrimaryKey("Id")]
    public class Rooms
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public int Location { get; set; }
        public int FeatureId { get; set; }
        [ForeignKey("FeatureId")]
        public RoomFeatures Feature { get; set; }
        public int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public Users User { get; set; }
    }
}
