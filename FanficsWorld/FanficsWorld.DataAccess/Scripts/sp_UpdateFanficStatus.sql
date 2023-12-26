SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		elkprostoelk
-- Create date: 26.12.2023
-- Description:	This procedure updates fanfics' statuses, if they are not changed for a long time, they become frozen automatically
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateFanficStatus]
	-- Add the parameters for the stored procedure here
	@FanficFrozenAfterDays INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
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
GO
