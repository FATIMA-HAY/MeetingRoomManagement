using MeetingRoomManagement.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingRoomManagement.Dtos
{
    public class AttendeesDto
    {
         public int MeetingId { get; set; }
        public bool IsPresent { get; set; }

    }
}
