using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedImagePathToHeroSectionMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "HeroSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "HeroSections");
        }
    }
}
