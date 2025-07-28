using System.Diagnostics.Metrics;
using MeetingRoomManagement.Entities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.DataBaseContext
{
    public class StoreDBContext : DbContext
    {
        public DbSet<Users> user { get; set; }//3m n3mil table esmo user bimasel mjmu3et object men type Users
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=UsersDB;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
