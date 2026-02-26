using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace College_Portal.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmissionAndStaffNo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamRegistrationRequests_Trainees_TraineeId",
                table: "ExamRegistrationRequests");

            migrationBuilder.RenameColumn(
                name: "TutorApprovedDate",
                table: "ExamRegistrationRequests",
                newName: "TutorApprovalDate");

            migrationBuilder.RenameColumn(
                name: "TutorApprovedBy",
                table: "ExamRegistrationRequests",
                newName: "RejectionReason");

            migrationBuilder.RenameColumn(
                name: "FinanceApprovedDate",
                table: "ExamRegistrationRequests",
                newName: "RejectionDate");

            migrationBuilder.RenameColumn(
                name: "FinanceApprovedBy",
                table: "ExamRegistrationRequests",
                newName: "RejectedById");

            migrationBuilder.RenameColumn(
                name: "FinalizedDate",
                table: "ExamRegistrationRequests",
                newName: "FinanceApprovalDate");

            migrationBuilder.RenameColumn(
                name: "FinalizedBy",
                table: "ExamRegistrationRequests",
                newName: "FinalizedByExamOfficerId");

            migrationBuilder.AlterColumn<string>(
                name: "TraineeId",
                table: "ExamRegistrationRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByFinanceId",
                table: "ExamRegistrationRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByTutorId",
                table: "ExamRegistrationRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalizationDate",
                table: "ExamRegistrationRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "ExamRegistrationRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AdmissionNo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StaffNo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudentLedgers",
                columns: table => new
                {
                    RegNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MyDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamRegistrationRequests_UnitId",
                table: "ExamRegistrationRequests",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamRegistrationRequests_AspNetUsers_TraineeId",
                table: "ExamRegistrationRequests",
                column: "TraineeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamRegistrationRequests_Units_UnitId",
                table: "ExamRegistrationRequests",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamRegistrationRequests_AspNetUsers_TraineeId",
                table: "ExamRegistrationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamRegistrationRequests_Units_UnitId",
                table: "ExamRegistrationRequests");

            migrationBuilder.DropTable(
                name: "StudentLedgers");

            migrationBuilder.DropIndex(
                name: "IX_ExamRegistrationRequests_UnitId",
                table: "ExamRegistrationRequests");

            migrationBuilder.DropColumn(
                name: "ApprovedByFinanceId",
                table: "ExamRegistrationRequests");

            migrationBuilder.DropColumn(
                name: "ApprovedByTutorId",
                table: "ExamRegistrationRequests");

            migrationBuilder.DropColumn(
                name: "FinalizationDate",
                table: "ExamRegistrationRequests");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "ExamRegistrationRequests");

            migrationBuilder.DropColumn(
                name: "AdmissionNo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StaffNo",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TutorApprovalDate",
                table: "ExamRegistrationRequests",
                newName: "TutorApprovedDate");

            migrationBuilder.RenameColumn(
                name: "RejectionReason",
                table: "ExamRegistrationRequests",
                newName: "TutorApprovedBy");

            migrationBuilder.RenameColumn(
                name: "RejectionDate",
                table: "ExamRegistrationRequests",
                newName: "FinanceApprovedDate");

            migrationBuilder.RenameColumn(
                name: "RejectedById",
                table: "ExamRegistrationRequests",
                newName: "FinanceApprovedBy");

            migrationBuilder.RenameColumn(
                name: "FinanceApprovalDate",
                table: "ExamRegistrationRequests",
                newName: "FinalizedDate");

            migrationBuilder.RenameColumn(
                name: "FinalizedByExamOfficerId",
                table: "ExamRegistrationRequests",
                newName: "FinalizedBy");

            migrationBuilder.AlterColumn<int>(
                name: "TraineeId",
                table: "ExamRegistrationRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamRegistrationRequests_Trainees_TraineeId",
                table: "ExamRegistrationRequests",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
