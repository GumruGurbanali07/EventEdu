using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BackToIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PersonalDatas");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "PersonalDatas");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "PersonalDatas",
                newName: "TwoFactorEnabled");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "PersonalDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "PersonalDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "PersonalDatas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "PersonalDatas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "PersonalDatas",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "PersonalDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "PersonalDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "PersonalDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "PersonalDatas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "PersonalDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "PersonalDatas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "PersonalDatas");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "PersonalDatas");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "PersonalDatas");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "PersonalDatas");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "PersonalDatas");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "PersonalDatas");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "PersonalDatas");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "PersonalDatas");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "PersonalDatas");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "PersonalDatas");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "PersonalDatas");

            migrationBuilder.RenameColumn(
                name: "TwoFactorEnabled",
                table: "PersonalDatas",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PersonalDatas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "PersonalDatas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
