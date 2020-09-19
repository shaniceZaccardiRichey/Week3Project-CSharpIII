using System;
using System.Data.Entity;

namespace Week3Project
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new JobListingContext())
            {
                // Add employer.
                var employer = new Employer
                {
                    Name = "Microsoft",
                    Location = "Redmond, WA" 
                };

                db.Employers.Add(employer);
                db.SaveChanges();

                // Add job listings using employer object.
                var listing = new JobListing
                {
                    Title = "Junior Developer - Front End",
                    PayRate = 20.00,
                    StartDate = Convert.ToDateTime("10/01/2020"),
                    EmployerId = employer
                };

                db.JobListings.Add(listing);

                listing = new JobListing
                {
                    Title = "Senior Developer - Back End",
                    PayRate = 20.00,
                    StartDate = Convert.ToDateTime("11/01/2020"),
                    EmployerId = employer
                };

                db.JobListings.Add(listing);
                db.SaveChanges();

            }
        }
    }

    public class JobListing
    {
        public int JobListingId { get; set; } //primary key
        public string Title { get; set; }
        public double PayRate { get; set; }
        public DateTime StartDate { get; set; }
        public Employer EmployerId { get; set; } //foreign key
    }

    public class Employer
    {
        public int EmployerId { get; set; } //primary key
        public string Name { get; set; }
        public string Location { get; set; }

    }

    public class JobListingContext : DbContext
    {
        public JobListingContext() { }

        public JobListingContext(string connString) 
        {
            Database.Connection.ConnectionString = connString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<JobListing>().MapToStoredProcedures
            (p => p.Insert(sp => sp.HasName("sp_InsertJobListing"))
                .Update(sp => sp.HasName("sp_UpdateJobListing"))
                .Delete(sp => sp.HasName("sp_DeleteJobListing"))
            );

            modelBuilder.Entity<Employer>().MapToStoredProcedures
            (p => p.Insert(sp => sp.HasName ("sp_InsertEmployer"))
                .Update(sp => sp.HasName ("sp_UpdateEmployer"))
                .Delete(sp => sp.HasName("sp_DeleteEmployer"))
            );
        }

        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<Employer> Employers { get; set; }
    }
}
