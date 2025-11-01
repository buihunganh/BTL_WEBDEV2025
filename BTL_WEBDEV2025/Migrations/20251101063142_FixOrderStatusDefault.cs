using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_WEBDEV2025.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderStatusDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [Orders] SET [Status] = 'New' WHERE [Status] IS NULL OR LTRIM(RTRIM([Status])) = ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // no-op
        }
    }
}
