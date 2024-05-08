using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanEnquiryApi.Migrations
{
    /// <inheritdoc />
    public partial class updatetable_enquiry_addbankid_removerecommendedbank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendedBankIds",
                table: "Enquiries");

            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "Enquiries",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_BankId",
                table: "Enquiries",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enquiries_Banks_BankId",
                table: "Enquiries",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enquiries_Banks_BankId",
                table: "Enquiries");

            migrationBuilder.DropIndex(
                name: "IX_Enquiries_BankId",
                table: "Enquiries");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "Enquiries");

            migrationBuilder.AddColumn<string>(
                name: "RecommendedBankIds",
                table: "Enquiries",
                type: "longtext",
                nullable: false);
        }
    }
}
