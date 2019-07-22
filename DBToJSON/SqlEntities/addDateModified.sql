use PicuDrugData
go

DECLARE @Dynsql nvarchar(max) 
SET @Dynsql = ''

SELECT @Dynsql = @Dynsql + '
alter table ' + QUOTENAME(SCHEMA_NAME(schema_id))+ '.' + QUOTENAME(name)  + 
' add [DateModified] datetime not null default getdate()' 
FROM sys.tables
WHERE type='U' and object_id NOT IN (select object_id from sys.columns where name='DateModified') 
	and name in ('BolusDrugs','DoseCats','DilutionMethods','DefibModels','DrugReferenceSources','SiUnits','SiPrefixes','Wards','InfusionDrugs','DefibJoules','BolusDoses','BolusSortOrdering','InfusionSortOrdering','DrugAmpuleConcentrations','FixedTimeDilutions','VariableTimeDilutions','VariableTimeConcentrations','FixedTimeConcentrations','InfusionDiluents','DrugRoutes','FixedDoses','FixedDrugs')


EXEC (@Dynsql)
go

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.RecordDeletions
	(
	TableId int NOT NULL,
	IdOfDeletedRecord int NOT NULL,
	Deleted datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.RecordDeletions ADD CONSTRAINT
	PK_RecordDeletions PRIMARY KEY CLUSTERED 
	(
	TableId,
	IdOfDeletedRecord
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE dbo.RecordDeletions SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.FixedTimeDilutions.SiPrefixVal', N'Tmp_SiPrefix', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.FixedTimeDilutions.Tmp_SiPrefix', N'SiPrefix', 'COLUMN' 
GO
ALTER TABLE dbo.FixedTimeDilutions SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.VariableTimeDilutions.SiPrefixVal', N'Tmp_SiPrefix_1', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.VariableTimeDilutions.Tmp_SiPrefix_1', N'SiPrefix', 'COLUMN' 
GO
ALTER TABLE dbo.VariableTimeDilutions SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.InfusionDrugs.SiPrefixVal', N'Tmp_SiPrefix_2', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.InfusionDrugs.Tmp_SiPrefix_2', N'SiPrefix', 'COLUMN' 
GO
ALTER TABLE dbo.InfusionDrugs SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetVariableInfusions2]
	-- Add the parameters for the stored procedure here
	@WardId int, 
	@AgeMonths int,
	@WeightKg float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT drug.InfusionDrugId,drug.Fullname, drug.Abbrev, drug.SiPrefix AS AmpulePrefix, drug.Note, drug.SiUnitId, cat.Category, dil.DilutionMethodId as DilutionMethod, 
		dil.SiPrefix AS InfusionPrefix, dil.Volume, dil.RateMin, dil.RateMax, dil.IsPerMin, conc.Concentration, ref.Hyperlink as HrefBase, 
                      dil.ReferencePage as HrefPage
FROM         dbo.InfusionDrugs AS drug INNER JOIN
                      dbo.InfusionSortOrdering AS so ON drug.InfusionDrugId = so.InfusionDrugId INNER JOIN
                      dbo.VariableTimeDilutions AS dil ON drug.InfusionDrugId = dil.InfusionDrugId INNER JOIN
                      dbo.VariableTimeConcentrations AS conc ON dil.InfusionDilutionId = conc.InfusionDilutionId LEFT OUTER JOIN
                      dbo.DoseCats AS cat ON conc.DoseCatId = cat.DoseCatId INNER JOIN
                      dbo.DrugReferenceSources AS ref ON drug.DrugReferenceId = ref.DrugReferenceId
WHERE     (so.WardId = @WardId) AND (dil.AgeMinMonths <= @AgeMonths) AND (dil.AgeMaxMonths >= @AgeMonths) AND (dil.WeightMin < @WeightKg) AND (dil.WeightMax >= @WeightKg)
ORDER BY so.SortOrder, cat.SortOrder
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

GO

CREATE PROCEDURE [dbo].[sp_GetFixedInfusions]
	-- Add the parameters for the stored procedure here
	@AmpuleConcentrationId int, 
	@AgeMonths int,
	@WeightKg float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT        drug.InfusionDrugId, drug.Fullname, drug.Abbrev, drug.SiPrefix as AmpulePrefix, drug.SiUnitId, drug.Note, 
                         dbo.DrugReferenceSources.ReferenceDescription, dbo.DrugReferenceSources.Abbrev AS RefAbbrev, dbo.DrugReferenceSources.Hyperlink, 
                         dbo.DrugRoutes.Description AS RouteDescription, dbo.DrugRoutes.Abbrev AS RouteAbbrev, dil.DilutionMethodId as DilutionMethod, 
                         dil.SiPrefix AS InfusionPrefix, dil.IsPerMin, dil.ReferencePage, 
                         dbo.FixedTimeConcentrations.Concentration, dbo.FixedTimeConcentrations.Volume, dbo.FixedTimeConcentrations.StopMinutes as StopMins, dbo.FixedTimeConcentrations.Rate, 
                         dbo.InfusionDiluents.DiluentType, dbo.InfusionDiluents.Abbrev AS DiluentAbbrev,
						 amp.Concentration as AmpuleConcentration
FROM            FixedTimeDilutions dil INNER JOIN
                         dbo.FixedTimeConcentrations ON dil.InfusionDilutionId = dbo.FixedTimeConcentrations.InfusionDilutionId INNER JOIN
                         dbo.InfusionDrugs drug ON dil.InfusionDrugId =drug.InfusionDrugId INNER JOIN
						 dbo.DrugAmpuleConcentrations amp on amp.InfusionDrugId = drug.InfusionDrugId INNER JOIN
                         dbo.InfusionDiluents ON drug.InfusionDiluentId = dbo.InfusionDiluents.DiluentId INNER JOIN
                         dbo.DrugRoutes ON drug.RouteId = dbo.DrugRoutes.RouteId INNER JOIN
                         dbo.DrugReferenceSources ON drug.DrugReferenceId = dbo.DrugReferenceSources.DrugReferenceId
WHERE			amp.AmpuleConcentrationId = @AmpuleConcentrationId AND (dil.AgeMinMonths <= @AgeMonths) AND (dil.AgeMaxMonths >= @AgeMonths) AND (dil.WeightMin < @WeightKg) AND (dil.WeightMax >= @WeightKg)
ORDER BY		FixedTimeConcentrations.StopMinutes
END

GO

DROP PROCEDURE dbo.sp_GetVariableInfusions
GO