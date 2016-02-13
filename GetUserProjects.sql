USE TSSTAgile
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserProjects]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserProjects]
GO
CREATE PROCEDURE [dbo].[GetUserProjects]
	@UserId int
AS
BEGIN
	SELECT * FROM Projects WHERE Id IN (SELECT ProjectId FROM UsersProjects WHERE UserId = @UserId);
END