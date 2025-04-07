using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nu_DataService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLibraryIndexingTableReferences1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FolderPathAfterRoot",
                table: "LibraryFolderIndexes",
                newName: "FolderPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FolderPath",
                table: "LibraryFolderIndexes",
                newName: "FolderPathAfterRoot");
        }
    }
}
