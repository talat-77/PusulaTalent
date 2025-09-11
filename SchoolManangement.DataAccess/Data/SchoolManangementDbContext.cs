using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManangement.DataAccess.Extensions;
using SchoolManangement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.DataAccess.Data
{
    public class SchoolManangementDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public SchoolManangementDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Absenteeism> Absenteeisms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<SchoolClass> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentAssignment> StudentAssignments { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplySoftDelete();
            SetAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new ApplicationRole
                {
                    Id = new Guid("22222222-2222-2222-2222-222222222222"),
                    Name = "Teacher",
                    NormalizedName = "TEACHER"
                },
                new ApplicationRole
                {
                    Id = new Guid("33333333-3333-3333-3333-333333333333"),
                    Name = "Student",
                    NormalizedName = "STUDENT"
                }
            );

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.ApplyGlobalFilters<bool>("IsDeleted", false);

            base.OnModelCreating(modelBuilder);
        }

        private void SetAuditFields()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var baseEntity = (BaseEntity)entry.Entity;
                baseEntity.UpdatedDate = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    baseEntity.CreatedDate = DateTime.UtcNow; 
                }
            }
        }

        private void ApplySoftDelete()
        {
            var deletedEntities = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in deletedEntities)
            {
                if (entry.Entity is BaseEntity entity)
                {
                    entity.IsDeleted = true;
                    entry.State = EntityState.Modified;
                }
            }
        }
    }
}