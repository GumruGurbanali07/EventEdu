using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedSponsorsDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactInfos_Sponsors_SponsorId",
                table: "ContactInfos");

            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropIndex(
                name: "IX_ContactInfos_SponsorId",
                table: "ContactInfos");

            migrationBuilder.DropColumn(
                name: "SponsorId",
                table: "ContactInfos");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Sponsors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Sponsors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Sponsors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SponsorDescription",
                table: "SponsorDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "SponsorDescription",
                table: "SponsorDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "SponsorId",
                table: "ContactInfos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeroSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SponsorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AboutSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medias_AboutSections_AboutSectionId",
                        column: x => x.AboutSectionId,
                        principalTable: "AboutSections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Medias_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Medias_HeroSections_HeroSectionId",
                        column: x => x.HeroSectionId,
                        principalTable: "HeroSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medias_Sponsors_SponsorId",
                        column: x => x.SponsorId,
                        principalTable: "Sponsors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfos_SponsorId",
                table: "ContactInfos",
                column: "SponsorId");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_AboutSectionId",
                table: "Medias",
                column: "AboutSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_EventId",
                table: "Medias",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_HeroSectionId",
                table: "Medias",
                column: "HeroSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_SponsorId",
                table: "Medias",
                column: "SponsorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactInfos_Sponsors_SponsorId",
                table: "ContactInfos",
                column: "SponsorId",
                principalTable: "Sponsors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
