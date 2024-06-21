using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronApp.Migrations
{
    /// <inheritdoc />
    public partial class Departments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false, comment: "流水號")
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false, comment: "名稱"),
                    Alias = table.Column<string>(type: "TEXT", nullable: true, comment: "縮寫"),
                    ParentID = table.Column<int>(type: "INTEGER", nullable: false, comment: "上層部門的ID"),
                    IsDelete = table.Column<int>(type: "INTEGER", nullable: false, defaultValueSql: "0", comment: "是否刪除"),
                    CreateDatetime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "date('now')", comment: "新增日期"),
                    UpdateDatetime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "date('now')", comment: "更新日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_IsDelete",
                table: "Departments",
                column: "IsDelete");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
