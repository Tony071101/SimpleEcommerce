using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleEcommerce_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTalentPropertiesNameInDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Talents",
                newName: "TalentName");

            migrationBuilder.RenameColumn(
                name: "Generation",
                table: "Talents",
                newName: "TalentGeneration");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TalentName",
                table: "Talents",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "TalentGeneration",
                table: "Talents",
                newName: "Generation");
        }
    }
}
