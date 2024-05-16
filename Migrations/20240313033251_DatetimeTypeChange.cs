using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronApp.Migrations
{
    /// <inheritdoc />
    public partial class DatetimeTypeChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDatetime",
                table: "UserProfiles",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "DateTime('now')",
                comment: "更新日期",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "date('now')",
                oldComment: "更新日期");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDatetime",
                table: "UserProfiles",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "DateTime('now')",
                comment: "新增日期",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "date('now')",
                oldComment: "新增日期");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDatetime",
                table: "Departments",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "DateTime('now')",
                comment: "更新日期",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "date('now')",
                oldComment: "更新日期");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDatetime",
                table: "Departments",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "DateTime('now')",
                comment: "新增日期",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "date('now')",
                oldComment: "新增日期");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDatetime",
                table: "UserProfiles",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "date('now')",
                comment: "更新日期",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "DateTime('now')",
                oldComment: "更新日期");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDatetime",
                table: "UserProfiles",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "date('now')",
                comment: "新增日期",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "DateTime('now')",
                oldComment: "新增日期");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDatetime",
                table: "Departments",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "date('now')",
                comment: "更新日期",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "DateTime('now')",
                oldComment: "更新日期");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDatetime",
                table: "Departments",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "date('now')",
                comment: "新增日期",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "DateTime('now')",
                oldComment: "新增日期");
        }
    }
}
