using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanficsWorld.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedFanficFreezingStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
            
                CREATE PROCEDURE [dbo].[sp_UpdateFanficStatus]
	                @FanficFrozenAfterDays INT
                AS
                BEGIN
	                SET NOCOUNT ON;

	                BEGIN TRANSACTION;

                    BEGIN TRY
		                UPDATE [dbo].[Fanfics]
		                SET [Status] = '2'
		                WHERE [Status] = '0' AND DATEDIFF(day, [LastModified], SYSDATETIME()) >= @FanficFrozenAfterDays;

		                COMMIT TRANSACTION;
	                END TRY
	                BEGIN CATCH
		                ROLLBACK TRANSACTION;
		                SELECT   
                        ERROR_NUMBER() AS ErrorNumber  
                       ,ERROR_MESSAGE() AS ErrorMessage;
	                END CATCH;
                END
                GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE sp_UpdateFanficStatus");
        }
    }
}
