using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace EFA_DEMO.Migrations
{
    public class Configuration : DbMigrationsConfiguration<EFA_DEMO.Data.EFAContext>
    {

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "EFA_DEMO.Data.EFAContext";
        }

        protected override void Seed(EFA_DEMO.Data.EFAContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}