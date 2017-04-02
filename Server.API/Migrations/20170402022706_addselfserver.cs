using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.API.Migrations
{
    public partial class addselfserver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHybrid",
                table: "servers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RedirectorServerId",
                table: "servers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrafficServerId",
                table: "servers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_servers_RedirectorServerId",
                table: "servers",
                column: "RedirectorServerId");

            migrationBuilder.CreateIndex(
                name: "IX_servers_TrafficServerId",
                table: "servers",
                column: "TrafficServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_servers_servers_RedirectorServerId",
                table: "servers",
                column: "RedirectorServerId",
                principalTable: "servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_servers_servers_TrafficServerId",
                table: "servers",
                column: "TrafficServerId",
                principalTable: "servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_servers_servers_RedirectorServerId",
                table: "servers");

            migrationBuilder.DropForeignKey(
                name: "FK_servers_servers_TrafficServerId",
                table: "servers");

            migrationBuilder.DropIndex(
                name: "IX_servers_RedirectorServerId",
                table: "servers");

            migrationBuilder.DropIndex(
                name: "IX_servers_TrafficServerId",
                table: "servers");

            migrationBuilder.DropColumn(
                name: "IsHybrid",
                table: "servers");

            migrationBuilder.DropColumn(
                name: "RedirectorServerId",
                table: "servers");

            migrationBuilder.DropColumn(
                name: "TrafficServerId",
                table: "servers");
        }
    }
}
