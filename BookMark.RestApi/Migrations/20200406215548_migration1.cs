using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookMark.RestApi.Migrations
{
    public partial class migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentGroups",
                columns: table => new
                {
                    AppointmentGroupID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Info = table.Column<string>(nullable: true),
                    OrganizationID = table.Column<long>(nullable: false),
                    IsPublic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentGroups", x => x.AppointmentGroupID);
                    table.ForeignKey(
                        name: "FK_AppointmentGroups_Organizations_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Info = table.Column<string>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false),
                    OrganizationID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_Events_Organizations_EventID",
                        column: x => x.EventID,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentID = table.Column<long>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    AppointmentGroupID = table.Column<long>(nullable: false),
                    UserID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentID);
                    table.ForeignKey(
                        name: "FK_Appointments_AppointmentGroups_AppointmentGroupID",
                        column: x => x.AppointmentGroupID,
                        principalTable: "AppointmentGroups",
                        principalColumn: "AppointmentGroupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_AppointmentID",
                        column: x => x.AppointmentID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEvents",
                columns: table => new
                {
                    UserEventID = table.Column<long>(nullable: false),
                    UserID = table.Column<long>(nullable: false),
                    EventID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEvents", x => x.UserEventID);
                    table.ForeignKey(
                        name: "FK_UserEvents_Events_UserEventID",
                        column: x => x.UserEventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEvents_Users_UserEventID",
                        column: x => x.UserEventID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "OrganizationID", "Email", "Name", "Password" },
                values: new object[] { 637217889481259457L, "Revature@Mail.com", "Revature", "$2a$11$zIi.pmS/ajWJYGgcXlK.4ebCoyxwDw15XWJv2j6FSEL6.IR9NcU56" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Email", "Name", "Password" },
                values: new object[] { 1L, null, "synaodev", "$2a$11$RO/C4HsZcEx.wSpk2TQxRe21eC3JvwgQxhgUHH9vdvIE6XUenZF7a" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Email", "Name", "Password" },
                values: new object[] { 2L, null, "Adrienne", "$2a$11$A81w4peyljR3nUkL9xZrA.Ikl5lMry1G62QIs/M4Te.7mKvqerH4G" });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentGroups_OrganizationID",
                table: "AppointmentGroups",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentGroupID",
                table: "Appointments",
                column: "AppointmentGroupID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "UserEvents");

            migrationBuilder.DropTable(
                name: "AppointmentGroups");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
