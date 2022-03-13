using Microsoft.EntityFrameworkCore.Migrations;

namespace hotel_booking_data.Migrations
{
    public partial class ManagerRequestUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HotelAddress",
                table: "ManagerRequests",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HotelAddress",
                table: "ManagerRequests");
        }
    }
}
