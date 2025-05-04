using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nu_DataService.Migrations
{
    /// <inheritdoc />
    public partial class PreviewThumbnailFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryPreviewThumbnailIndexes_LibraryFileIndexes_LibraryFi~",
                table: "LibraryPreviewThumbnailIndexes");

            migrationBuilder.DropIndex(
                name: "IX_LibraryPreviewThumbnailIndexes_LibraryFileIndexId",
                table: "LibraryPreviewThumbnailIndexes");

            migrationBuilder.AddColumn<int>(
                name: "LibraryPreviewThumbnailIndexId",
                table: "LibraryFileIndexes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryFileIndexes_LibraryPreviewThumbnailIndexId",
                table: "LibraryFileIndexes",
                column: "LibraryPreviewThumbnailIndexId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryFileIndexes_LibraryPreviewThumbnailIndexes_LibraryPr~",
                table: "LibraryFileIndexes",
                column: "LibraryPreviewThumbnailIndexId",
                principalTable: "LibraryPreviewThumbnailIndexes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryFileIndexes_LibraryPreviewThumbnailIndexes_LibraryPr~",
                table: "LibraryFileIndexes");

            migrationBuilder.DropIndex(
                name: "IX_LibraryFileIndexes_LibraryPreviewThumbnailIndexId",
                table: "LibraryFileIndexes");

            migrationBuilder.DropColumn(
                name: "LibraryPreviewThumbnailIndexId",
                table: "LibraryFileIndexes");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryPreviewThumbnailIndexes_LibraryFileIndexId",
                table: "LibraryPreviewThumbnailIndexes",
                column: "LibraryFileIndexId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryPreviewThumbnailIndexes_LibraryFileIndexes_LibraryFi~",
                table: "LibraryPreviewThumbnailIndexes",
                column: "LibraryFileIndexId",
                principalTable: "LibraryFileIndexes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
