using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.Entities
{
    [PrimaryKey("Id")]
    public class Role
    {
        public int Id { get; set; }
        public string USERROLE { get; set; }
        public ICollection<Users> Users { get; set; }

    }
}
