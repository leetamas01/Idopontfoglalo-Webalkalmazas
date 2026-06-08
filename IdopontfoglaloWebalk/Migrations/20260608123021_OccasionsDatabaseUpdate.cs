using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdopontfoglaloWebalk.Migrations
{
    /// <inheritdoc />
    public partial class OccasionsDatabaseUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "reservation_date",
                table: "Occasions",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<int>(
                name: "service_id",
                table: "Occasions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Occasions_service_id",
                table: "Occasions",
                column: "service_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Occasions_Services_service_id",
                table: "Occasions",
                column: "service_id",
                principalTable: "Services",
                principalColumn: "service_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Occasions_Services_service_id",
                table: "Occasions");

            migrationBuilder.DropIndex(
                name: "IX_Occasions_service_id",
                table: "Occasions");

            migrationBuilder.DropColumn(
                name: "service_id",
                table: "Occasions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "reservation_date",
                table: "Occasions",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);
        }
    }
}
