Create PROCEDURE [dbo].[spBackupDatabase] 
(
	@xDBName NVARCHAR(50),
	@xDesAdd NVARCHAR(250)
)
AS
Begin Try 
	BACKUP DATABASE @xDBName TO  DISK = @xDesAdd WITH NOFORMAT, NOINIT,  NAME = 'Offset-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10
	Select @xDesAdd
End Try
Begin Catch
	SELECT ERROR_MESSAGE()
end Catch