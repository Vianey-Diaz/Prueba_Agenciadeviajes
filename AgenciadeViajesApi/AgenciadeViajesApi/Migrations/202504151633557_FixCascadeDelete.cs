namespace AgenciadeViajesApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Vueloes", new[] { "Destino_Id" });
            DropIndex("dbo.Vueloes", new[] { "Origen_Id" });
            RenameColumn(table: "dbo.Vueloes", name: "Destino_Id", newName: "DestinoId");
            RenameColumn(table: "dbo.Vueloes", name: "Origen_Id", newName: "OrigenId");
            AlterColumn("dbo.Vueloes", "DestinoId", c => c.Int(nullable: false));
            AlterColumn("dbo.Vueloes", "OrigenId", c => c.Int(nullable: false));
            // Cambiar 'Destinos' a 'Destinoes'
            AddForeignKey("dbo.Vueloes", "DestinoId", "dbo.Destinoes", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Vueloes", "OrigenId", "dbo.Destinoes", "Id", cascadeDelete: false);
            CreateIndex("dbo.Vueloes", "OrigenId");
            CreateIndex("dbo.Vueloes", "DestinoId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Vueloes", new[] { "DestinoId" });
            DropIndex("dbo.Vueloes", new[] { "OrigenId" });
            // Eliminar las claves foráneas al revertir
            DropForeignKey("dbo.Vueloes", "DestinoId", "dbo.Destinoes");
            DropForeignKey("dbo.Vueloes", "OrigenId", "dbo.Destinoes");
            AlterColumn("dbo.Vueloes", "OrigenId", c => c.Int());
            AlterColumn("dbo.Vueloes", "DestinoId", c => c.Int());
            RenameColumn(table: "dbo.Vueloes", name: "OrigenId", newName: "Origen_Id");
            RenameColumn(table: "dbo.Vueloes", name: "DestinoId", newName: "Destino_Id");
            CreateIndex("dbo.Vueloes", "Origen_Id");
            CreateIndex("dbo.Vueloes", "Destino_Id");
        }
    }
}
