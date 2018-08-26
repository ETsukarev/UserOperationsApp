using Microsoft.EntityFrameworkCore.Migrations;

namespace UserWebApi.Migrations
{
    public partial class MakeLoginAsUniqueKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(name: "Login", table: "Users", type: "nvarchar(20)");
            migrationBuilder.AddUniqueConstraint("FieldLoginUniqueConstraint", "Users", "Login");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint("FieldLoginUniqueConstraint", "Users");
            migrationBuilder.AlterColumn<string>(name: "Login", table: "Users", type: "nvarchar(max)");
        }
    }
}
