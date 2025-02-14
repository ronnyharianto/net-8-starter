﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NET.Starter.DataAccess.SqlServer;

#nullable disable

namespace NET.Starter.DataAccess.SqlServer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NET.Starter.DataAccess.SqlServer.Models.Security.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PermissionCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RowStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PermissionCode")
                        .IsUnique()
                        .HasFilter("[RowStatus] = 0");

                    b.ToTable("Permissions", "Security");

                    b.HasData(
                        new
                        {
                            Id = new Guid("6659f17a-c52e-4ec3-847b-46866a3b2abf"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionCode = "MyPermission",
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("69821b03-b132-4d35-88f0-5502908d50fa"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionCode = "IamAdministrator",
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("a0d9d4d0-1b6f-4e4c-8e5e-a7c6a3b2abf1"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionCode = "Security.Permission.View",
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("1a010b41-b5bf-4e15-8042-94bee253c835"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionCode = "Security.Role.Menu",
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("e298741f-3027-4299-bf56-66bd712219e0"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionCode = "Security.Role.View",
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("a8b4b425-8827-4fde-b2c5-1fc3e059f061"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionCode = "Security.Role.Create",
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("ef4baa13-1ed0-4284-8a77-2f6e315aecf8"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionCode = "Security.Role.Update",
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("b3a934db-1d2b-40fd-b6df-00b9594637ce"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionCode = "Security.Role.Delete",
                            RowStatus = 0
                        });
                });

            modelBuilder.Entity("NET.Starter.DataAccess.SqlServer.Models.Security.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RowStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleCode")
                        .IsUnique()
                        .HasFilter("[RowStatus] = 0");

                    b.ToTable("Roles", "Security");

                    b.HasData(
                        new
                        {
                            Id = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            RoleCode = "Administrator",
                            RowStatus = 0
                        });
                });

            modelBuilder.Entity("NET.Starter.DataAccess.SqlServer.Models.Security.RolePermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RowStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId", "PermissionId")
                        .IsUnique()
                        .HasFilter("[RowStatus] = 0");

                    b.ToTable("RolePermissions", "Security");

                    b.HasData(
                        new
                        {
                            Id = new Guid("971cf134-1f3e-4719-9d56-e60ed967a117"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionId = new Guid("6659f17a-c52e-4ec3-847b-46866a3b2abf"),
                            RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"),
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("02abe492-24b8-4a90-af18-3a282f9fcc85"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionId = new Guid("69821b03-b132-4d35-88f0-5502908d50fa"),
                            RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"),
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("19dfa24e-a82a-449d-a1ce-11456ee5322c"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionId = new Guid("a0d9d4d0-1b6f-4e4c-8e5e-a7c6a3b2abf1"),
                            RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"),
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("2f0e1598-1ba3-495d-9ebe-673b15512c60"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionId = new Guid("1a010b41-b5bf-4e15-8042-94bee253c835"),
                            RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"),
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("f324ee94-14d8-4fab-a703-dbe0cecfdf30"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionId = new Guid("e298741f-3027-4299-bf56-66bd712219e0"),
                            RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"),
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("58df83d3-2f40-450a-86d2-604dfa27fe35"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionId = new Guid("a8b4b425-8827-4fde-b2c5-1fc3e059f061"),
                            RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"),
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("223c7549-ddcc-4d05-95dc-9336c76a3e57"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionId = new Guid("ef4baa13-1ed0-4284-8a77-2f6e315aecf8"),
                            RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"),
                            RowStatus = 0
                        },
                        new
                        {
                            Id = new Guid("c28724cb-c0c1-462c-8a71-3161dc638920"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            PermissionId = new Guid("b3a934db-1d2b-40fd-b6df-00b9594637ce"),
                            RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"),
                            RowStatus = 0
                        });
                });

            modelBuilder.Entity("NET.Starter.DataAccess.SqlServer.Models.Security.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BadPasswordCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("RowStatus")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress")
                        .IsUnique()
                        .HasFilter("[RowStatus] = 0");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[RowStatus] = 0");

                    b.ToTable("Users", "Security");

                    b.HasData(
                        new
                        {
                            Id = new Guid("73b4c7d1-e6a3-41dc-a8da-6d9a45092761"),
                            BadPasswordCount = 0,
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            EmailAddress = "admin@example.com",
                            Fullname = "Administrator",
                            IsActive = true,
                            Password = "1234qwER",
                            RowStatus = 0,
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("NET.Starter.DataAccess.SqlServer.Models.Security.UserFcmToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FcmToken")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RowStatus")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FcmToken")
                        .IsUnique()
                        .HasFilter("[RowStatus] = 0");

                    b.HasIndex("UserId");

                    b.ToTable("UserFcmTokens", "dbo");
                });

            modelBuilder.Entity("NET.Starter.DataAccess.SqlServer.Models.Security.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RowStatus")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId", "RoleId")
                        .IsUnique()
                        .HasFilter("[RowStatus] = 0");

                    b.ToTable("UserRoles", "Security");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8595575f-0851-47b5-8950-7583a8f28927"),
                            Created = new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = "",
                            RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"),
                            RowStatus = 0,
                            UserId = new Guid("73b4c7d1-e6a3-41dc-a8da-6d9a45092761")
                        });
                });

            modelBuilder.Entity("NET.Starter.DataAccess.SqlServer.Models.Security.RolePermission", b =>
                {
                    b.HasOne("NET.Starter.DataAccess.SqlServer.Models.Security.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NET.Starter.DataAccess.SqlServer.Models.Security.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("NET.Starter.DataAccess.SqlServer.Models.Security.UserFcmToken", b =>
                {
                    b.HasOne("NET.Starter.DataAccess.SqlServer.Models.Security.User", "User")
                        .WithMany("UserFcmTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NET.Starter.DataAccess.SqlServer.Models.Security.UserRole", b =>
                {
                    b.HasOne("NET.Starter.DataAccess.SqlServer.Models.Security.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NET.Starter.DataAccess.SqlServer.Models.Security.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NET.Starter.DataAccess.SqlServer.Models.Security.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("NET.Starter.DataAccess.SqlServer.Models.Security.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("NET.Starter.DataAccess.SqlServer.Models.Security.User", b =>
                {
                    b.Navigation("UserFcmTokens");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
