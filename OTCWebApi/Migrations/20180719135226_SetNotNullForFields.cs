using Microsoft.EntityFrameworkCore.Migrations;

namespace UserWebApi.Migrations
{
    public partial class SetNotNullForFields : Migration
    {
       protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(name: "Login", table: "Users", nullable: false);
            migrationBuilder.AlterColumn<string>(name: "Password", table: "Users", nullable: false);
            migrationBuilder.AlterColumn<string>(name: "FirstName", table: "Users", nullable: false);
            migrationBuilder.AlterColumn<string>(name: "MiddleName", table: "Users", nullable: false);
            migrationBuilder.AlterColumn<string>(name: "LastName", table: "Users", nullable: false);
            migrationBuilder.AlterColumn<string>(name: "Telephone", table: "Users", nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(name: "Login", table: "Users", nullable: true);
            migrationBuilder.AlterColumn<string>(name: "Password", table: "Users", nullable: true);
            migrationBuilder.AlterColumn<string>(name: "FirstName", table: "Users", nullable: true);
            migrationBuilder.AlterColumn<string>(name: "MiddleName", table: "Users", nullable: true);
            migrationBuilder.AlterColumn<string>(name: "LastName", table: "Users", nullable: true);
            migrationBuilder.AlterColumn<string>(name: "Telephone", table: "Users", nullable: true);
        }
    }
}
