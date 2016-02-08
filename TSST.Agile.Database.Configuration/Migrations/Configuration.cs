namespace TSST.Agile.Database.Configuration.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TSST.Agile.Database.Configuration.Implementations.AgileDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TSST.Agile.Database.Configuration.Implementations.AgileDbContext context)
        {
            
        }
    }
}
