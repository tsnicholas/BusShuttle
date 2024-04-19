using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class KeysRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Stops",
                table: "Stops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Routes",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Loops",
                table: "Loops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entries",
                table: "Entries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Drivers",
                table: "Drivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Buses",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Stops");

            migrationBuilder.AddColumn<int>(
                name: "StopId",
                table: "Routes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PrimaryKey_Id",
                table: "Stops",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PrimaryKey_Id",
                table: "Routes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PrimaryKey_Id",
                table: "Loops",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PrimaryKey_Id",
                table: "Entries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PrimaryKey_Id",
                table: "Drivers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PrimaryKey_Id",
                table: "Buses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_StopId",
                table: "Routes",
                column: "StopId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BusId",
                table: "Entries",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_DriverId",
                table: "Entries",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_LoopId",
                table: "Entries",
                column: "LoopId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_StopId",
                table: "Entries",
                column: "StopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Buses_BusId",
                table: "Entries",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Drivers_DriverId",
                table: "Entries",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Loops_LoopId",
                table: "Entries",
                column: "LoopId",
                principalTable: "Loops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Stops_StopId",
                table: "Entries",
                column: "StopId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Stops_StopId",
                table: "Routes",
                column: "StopId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Buses_BusId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Drivers_DriverId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Loops_LoopId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Stops_StopId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Stops_StopId",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PrimaryKey_Id",
                table: "Stops");

            migrationBuilder.DropPrimaryKey(
                name: "PrimaryKey_Id",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_StopId",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PrimaryKey_Id",
                table: "Loops");

            migrationBuilder.DropPrimaryKey(
                name: "PrimaryKey_Id",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_BusId",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_DriverId",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_LoopId",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_StopId",
                table: "Entries");

            migrationBuilder.DropPrimaryKey(
                name: "PrimaryKey_Id",
                table: "Drivers");

            migrationBuilder.DropPrimaryKey(
                name: "PrimaryKey_Id",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "StopId",
                table: "Routes");

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "Stops",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stops",
                table: "Stops",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Routes",
                table: "Routes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Loops",
                table: "Loops",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entries",
                table: "Entries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Drivers",
                table: "Drivers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Buses",
                table: "Buses",
                column: "Id");
        }
    }
}
