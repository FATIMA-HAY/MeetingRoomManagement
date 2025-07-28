using System.ComponentModel.DataAnnotations;
using MeetingRoomManagement.DataBaseContext;
using MeetingRoomManagement.Entities;

namespace MeetingRoomManagement
{
    public class Test
    {
        public static void testDb()
        {
            var context = new StoreDBContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();// it creates a new db if it doesn't exist
            List<Users> user = new List<Users>()
            {
                new Users { FIRSTNAME = "FATIMA", LASTNAME = "HAYDAR", EMAIL = "FATIMAHAY27@GMAIL.COM", PASSWORD = "FH1234" },

            };
            context.SaveChanges();

        }
    }
}
