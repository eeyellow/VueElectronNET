﻿// <auto-generated />
using System;
using ElectronApp.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ElectronApp.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240313032452_Departments.EstablishDate")]
    partial class DepartmentsEstablishDate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("ElectronApp.Database.Entities.Departments", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasComment("流水號");

                    b.Property<string>("Alias")
                        .HasColumnType("TEXT")
                        .HasComment("縮寫");

                    b.Property<DateTime>("CreateDatetime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("date('now')")
                        .HasComment("新增日期");

                    b.Property<DateTime>("EstablishDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("DateTime('now')")
                        .HasComment("成立日期");

                    b.Property<int>("IsDelete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("0")
                        .HasComment("是否刪除");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasComment("名稱");

                    b.Property<int>("ParentID")
                        .HasColumnType("INTEGER")
                        .HasComment("上層部門的ID");

                    b.Property<DateTime>("UpdateDatetime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("date('now')")
                        .HasComment("更新日期");

                    b.HasKey("ID");

                    b.HasIndex("IsDelete");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("ElectronApp.Database.Entities.UserProfiles", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasComment("流水號");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasComment("帳號");

                    b.Property<DateTime>("CreateDatetime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("date('now')")
                        .HasComment("新增日期");

                    b.Property<int>("IsDelete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("0")
                        .HasComment("是否刪除");

                    b.Property<string>("Mima")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasComment("密碼");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasComment("名稱");

                    b.Property<DateTime>("UpdateDatetime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("date('now')")
                        .HasComment("更新日期");

                    b.HasKey("ID");

                    b.HasIndex("IsDelete");

                    b.ToTable("UserProfiles");

                    b.HasData(
                        new
                        {
                            ID = 1L,
                            Account = "test@linkchain.tw",
                            CreateDatetime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDelete = 0,
                            Mima = "123456",
                            Name = "測試帳號",
                            UpdateDatetime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
