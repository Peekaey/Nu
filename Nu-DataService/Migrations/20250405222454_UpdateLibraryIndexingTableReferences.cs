using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nu_DataService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLibraryIndexingTableReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "LibraryFolderIndexes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedDate",
                table: "LibraryFolderIndexes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ParentFolderId",
                table: "LibraryFileIndexes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryFileIndexes_ParentFolderId",
                table: "LibraryFileIndexes",
                column: "ParentFolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryFileIndexes_LibraryFolderIndexes_ParentFolderId",
                table: "LibraryFileIndexes",
                column: "ParentFolderId",
                principalTable: "LibraryFolderIndexes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryFileIndexes_LibraryFolderIndexes_ParentFolderId",
                table: "LibraryFileIndexes");

            migrationBuilder.DropIndex(
                name: "IX_LibraryFileIndexes_ParentFolderId",
                table: "LibraryFileIndexes");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "LibraryFolderIndexes");

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                table: "LibraryFolderIndexes");

            migrationBuilder.DropColumn(
                name: "ParentFolderId",
                table: "LibraryFileIndexes");
        }
    }
}
