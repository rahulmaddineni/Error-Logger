namespace ErrorLoggerModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ErrorModel : DbContext
    {
        public ErrorModel()
            : base("ErrorModel")
        {
            // Set the custom initializer
            //Database.SetInitializer<ErrorModel>(new ProjectDBInitializer());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ErrorModel, ErrorLoggerModel.ProjectDBInitializer>("ErrorModel"));
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<ApplicationModel> Applications { get; set; }
        public DbSet<ErrorLogModel> ErrorLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
