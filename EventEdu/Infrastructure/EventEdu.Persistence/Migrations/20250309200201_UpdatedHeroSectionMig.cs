using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedHeroSectionMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "HeroSections");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "HeroSections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "HeroSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "HeroSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
