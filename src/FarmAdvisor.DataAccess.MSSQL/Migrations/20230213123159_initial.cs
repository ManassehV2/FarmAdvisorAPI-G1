using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmAdvisor.DataAccess.MSSQL.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notification_farm_Notification_id",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "FK_sensor_field_sensor_id",
                table: "sensor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SensorDatas",
                table: "SensorDatas");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SensorDatas",
                newName: "SensorId");

            migrationBuilder.AddColumn<Guid>(
                name: "SensorDataId",
                table: "SensorDatas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "optimal_gdd",
                table: "sensor",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_forecast_date",
                table: "sensor",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_communication",
                table: "sensor",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "estimated_date",
                table: "sensor",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "battery_status",
                table: "sensor",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "FieldId",
                table: "sensor",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FarmId",
                table: "notification",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "accumulatedGdd",
                table: "field",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "forecastedGdd",
                table: "field",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SensorDatas",
                table: "SensorDatas",
                column: "SensorDataId");

            migrationBuilder.CreateTable(
                name: "CalculatedGDD",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<int>(type: "int", nullable: false),
                    currentValue = table.Column<int>(type: "int", nullable: false),
                    SensorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatedGDD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculatedGDD_sensor_SensorId",
                        column: x => x.SensorId,
                        principalTable: "sensor",
                        principalColumn: "sensor_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensorDatas_SensorId",
                table: "SensorDatas",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_sensor_FieldId",
                table: "sensor",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_notification_FarmId",
                table: "notification",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedGDD_SensorId",
                table: "CalculatedGDD",
                column: "SensorId");

            migrationBuilder.AddForeignKey(
                name: "FK_notification_farm_FarmId",
                table: "notification",
                column: "FarmId",
                principalTable: "farm",
                principalColumn: "farm_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sensor_field_FieldId",
                table: "sensor",
                column: "FieldId",
                principalTable: "field",
                principalColumn: "Field_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SensorDatas_sensor_SensorId",
                table: "SensorDatas",
                column: "SensorId",
                principalTable: "sensor",
                principalColumn: "sensor_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notification_farm_FarmId",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "FK_sensor_field_FieldId",
                table: "sensor");

            migrationBuilder.DropForeignKey(
                name: "FK_SensorDatas_sensor_SensorId",
                table: "SensorDatas");

            migrationBuilder.DropTable(
                name: "CalculatedGDD");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SensorDatas",
                table: "SensorDatas");

            migrationBuilder.DropIndex(
                name: "IX_SensorDatas_SensorId",
                table: "SensorDatas");

            migrationBuilder.DropIndex(
                name: "IX_sensor_FieldId",
                table: "sensor");

            migrationBuilder.DropIndex(
                name: "IX_notification_FarmId",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "SensorDataId",
                table: "SensorDatas");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "sensor");

            migrationBuilder.DropColumn(
                name: "FarmId",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "accumulatedGdd",
                table: "field");

            migrationBuilder.DropColumn(
                name: "forecastedGdd",
                table: "field");

            migrationBuilder.RenameColumn(
                name: "SensorId",
                table: "SensorDatas",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "optimal_gdd",
                table: "sensor",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_forecast_date",
                table: "sensor",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_communication",
                table: "sensor",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "estimated_date",
                table: "sensor",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "battery_status",
                table: "sensor",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SensorDatas",
                table: "SensorDatas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_notification_farm_Notification_id",
                table: "notification",
                column: "Notification_id",
                principalTable: "farm",
                principalColumn: "farm_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sensor_field_sensor_id",
                table: "sensor",
                column: "sensor_id",
                principalTable: "field",
                principalColumn: "Field_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
