using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nu_DataService.Migrations
{
    /// <inheritdoc />
    public partial class AddLibraryIndexing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LibraryFileIndexes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SystemCreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DirectoryName = table.Column<string>(type: "text", nullable: false),
                    FileType = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FileSizeInMB = table.Column<double>(type: "double precision", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryFileIndexes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryFolderIndexes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FolderName = table.Column<string>(type: "text", nullable: false),
                    FolderPathAfterRoot = table.Column<string>(type: "text", nullable: false),
                    FolderLevel = table.Column<int>(type: "integer", nullable: false),
                    ParentFolderId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryFolderIndexes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryFolderIndexes_LibraryFolderIndexes_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "LibraryFolderIndexes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryFolderIndexes_ParentFolderId",
                table: "LibraryFolderIndexes",
                column: "ParentFolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryFileIndexes");

            migrationBuilder.DropTable(
                name: "LibraryFolderIndexes");
        }
    }
}
