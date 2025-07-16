// File: CollegeConnect.Api/Data/CollegeConnectDbContext.cs
using System;
using Microsoft.EntityFrameworkCore;
using api.models;
using api.models.User;
using api.models.Chat;
using api.models.Announcements;
using api.models.Assignment;
using api.models.Events;

namespace api.Data
{

    public class ConnectDbContext : DbContext
    {
        public ConnectDbContext(DbContextOptions<ConnectDbContext> options)
            : base(options)
        {
        }

        // ─── Users & Roles
        public DbSet<Users> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        // ─── Chat 
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        // ─── Announcements 
        public DbSet<Announcement> Announcements { get; set; }

        // ─── Assignments & Submissions
        public DbSet<Assignments> Assignments { get; set; }
        public DbSet<Submission> Submissions { get; set; }

        // ─── Events 
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // If you used [ForeignKey] attributes in your classes, EF Core will wire up FKs automatically.
            // You can still configure cascade behaviors, indexes, or table names here as needed.

            // ─── Users ↔ UserRole ↔ Role 
            modelBuilder.Entity<UserRole>(b =>
            {
                b.HasKey(ur => ur.Id);

                b.HasOne(ur => ur.User)
                  .WithMany(u => u.UserRoles)
                  .HasForeignKey(ur => ur.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(ur => ur.Role)
                  .WithMany(r => r.UserRoles)
                  .HasForeignKey(ur => ur.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // ─── ChatGroup ↔ GroupMember ↔ Users 
            modelBuilder.Entity<GroupMember>(b =>
            {
                b.HasKey(gm => gm.Id);

                b.HasOne(gm => gm.User)
                  .WithMany(u => u.GroupMemberships)
                  .HasForeignKey(gm => gm.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(gm => gm.Group)
                  .WithMany(g => g.Members)
                  .HasForeignKey(gm => gm.GroupId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // ─── ChatMessage
            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasKey(cm => cm.Id);

                b.HasOne(cm => cm.Sender)
                  .WithMany(u => u.SentMessages)
                  .HasForeignKey(cm => cm.SenderId)
                  .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(cm => cm.Group)
                  .WithMany(g => g.Messages)
                  .HasForeignKey(cm => cm.GroupId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // ─── ChatGroup (creator relationship)
            modelBuilder.Entity<ChatGroup>(b =>
            {
                b.HasKey(g => g.Id);

                b.HasOne(g => g.CreatedBy)
                  .WithMany(u => u.CreatedChatGroups)
                  // Note: if you’d rather track “groups I created” separately, 
                  // add a new ICollection<ChatGroup> on Users (e.g. CreatedGroups).
                  .HasForeignKey(g => g.CreatedById)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            // ─── Announcement 
            modelBuilder.Entity<Announcement>(b =>
            {
                b.HasKey(a => a.Id);

                b.HasOne(a => a.PostedBy)
                  .WithMany(u => u.Announcements)
                  .HasForeignKey(a => a.PostedById)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // ─── Assignment ↔ Submission 
            modelBuilder.Entity<Assignments>(b =>
            {
                b.HasKey(a => a.Id);

                b.HasOne(a => a.CreatedBy)
                  .WithMany(u => u.AssignmentsCreated)
                  .HasForeignKey(a => a.CreatedById)
                  .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(a => a.Submissions)
                  .WithOne(s => s.Assignment)
                  .HasForeignKey(s => s.AssignmentId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Submission>(b =>
            {
                b.HasKey(s => s.Id);

                b.HasOne(s => s.Student)
                  .WithMany(u => u.Submissions)
                  .HasForeignKey(s => s.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            // ─── Event
            modelBuilder.Entity<Event>(b =>
            {
                b.HasKey(e => e.Id);

                b.HasOne(e => e.CreatedBy)
                  .WithMany(u => u.EventsCreated)
                  .HasForeignKey(e => e.CreatedById)
                  .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(e => e.SpecificGroup)
                  .WithMany(g => g.Events)
                  // If you’d prefer a separate navigation on ChatGroup (e.g. TargetedEvents), 
                  // add that to ChatGroup and point .WithMany(g => g.TargetedEvents).
                  .HasForeignKey(e => e.SpecificGroupId)
                  .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
