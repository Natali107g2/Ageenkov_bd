using Microsoft.EntityFrameworkCore;

namespace AgeenkovApp.Model {
    public class AggContext : DbContext {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<AreaCoords> AreaCoords { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileCoords> ProfileCoords { get; set; }
        public DbSet<Picket> Pickets { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<Measuring> Measurings { get; set; }
        public static string ConnectionString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer($@"Server=(localdb)\mssqllocaldb;Database={ConnectionString};Trusted_Connection=True;");
        }

        public AggContext() { }
        public AggContext(string connectionStr) {
            ConnectionString = connectionStr;
        }

        private static AggContext loadAll;
        public static AggContext LoadAll(){
            if(loadAll == null) { 
                loadAll = new AggContext(ConnectionString);

                loadAll.Customers.Load();
                loadAll.Projects.Load();
                loadAll.Areas.Load();
                loadAll.AreaCoords.Load();
                loadAll.Profiles.Load();
                loadAll.ProfileCoords.Load();
                loadAll.Pickets.Load();
                loadAll.Operators.Load();
                loadAll.Measurings.Load();
                loadAll.SaveChanges();
            }
            return loadAll;
        }
    }    
}
