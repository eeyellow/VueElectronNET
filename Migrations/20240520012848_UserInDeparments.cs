using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronApp.Migrations
{
    /// <inheritdoc />
    public partial class UserInDeparments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInDepartments",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false, comment: "流水號")
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<long>(type: "INTEGER", nullable: false, comment: "人員ID"),
                    DepartmentID = table.Column<long>(type: "INTEGER", nullable: false, comment: "部門ID"),
                    IsDelete = table.Column<int>(type: "INTEGER", nullable: false, defaultValueSql: "0", comment: "是否刪除"),
                    CreateDatetime = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "DateTime('now')", comment: "新增日期"),
                    UpdateDatetime = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "DateTime('now')", comment: "更新日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInDepartments", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInDepartments_IsDelete",
                table: "UserInDepartments",
                column: "IsDelete");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInDepartments");
        }
    }
}
