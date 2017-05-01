using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using _502Finder.Models.Data;

namespace _502Finder.Data
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Strain> Strains { get; set; }

        public DataContext() : base("name=DefaultConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DataContext>());
            Configuration.LazyLoadingEnabled = true;
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<Dispensary>().ToTable("Dispensaries");
            modelBuilder.Entity<Strain>().ToTable("Strains");
        }

        public DbQuery<T> Query<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }
    }
}
