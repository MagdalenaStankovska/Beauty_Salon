using System;
using System.Data.Entity.Migrations;

namespace BeautySalon.Migrations
{
    public partial class FixAppointmentsTable : DbMigration
    {
        public override void Up()
        {
            // Add the 'date' column if it doesn't exist
            AddColumn("dbo.Appoitments", "date", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
        }

        public override void Down()
        {
            // Remove the 'date' column if rolling back
            DropColumn("dbo.Appoitments", "date");
        }
    }
}
