using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MNApp.DAL.Migrations
{
    public partial class DomainDetailAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DomainDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    IsPing = table.Column<bool>(nullable: true),
                    HasDate = table.Column<bool>(nullable: true),
                    HasMedia = table.Column<bool>(nullable: true),
                    HasMail = table.Column<bool>(nullable: true),
                    HasContact = table.Column<bool>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    RegisterAt = table.Column<DateTime>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    ExpiryAt = table.Column<DateTime>(nullable: true),
                    FileDetailId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DomainDetails_FileDetails_FileDetailId",
                        column: x => x.FileDetailId,
                        principalTable: "FileDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DomainDetails_FileDetailId",
                table: "DomainDetails",
                column: "FileDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomainDetails");

            migrationBuilder.DropTable(
                name: "FileDetails");
        }
    }
}
