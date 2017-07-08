using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounting.API.Migrations
{
    public partial class addpropstoeventrawnull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EventTimeStamp",
                table: "eventraw",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<int>(
                name: "AcctStatusType",
                table: "eventraw",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "AcctOutputOctets",
                table: "eventraw",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "AcctInputOctets",
                table: "eventraw",
                nullable: true,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EventTimeStamp",
                table: "eventraw",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AcctStatusType",
                table: "eventraw",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AcctOutputOctets",
                table: "eventraw",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AcctInputOctets",
                table: "eventraw",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
