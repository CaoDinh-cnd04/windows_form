using Empty_framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace StudentManagement
{
    public partial class StudentContextDB : DbContext
    {
        public StudentContextDB()
            : base("name=StudentContextDB")
        {
        }
   

        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .Property(e => e.AverageScore);
                
        }

        public void ThucHienCacHoatDongSinhVien(int ID)
        {

            StudentContextDB context = new StudentContextDB();
            List<Student> listStudent = context.Students.ToList();
            Student db = context.Students.FirstOrDefault(p => p.StudentID == ID);
            Student s = new Student() { StudentID = 99, FullName = "test insert", AverageScore = 10 };
            context.Students.Add(s);
            context.SaveChanges();
            Student dbUpdate = context.Students.FirstOrDefault(p => p.StudentID == ID);
            if (dbUpdate != null)
            {
                dbUpdate.FullName = "Update FullName";
                context.SaveChanges();
            }

            Student dbDelete = context.Students.FirstOrDefault(p => p.StudentID == ID);
            if (dbDelete != null)
            {
                context.Students.Remove(dbDelete);
                context.SaveChanges();
            }
            context.Students.AddOrUpdate(s);
            context.SaveChanges();
        }
    }
}
