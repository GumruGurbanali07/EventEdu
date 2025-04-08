using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mg8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventSpeakers_Speakers_SpeakerId",
                table: "EventSpeakers");

            migrationBuilder.DropForeignKey(
                name: "FK_EventSponsors_Sponsors_SponsorId",
                table: "EventSponsors");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedBacks_Events_EventId",
                table: "FeedBacks");

            migrationBuilder.DropForeignKey(
                name: "FK_SubsEvents_Subscriptions_SubscriptionId",
                table: "SubsEvents");

            migrationBuilder.DropTable(
                name: "FeedBackDetails");

            migrationBuilder.DropColumn(
                name: "RatingEvenets",
                table: "FeedBacks");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "FeedBacks",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "FeedBacks",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "FeedBacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "FeedBacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "TotalRating",
                table: "FeedBacks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RoleModel",
                table: "AspNetRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_EventSpeakers_Speakers_SpeakerId",
                table: "EventSpeakers",
                column: "SpeakerId",
                principalTable: "Speakers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventSponsors_Sponsors_SponsorId",
                table: "EventSponsors",
                column: "SponsorId",
                principalTable: "Sponsors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBacks_Events_EventId",
                table: "FeedBacks",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubsEvents_Subscriptions_SubscriptionId",
                table: "SubsEvents",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventSpeakers_Speakers_SpeakerId",
                table: "EventSpeakers");

            migrationBuilder.DropForeignKey(
                name: "FK_EventSponsors_Sponsors_SponsorId",
                table: "EventSponsors");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedBacks_Events_EventId",
                table: "FeedBacks");

            migrationBuilder.DropForeignKey(
                name: "FK_SubsEvents_Subscriptions_SubscriptionId",
                table: "SubsEvents");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "FeedBacks");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "FeedBacks");

            migrationBuilder.DropColumn(
                name: "TotalRating",
                table: "FeedBacks");

            migrationBuilder.DropColumn(
                name: "RoleModel",
                table: "AspNetRoles");

            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "FeedBacks",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "FeedBacks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RatingEvenets",
                table: "FeedBacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FeedBackDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeedBackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBackDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedBackDetails_FeedBacks_FeedBackId",
                        column: x => x.FeedBackId,
                        principalTable: "FeedBacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedBackDetails_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedBackDetails_FeedBackId",
                table: "FeedBackDetails",
                column: "FeedBackId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBackDetails_LanguageId",
                table: "FeedBackDetails",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventSpeakers_Speakers_SpeakerId",
                table: "EventSpeakers",
                column: "SpeakerId",
                principalTable: "Speakers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventSponsors_Sponsors_SponsorId",
                table: "EventSponsors",
                column: "SponsorId",
                principalTable: "Sponsors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBacks_Events_EventId",
                table: "FeedBacks",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubsEvents_Subscriptions_SubscriptionId",
                table: "SubsEvents",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
