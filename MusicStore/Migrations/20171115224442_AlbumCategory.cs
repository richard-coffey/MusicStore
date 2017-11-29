using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MusicStore.Migrations
{
    public partial class AlbumCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Categories_CategoryId1",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Albums_CategoryId1",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Albums");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Albums",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_CategoryId1",
                table: "Albums",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Categories_CategoryId1",
                table: "Albums",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
