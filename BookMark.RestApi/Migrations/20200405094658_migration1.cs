using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookMark.RestApi.Migrations
{
    public partial class migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentID = table.Column<long>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentID);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
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
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
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
                        name: "FK_Events_Organizations_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAppointment",
                columns: table => new
                {
                    UserAppointmentID = table.Column<long>(nullable: false),
                    UserID = table.Column<long>(nullable: false),
                    AppointmentID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAppointment", x => x.UserAppointmentID);
                    table.ForeignKey(
                        name: "FK_UserAppointment_Appointments_AppointmentID",
                        column: x => x.AppointmentID,
                        principalTable: "Appointments",
                        principalColumn: "AppointmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAppointment_Users_UserID",
                        column: x => x.UserID,
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
                        name: "FK_UserEvents_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEvents_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "OrganizationID", "Name", "Password" },
                values: new object[] { 637216588176268060L, "Revature", "$2a$11$iJUdOFrGEqvoYE87FMO/4e1M2.YGUB4epVYYk1Z.ZAu24Hi4Pjshu" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Name", "Password" },
                values: new object[] { 1L, "synaodev", "$2a$11$QEyeGMOrJxL8KXvGrxRFnuXkPwi7eKgIyt2cWWtM9GlF9AE93N5EC" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganizationID",
                table: "Events",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_UserAppointment_AppointmentID",
                table: "UserAppointment",
                column: "AppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_UserAppointment_UserID",
                table: "UserAppointment",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_EventID",
                table: "UserEvents",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_UserID",
                table: "UserEvents",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAppointment");

            migrationBuilder.DropTable(
                name: "UserEvents");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
