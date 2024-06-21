using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronApp.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentsIsEnable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsEnable",
                table: "Departments",
                type: "INTEGER",
                nullable: false,
                defaultValueSql: "1",
                comment: "是否啟用");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnable",
                table: "Departments");
        }
    }
}
