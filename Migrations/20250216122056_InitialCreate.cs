using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThomasonAlgorithm.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Experiments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VerticesNumber = table.Column<int>(type: "integer", nullable: false),
                    K = table.Column<int>(type: "integer", nullable: false),
                    MaxChordLength = table.Column<int>(type: "integer", nullable: false),
                    TimeToFindFirstCycle = table.Column<double>(type: "double precision", nullable: false),
                    TimeToFindSecondCycle = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Experiments");
        }
    }
}
