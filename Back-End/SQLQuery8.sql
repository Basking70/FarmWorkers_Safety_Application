USE [FarmWorkersAppDatabase]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[ReadFarmsByAnyParameter]
		@ParameterName = N'FarmID',
		@ParamenterData = N'1',
		@ExactMatch = N'Yes'

SELECT	@return_value as 'Return Value'

GO
