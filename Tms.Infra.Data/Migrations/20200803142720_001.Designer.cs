﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tms.Infra.Data;

namespace Tms.Infra.Data.Migrations
{
    [DbContext(typeof(TmsDbContext))]
    [Migration("20200803142720_001")]
    partial class _001
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Tms.Domain.SubTask", b =>
                {
                    b.Property<int?>("ParentTaskId")
                        .HasColumnType("int");

                    b.Property<int?>("ChildTaskId")
                        .HasColumnType("int");

                    b.HasKey("ParentTaskId", "ChildTaskId");

                    b.HasIndex("ChildTaskId");

                    b.ToTable("SubTasks");
                });

            modelBuilder.Entity("Tms.Domain.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("ChangeDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FinishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<int?>("ParentTaskId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TaskState")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Task");
                });

            modelBuilder.Entity("Tms.Domain.SubTask", b =>
                {
                    b.HasOne("Tms.Domain.Task", "ChildTask")
                        .WithMany()
                        .HasForeignKey("ChildTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tms.Domain.Task", "ParentTask")
                        .WithMany("SubTasks")
                        .HasForeignKey("ParentTaskId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
