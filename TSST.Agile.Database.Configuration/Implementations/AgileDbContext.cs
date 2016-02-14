namespace TSST.Agile.Database.Configuration.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using TSST.Agile.Database.Configuration.Interfaces;
    using Models;

    public class AgileDbContext: DbContext, IAgileDbContext
    {
        public AgileDbContext() : base("AgileDbContext") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.Projects)
                .WithMany(x => x.Users)
                .Map(x =>
                {
                    x.ToTable("UsersProjects");
                    x.MapLeftKey("UserId");
                    x.MapRightKey("ProjectId");
                });

            modelBuilder.Entity<Task>()
                .HasRequired<Project>(x => x.Project)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<Task>()
                .HasRequired<User>(x => x.User)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<File>()
                .HasRequired<Task>(x => x.Task)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.TaskId);

            modelBuilder.Entity<Friendship>()
                .HasRequired(x => x.Friend)
                .WithMany()
                .HasForeignKey(x => x.FriendId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Friendship>()
                .HasRequired(x => x.User)
                .WithMany(x => x.Friendships)
                .HasForeignKey(x => x.UserId);
        }
    }
}
