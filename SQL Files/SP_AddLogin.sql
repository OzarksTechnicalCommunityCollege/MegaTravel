USE [MegaTravel]
GO

/****** Object:  StoredProcedure [dbo].[AddLogin]    Script Date: 8/15/2022 4:26:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		Tiffany Ford
-- Create date: June 28th, 2022
-- Description:	Adds a new user to the login table with the supplied username and password
-- =============================================
CREATE PROCEDURE [dbo].[AddLogin] 
	-- Add the parameters for the stored procedure here
		@ID int, @USERNAME Varchar(50), @PASSWORD Varchar(MAX), @TYPE Varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRY       
		BEGIN  
			INSERT INTO [dbo].[Login](
							ID,
                           UserType,       
                           UserName,       
                           Password)       
              VALUES       (@ID, @TYPE, @USERNAME, @PASSWORD)
		END       
	END TRY       
      
      BEGIN CATCH       
      -- Error in the query                                               
      END CATCH       
END
GO


