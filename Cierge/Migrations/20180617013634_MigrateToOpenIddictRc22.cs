using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Cierge.Migrations
{
    public partial class MigrateToOpenIddictRc22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                table: "OpenIddictTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "OpenIddictScopes");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "OpenIddictApplications");

            migrationBuilder.RenameColumn(
                name: "Hash",
                table: "OpenIddictTokens",
                newName: "ReferenceId");

            migrationBuilder.RenameColumn(
                name: "Ciphertext",
                table: "OpenIddictTokens",
                newName: "Properties");

            migrationBuilder.RenameIndex(
                name: "IX_OpenIddictTokens_Hash",
                table: "OpenIddictTokens",
                newName: "IX_OpenIddictTokens_ReferenceId");

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyToken",
                table: "OpenIddictTokens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Payload",
                table: "OpenIddictTokens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyToken",
                table: "OpenIddictScopes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "OpenIddictScopes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "OpenIddictScopes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resources",
                table: "OpenIddictScopes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyToken",
                table: "OpenIddictAuthorizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "OpenIddictAuthorizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyToken",
                table: "OpenIddictApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConsentType",
                table: "OpenIddictApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "OpenIddictApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "OpenIddictApplications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictScopes_Name",
                table: "OpenIddictScopes",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                table: "OpenIddictTokens",
                column: "ApplicationId",
                principalTable: "OpenIddictApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                table: "OpenIddictTokens",
                column: "AuthorizationId",
                principalTable: "OpenIddictAuthorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                table: "OpenIddictTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                table: "OpenIddictTokens");

            migrationBuilder.DropIndex(
                name: "IX_OpenIddictScopes_Name",
                table: "OpenIddictScopes");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "Payload",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "OpenIddictScopes");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "OpenIddictScopes");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "OpenIddictScopes");

            migrationBuilder.DropColumn(
                name: "Resources",
                table: "OpenIddictScopes");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "OpenIddictApplications");

            migrationBuilder.DropColumn(
                name: "ConsentType",
                table: "OpenIddictApplications");

            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "OpenIddictApplications");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "OpenIddictApplications");

            migrationBuilder.RenameColumn(
                name: "ReferenceId",
                table: "OpenIddictTokens",
                newName: "Hash");

            migrationBuilder.RenameColumn(
                name: "Properties",
                table: "OpenIddictTokens",
                newName: "Ciphertext");

            migrationBuilder.RenameIndex(
                name: "IX_OpenIddictTokens_ReferenceId",
                table: "OpenIddictTokens",
                newName: "IX_OpenIddictTokens_Hash");

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "OpenIddictTokens",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "OpenIddictScopes",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "OpenIddictAuthorizations",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "OpenIddictApplications",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                table: "OpenIddictTokens",
                column: "ApplicationId",
                principalTable: "OpenIddictApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                table: "OpenIddictTokens",
                column: "AuthorizationId",
                principalTable: "OpenIddictAuthorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
