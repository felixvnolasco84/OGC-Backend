using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OGCBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "partidas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    familia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sub_partida = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "numeric", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    Iva = table.Column<decimal>(type: "numeric", nullable: false),
                    total = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    aprobado = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    pagado = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    por_liquidar = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    actual = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    fecha_carga = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    archivo_origen = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partidas", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_partidas_familia",
                table: "partidas",
                column: "familia");

            migrationBuilder.CreateIndex(
                name: "idx_partidas_fecha_carga",
                table: "partidas",
                column: "fecha_carga");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "partidas");
        }
    }
}
