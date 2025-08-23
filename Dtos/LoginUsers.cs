using MeetingRoomManagement.Entities;
using Microsoft.Identity.Client;

namespace MeetingRoomManagement.Dtos
{
    public class LoginUsers
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
