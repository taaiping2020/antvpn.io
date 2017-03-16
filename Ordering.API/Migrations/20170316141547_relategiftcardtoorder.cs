using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.API.Migrations
{
    public partial class relategiftcardtoorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GiftCardKey",
                schema: "ordering",
                table: "orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_GiftCardKey",
                schema: "ordering",
                table: "orders",
                column: "GiftCardKey");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_giftcards_GiftCardKey",
                schema: "ordering",
                table: "orders",
                column: "GiftCardKey",
                principalSchema: "ordering",
                principalTable: "giftcards",
                principalColumn: "GiftCardKey",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_giftcards_GiftCardKey",
                schema: "ordering",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_GiftCardKey",
                schema: "ordering",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "GiftCardKey",
                schema: "ordering",
                table: "orders");
        }
    }
}
