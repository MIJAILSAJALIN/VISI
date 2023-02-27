using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VISI.Migrations
{
    /// <inheritdoc />
    public partial class AdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS(select Id from AspNetRoles where Id = 1)
                                    begin
	                                    insert AspNetRoles (Id,[Name], [NormalizedName])
	                                    values (1,'admin', 'ADMIN')
                                    end");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete AspNetRoles where Id = 1");
        }
    }
}
