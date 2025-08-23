using MeetingRoomManagement.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingRoomManagement.Dtos
{
    public class AttendeesDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int MeetingId { get; set; }
        public bool IsPresent { get; set; }

    }
}
