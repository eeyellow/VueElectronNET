using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronApp.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentsEstablishDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EstablishDate",
                table: "Departments",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "DateTime('now')",
                comment: "成立日期");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstablishDate",
                table: "Departments");
        }
    }
}
