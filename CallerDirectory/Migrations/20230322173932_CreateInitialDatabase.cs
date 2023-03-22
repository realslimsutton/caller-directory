using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CallerDirectory.Migrations
{
    /// <inheritdoc />
    public partial class CreateInitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CallRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Caller = table.Column<long>(type: "bigint", nullable: true),
                    Recipient = table.Column<long>(type: "bigint", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Cost = table.Column<float>(type: "float", nullable: false),
                    Reference = table.Column<string>(type: "VARCHAR(33)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Currency = table.Column<string>(type: "VARCHAR(3)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallRecords", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CallRecords_Caller",
                table: "CallRecords",
                column: "Caller");

            migrationBuilder.CreateIndex(
                name: "IX_CallRecords_EndDateTime",
                table: "CallRecords",
                column: "EndDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_CallRecords_Recipient",
                table: "CallRecords",
                column: "Recipient");

            migrationBuilder.CreateIndex(
                name: "IX_CallRecords_Reference",
                table: "CallRecords",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CallRecords_StartDateTime",
                table: "CallRecords",
                column: "StartDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallRecords");
        }
    }
}
