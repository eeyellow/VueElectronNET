using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronApp.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false, comment: "流水號")
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false, comment: "名稱"),
                    Account = table.Column<string>(type: "TEXT", nullable: false, comment: "帳號"),
                    Mima = table.Column<string>(type: "TEXT", nullable: false, comment: "密碼"),
                    IsDelete = table.Column<int>(type: "INTEGER", nullable: false, defaultValueSql: "0", comment: "是否刪除"),
                    CreateDatetime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "date('now')", comment: "新增日期"),
                    UpdateDatetime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "date('now')", comment: "更新日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "ID", "Account", "Mima", "Name" },
                values: new object[] { 1L, "test@linkchain.tw", "123456", "測試帳號" });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_IsDelete",
                table: "UserProfiles",
                column: "IsDelete");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");
        }
    }
}
