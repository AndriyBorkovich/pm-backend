﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjectManager.Persistence.Context;

#nullable disable

namespace ProjectManager.Persistence.Migrations
{
    [DbContext(typeof(ProjectDbContext))]
    [Migration("20241116143942_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<string>", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ProjectAssignments", b =>
                {
                    b.Property<string>("ParticipantsId")
                        .HasColumnType("text");

                    b.Property<int>("ProjectsId")
                        .HasColumnType("integer");

                    b.HasKey("ParticipantsId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("ProjectAssignments");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.Board", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("jsonb");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("TaskId")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.Resource", b =>
                {
                    b.Property<int>("ResourceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ResourceId"));

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.Property<string>("ResourceType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("ResourceId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AssigneeId")
                        .HasColumnType("text");

                    b.Property<int>("BoardId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatorId")
                        .HasColumnType("text");

                    b.Property<string>("CurrentStatus")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("CustomFields")
                        .HasColumnType("jsonb");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("BoardId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.TaskHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("PayloadClassName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PayloadJson")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<int>("TaskId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("TasksHistory");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("LastName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<int>("SendNotificationType")
                        .HasColumnType("integer");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<string>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ProjectManager.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ProjectManager.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<string>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ProjectManager.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectAssignments", b =>
                {
                    b.HasOne("ProjectManager.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("ParticipantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Core.Entities.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.Board", b =>
                {
                    b.HasOne("ProjectManager.Core.Entities.Project", "Project")
                        .WithMany("Boards")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.Notification", b =>
                {
                    b.HasOne("ProjectManager.Core.Entities.Task", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Task");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.Resource", b =>
                {
                    b.HasOne("ProjectManager.Core.Entities.Project", "Project")
                        .WithMany("Resources")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.Task", b =>
                {
                    b.HasOne("ProjectManager.Core.Entities.User", "Assignee")
                        .WithMany("AssignedTasks")
                        .HasForeignKey("AssigneeId");

                    b.HasOne("ProjectManager.Core.Entities.Board", "Board")
                        .WithMany("Tasks")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Core.Entities.User", "Creator")
                        .WithMany("CreatedTasks")
                        .HasForeignKey("CreatorId");

                    b.Navigation("Assignee");

                    b.Navigation("Board");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.TaskHistory", b =>
                {
                    b.HasOne("ProjectManager.Core.Entities.Task", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Task");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.Board", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.Project", b =>
                {
                    b.Navigation("Boards");

                    b.Navigation("Resources");
                });

            modelBuilder.Entity("ProjectManager.Core.Entities.User", b =>
                {
                    b.Navigation("AssignedTasks");

                    b.Navigation("CreatedTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
