using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanEnquiryApi.Migrations
{
    /// <inheritdoc />
    public partial class addwhatsappmessagestatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WhatsAppMessageStatus",
                table: "Enquiries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhatsAppMessageStatus",
                table: "Enquiries");
        }
    }
}
