using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManangement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mg_48 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Students");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClassId",
                table: "Students",
                type: "uuid",
                nullable: true);
        }
    }
}
