using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NET.Starter.DataAccess.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BadPasswordCount = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Security",
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserFcmTokens",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FcmToken = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFcmTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFcmTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Permissions",
                columns: new[] { "Id", "Created", "CreatedBy", "Modified", "ModifiedBy", "PermissionCode", "RowStatus" },
                values: new object[,]
                {
                    { new Guid("1a010b41-b5bf-4e15-8042-94bee253c835"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, "Security.Role.Menu", 0 },
                    { new Guid("6659f17a-c52e-4ec3-847b-46866a3b2abf"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, "MyPermission", 0 },
                    { new Guid("69821b03-b132-4d35-88f0-5502908d50fa"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, "IamAdministrator", 0 },
                    { new Guid("a0d9d4d0-1b6f-4e4c-8e5e-a7c6a3b2abf1"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, "Security.Permission.View", 0 },
                    { new Guid("a8b4b425-8827-4fde-b2c5-1fc3e059f061"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, "Security.Role.Create", 0 },
                    { new Guid("b3a934db-1d2b-40fd-b6df-00b9594637ce"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, "Security.Role.Delete", 0 },
                    { new Guid("e298741f-3027-4299-bf56-66bd712219e0"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, "Security.Role.View", 0 },
                    { new Guid("ef4baa13-1ed0-4284-8a77-2f6e315aecf8"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, "Security.Role.Update", 0 }
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "Id", "Created", "CreatedBy", "Modified", "ModifiedBy", "RoleCode", "RowStatus" },
                values: new object[] { new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, "Administrator", 0 });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Users",
                columns: new[] { "Id", "BadPasswordCount", "Created", "CreatedBy", "EmailAddress", "Fullname", "IsActive", "Modified", "ModifiedBy", "Password", "RowStatus", "Username" },
                values: new object[] { new Guid("73b4c7d1-e6a3-41dc-a8da-6d9a45092761"), 0, new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", "admin@example.com", "Administrator", true, null, null, "1234qwER", 0, "admin" });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "RolePermissions",
                columns: new[] { "Id", "Created", "CreatedBy", "Modified", "ModifiedBy", "PermissionId", "RoleId", "RowStatus" },
                values: new object[,]
                {
                    { new Guid("02abe492-24b8-4a90-af18-3a282f9fcc85"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, new Guid("69821b03-b132-4d35-88f0-5502908d50fa"), new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), 0 },
                    { new Guid("19dfa24e-a82a-449d-a1ce-11456ee5322c"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, new Guid("a0d9d4d0-1b6f-4e4c-8e5e-a7c6a3b2abf1"), new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), 0 },
                    { new Guid("223c7549-ddcc-4d05-95dc-9336c76a3e57"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, new Guid("ef4baa13-1ed0-4284-8a77-2f6e315aecf8"), new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), 0 },
                    { new Guid("2f0e1598-1ba3-495d-9ebe-673b15512c60"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, new Guid("1a010b41-b5bf-4e15-8042-94bee253c835"), new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), 0 },
                    { new Guid("58df83d3-2f40-450a-86d2-604dfa27fe35"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, new Guid("a8b4b425-8827-4fde-b2c5-1fc3e059f061"), new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), 0 },
                    { new Guid("971cf134-1f3e-4719-9d56-e60ed967a117"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, new Guid("6659f17a-c52e-4ec3-847b-46866a3b2abf"), new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), 0 },
                    { new Guid("c28724cb-c0c1-462c-8a71-3161dc638920"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, new Guid("b3a934db-1d2b-40fd-b6df-00b9594637ce"), new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), 0 },
                    { new Guid("f324ee94-14d8-4fab-a703-dbe0cecfdf30"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, new Guid("e298741f-3027-4299-bf56-66bd712219e0"), new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), 0 }
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "Id", "Created", "CreatedBy", "Modified", "ModifiedBy", "RoleId", "RowStatus", "UserId" },
                values: new object[] { new Guid("8595575f-0851-47b5-8950-7583a8f28927"), new DateTime(2025, 2, 12, 13, 30, 0, 0, DateTimeKind.Unspecified), "", null, null, new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), 0, new Guid("73b4c7d1-e6a3-41dc-a8da-6d9a45092761") });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_PermissionCode",
                schema: "Security",
                table: "Permissions",
                column: "PermissionCode",
                unique: true,
                filter: "[RowStatus] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                schema: "Security",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId_PermissionId",
                schema: "Security",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                unique: true,
                filter: "[RowStatus] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleCode",
                schema: "Security",
                table: "Roles",
                column: "RoleCode",
                unique: true,
                filter: "[RowStatus] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_UserFcmTokens_FcmToken",
                schema: "dbo",
                table: "UserFcmTokens",
                column: "FcmToken",
                unique: true,
                filter: "[RowStatus] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_UserFcmTokens_UserId",
                schema: "dbo",
                table: "UserFcmTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Security",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_RoleId",
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true,
                filter: "[RowStatus] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress",
                schema: "Security",
                table: "Users",
                column: "EmailAddress",
                unique: true,
                filter: "[RowStatus] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "Security",
                table: "Users",
                column: "Username",
                unique: true,
                filter: "[RowStatus] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "UserFcmTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Security");
        }
    }
}
