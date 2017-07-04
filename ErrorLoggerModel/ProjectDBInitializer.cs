using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace ErrorLoggerModel
{
    //class ProjectDBInitializer : DropCreateDatabaseIfModelChanges<ErrorModel>
    //{
    //    /// <summary>
    //    /// Seeds data into the DB
    //    /// </summary>
    //    protected override void Seed(ErrorModel context)
    //    {
    //        Console.WriteLine(" ### Seeding ###");

    //        // letting the base method do anything it needs to get done
    //        base.Seed(context);

    //        // Save the changes you made, when adding the data above
    //        context.SaveChanges();
    //    }
    //}

    internal sealed class ProjectDBInitializer : DbMigrationsConfiguration<ErrorLoggerModel.ErrorModel>
    {
        public ProjectDBInitializer()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "ErrorLoggerModel.ErrorModel";
        }
        /// <summary>
        /// Seeds data into the DB
        /// </summary>
        protected override void Seed(ErrorModel context)
        {
            //Console.WriteLine(" ### Seeding ###");

            // letting the base method do anything it needs to get done
            //base.Seed(context);

            // Save the changes you made, when adding the data above
            //context.SaveChanges();
        }
    }
}
