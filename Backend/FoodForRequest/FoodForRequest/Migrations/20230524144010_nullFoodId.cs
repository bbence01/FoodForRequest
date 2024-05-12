using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodForRequest.Migrations
{
    public partial class nullFoodId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Founds = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Foodrequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Payment = table.Column<int>(type: "int", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    InProgress = table.Column<bool>(type: "bit", nullable: false),
                    Deliveryoptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PictureURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foodrequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Foodrequests_AspNetUsers_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Foodrequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Foodrequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_Foodrequests_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foodrequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Choosen = table.Column<bool>(type: "bit", nullable: false),
                    FoodId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_AspNetUsers_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offers_Foodrequests_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foodrequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Admin", "ADMIN" },
                    { "2", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "FoodUserName", "Founds", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "b08e0982-d3e6-4172-abd3-b97eb0ed5eb7", 0, "7435fdeb-46d5-466d-867c-1bfa140f99f1", "FoodUser", "bence@gmail.com", true, "Bence", "bence@gmail.com", 10000, "Bognár", false, null, null, "BENCE@GMAIL.COM", "AQAAAAEAACcQAAAAEGBOoj5D43o8oUg88COaoBvU60c0+NhOGjyYxOFVj5HTRe2P6hD4EIxVis3euVZRcQ==", null, false, "46be6d8d-34d0-41e8-87aa-7a34b94c4bc5", false, "bence@gmail.com" },
                    { "c45cfd38-e421-48fe-ba32-9924d1fdbc8a", 0, "93204ff0-f3a6-4950-a4a7-8d98f7a6b62b", "FoodUser", "tibi@gmail.com", true, "Tibor", "tibi@gmail.com", 10000, "Bognár", false, null, null, "TIBI@GMAIL.COM", "AQAAAAEAACcQAAAAEPyKuJftPfDfCrkEA5jvLq0jaRP+TetfTYlgXCNWBe8Z0UR/SyZAS5gLDdDVC5VPMA==", null, false, "a14ee292-2522-4826-8dd5-69574eece358", false, "tibi@gmail.com" },
                    { "c773a6e8-6b18-4a72-bc2e-e1d350b70300", 0, "fc5203d9-bd63-4ba1-b178-8a5321811db5", "FoodUser", "anita@gmail.com", true, "Anita", "anita@gmail.com", 10000, "Koczó", false, null, null, "ANITA@GMAIL.COM", "AQAAAAEAACcQAAAAELlyIcoYCXs+L7LWsdxy2ySBqu2t8k+6wRCarA6Tg6TVD2Inus5BXHFet8VupalfsQ==", null, false, "e41a5db6-0114-45b8-aa9e-3b7a32c70e3b", false, "anita@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "Foodrequests",
                columns: new[] { "Id", "Deliveryoptions", "Description", "InProgress", "IsDone", "Name", "Payment", "PictureURL", "RequestorId" },
                values: new object[,]
                {
                    { "230753fc-9660-4287-a3ff-cf393fec50ce", "Uber", "Nyers hal", false, false, "Susi", 2000, "https://eda.yandex/images/1368744/8daf396b41ede90d45e8c99c83f24500-1100x825.jpg", "b08e0982-d3e6-4172-abd3-b97eb0ed5eb7" },
                    { "3", "No", "Tosted.", false, false, "Toast", 2000, "data:image/jpeg;base64,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", "c773a6e8-6b18-4a72-bc2e-e1d350b70300" },
                    { "307dcad3-b45c-4464-9252-766bc16c515d", "Uber", "Sülthus", false, false, "Stake", 3000, "data:image/jpeg;base64,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", "b08e0982-d3e6-4172-abd3-b97eb0ed5eb7" },
                    { "4", "No", "All the chocklate", false, false, "Chocklate ckae", 3500, "data:image/jpeg;base64,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", "c773a6e8-6b18-4a72-bc2e-e1d350b70300" },
                    { "5", "Uber", "I want to see myself eating", false, false, "Mirror ckae", 2002, "data:image/jpeg;base64,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", "c773a6e8-6b18-4a72-bc2e-e1d350b70300" }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "ContractorId", "RequestId", "Text" },
                values: new object[,]
                {
                    { "c8917c88-53eb-4c03-b3c1-8d0e44a38442", "c45cfd38-e421-48fe-ba32-9924d1fdbc8a", "4", "Hello" },
                    { "f4542c8b-3101-4479-bab4-f5950c2253e9", "c45cfd38-e421-48fe-ba32-9924d1fdbc8a", "3", "Hi" }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "Description", "FoodId", "Name" },
                values: new object[,]
                {
                    { "200566e5-94bf-4b40-ae3f-dd1dabbead27", "Tuna", "230753fc-9660-4287-a3ff-cf393fec50ce", "Hal" },
                    { "30e30ce2-95af-4664-823b-3b969b3569d3", "Dark", "4", "Choko" },
                    { "9e87a371-756d-4767-bedb-9c3b5c7c3cf7", "Rizs", "230753fc-9660-4287-a3ff-cf393fec50ce", "Rizs" }
                });

            migrationBuilder.InsertData(
                table: "Offers",
                columns: new[] { "Id", "Choosen", "ContractorId", "FoodId" },
                values: new object[,]
                {
                    { "03a33287-b9dc-4ad6-a7dd-52ccb157fbff", false, "c45cfd38-e421-48fe-ba32-9924d1fdbc8a", "3" },
                    { "21fc067c-8e84-49bd-bfb7-8387b4ad0147", false, "b08e0982-d3e6-4172-abd3-b97eb0ed5eb7", "3" },
                    { "52097536-68b6-4d0d-a019-352ac6d2917a", false, "c45cfd38-e421-48fe-ba32-9924d1fdbc8a", "4" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ContractorId",
                table: "Comments",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RequestId",
                table: "Comments",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Foodrequests_RequestorId",
                table: "Foodrequests",
                column: "RequestorId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_FoodId",
                table: "Ingredients",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ContractorId",
                table: "Offers",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_FoodId",
                table: "Offers",
                column: "FoodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Foodrequests");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
