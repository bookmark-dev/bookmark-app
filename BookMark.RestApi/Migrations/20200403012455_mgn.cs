using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookMark.RestApi.Migrations
{
    public partial class mgn : Migration
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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Name", "Password" },
                values: new object[] { 1L, "synaodev", "$2a$11$vNhy7C7CoYNcWJZwkhb1ceXlGlMY7eqV3hU4MFpjAOhm9ugI9Q3tK" });

            migrationBuilder.CreateIndex(
                name: "IX_UserAppointment_AppointmentID",
                table: "UserAppointment",
                column: "AppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_UserAppointment_UserID",
                table: "UserAppointment",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAppointment");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
