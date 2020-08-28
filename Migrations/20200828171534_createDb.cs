using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace lista_tarefas_api.Migrations
{
    public partial class createDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListarTarefas",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MacAddress = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    When = table.Column<DateTime>(nullable: false),
                    Done = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListarTarefas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListarTarefas");
        }
    }
}
