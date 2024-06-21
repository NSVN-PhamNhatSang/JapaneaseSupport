﻿// <auto-generated />
using System;
using JLearning.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JLearning.Migrations
{
    [DbContext(typeof(WebContext))]
    [Migration("20240601010310_AddUserCourseEntity")]
    partial class AddUserCourseEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("JLearning.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ImgUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("Roll")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserCategory")
                        .HasColumnType("longtext");

                    b.Property<int?>("UserLevel")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserPassword")
                        .HasColumnType("longtext");

                    b.Property<string>("UserPoint")
                        .HasColumnType("longtext");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("JLearning.Models.UserCourse", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double?>("CourseNote")
                        .HasColumnType("double");

                    b.Property<string>("CoursePoint")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CourseStatus")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CurrentWord")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("LearnedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserProgress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("UserId");

                    b.ToTable("UserCourse");
                });

            modelBuilder.Entity("JLearning.Models.UserDetail", b =>
                {
                    b.Property<string>("userId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("userId");

                    b.ToTable("UserDetail");
                });
#pragma warning restore 612, 618
        }
    }
}
