using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmAdvisor.DataAccess.MSSQL.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SensorDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    serialNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    batteryStatus = table.Column<bool>(type: "bit", nullable: true),
                    measurementPeriodBase = table.Column<int>(type: "int", nullable: false),
                    nextTransmissionAt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    signal = table.Column<int>(type: "int", nullable: false),
                    timeStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    startPoint = table.Column<int>(type: "int", nullable: false),
                    sampleOffsets = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cloudToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    user_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "user"),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "user@test.com"),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "System.Random"),
                    auth_id = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, defaultValue: "token")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "farm",
                columns: table => new
                {
                    farm_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    farm_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, defaultValue: "Farm mock"),
                    postcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, defaultValue: "12,AA"),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "Addis Ababa"),
                    country = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, defaultValue: "Ethio"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_farm", x => x.farm_id);
                    table.ForeignKey(
                        name: "FK_farm_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "field",
                columns: table => new
                {
                    Field_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    Field_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    altitude = table.Column<int>(type: "int", nullable: false),
                    polygon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FarmId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_field", x => x.Field_id);
                    table.ForeignKey(
                        name: "FK_field_farm_FarmId",
                        column: x => x.FarmId,
                        principalTable: "farm",
                        principalColumn: "farm_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    Notification_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    message = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    sent_by = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.Notification_id);
                    table.ForeignKey(
                        name: "FK_notification_farm_FarmId",
                        column: x => x.FarmId,
                        principalTable: "farm",
                        principalColumn: "farm_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sensor",
                columns: table => new
                {
                    sensor_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    serial_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    last_communication = table.Column<DateTime>(type: "datetime2", nullable: true),
                    battery_status = table.Column<int>(type: "int", nullable: true),
                    optimal_gdd = table.Column<int>(type: "int", nullable: true),
                    estimated_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_forecast_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: false),
                    longitude = table.Column<double>(type: "float", nullable: false),
                    state = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sensor", x => x.sensor_id);
                    table.ForeignKey(
                        name: "FK_sensor_field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "field",
                        principalColumn: "Field_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_farm_UserId",
                table: "farm",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_field_FarmId",
                table: "field",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_notification_FarmId",
                table: "notification",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_sensor_FieldId",
                table: "sensor",
                column: "FieldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "sensor");

            migrationBuilder.DropTable(
                name: "SensorDatas");

            migrationBuilder.DropTable(
                name: "field");

            migrationBuilder.DropTable(
                name: "farm");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
