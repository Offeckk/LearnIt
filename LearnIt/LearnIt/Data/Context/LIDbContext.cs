using LearnIt.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace LearnIt.Data.Context
{
    public class LIDbContext : IdentityDbContext<LIUser, IdentityRole, string>
    {
        public DbSet<LearnCourse> LearnCourses { get; set; }
        public DbSet<CourseStatus> CourseStatuses { get; set; }
        public LIDbContext(DbContextOptions<LIDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CourseStatus>().HasData(new CourseStatus
            {
                Id = "1",
                Name = "Upcoming"
            },
            new CourseStatus
            {
                Id = "2",
                Name = "Active"
            },
            new CourseStatus
            {
                Id = "3",
                Name = "Completed"
            },
            new CourseStatus
            {
                Id = "4",
                Name = "Errored"
            }
            );


           builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Student",
                NormalizedName = "STUDENT"
            },
            new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Teacher",
                NormalizedName = "TEACHER"
            }
            );
        }
    }
}
