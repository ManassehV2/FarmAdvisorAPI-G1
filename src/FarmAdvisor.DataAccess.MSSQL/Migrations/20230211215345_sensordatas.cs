using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmAdvisor.DataAccess.MSSQL.Migrations
{
    public partial class sensordatas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "state",
                table: "sensor",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "serial_number",
                table: "sensor",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "SensorDatas",
                columns: table => new
                {
                    serialNum = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_SensorDatas", x => x.serialNum);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorDatas");

            migrationBuilder.AlterColumn<string>(
                name: "state",
                table: "sensor",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "serial_number",
                table: "sensor",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
