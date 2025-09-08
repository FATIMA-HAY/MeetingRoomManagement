using System.ComponentModel.DataAnnotations;

namespace MeetingRoomManagement.Dtos
{
    public class AddUsers
    {
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string EMAIL { get; set; }
        [Required (ErrorMessage ="Password is required")]
        public string PASSWORD { get; set; }
        public int ROLEID { get; set; }
    }
}
