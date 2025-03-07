using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBacks_AspNetUsers_AppUserId",
                table: "FeedBacks");

            migrationBuilder.DropTable(
                name: "CommentDetails");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_FeedBacks_AppUserId",
                table: "FeedBacks");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "FeedBacks");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FeedBacks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBacks_UserId",
                table: "FeedBacks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBacks_AspNetUsers_UserId",
                table: "FeedBacks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBacks_AspNetUsers_UserId",
                table: "FeedBacks");

            migrationBuilder.DropIndex(
                name: "IX_FeedBacks_UserId",
                table: "FeedBacks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FeedBacks");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "FeedBacks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommentDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentDetails_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentDetails_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedBacks_AppUserId",
                table: "FeedBacks",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentDetails_CommentId",
                table: "CommentDetails",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentDetails_LanguageId",
                table: "CommentDetails",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AppUserId",
                table: "Comments",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBacks_AspNetUsers_AppUserId",
                table: "FeedBacks",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
