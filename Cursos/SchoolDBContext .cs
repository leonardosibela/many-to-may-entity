namespace Cursos
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class SchoolDBContext : DbContext
    {
        public SchoolDBContext() : base("SchoolDB-DataAnnotations")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Student>()
                        .HasMany<Course>(s => s.Courses)
                        .WithMany(c => c.Students)
                        .Map(cs =>
                        {
                            cs.MapLeftKey("StudentRefId");
                            cs.MapRightKey("CourseRefId");
                            cs.ToTable("StudentCourse");
                        });

        }
    }
}