using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronApp.Migrations
{
    /// <inheritdoc />
    public partial class UploadFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UploadFile",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false, comment: "流水號")
                        .Annotation("Sqlite:Autoincrement", true),
                    RefID = table.Column<long>(type: "INTEGER", nullable: false, comment: "參照資料編號"),
                    RefType = table.Column<int>(type: "INTEGER", nullable: false, comment: "參照功能編號"),
                    Name = table.Column<string>(type: "TEXT", nullable: true, comment: "檔案名稱"),
                    FileName = table.Column<string>(type: "TEXT", nullable: true, comment: "實體檔案名稱"),
                    ContentType = table.Column<string>(type: "TEXT", nullable: true, comment: "ContentType"),
                    IsDelete = table.Column<int>(type: "INTEGER", nullable: false, defaultValueSql: "0", comment: "是否刪除"),
                    CreateDatetime = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "DateTime('now')", comment: "新增日期"),
                    UpdateDatetime = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "DateTime('now')", comment: "更新日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFile", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploadFile_IsDelete",
                table: "UploadFile",
                column: "IsDelete");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadFile");
        }
    }
}
