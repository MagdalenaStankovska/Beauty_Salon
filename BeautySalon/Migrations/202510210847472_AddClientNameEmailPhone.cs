namespace BeautySalon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientNameEmailPhone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Clients", "MobilePhone", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "MobilePhone");
            DropColumn("dbo.Clients", "Email");
        }
    }
}
