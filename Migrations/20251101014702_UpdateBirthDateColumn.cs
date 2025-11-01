using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultorioDB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBirthDateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Patients",
                type: "DATE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Patients",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATE");
        }
    }
}
