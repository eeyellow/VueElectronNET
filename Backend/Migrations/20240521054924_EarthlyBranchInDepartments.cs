using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronApp.Migrations
{
    /// <inheritdoc />
    public partial class EarthlyBranchInDepartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EarthlyBranchInDepartments",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false, comment: "流水號")
                        .Annotation("Sqlite:Autoincrement", true),
                    EnumValue = table.Column<int>(type: "INTEGER", nullable: false, comment: "列舉值"),
                    DepartmentID = table.Column<long>(type: "INTEGER", nullable: false, comment: "部門ID"),
                    IsDelete = table.Column<int>(type: "INTEGER", nullable: false, defaultValueSql: "0", comment: "是否刪除"),
                    CreateDatetime = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "DateTime('now')", comment: "新增日期"),
                    UpdateDatetime = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "DateTime('now')", comment: "更新日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EarthlyBranchInDepartments", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EarthlyBranchInDepartments_IsDelete",
                table: "EarthlyBranchInDepartments",
                column: "IsDelete");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EarthlyBranchInDepartments");
        }
    }
}
