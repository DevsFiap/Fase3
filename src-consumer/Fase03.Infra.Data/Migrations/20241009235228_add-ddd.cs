using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fase03.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class addddd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DDDTelefone",
                table: "Contato",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DDDTelefone",
                table: "Contato");
        }
    }
}
