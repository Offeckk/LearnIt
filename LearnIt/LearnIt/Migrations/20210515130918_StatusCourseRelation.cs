using Microsoft.EntityFrameworkCore.Migrations;

namespace LearnIt.Migrations
{
    public partial class StatusCourseRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37dfb4fa-4608-40e2-8e19-5f31ced86f9c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "55ab200a-d015-436b-8dc2-f84e94886136");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6cc11c65-c420-4418-bac5-9eaecd111ca4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5cf9d8f6-b20c-4a55-9e8d-b562c0cbac3a", "b1333d77-71b1-4464-8f7e-c78631e7f0b5", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b66e17e5-a2d9-4891-80c0-4850cfa8a399", "aa9af8d3-7ce6-4ec2-9a38-6bf4a5478b9f", "Student", "STUDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b858fc7f-5f36-4152-8441-82cd2994b6e6", "7b759ea6-e1a1-42c6-850d-34dc7a490d24", "Teacher", "TEACHER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5cf9d8f6-b20c-4a55-9e8d-b562c0cbac3a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b66e17e5-a2d9-4891-80c0-4850cfa8a399");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b858fc7f-5f36-4152-8441-82cd2994b6e6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "37dfb4fa-4608-40e2-8e19-5f31ced86f9c", "a20ea0ed-3f18-4419-b2bc-fcb7bb570ced", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "55ab200a-d015-436b-8dc2-f84e94886136", "2ccaa605-8269-4020-b6e1-cb701f92022e", "Student", "STUDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6cc11c65-c420-4418-bac5-9eaecd111ca4", "387acf45-df33-4dc1-8bde-4e8e745e8f82", "Teacher", "TEACHER" });
        }
    }
}
