namespace MeetingRoomManagement.Entities
{
    public class UserWithToken:Users
    {
        public string Token { get; set; }
        public UserWithToken(Users user)
        {
            this.ID = user.ID;
            this.FIRSTNAME = user.FIRSTNAME;
            this.LASTNAME = user.LASTNAME;
            this.EMAIL = user.EMAIL;
        }
    }
}
