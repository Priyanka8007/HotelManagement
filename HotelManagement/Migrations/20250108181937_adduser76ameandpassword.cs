using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagement.Migrations
{
    /// <inheritdoc />
    public partial class adduser76ameandpassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "UserName4",
            //    table: "Hotels",
            //    newName: "UserNamee");
            migrationBuilder.AddColumn<string>(
               name: "UserNamee",
               table: "Hotels",
               nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
         name: "UserNamee",
         table: "Hotels");
        }
    }
}
