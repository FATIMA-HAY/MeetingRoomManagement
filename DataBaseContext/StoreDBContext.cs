using System.Diagnostics.Metrics;
using MeetingRoomManagement.Entities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MeetingRoomManagement.DataBaseContext
{
    public class StoreDBContext : DbContext
    {
        public StoreDBContext(DbContextOptions<StoreDBContext> options) { }
        public DbSet<Users> user { get; set; }//3m n3mil table esmo user bimasel mjmu3et object men type Users
        public DbSet<Role> roles { get; set; }//hayda sater daroure 7atta ef ye2dar ye5la2le l table
        public DbSet<Rooms> rooms { get; set; }
        public DbSet<Meetings> meetings { get; set; }
        public DbSet<RoomFeatures> roomFeatures { get; set; }
        public DbSet<Minutes> minutes { get; set; }
        public DbSet<Attendees> attendees { get; set; }
        public DbSet<Assignements> assignements { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // مثلاً إذا عندك جدول meetings
            modelBuilder.Entity<Meetings>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);  // ✅ حل المشكلة

            // كرّر الشي نفسه لأي علاقة فيها نفس المشكلة
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Assignements>()
                   .HasOne(m=>m.Minute)
                   .WithMany()
                   .HasForeignKey(m=>m.MomId)
                   .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=UsersDB;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
