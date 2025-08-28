using System.ComponentModel.DataAnnotations;
using MeetingRoomManagement.DataBaseContext;
using MeetingRoomManagement.Entities;

namespace MeetingRoomManagement
{
    public class Test
    {
        public readonly StoreDBContext _dbContext;
        public  void testDb()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();// it creates a new db if it doesn't exist
            List<Users> user = new List<Users>()
            {
                new Users { FIRSTNAME = "FATIMA", LASTNAME = "HAYDAR", EMAIL = "FATIMAHAY27@GMAIL.COM", PASSWORD = "FH1234" },

            };
            _dbContext.SaveChanges();

        }
    }
}
