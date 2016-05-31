
CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml]
(
    @Application NVARCHAR(60),
    @ErrorId UNIQUEIDENTIFIER
)
AS

    SET NOCOUNT ON

    SELECT 
        [AllXml]
    FROM 
         [Scrapper].[dbo].[ElmahErrors]
    WHERE
        [ErrorId] = @ErrorId
    AND
        [Application] = @Application
