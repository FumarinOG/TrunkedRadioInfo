/****** Object:  StoredProcedure [dbo].[_DatabaseGetStats]    Script Date: 2/12/2019 2:49:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[_DatabaseGetStats]
AS
BEGIN
    SET NOCOUNT ON

    SELECT
        ( SELECT COUNT(*)
          FROM dbo.ProcessedFiles ) AS ProcessedFilesCount,
        ( SELECT SUM(pf.[RowCount])
          FROM dbo.ProcessedFiles pf ) AS [RowCount],
        ( SELECT COUNT(*) 
          FROM dbo.Systems) AS SystemsCount,
        ( SELECT COUNT(*) 
          FROM dbo.Talkgroups ) AS TalkgroupsCount,
        ( SELECT COUNT(*) 
          FROM dbo.TalkgroupHistory ) AS TalkgroupHistoryCount,
        ( SELECT COUNT(*) 
          FROM dbo.Radios ) AS RadiosCount,
        ( SELECT COUNT(*) 
          FROM dbo.RadioHistory ) AS RadioHistoryCount,
        ( SELECT COUNT(*) 
          FROM dbo.Towers ) AS TowersCount,
        ( SELECT COUNT(*) 
          FROM dbo.TowerFrequencies ) AS TowerFrequenciesCount,
        ( SELECT COUNT(*) 
          FROM dbo.TowerFrequencyUsage ) AS TowerFrequencyUsageCount,
        ( SELECT COUNT(*) 
          FROM dbo.TowerTalkgroups ) AS TowerTalkgroupsCount,
        ( SELECT COUNT(*) 
          FROM dbo.TowerRadios ) AS TowerRadiosCount,
        ( SELECT COUNT(*) 
          FROM dbo.TowerTalkgroupRadios ) AS TowerTalkgroupRadiosCount,
        ( SELECT COUNT(*) 
          FROM dbo.TowerFrequencyTalkgroups ) AS TowerFrequencyTalkgroupsCount,
        ( SELECT COUNT(*) 
          FROM dbo.TowerFrequencyRadios ) AS TowerFrequencyRadiosCount
END
GO

/****** Object:  StoredProcedure [dbo].[_FindBadTalkgroups]    Script Date: 2/12/2019 2:49:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[_FindBadTalkgroups]
(
    @systemID NVARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @systemIDKey INT;

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    WITH TGs (ID, SystemID, TalkgroupID, [Description], FirstSeen, LastSeen, EncryptionSeen, PhaseIISeen, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, AlertCount)
    AS
    (
	    SELECT tg.ID,
            tg.SystemID,
		    tg.TalkgroupID,
		    tg.[Description],
		    tg.FirstSeen,
		    tg.LastSeen,
		    tg.EncryptionSeen,
            tg.PhaseIISeen,
		    SUM(ISNULL(ttg.AffiliationCount, 0)) AS AffiliationCount,
		    SUM(ISNULL(ttg.DeniedCount, 0)) AS DeniedCount,
		    SUM(ISNULL(ttg.VoiceGrantCount, 0)) AS VoiceGrantCount,
		    SUM(ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		    SUM(ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
            SUM(ISNULL(ttg.AlertCount, 0)) AS AlertCount
	    FROM Talkgroups tg WITH (NOLOCK)
	    LEFT JOIN TowerTalkgroups ttg WITH (NOLOCK)
		    ON (ttg.TalkgroupID = tg.ID)
	    WHERE (tg.SystemID = @systemIDKey)
	    GROUP BY tg.ID,
            tg.SystemID,
            tg.TalkgroupID,
		    tg.[Description],
		    tg.FirstSeen,
		    tg.LastSeen,
		    tg.EncryptionSeen,
            tg.PhaseIISeen
    ),
    Summary (TalkgroupID, TGName, RadioID, RadioName, FromTG, ToTG, FirstSeen, LastSeen, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, AlertCount)
    AS
    (
        SELECT DISTINCT tg.TalkgroupID,
            tg.[Description],
            r.RadioID,
            r.[Description],
            ( SELECT ftg.[Description]
              FROM dbo.Talkgroups ftg
              WHERE (ftg.ID = p.FromTalkgroupID) ) AS FromTG,
            ( SELECT ttg.[Description]
              FROM dbo.Talkgroups ttg
              WHERE (ttg.ID = p.ToTalkgroupID) ) AS ToTG,
            tg.FirstSeen,
            tg.LastSeen,
            tg.AffiliationCount,
            tg.DeniedCount,
            tg.VoiceGrantCount,
            tg.EmergencyVoiceGrantCount,
            tg.EncryptedVoiceGrantCount,
            tg.AlertCount
        FROM TGs tg
        LEFT JOIN dbo.Radios r
            ON ((r.SystemID = tg.SystemID)
                AND (r.RadioID = tg.TalkgroupID))
        LEFT JOIN dbo.Patches p
            ON ((p.SystemID = tg.SystemID)
                AND ((p.FromTalkgroupID = tg.ID)
                    OR (p.ToTalkgroupID = tg.ID)))
        WHERE ((tg.AffiliationCount = 0)
            AND (tg.DeniedCount = 0)
            AND (tg.VoiceGrantCount = 0)
            AND (tg.EmergencyVoiceGrantCount = 0)
            AND (tg.EncryptedVoiceGrantCount = 0))
    )
    SELECT *
    FROM Summary s
    WHERE ((s.TGName LIKE '%<Unknown>%')
        AND ((s.FromTG IS NULL)
            OR (s.ToTG IS NULL)));
END
GO

/****** Object:  StoredProcedure [dbo].[_ReseedIDs]    Script Date: 2/12/2019 2:49:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[_ReseedIDs]
AS
BEGIN
    SET NOCOUNT ON

	DBCC CHECKIDENT ('dbo.ProcessedFiles', RESEED, 0);
	DBCC CHECKIDENT ('dbo.Patches', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TowerTalkgroupRadios', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TowerTalkgroups', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TowerRadios', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TowerTables', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TowerFrequencyUsage', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TowerFrequencies', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TowerNeighbors', RESEED, 0);
	DBCC CHECKIDENT ('dbo.Towers', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TalkgroupRadios', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TalkgroupHistory', RESEED, 0);
	DBCC CHECKIDENT ('dbo.Talkgroups', RESEED, 0);
	DBCC CHECKIDENT ('dbo.RadioHistory', RESEED, 0);
	DBCC CHECKIDENT ('dbo.Radios', RESEED, 0);
	DBCC CHECKIDENT ('dbo.Systems', RESEED, 0);

	DBCC CHECKIDENT ('dbo.TempPatches', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempTowerTalkgroupRadios', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempTowerTalkgroups', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempTowerRadios', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempTowerTables', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempTowerFrequencyUsage', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempTowerFrequencies', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempTowerNeighbors', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempTowers', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempTalkgroupRadios', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempTalkgroupHistory', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempTalkgroups', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempRadioHistory', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempRadios', RESEED, 0);
	DBCC CHECKIDENT ('dbo.TempSystems', RESEED, 0);
END
GO

/****** Object:  StoredProcedure [dbo].[DeleteAll]    Script Date: 2/12/2019 2:49:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[DeleteAll]
AS
BEGIN
	DELETE
	FROM dbo.ProcessedFiles;
	
	DELETE
	FROM dbo.Patches;
	
	DELETE
	FROM dbo.TowerTalkgroupRadios;
	
	DELETE
	FROM dbo.TowerTalkgroups;
	
	DELETE
	FROM dbo.TowerRadios;
	
	DELETE
	FROM dbo.TowerTables;
	
    DELETE
    FROM dbo.TowerFrequencyRadios;

    DELETE
    FROM dbo.TowerFrequencyTalkgroups;

	DELETE
	FROM dbo.TowerFrequencyUsage;

	DELETE
	FROM dbo.TowerFrequencies;
	
	DELETE
	FROM dbo.TowerNeighbors;
	
	DELETE
	FROM dbo.Towers;
	
	DELETE
	FROM dbo.TalkgroupRadios;
	
	DELETE
	FROM dbo.TalkgroupHistory;
	
	DELETE
	FROM dbo.Talkgroups;
	
	DELETE
	FROM dbo.RadioHistory;
	
	DELETE
	FROM dbo.Radios;
	
	DELETE
	FROM dbo.Systems;

		DELETE
	FROM TempSystems;

	DELETE
	FROM TempRadios;

	DELETE
	FROM TempRadioHistory;

	DELETE
	FROM TempTalkgroups;

	DELETE
	FROM TempTalkgroupHistory;

	DELETE
	FROM TempTalkgroupRadios;

	DELETE
	FROM TempTowers;

	DELETE
	FROM TempTowerFrequencies;

	DELETE
	FROM TempTowerFrequencyUsage;

	DELETE
	FROM TempTowerRadios;

	DELETE
	FROM TempTowerTalkgroups;

	DELETE
	FROM TempTowerTalkgroupRadios;

	DELETE
	FROM TempPatches;
END
GO

/****** Object:  StoredProcedure [dbo].[DeleteAllForSystem]    Script Date: 2/12/2019 2:49:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[DeleteAllForSystem]
(
	@actualSystemID NVARCHAR(50)
)
AS
BEGIN
	DECLARE @systemID INT;

	SELECT @systemID = s.ID
	FROM dbo.Systems s
	WHERE (s.SystemID = @systemID);

	DELETE
	FROM dbo.ProcessedFiles
	WHERE (SystemID = @systemID);
	
	DELETE
	FROM dbo.Patches
	WHERE (SystemID = @systemID);
	
	DELETE
	FROM dbo.TowerTalkgroupRadios
	WHERE (SystemID = @systemID);
	
	DELETE
	FROM dbo.TowerTalkgroups
	WHERE (SystemID = @systemID);
	
	DELETE
	FROM dbo.TowerRadios
	WHERE (SystemID = @systemID);
	
	DELETE
	FROM dbo.TowerTables
	WHERE (SystemID = @systemID);
	
	DELETE
	FROM dbo.TowerFrequencies
	WHERE (SystemID = @systemID);
	
    DELETE
    FROM dbo.TowerFrequencyRadios
    WHERE (SystemID = @systemID);

    DELETE
    FROM dbo.TowerFrequencyTalkgroups
    WHERE (SystemID = @systemID);

    DELETE
    FROM dbo.TowerFrequencyUsage
    WHERE (SystemID = @systemID);

	DELETE
	FROM dbo.TowerNeighbors
	WHERE (SystemID = @systemID);
	
	DELETE
	FROM dbo.Towers
	WHERE (SystemID = @systemID);
	
	DELETE
	FROM dbo.TalkgroupRadios
	WHERE (SystemID = @systemID);
	
	DELETE
	FROM dbo.TalkgroupHistory
	WHERE (SystemID = @systemID);
	
	DELETE
	FROM dbo.Talkgroups
	WHERE (SystemID = @systemID);
	
	DELETE
	FROM dbo.RadioHistory
	WHERE (SystemID = @systemID);
	
	DELETE
	FROM dbo.Radios
	WHERE (SystemID = @systemID);
	
	DELETE
	FROM dbo.Systems
	WHERE (ID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[DeleteTempTables]    Script Date: 2/12/2019 2:49:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[DeleteTempTables]
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TempSystems;

	DELETE
	FROM dbo.TempRadios;

	DELETE
	FROM dbo.TempRadioHistory;

	DELETE
	FROM dbo.TempTalkgroups;

	DELETE
	FROM dbo.TempTalkgroupHistory;

	DELETE
	FROM dbo.TempTalkgroupRadios;

	DELETE
	FROM dbo.TempTowers;

	DELETE
	FROM dbo.TempTowerFrequencies;

    DELETE
    FROM dbo.TempTowerFrequencyRadios;

    DELETE
    FROM dbo.TempTowerFrequencyTalkgroups;

	DELETE
	FROM dbo.TempTowerFrequencyUsage;

	DELETE
	FROM dbo.TempTowerRadios;

	DELETE
	FROM dbo.TempTowerTalkgroups;

	DELETE
	FROM dbo.TempTowerTalkgroupRadios;

	DELETE
	FROM dbo.TempPatches;
END
GO

/****** Object:  StoredProcedure [dbo].[MergeDeleteTempTables]    Script Date: 2/12/2019 2:49:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[MergeDeleteTempTables]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TempSystems
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempRadios
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempRadioHistory
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempTalkgroups
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempTalkgroupHistory
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempTalkgroupRadios
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempTowers
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempTowerFrequencies
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempTowerFrequencyRadios
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempTowerFrequencyTalkgroups
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempTowerFrequencyUsage
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempTowerRadios
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempTowerTalkgroups
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempTowerTalkgroupRadios
    WHERE (SessionID = @sessionID);

	DELETE
	FROM dbo.TempPatches
    WHERE (SessionID = @sessionID);
END
GO

/****** Object:  StoredProcedure [dbo].[MergePatches]    Script Date: 2/12/2019 2:49:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[MergePatches]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		MERGE dbo.Patches AS tgt
		USING ( SELECT tp.SystemID,
					t.ID AS TowerID,
					ftg.ID AS FromTalkgroupID,
					ttg.ID AS ToTalkgroupID,
					tp.[Date],
					tp.FirstSeen,
					tp.LastSeen,
					tp.HitCount
				FROM dbo.TempPatches tp
				INNER JOIN dbo.Towers t
					ON ((t.SystemID = tp.SystemID)
						AND (t.TowerNumber = tp.TowerID))
				INNER JOIN dbo.Talkgroups ftg
					ON ((ftg.SystemID = tp.SystemID)
						AND (ftg.TalkgroupID = tp.FromTalkgroupID))
				INNER JOIN dbo.Talkgroups ttg
					ON ((ttg.SystemID = tp.SystemID)
						AND (ttg.TalkgroupID = tp.ToTalkgroupID))
                WHERE (tp.SessionID = @sessionID) ) AS src
			ON ((tgt.SystemID = src.SystemID)
				AND (tgt.TowerID = src.TowerID)
				AND (tgt.FromTalkgroupID = src.FromTalkgroupID)
				AND (tgt.ToTalkgroupID = src.ToTalkgroupID)
				AND (tgt.[Date] = src.[Date]))
		WHEN MATCHED THEN
			UPDATE
			SET FirstSeen = src.FirstSeen,
				LastSeen = src.LastSeen,
				HitCount = src.HitCount,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemID,
				TowerID,
				FromTalkgroupID,
				ToTalkgroupID,
				[Date],
				FirstSeen,
				LastSeen,
				HitCount
			)
			VALUES
			(
				src.SystemID,
				src.TowerID,
				src.FromTalkgroupID,
				src.ToTalkgroupID,
				src.[Date],
				src.FirstSeen,
				src.LastSeen,
				src.HitCount
			);
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergePatches generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH

END
GO

/****** Object:  StoredProcedure [dbo].[MergeRadioHistory]    Script Date: 2/12/2019 2:49:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[MergeRadioHistory]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRANSACTION;

		MERGE dbo.RadioHistory AS tgt
		USING ( SELECT trh.SystemID,
					ISNULL(r.ID, ( SELECT rr.ID
                                   FROM #RadioResults rr
                                   WHERE (rr.RadioID = trh.RadioID) )) AS RadioID,
					trh.[Description],
					trh.FirstSeen,
					trh.LastSeen
				FROM dbo.TempRadioHistory trh
				LEFT JOIN dbo.Radios r
					ON ((r.SystemID = trh.SystemID)
						AND (r.RadioID = trh.RadioID))
                WHERE (trh.SessionID = @sessionID) ) src
		ON ((tgt.SystemID = src.SystemID)
			AND (tgt.RadioID = src.RadioID)
			AND (tgt.[Description] = src.[Description]))
		WHEN MATCHED THEN
			UPDATE
			SET [Description] = src.[Description],
				FirstSeen = src.FirstSeen,
				LastSeen = src.LastSeen,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemID,
				RadioID,
				[Description],
				FirstSeen,
				LastSeen
			)
			VALUES
			(
				src.SystemID,
                src.RadioID,
				src.[Description],
				src.FirstSeen,
				src.LastSeen
			);

		COMMIT TRANSACTION;
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeRadioHistory generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH
END
GO

/****** Object:  StoredProcedure [dbo].[MergeRadios]    Script Date: 2/12/2019 2:49:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[MergeRadios]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		MERGE dbo.Radios AS tgt
		USING ( SELECT tr.SystemID,
                    tr.RadioID,
                    tr.[Description],
                    tr.LastSeen,
                    tr.LastSeenProgram,
                    tr.LastSeenProgramUnix,
                    tr.FirstSeen,
                    tr.FGColor,
                    tr.BGColor,
                    tr.HitCount,
                    tr.PhaseIISeen
                FROM dbo.TempRadios tr
                WHERE (tr.SessionID = @sessionID) ) AS src
			ON ((tgt.SystemID = src.SystemID)
				AND (tgt.RadioID = src.RadioID))
		WHEN MATCHED THEN
			UPDATE
			SET [Description] = src.[Description],
				RadioID = src.RadioID,
				LastSeen = src.LastSeen,
				LastSeenProgram = src.LastSeenProgram,
				LastSeenProgramUnix = src.LastSeenProgramUnix,
				FirstSeen = src.FirstSeen,
				FGColor = src.FGColor,
				BGColor = src.BGColor,
				HitCount = src.HitCount,
                PhaseIISeen = src.PhaseIISeen,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemID,
				RadioID,
				[Description],
				LastSeen,
				LastSeenProgram,
				LastSeenProgramUnix,
				FirstSeen,
				FGColor,
				BGColor,
				HitCount,
                PhaseIISeen
			)
			VALUES
			(
				src.SystemID,
				src.RadioID,
				src.[Description],
				src.LastSeen,
				src.LastSeenProgram,
				src.LastSeenProgramUnix,
				src.FirstSeen,
				src.FGColor,
				src.BGColor,
				src.HitCount,
                src.PhaseIISeen
			)
        OUTPUT $action, inserted.ID, inserted.RadioID INTO #RadioResults;
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeRadios generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH
END
GO

/****** Object:  StoredProcedure [dbo].[MergeRecords]    Script Date: 2/12/2019 2:49:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[MergeRecords]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @returnValue INT;

	BEGIN TRY
		BEGIN TRANSACTION;

        CREATE TABLE #TalkgroupResults
        (
            ChangeType VARCHAR(10),
            ID INT,
            TalkgroupID INT
        );

        CREATE TABLE #RadioResults
        (
            ChangeType VARCHAR(10),
            ID INT,
            RadioID INT
        );

		PRINT 'MergeSystems...';
		EXEC @returnValue = MergeSystems @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeSystems encountered an error', 1;

		PRINT 'MergeRadios...';
		EXEC @returnValue = MergeRadios @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeRadios encountered an error', 1;

		PRINT 'MergeTalkgroups...';
		EXEC @returnValue = MergeTalkgroups @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeTalkgroups encountered an error', 1;

		PRINT 'MergeRadioHistory...';
		EXEC @returnValue = MergeRadioHistory @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeRadioHistory encountered an error', 1;

		PRINT 'MergeTalkgroupHistory...';
		EXEC @returnValue = MergeTalkgroupHistory @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeTalkgroupHistory encountered an error', 1;

		PRINT 'MergeTalkgroupRadios...';
		EXEC @returnValue = MergeTalkgroupRadios @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeTalkgroupRadios encountered an error', 1;

		PRINT 'MergeTowerFrequencyRadios...';
		EXEC @returnValue = MergeTowerFrequencyRadios @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeTowerFrequencyRadios encountered an error', 1;

		PRINT 'MergeTowerFrequencyTalkgroups...';
		EXEC @returnValue = MergeTowerFrequencyTalkgroups @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeTowerFrequencyTalkgroups encountered an error', 1;

		PRINT 'MergeTowerFrequencyUsage...';
		EXEC @returnValue = MergeTowerFrequencyUsage @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeTowerFrequencies encountered an error', 1;

		PRINT 'MergeTowers...';
		EXEC @returnValue = MergeTowers @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeTowers encountered an error', 1;

		PRINT 'MergeTowerRadios...';
		EXEC @returnValue = MergeTowerRadios @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeTowerRadios encountered an error', 1;

		PRINT 'MergeTowerTalkgroups...';
		EXEC @returnValue = MergeTowerTalkgroups @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeTowerTalkgroups encountered an error', 1;

		PRINT 'MergeTowerTalkgroupRadios...';
		EXEC @returnValue = MergeTowerTalkgroupRadios @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeTowerTalkgroupRadios encountered an error', 1;

		PRINT 'MergePatches...';
		EXEC @returnValue = MergePatches @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergePatches encountered an error', 1;

		PRINT 'MergeDeleteTempTables...';
		EXEC @returnValue = MergeDeleteTempTables @sessionID;

		IF (@returnValue <> 0)
			THROW 50000, 'MergeDeleteTempTables encountered an error', 1;

		COMMIT TRANSACTION;
        DROP TABLE #TalkgroupResults;
        DROP TABLE #RadioResults;
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeRecords generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH

END
GO

/****** Object:  StoredProcedure [dbo].[MergeSystems]    Script Date: 2/12/2019 2:49:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[MergeSystems]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		MERGE dbo.Systems AS tgt
		USING ( SELECT ts.SystemID,
                    ts.SystemIDDecimal,
                    ts.[Description],
                    ts.WACN,
                    ts.FirstSeen,
                    ts.LastSeen
                FROM dbo.TempSystems ts
                WHERE (ts.SessionID = @sessionID) ) AS src
			ON (tgt.SystemID = src.SystemID)
		WHEN MATCHED THEN
			UPDATE
			SET SystemIDDecimal = src.SystemIDDecimal,
				[Description] = src.[Description],
				WACN = src.WACN,
				FirstSeen = src.FirstSeen,
				LastSeen = src.LastSeen,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemIDDecimal,
				[Description],
				WACN,
				FirstSeen,
				LastSeen
			)
			VALUES
			(
				src.SystemIDDecimal,
				src.[Description],
				src.WACN,
				src.FirstSeen,
				src.LastSeen
			);
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeSystems generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH
END
GO

/****** Object:  StoredProcedure [dbo].[MergeTalkgroupHistory]    Script Date: 2/12/2019 2:49:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[MergeTalkgroupHistory]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT * FROM #TalkgroupResults;

	BEGIN TRY
		MERGE dbo.TalkgroupHistory AS tgt
		USING ( SELECT ttgh.SystemID,
                    ISNULL(tg.ID, ( SELECT tr.ID
                                    FROM #TalkgroupResults tr
                                    WHERE (tr.TalkgroupID = ttgh.TalkgroupID) )) AS TalkgroupID,
					ttgh.[Description],
					ttgh.FirstSeen,
					ttgh.LastSeen
				FROM dbo.TempTalkgroupHistory ttgh
				LEFT JOIN dbo.Talkgroups tg
					ON ((tg.SystemID = ttgh.SystemID)
						AND (tg.TalkgroupID = ttgh.TalkgroupID))
                WHERE (ttgh.SessionID = @sessionID) ) AS src
			ON ((tgt.SystemID = src.SystemID)
				AND (tgt.TalkgroupID = src.TalkgroupID)
				AND (tgt.[Description] = src.[Description]))
		WHEN MATCHED THEN
			UPDATE
			SET [Description] = src.[Description],
				FirstSeen = src.FirstSeen,
				LastSeen = src.LastSeen,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemID,
				TalkgroupID,
				[Description],
				FirstSeen,
				LastSeen
			)
			VALUES
			(
				src.SystemID,
                src.TalkgroupID,
				src.[Description],
				src.FirstSeen,
				src.LastSeen
			);
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeTalkgroupHistory generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH

END
GO

/****** Object:  StoredProcedure [dbo].[MergeTalkgroupRadios]    Script Date: 2/12/2019 2:49:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[MergeTalkgroupRadios]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		MERGE dbo.TalkgroupRadios AS tgt
		USING ( SELECT ttgr.SystemID,
					tg.ID AS TalkgroupID,
					r.ID AS RadioID,
					ttgr.[Date],
					ttgr.AffiliationCount,
					ttgr.DeniedCount,
					ttgr.VoiceGrantCount,
					ttgr.EmergencyVoiceGrantCount,
					ttgr.EncryptedVoiceGrantCount,
					ttgr.DataCount,
					ttgr.PrivateDataCount,
					ttgr.AlertCount,
					ttgr.FirstSeen,
					ttgr.LastSeen
				FROM dbo.TempTalkgroupRadios ttgr
				INNER JOIN dbo.Talkgroups tg
					ON ((tg.SystemID = ttgr.SystemID)
						AND (tg.TalkgroupID = ttgr.TalkgroupID))
				INNER JOIN dbo.Radios r
					ON ((r.SystemID = ttgr.SystemID)
						AND (r.RadioID = ttgr.RadioID))
                WHERE (ttgr.SessionID = @sessionID) ) AS src
			ON ((tgt.SystemID = src.SystemID)
				AND (tgt.TalkgroupID = src.TalkgroupID)
				AND (tgt.RadioID = src.RadioID)
				AND (tgt.[Date] = src.[Date]))
		WHEN MATCHED THEN
			UPDATE
			SET [Date] = src.[Date],
				AffiliationCount = src.AffiliationCount,
				DeniedCount = src.DeniedCount,
				VoiceGrantCount = src.VoiceGrantCount,
				EmergencyVoiceGrantCount = src.EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount = src.EncryptedVoiceGrantCount,
				DataCount = src.DataCount,
				PrivateDataCount = src.PrivateDataCount,
				AlertCount = src.AlertCount,
				FirstSeen = src.FirstSeen,
				LastSeen = src.LastSeen,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemID,
				TalkgroupID,
				RadioID,
				[Date],
				AffiliationCount,
				DeniedCount,
				VoiceGrantCount,
				EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount,
				DataCount,
				PrivateDataCount,
				AlertCount,
				FirstSeen,
				LastSeen
			)
			VALUES
			(
				src.SystemID,
				src.TalkgroupID,
				src.RadioID,
				src.[Date],
				src.AffiliationCount,
				src.DeniedCount,
				src.VoiceGrantCount,
				src.EmergencyVoiceGrantCount,
				src.EncryptedVoiceGrantCount,
				src.DataCount,
				src.PrivateDataCount,
				src.AlertCount,
				src.FirstSeen,
				src.LastSeen
			);
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeTalkgroupRadios generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH

END
GO

/****** Object:  StoredProcedure [dbo].[MergeTalkgroups]    Script Date: 2/12/2019 2:49:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[MergeTalkgroups]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		MERGE dbo.Talkgroups AS tgt
		USING ( SELECT ttg.SystemID,
                    ttg.TalkgroupID,
					ttg.[Priority],
					ttg.[Description],
					ttg.LastSeen,
					ttg.LastSeenProgram,
					ttg.LastSeenProgramUnix,
					ttg.FirstSeen,
					ttg.FirstSeenProgram,
					ttg.FirstSeenProgramUnix,
					ttg.FGColor,
					ttg.BGColor,
					ttg.EncryptionSeen,
					ttg.IgnoreEmergencySignal,
					ttg.HitCount,
					ttg.HitCountProgram,
                    ttg.PhaseIISeen
				FROM dbo.TempTalkgroups ttg
                WHERE (ttg.SessionID = @sessionID) ) AS src
			ON ((tgt.SystemID = src.SystemID)
				AND (tgt.TalkgroupID = src.TalkgroupID))
		WHEN MATCHED THEN
			UPDATE
			SET [Priority] = src.[Priority],
				[Description] = src.[Description],
				LastSeen = src.LastSeen,
				LastSeenProgram = src.LastSeenProgram,
				LastSeenProgramUnix = src.LastSeenProgramUnix,
				FirstSeen = src.FirstSeen,
				FirstSeenProgram = src.FirstSeenProgram,
				FirstSeenProgramUnix = src.FirstSeenProgramUnix,
				BGColor = src.BGColor,
				FGColor = src.FGColor,
				EncryptionSeen = src.EncryptionSeen,
				IgnoreEmergencySignal = src.IgnoreEmergencySignal,
				HitCount = src.HitCount,
				HitCountProgram = src.HitCountProgram,
                PhaseIISeen = src.PhaseIISeen,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemID,
				TalkgroupID,
				[Priority],
				[Description],
				LastSeen,
				LastSeenProgram,
				LastSeenProgramUnix,
				FirstSeen,
				FirstSeenProgram,
				FirstSeenProgramUnix,
				BGColor,
				FGColor,
				EncryptionSeen,
				IgnoreEmergencySignal,
				HitCount,
				HitCountProgram,
                PhaseIISeen
			)
			VALUES
			(
				src.SystemID,
				src.TalkgroupID,
				src.[Priority],
				src.[Description],
				src.LastSeen,
				src.LastSeenProgram,
				src.LastSeenProgramUnix,
				src.FirstSeen,
				src.FirstSeenProgram,
				src.FirstSeenProgramUnix,
				src.BGColor,
				src.FGColor,
				src.EncryptionSeen,
				src.IgnoreEmergencySignal,
				src.HitCount,
				src.HitCountProgram,
                src.PhaseIISeen
			)
        OUTPUT $action, inserted.ID, inserted.TalkgroupID INTO #TalkgroupResults;
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeTalkgroups generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH

END
GO

/****** Object:  StoredProcedure [dbo].[MergeTowerFrequencyRadios]    Script Date: 2/12/2019 2:49:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[MergeTowerFrequencyRadios]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		MERGE dbo.TowerFrequencyRadios AS tgt
		USING ( SELECT ttfr.SystemID,
					ttfr.TowerID,
                    ttfr.Frequency,
					r.ID AS RadioID,
					ttfr.[Date],
					ttfr.AffiliationCount,
					ttfr.DeniedCount,
					ttfr.VoiceGrantCount,
					ttfr.EmergencyVoiceGrantCount,
					ttfr.EncryptedVoiceGrantCount,
					ttfr.DataCount,
					ttfr.PrivateDataCount,
					ttfr.AlertCount,
					ttfr.FirstSeen,
					ttfr.LastSeen
				FROM dbo.TempTowerFrequencyRadios ttfr
				INNER JOIN dbo.Radios r
					ON ((r.SystemID = ttfr.SystemID)
						AND (r.RadioID = ttfr.RadioID))
                WHERE (SessionID = @sessionID) ) AS src
			ON ((tgt.SystemID = src.SystemID)
				AND (tgt.TowerID = src.TowerID)
                AND (tgt.Frequency = src.Frequency)
				AND (tgt.RadioID = src.RadioID)
				AND (tgt.[Date] = src.[Date]))
		WHEN MATCHED THEN
			UPDATE
			SET [Date] = src.[Date],
				AffiliationCount = src.AffiliationCount,
				DeniedCount = src.DeniedCount,
				VoiceGrantCount = src.VoiceGrantCount,
				EmergencyVoiceGrantCount = src.EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount = src.EncryptedVoiceGrantCount,
				DataCount = src.DataCount,
				PrivateDataCount = src.PrivateDataCount,
				AlertCount = src.AlertCount,
				FirstSeen = src.FirstSeen,
				LastSeen = src.LastSeen,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemID,
				TowerID,
                Frequency,
				RadioID,
				[Date],
				AffiliationCount,
				DeniedCount,
				VoiceGrantCount,
				EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount,
				DataCount,
				PrivateDataCount,
				AlertCount,
				FirstSeen,
				LastSeen
			)
			VALUES
			(
				src.SystemID,
				src.TowerID,
                src.Frequency,
				src.RadioID,
				src.[Date],
				src.AffiliationCount,
				src.DeniedCount,
				src.VoiceGrantCount,
				src.EmergencyVoiceGrantCount,
				src.EncryptedVoiceGrantCount,
				src.DataCount,
				src.PrivateDataCount,
				src.AlertCount,
				src.FirstSeen,
				src.LastSeen
			);
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeTowerFrquencyRadios generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH

END
GO

/****** Object:  StoredProcedure [dbo].[MergeTowerFrequencyTalkgroups]    Script Date: 2/12/2019 2:49:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[MergeTowerFrequencyTalkgroups]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		MERGE dbo.TowerFrequencyTalkgroups AS tgt
		USING ( SELECT ttftg.SystemID,
					ttftg.TowerID,
                    ttftg.Frequency,
					tg.ID AS TalkgroupID,
					ttftg.[Date],
					ttftg.AffiliationCount,
					ttftg.DeniedCount,
					ttftg.VoiceGrantCount,
					ttftg.EmergencyVoiceGrantCount,
					ttftg.EncryptedVoiceGrantCount,
					ttftg.DataCount,
					ttftg.PrivateDataCount,
					ttftg.AlertCount,
					ttftg.FirstSeen,
					ttftg.LastSeen
				FROM dbo.TempTowerFrequencyTalkgroups ttftg
				INNER JOIN dbo.Talkgroups tg
					ON ((tg.SystemID = ttftg.SystemID)
						AND (tg.TalkgroupID = ttftg.TalkgroupID))
                WHERE (SessionID = @sessionID) ) AS src
			ON ((tgt.SystemID = src.SystemID)
				AND (tgt.TowerID = src.TowerID)
                AND (tgt.Frequency = src.Frequency)
				AND (tgt.TalkgroupID = src.TalkgroupID)
				AND (tgt.[Date] = src.[Date]))
		WHEN MATCHED THEN
			UPDATE
			SET [Date] = src.[Date],
				AffiliationCount = src.AffiliationCount,
				DeniedCount = src.DeniedCount,
				VoiceGrantCount = src.VoiceGrantCount,
				EmergencyVoiceGrantCount = src.EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount = src.EncryptedVoiceGrantCount,
				DataCount = src.DataCount,
				PrivateDataCount = src.PrivateDataCount,
				AlertCount = src.AlertCount,
				FirstSeen = src.FirstSeen,
				LastSeen = src.LastSeen,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemID,
				TowerID,
                Frequency,
				TalkgroupID,
				[Date],
				AffiliationCount,
				DeniedCount,
				VoiceGrantCount,
				EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount,
				DataCount,
				PrivateDataCount,
				AlertCount,
				FirstSeen,
				LastSeen
			)
			VALUES
			(
				src.SystemID,
				src.TowerID,
                src.Frequency,
				src.TalkgroupID,
				src.[Date],
				src.AffiliationCount,
				src.DeniedCount,
				src.VoiceGrantCount,
				src.EmergencyVoiceGrantCount,
				src.EncryptedVoiceGrantCount,
				src.DataCount,
				src.PrivateDataCount,
				src.AlertCount,
				src.FirstSeen,
				src.LastSeen
			);
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeTowerFrequencyTalkgroups generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH

END
GO

/****** Object:  StoredProcedure [dbo].[MergeTowerFrequencyUsage]    Script Date: 2/12/2019 2:49:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[MergeTowerFrequencyUsage]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		MERGE dbo.TowerFrequencyUsage AS tgt
		USING ( SELECT ttfu.SystemID,
					t.ID AS TowerID,
					ttfu.Channel,
					ttfu.Frequency,
					ttfu.[Date],
					ttfu.AffiliationCount,
					ttfu.DeniedCount,
					ttfu.VoiceGrantCount,
					ttfu.EmergencyVoiceGrantCount,
					ttfu.EncryptedVoiceGrantCount,
					ttfu.DataCount,
					ttfu.PrivateDataCount,
					ttfu.CWIDCount,
					ttfu.AlertCount,
					ttfu.FirstSeen,
					ttfu.LastSeen
				FROM dbo.TempTowerFrequencyUsage ttfu
				INNER JOIN dbo.Towers t
					ON ((t.SystemID = ttfu.SystemID)
						AND (t.TowerNumber = ttfu.TowerID))
                WHERE (ttfu.SessionID = @sessionID) ) AS src
			ON ((tgt.SystemID = src.SystemID)
				AND (tgt.TowerID = src.TowerID)
				AND (tgt.Frequency = src.Frequency)
				AND (tgt.[Date] = src.[Date]))
		WHEN MATCHED THEN
			UPDATE
			SET AffiliationCount = src.AffiliationCount,
				DeniedCount = src.DeniedCount,
				VoiceGrantCount = src.VoiceGrantCount,
				EmergencyVoiceGrantCount = src.EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount = src.EncryptedVoiceGrantCount,
				DataCount = src.DataCount,
				PrivateDataCount = src.PrivateDataCount,
				CWIDCount = src.CWIDCount,
				AlertCount = src.AlertCount,
				FirstSeen = src.FirstSeen,
				LastSeen = src.LastSeen,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemID,
				TowerID,
				Channel,
				Frequency,
				[Date],
				AffiliationCount,
				DeniedCount,
				VoiceGrantCount,
				EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount,
				DataCount,
				PrivateDataCount,
				CWIDCount,
				AlertCount,
				FirstSeen,
				LastSeen
			)
			VALUES
			(
				src.SystemID,
				src.TowerID,
				src.Channel,
				src.Frequency,
				src.[Date],
				src.AffiliationCount,
				src.DeniedCount,
				src.VoiceGrantCount,
				src.EmergencyVoiceGrantCount,
				src.EncryptedVoiceGrantCount,
				src.DataCount,
				src.PrivateDataCount,
				src.CWIDCount,
				src.AlertCount,
				src.FirstSeen,
				src.LastSeen
			);
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeTowerFrequencies generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH
END
GO

/****** Object:  StoredProcedure [dbo].[MergeTowerRadios]    Script Date: 2/12/2019 2:49:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[MergeTowerRadios]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		MERGE dbo.TowerRadios AS tgt
		USING ( SELECT ttr.SystemID,
					ttr.TowerID,
					r.ID AS RadioID,
					ttr.[Date],
					ttr.AffiliationCount,
					ttr.DeniedCount,
					ttr.VoiceGrantCount,
					ttr.EmergencyVoiceGrantCount,
					ttr.EncryptedVoiceGrantCount,
					ttr.DataCount,
					ttr.PrivateDataCount,
					ttr.AlertCount,
					ttr.FirstSeen,
					ttr.LastSeen
				FROM dbo.TempTowerRadios ttr
				INNER JOIN dbo.Radios r
					ON ((r.SystemID = ttr.SystemID)
						AND (r.RadioID = ttr.RadioID))
                WHERE (SessionID = @sessionID) ) AS src
			ON ((tgt.SystemID = src.SystemID)
				AND (tgt.TowerID = src.TowerID)
				AND (tgt.RadioID = src.RadioID)
				AND (tgt.[Date] = src.[Date]))
		WHEN MATCHED THEN
			UPDATE
			SET [Date] = src.[Date],
				AffiliationCount = src.AffiliationCount,
				DeniedCount = src.DeniedCount,
				VoiceGrantCount = src.VoiceGrantCount,
				EmergencyVoiceGrantCount = src.EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount = src.EncryptedVoiceGrantCount,
				DataCount = src.DataCount,
				PrivateDataCount = src.PrivateDataCount,
				AlertCount = src.AlertCount,
				FirstSeen = src.FirstSeen,
				LastSeen = src.LastSeen,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemID,
				TowerID,
				RadioID,
				[Date],
				AffiliationCount,
				DeniedCount,
				VoiceGrantCount,
				EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount,
				DataCount,
				PrivateDataCount,
				AlertCount,
				FirstSeen,
				LastSeen
			)
			VALUES
			(
				src.SystemID,
				src.TowerID,
				src.RadioID,
				src.[Date],
				src.AffiliationCount,
				src.DeniedCount,
				src.VoiceGrantCount,
				src.EmergencyVoiceGrantCount,
				src.EncryptedVoiceGrantCount,
				src.DataCount,
				src.PrivateDataCount,
				src.AlertCount,
				src.FirstSeen,
				src.LastSeen
			);
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeTowerRadios generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH

END
GO

/****** Object:  StoredProcedure [dbo].[MergeTowers]    Script Date: 2/12/2019 2:49:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[MergeTowers]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		MERGE dbo.Towers AS tgt
		USING ( SELECT tt.SystemID,
                    tt.TowerNumber,
                    tt.TowerNumberHex,
                    tt.[Description],
                    tt.HitCount,
                    tt.WACN,
                    tt.ControlCapabilities,
                    tt.Flavor,
                    tt.CallSigns,
                    tt.[TimeStamp],
                    tt.FirstSeen,
                    tt.LastSeen
                FROM dbo.TempTowers tt
                WHERE (tt.SessionID = @sessionID) ) AS src
			ON ((tgt.SystemID = src.SystemID)
				AND (tgt.TowerNumber = src.TowerNumber))
		WHEN MATCHED THEN
			UPDATE
			SET TowerNumberHex = src.TowerNumberHex,
				[Description] = src.[Description],
				HitCount = src.HitCount,
				WACN = src.WACN,
				ControlCapabilities = src.ControlCapabilities,
				Flavor = src.Flavor,
				CallSigns = src.CallSigns,
				[TimeStamp] = src.[TimeStamp],
				FirstSeen = src.FirstSeen,
				LastSeen = src.LastSeen,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemID,
				TowerNumber,
				TowerNumberHex,
				[Description],
				HitCount,
				WACN,
				ControlCapabilities,
				Flavor,
				CallSigns,
				[TimeStamp],
				FirstSeen,
				LastSeen
			)
			VALUES
			(
				src.SystemID,
				src.TowerNumber,
				src.TowerNumberHex,
				src.[Description],
				src.HitCount,
				src.WACN,
				src.ControlCapabilities,
				src.Flavor,
				src.CallSigns,
				src.[TimeStamp],
				src.FirstSeen,
				src.LastSeen
			);
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeTowers generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH

END
GO

/****** Object:  StoredProcedure [dbo].[MergeTowerTalkgroupRadios]    Script Date: 2/12/2019 2:49:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[MergeTowerTalkgroupRadios]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		MERGE dbo.TowerTalkgroupRadios AS tgt
		USING ( SELECT ttgr.SystemID,
					ttgr.TowerID,
					tg.ID AS TalkgroupID,
					r.ID AS RadioID,
					ttgr.[Date],
					ttgr.AffiliationCount,
					ttgr.DeniedCount,
					ttgr.VoiceGrantCount,
					ttgr.EmergencyVoiceGrantCount,
					ttgr.EncryptedVoiceGrantCount,
					ttgr.DataCount,
					ttgr.PrivateDataCount,
					ttgr.AlertCount,
					ttgr.FirstSeen,
					ttgr.LastSeen
				FROM dbo.TempTowerTalkgroupRadios ttgr
				INNER JOIN dbo.Talkgroups tg
					ON ((tg.SystemID = ttgr.SystemID)
						AND (tg.TalkgroupID = ttgr.TalkgroupID))
				INNER JOIN dbo.Radios r
					ON ((r.SystemID = ttgr.SystemID)
						AND (r.RadioID = ttgr.RadioID))
                WHERE (ttgr.SessionID = @sessionID) ) AS src
			ON ((tgt.SystemID = src.SystemID)
				AND (tgt.TowerID = src.TowerID)
				AND (tgt.TalkgroupID = src.TalkgroupID)
				AND (tgt.RadioID = src.RadioID)
				AND (tgt.[Date] = src.[Date]))
		WHEN MATCHED THEN
			UPDATE
			SET [Date] = src.[Date],
				AffiliationCount = src.AffiliationCount,
				DeniedCount = src.DeniedCount,
				VoiceGrantCount = src.VoiceGrantCount,
				EmergencyVoiceGrantCount = src.EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount = src.EncryptedVoiceGrantCount,
				DataCount = src.DataCount,
				PrivateDataCount = src.PrivateDataCount,
				AlertCount = src.AlertCount,
				FirstSeen = src.FirstSeen,
				LastSeen = src.LastSeen,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemID,
				TowerID,
				TalkgroupID,
				RadioID,
				[Date],
				AffiliationCount,
				DeniedCount,
				VoiceGrantCount,
				EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount,
				DataCount,
				PrivateDataCount,
				AlertCount,
				FirstSeen,
				LastSeen
			)
			VALUES
			(
				src.SystemID,
				src.TowerID,
				src.TalkgroupID,
				src.RadioID,
				src.[Date],
				src.AffiliationCount,
				src.DeniedCount,
				src.VoiceGrantCount,
				src.EmergencyVoiceGrantCount,
				src.EncryptedVoiceGrantCount,
				src.DataCount,
				src.PrivateDataCount,
				src.AlertCount,
				src.FirstSeen,
				src.LastSeen
			);
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeTowerTalkgroupRadios generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH
END
GO

/****** Object:  StoredProcedure [dbo].[MergeTowerTalkgroups]    Script Date: 2/12/2019 2:49:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[MergeTowerTalkgroups]
(
    @sessionID UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		MERGE dbo.TowerTalkgroups AS tgt
		USING ( SELECT tttg.SystemID,
					tttg.TowerID,
					tg.ID AS TalkgroupID,
					tttg.[Date],
					tttg.AffiliationCount,
					tttg.DeniedCount,
					tttg.VoiceGrantCount,
					tttg.EmergencyVoiceGrantCount,
					tttg.EncryptedVoiceGrantCount,
					tttg.DataCount,
					tttg.PrivateDataCount,
					tttg.AlertCount,
					tttg.FirstSeen,
					tttg.LastSeen
				FROM dbo.TempTowerTalkgroups tttg
				INNER JOIN dbo.Talkgroups tg
					ON ((tg.SystemID = tttg.SystemID)
						AND (tg.TalkgroupID = tttg.TalkgroupID))
                WHERE (tttg.SessionID = @sessionID) ) AS src
			ON ((tgt.SystemID = src.SystemID)
				AND (tgt.TowerID = src.TowerID)
				AND (tgt.TalkgroupID = src.TalkgroupID)
				AND (tgt.[Date] = src.[Date]))
		WHEN MATCHED THEN
			UPDATE
			SET [Date] = src.[Date],
				AffiliationCount = src.AffiliationCount,
				DeniedCount = src.DeniedCount,
				VoiceGrantCount = src.VoiceGrantCount,
				EmergencyVoiceGrantCount = src.EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount = src.EncryptedVoiceGrantCount,
				DataCount = src.DataCount,
				PrivateDataCount = src.PrivateDataCount,
				AlertCount = src.AlertCount,
				FirstSeen = src.FirstSeen,
				LastSeen = src.LastSeen,
				LastModified = GETDATE()
		WHEN NOT MATCHED THEN
			INSERT
			(
				SystemID,
				TowerID,
				TalkgroupID,
				[Date],
				AffiliationCount,
				DeniedCount,
				VoiceGrantCount,
				EmergencyVoiceGrantCount,
				EncryptedVoiceGrantCount,
				DataCount,
				PrivateDataCount,
				AlertCount,
				FirstSeen,
				LastSeen
			)
			VALUES
			(
				src.SystemID,
				src.TowerID,
				src.TalkgroupID,
				src.[Date],
				src.AffiliationCount,
				src.DeniedCount,
				src.VoiceGrantCount,
				src.EmergencyVoiceGrantCount,
				src.EncryptedVoiceGrantCount,
				src.DataCount,
				src.PrivateDataCount,
				src.AlertCount,
				src.FirstSeen,
				src.LastSeen
			);
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(400);
		DECLARE @errorSeverity INT;
		DECLARE @errorState INT;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRANSACTION;
		END

		SELECT @errorMessage = ERROR_MESSAGE(),
			@errorSeverity = ERROR_SEVERITY(),
			@errorState = ERROR_STATE();

		RAISERROR(N'MergeTowerTalkgroups generated an error - %s', @errorSeverity, @errorState, @errorMessage);
	END CATCH

END
GO

/****** Object:  StoredProcedure [dbo].[NLog_AddEntry_p]    Script Date: 2/12/2019 2:49:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[NLog_AddEntry_p] (
  @machineName nvarchar(200),
  @siteName nvarchar(200),
  @logged datetime,
  @level varchar(5),
  @userName nvarchar(200),
  @message nvarchar(max),
  @logger nvarchar(300),
  @properties nvarchar(max),
  @serverName nvarchar(200),
  @port nvarchar(100),
  @url nvarchar(2000),
  @https bit,
  @serverAddress nvarchar(100),
  @remoteAddress nvarchar(100),
  @callSite nvarchar(300),
  @exception nvarchar(max)
) AS
BEGIN
  INSERT INTO [dbo].[NLog] (
    [MachineName],
    [SiteName],
    [Logged],
    [Level],
    [UserName],
    [Message],
    [Logger],
    [Properties],
    [ServerName],
    [Port],
    [Url],
    [Https],
    [ServerAddress],
    [RemoteAddress],
    [CallSite],
    [Exception]
  ) VALUES (
    @machineName,
    @siteName,
    @logged,
    @level,
    @userName,
    @message,
    @logger,
    @properties,
    @serverName,
    @port,
    @url,
    @https,
    @serverAddress,
    @remoteAddress,
    @callSite,
    @exception
  );
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGet]    Script Date: 2/12/2019 2:49:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[PatchesGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.SystemID,
		p.TowerID,
		p.FromTalkgroupID,
		p.ToTalkgroupID,
		p.[Date],
		p.FirstSeen,
		p.LastSeen,
		p.HitCount,
		p.LastModified
	FROM dbo.Patches p
	WHERE (p.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetCountForSystem]    Script Date: 2/12/2019 2:49:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[PatchesGetCountForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	WITH PatchSummary([RowCount])
	AS
	(
		SELECT COUNT(1) AS [RowCount]
		FROM dbo.Patches p
		WHERE (p.SystemID = @systemID)
        GROUP BY p.FromTalkgroupID,
            p.ToTalkgroupID
	)
	SELECT COUNT(*)
	FROM PatchSummary;

END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetCountForSystemSearch]    Script Date: 2/12/2019 2:49:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[PatchesGetCountForSystemSearch]
(
	@systemID INT,
	@searchText NVARCHAR(200)
)
AS
BEGIN
	SET NOCOUNT ON;

	WITH PatchSummary(SystemID, FromTalkgroupID, ToTalkgroupID)
	AS
	(
		SELECT DISTINCT p.SystemID,
			p.FromTalkgroupID,
			p.ToTalkgroupID
		FROM dbo.Patches p WITH (NOLOCK)
		INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
			ON (ftg.ID= p.FromTalkgroupID)
		INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
			ON (ttg.ID = p.ToTalkgroupID)
		WHERE ((p.SystemID = @systemID)
			AND ((CAST(ftg.TalkgroupID AS NVARCHAR(10)) LIKE N'%' + @searchText + N'%')
				OR (ftg.[Description] LIKE N'%' + @searchText + N'%')
				OR (CAST(ttg.TalkgroupID AS NVARCHAR(10)) LIKE N'%' + @searchText + N'%')
				OR (ftg.[Description] LIKE N'%' + @searchText + N'%')))
	)
	SELECT COUNT(*)
	FROM PatchSummary
    OPTION (RECOMPILE);

END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetForPatchByDate]    Script Date: 2/12/2019 2:49:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[PatchesGetForPatchByDate]
(
	@systemID INT,
	@fromTalkgroupID INT,
	@toTalkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ftg.TalkgroupID AS FromTalkgroupID,
		ftg.[Description] AS FromTalkgroupDescription,
		ttg.TalkgroupID AS ToTalkgroupID,
		ttg.[Description] AS ToTalkgroupDescription,
		t.TowerNumber,
		t.[Description] AS TowerDescription,
		p.[Date],
		SUM(ISNULL(p.HitCount, 0)) AS HitCount
	FROM dbo.Patches p WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
		ON (ftg.ID = p.FromTalkgroupID)
	INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
		ON (ttg.ID = p.ToTalkgroupID)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = p.TowerID)
	WHERE ((p.SystemID = @systemID)
		AND (ftg.TalkgroupID = @fromTalkgroupID)
		AND (ttg.TalkgroupID = @toTalkgroupID))
	GROUP BY ftg.TalkgroupID,
		ftg.[Description],
		ttg.TalkgroupID,
		ttg.[Description],
		t.TowerNumber,
		t.[Description],
		p.[Date]
	ORDER BY ftg.TalkgroupID,
		ttg.TalkgroupID,
		p.[Date]
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetForPatchByDateCount]    Script Date: 2/12/2019 2:49:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[PatchesGetForPatchByDateCount]
(
	@systemID INT,
	@fromTalkgroupID INT,
	@toTalkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*)
	FROM dbo.Patches p WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
		ON (ftg.ID = p.FromTalkgroupID)
	INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
		ON (ttg.ID = p.ToTalkgroupID)
	WHERE ((p.SystemID = @systemID)
		AND (ftg.TalkgroupID = @fromTalkgroupID)
		AND (ttg.TalkgroupID = @toTalkgroupID))
	GROUP BY p.SystemID,
		p.FromTalkgroupID,
		p.ToTalkgroupID;
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetForPatchByDateWithPaging]    Script Date: 2/12/2019 2:49:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[PatchesGetForPatchByDateWithPaging]
(
	@systemID INT,
	@fromTalkgroupID INT,
	@toTalkgroupID INT,
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ftg.TalkgroupID AS FromTalkgroupID,
		ftg.[Description] AS FromTalkgroupDescription,
		ttg.TalkgroupID AS ToTalkgroupID,
		ttg.[Description] AS ToTalkgroupDescription,
		t.TowerNumber,
		t.[Description] AS TowerDescription,
		p.[Date],
		SUM(ISNULL(p.HitCount, 0)) AS HitCount
	FROM dbo.Patches p WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
		ON (ftg.ID = p.FromTalkgroupID)
	INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
		ON (ttg.ID = p.ToTalkgroupID)
	INNER JOIN dbo.Towers t
		ON (t.ID = p.TowerID)
	WHERE ((p.SystemID = @systemID)
		AND (ftg.TalkgroupID = @fromTalkgroupID)
		AND (ttg.TalkgroupID = @toTalkgroupID))
	GROUP BY ftg.TalkgroupID,
		ftg.[Description],
		ttg.TalkgroupID,
		ttg.[Description],
		t.TowerNumber,
		t.[Description],
		p.[Date]
	ORDER BY p.[Date] DESC
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY;
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetForSystem]    Script Date: 2/12/2019 2:49:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[PatchesGetForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.ID,
		p.SystemID,
		p.TowerID,
		p.FromTalkgroupID,
		p.ToTalkgroupID,
		p.[Date],
		p.FirstSeen,
		p.LastSeen,
		p.HitCount,
		p.LastModified
	FROM dbo.Patches p
	WHERE (p.SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetForSystemFromTalkgroupID]    Script Date: 2/12/2019 2:49:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[PatchesGetForSystemFromTalkgroupID]
(
	@systemID INT,
	@fromTalkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		ftg.TalkgroupID AS FromTalkgroupID,
		ftg.[Description] AS FromTalkgroupDescription,
		ttg.TalkgroupID ToTalkgroupID,
		ttg.[Description] AS ToTalkgroupDescription,
		MIN(p.FirstSeen) AS FirstSeen,
		MAX(p.LastSeen) AS LastSeen,
		SUM(p.HitCount) AS HitCount
	FROM dbo.Patches p WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = p.TowerID)
	INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
		ON (ftg.ID = p.FromTalkgroupID)
	INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
		ON (ttg.ID = p.ToTalkgroupID)
	WHERE ((p.SystemID = @systemID)
		AND (ftg.TalkgroupID = @fromTalkgroupID))
	GROUP BY t.TowerNumber,
		t.[Description],
		ftg.TalkgroupID,
		ftg.[Description],
		ttg.TalkgroupID,
		ttg.[Description]
	ORDER BY ftg.TalkgroupID,
		ttg.TalkgroupID;
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetForSystemTalkgroup]    Script Date: 2/12/2019 2:49:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[PatchesGetForSystemTalkgroup]
(
	@systemID INT,
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		ftg.TalkgroupID AS FromTalkgroupID,
		ftg.[Description] AS FromTalkgroupDescription,
		ttg.TalkgroupID AS ToTalkgroupID,
		ttg.[Description] AS ToTalkgroupDescription,
		MIN(p.FirstSeen) AS FirstSeen,
		MAX(p.LastSeen) AS LastSeen,
		SUM(p.HitCount) AS HitCount,
        COUNT(1) OVER() AS RecordCount
	FROM dbo.Patches p WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = p.TowerID)
	INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
		ON (ftg.ID = p.FromTalkgroupID)
	INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
		ON (ttg.ID = p.ToTalkgroupID)
	WHERE ((p.SystemID = @systemID)
		AND ((ftg.TalkgroupID = @talkgroupID)
			OR (ttg.TalkgroupID = @talkgroupID)))
	GROUP BY t.TowerNumber,
		t.[Description],
		ftg.TalkgroupID,
		ftg.[Description],
		ttg.TalkgroupID,
		ttg.[Description]
	ORDER BY ftg.TalkgroupID,
		ttg.TalkgroupID;
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetForSystemTalkgroupCount]    Script Date: 2/12/2019 2:49:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[PatchesGetForSystemTalkgroupCount]
(
    @systemID INT,
    @talkgroupID INT
)
AS
BEGIN
    SET NOCOUNT ON;

    WITH PatchResults (TowerNumber)
    AS
    (
 	    SELECT t.TowerNumber
	    FROM dbo.Patches p
	    INNER JOIN dbo.Towers t WITH (NOLOCK)
		    ON (t.ID = p.TowerID)
	    INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
		    ON (ftg.ID = p.FromTalkgroupID)
	    INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
		    ON (ttg.ID = p.ToTalkgroupID)
	    WHERE ((p.SystemID = @systemID)
		    AND ((ftg.TalkgroupID = @talkgroupID)
			    OR (ttg.TalkgroupID = @talkgroupID)))
	    GROUP BY t.TowerNumber,
		    t.[Description],
		    ftg.TalkgroupID,
		    ftg.[Description],
		    ttg.TalkgroupID,
		    ttg.[Description]
    )
    SELECT COUNT(*)
    FROM PatchResults;
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetForSystemTalkgroupFiltersWithPaging]    Script Date: 2/12/2019 2:49:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[PatchesGetForSystemTalkgroupFiltersWithPaging]
(
	@systemID NVARCHAR(50),
	@talkgroupID INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @talkgroupIDFrom INT = NULL,
    @talkgroupIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(200),
    @sortDirection NVARCHAR(200),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE  @FIELD_TOWER_NUMBER NVARCHAR(50) = N'TowerNumber',
        @FIELD_TOWER_NAME NVARCHAR(50) = N'TowerName',
        @FIELD_FROM_TALKGROUP_ID NVARCHAR(50) = N'FromTalkgroupID',
		@FIELD_FROM_TALKGROUP_NAME NVARCHAR(50) = N'FromTalkgroupName',
        @FIELD_TO_TALKGROUP_ID NVARCHAR(50) = N'ToTalkgroupID',
		@FIELD_TO_TALKGROUP_NAME NVARCHAR(50) = N'ToTalkgroupName',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
        @FIELD_HIT_COUNT NVARCHAR(50) = N'HitCount',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @recordCount INT;

    WITH PatchResults (TowerNumber, TowerDescription, FromTalkgroupID, FromTalkgroupDescription, ToTalkgroupID, ToTalkgroupDescription, FirstSeen, LastSeen, HitCount)
    AS
    (
	    SELECT t.TowerNumber,
		    t.[Description] AS TowerDescription,
		    ftg.TalkgroupID AS FromTalkgroupID,
		    ftg.[Description] AS FromTalkgroupDescription,
		    ttg.TalkgroupID AS ToTalkgroupID,
		    ttg.[Description] AS ToTalkgroupDescription,
		    MIN(p.FirstSeen) AS FirstSeen,
		    MAX(p.LastSeen) AS LastSeen,
		    SUM(p.HitCount) AS HitCount
	    FROM dbo.Patches p WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = p.SystemID)
	    INNER JOIN dbo.Towers t WITH (NOLOCK)
		    ON (t.ID = p.TowerID)
	    INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
		    ON (ftg.ID = p.FromTalkgroupID)
	    INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
		    ON (ttg.ID = p.ToTalkgroupID)
		WHERE ((s.SystemID = @systemID)
		    AND ((ftg.TalkgroupID = @talkgroupID)
			    OR (ttg.TalkgroupID = @talkgroupID))
			AND ((@searchText IS NULL)
                OR (ftg.[Description] LIKE N'%' + @searchText + N'%')
				OR (ttg.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@dateFrom IS NULL)
                OR (p.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (p.[Date] <= @dateTo))
            AND ((@talkgroupIDFrom IS NULL)
                OR (ftg.TalkgroupID >= @talkgroupIDFrom)
                OR (ttg.TalkgroupID >= @talkgroupIDFrom))
            AND ((@talkgroupIDTo IS NULL)
                OR (ftg.TalkgroupID <= @talkgroupIDTo)
                OR (ttg.TalkgroupID <= @talkgroupIDTo))
            AND ((@firstSeenFrom IS NULL)
                OR (p.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (p.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (p.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (p.LastSeen <= @lastSeenTo)))
	    GROUP BY t.TowerNumber,
		    t.[Description],
		    ftg.TalkgroupID,
		    ftg.[Description],
		    ttg.TalkgroupID,
		    ttg.[Description]
    )
    SELECT pr.TowerNumber, 
        pr.TowerDescription, 
        pr.FromTalkgroupID, 
        pr.FromTalkgroupDescription, 
        pr.ToTalkgroupID, 
        pr.ToTalkgroupDescription, 
        pr.FirstSeen, 
        pr.LastSeen, 
        pr.HitCount,
        COUNT(1) OVER() AS RecordCount
    FROM PatchResults pr
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.TowerNumber END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.TowerNumber END DESC,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.TowerDescription END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.TowerDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_FROM_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.FromTalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_FROM_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.FromTalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_FROM_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.FromTalkgroupDescription END,
		CASE WHEN ((@sortField = @FIELD_FROM_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.FromTalkgroupDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_TO_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.ToTalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_TO_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.ToTalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_TO_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.ToTalkgroupDescription END,
		CASE WHEN ((@sortField = @FIELD_TO_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.ToTalkgroupDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.LastSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.HitCount END,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.HitCount END DESC,
        pr.TowerNumber
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetForSystemToTalkgroupID]    Script Date: 2/12/2019 2:49:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[PatchesGetForSystemToTalkgroupID]
(
	@systemID INT,
	@toTalkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		ftg.TalkgroupID AS FromTalkgroupID,
		ftg.[Description] AS FromTalkgroupDescription,
		ttg.TalkgroupID AS ToTalkgroupID,
		ttg.[Description] AS ToTalkgroupDescription,
		MIN(p.FirstSeen) AS FirstSeen,
		MAX(p.LastSeen) AS LastSeen,
		SUM(p.HitCount) AS HitCount
	FROM dbo.Patches p WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = p.TowerID)
	INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
		ON (ftg.ID = p.FromTalkgroupID)
	INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
		ON (ttg.ID = p.ToTalkgroupID)
	WHERE ((p.SystemID = @systemID)
		AND (ttg.TalkgroupID = @toTalkgroupID))
	GROUP BY t.TowerNumber,
		t.[Description],
		ftg.TalkgroupID,
		ftg.[Description],
		ttg.TalkgroupID,
		ttg.[Description]
	ORDER BY ftg.TalkgroupID,
		ttg.TalkgroupID;
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetForSystemTower]    Script Date: 2/12/2019 2:49:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[PatchesGetForSystemTower]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		ftg.TalkgroupID AS FromTalkgroupID,
		ftg.[Description] AS FromTalkgroupDescription,
		ttg.TalkgroupID AS ToTalkgroupID,
		ttg.[Description] AS ToTalkgroupDescription,
		MIN(p.FirstSeen) AS FirstSeen,
		MAX(p.LastSeen) AS LastSeen,
		SUM(p.HitCount) AS HitCount
	FROM dbo.Patches p WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = p.TowerID)
	INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
		ON (ftg.ID = p.FromTalkgroupID)
	INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
		ON (ttg.ID = p.ToTalkgroupID)
	WHERE ((p.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber))
	GROUP BY t.TowerNumber,
		t.[Description],
		ftg.TalkgroupID,
		ftg.[Description],
		ttg.TalkgroupID,
		ttg.[Description]
	ORDER BY ftg.TalkgroupID,
		ttg.TalkgroupID;
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetForSystemTowerCount]    Script Date: 2/12/2019 2:49:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[PatchesGetForSystemTowerCount]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*)
	FROM dbo.Patches p WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (p.TowerID = t.ID)
	WHERE ((p.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber))
	GROUP BY p.SystemID,
		t.TowerNumber;
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetForSystemTowerFiltersWithPaging]    Script Date: 2/12/2019 2:49:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[PatchesGetForSystemTowerFiltersWithPaging]
(
	@systemID NVARCHAR(50),
	@towerNumber INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @talkgroupIDFrom INT = NULL,
    @talkgroupIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(200),
    @sortDirection NVARCHAR(200),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_FROM_TALKGROUP_ID NVARCHAR(50) = N'FromTalkgroupID',
		@FIELD_FROM_TALKGROUP_NAME NVARCHAR(50) = N'FromTalkgroupName',
        @FIELD_TO_TALKGROUP_ID NVARCHAR(50) = N'ToTalkgroupID',
		@FIELD_TO_TALKGROUP_NAME NVARCHAR(50) = N'ToTalkgroupName',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
        @FIELD_HIT_COUNT NVARCHAR(50) = N'HitCount',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH PatchResults (TowerNumber, TowerDescription, FromTalkgroupID, FromTalkgroupDescription, ToTalkgroupID, ToTalkgroupDescription, FirstSeen, LastSeen, HitCount)
    AS
    (
	    SELECT t.TowerNumber,
		    t.[Description] AS TowerDescription,
		    ftg.TalkgroupID AS FromTalkgroupID,
		    ftg.[Description] AS FromTalkgroupDescription,
		    ttg.TalkgroupID AS ToTalkgroupID,
		    ttg.[Description] AS ToTalkgroupDescription,
		    MIN(p.FirstSeen) AS FirstSeen,
		    MAX(p.LastSeen) AS LastSeen,
		    SUM(p.HitCount) AS HitCount
	    FROM dbo.Patches p WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = p.SystemID)
	    INNER JOIN dbo.Towers t WITH (NOLOCK)
		    ON (t.ID = p.TowerID)
	    INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
		    ON (ftg.ID = p.FromTalkgroupID)
	    INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
		    ON (ttg.ID = p.ToTalkgroupID)
		WHERE ((s.SystemID = @systemID)
            AND (t.TowerNumber = @towerNumber)
			AND ((@searchText IS NULL)
                OR (ftg.[Description] LIKE N'%' + @searchText + N'%')
				OR (ttg.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@dateFrom IS NULL)
                OR (p.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (p.[Date] <= @dateTo))
            AND ((@talkgroupIDFrom IS NULL)
                OR (ftg.TalkgroupID >= @talkgroupIDFrom)
                OR (ttg.TalkgroupID >= @talkgroupIDFrom))
            AND ((@talkgroupIDTo IS NULL)
                OR (ftg.TalkgroupID <= @talkgroupIDTo)
                OR (ttg.TalkgroupID <= @talkgroupIDTo))
            AND ((@firstSeenFrom IS NULL)
                OR (p.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (p.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (p.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (p.LastSeen <= @lastSeenTo)))
	    GROUP BY t.TowerNumber,
            t.[Description],
            ftg.TalkgroupID,
		    ftg.[Description],
		    ttg.TalkgroupID,
		    ttg.[Description]
    )
    SELECT pr.TowerNumber, 
        pr.TowerDescription, 
        pr.FromTalkgroupID, 
        pr.FromTalkgroupDescription, 
        pr.ToTalkgroupID, 
        pr.ToTalkgroupDescription, 
        pr.FirstSeen, 
        pr.LastSeen, 
        pr.HitCount,
		COUNT(1) OVER() AS RecordCount
    FROM PatchResults pr
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_FROM_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.FromTalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_FROM_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.FromTalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_FROM_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.FromTalkgroupDescription END,
		CASE WHEN ((@sortField = @FIELD_FROM_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.FromTalkgroupDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_TO_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.ToTalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_TO_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.ToTalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_TO_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.ToTalkgroupDescription END,
		CASE WHEN ((@sortField = @FIELD_TO_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.ToTalkgroupDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.LastSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pr.HitCount END,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pr.HitCount END DESC,
        pr.FromTalkgroupID
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetSummary]    Script Date: 2/12/2019 2:49:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[PatchesGetSummary]
(
	@systemID INT,
	@fromTalkgroupID INT,
	@toTalkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ftg.TalkgroupID AS FromTalkgroupID,
		ftg.[Description] AS FromTalkgroupDescription,
		ttg.TalkgroupID AS ToTalkgroupID,
		ttg.[Description] AS ToTalkgroupDescription,
		MIN(p.FirstSeen) AS FirstSeen,
		MAX(p.LastSeen) AS LastSeen,
		SUM(p.HitCount) AS HitCount,
		COUNT(1) OVER() AS RecordCount
	FROM dbo.Patches p WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
		ON (ftg.ID = p.FromTalkgroupID)
	INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
		ON (ttg.ID = p.ToTalkgroupID)
	WHERE ((p.SystemID = @systemID)
		AND (ftg.TalkgroupID = @fromTalkgroupID)
		AND (ttg.TalkgroupID = @toTalkgroupID))
	GROUP BY ftg.TalkgroupID,
		ftg.[Description],
		ttg.TalkgroupID,
		ttg.[Description];
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetSummaryForSystem]    Script Date: 2/12/2019 2:49:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[PatchesGetSummaryForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @recordCount INT;

    SELECT @recordCount = COUNT(*)
    FROM dbo.Patches p
    WHERE (p.SystemID = @systemID);

	SELECT ftg.TalkgroupID AS FromTalkgroupID,
		ftg.[Description] AS FromTalkgroupDescription,
		ttg.TalkgroupID AS ToTalkgroupID,
		ttg.[Description] AS ToTalkgroupDescription,
		SUM(p.HitCount) AS HitCount,
		MIN(p.FirstSeen) AS FirstSeen,
		MAX(p.LastSeen) AS LastSeen,
        @recordCount AS RecordCount
	FROM dbo.Patches p WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
		ON (ftg.ID = p.FromTalkgroupID)
	INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
		ON (ttg.ID = p.ToTalkgroupID)
	WHERE (p.SystemID = @systemID)
	GROUP BY ftg.TalkgroupID,
		ftg.[Description],
		ttg.TalkgroupID,
		ttg.[Description]
	ORDER BY ftg.TalkgroupID,
		ttg.TalkgroupID;
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesGetSummaryForSystemFiltersWithPaging]    Script Date: 2/12/2019 2:49:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[PatchesGetSummaryForSystemFiltersWithPaging]
(
	@systemID NVARCHAR(50),
	@searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @talkgroupIDFrom INT = NULL,
    @talkgroupIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
	@sortField NVARCHAR(50),
	@sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_FROM_TALKGROUP_ID NVARCHAR(50) = N'FromTalkgroupID',
		@FIELD_FROM_TALKGROUP_NAME NVARCHAR(50) = N'FromTalkgroupName',
		@FIELD_TO_TALKGROUP_ID NVARCHAR(50) = N'ToTalkgroupID',
		@FIELD_TO_TALKGROUP_NAME NVARCHAR(50) = N'ToTalkgroupName',
		@FIELD_HIT_COUNT NVARCHAR(50) = N'HitCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

	WITH PatchesTotal (FromTalkgroupID, FromTalkgroupDescription, ToTalkgroupID, ToTalkgroupDescription, HitCount, FirstSeen, LastSeen)
	AS
	(
		SELECT ftg.TalkgroupID AS FromTalkgroupID,
			ftg.[Description] AS FromTalkgroupDescription,
			ttg.TalkgroupID AS ToTalkgroupID,
			ttg.[Description] AS ToTalkgroupDescription,
			SUM(p.HitCount) AS HitCount,
			MIN(p.FirstSeen) AS FirstSeen,
			MAX(p.LastSeen) AS LastSeen
		FROM dbo.Patches p WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = p.SystemID)
		INNER JOIN dbo.Talkgroups ftg WITH (NOLOCK)
			ON (ftg.ID= p.FromTalkgroupID)
		INNER JOIN dbo.Talkgroups ttg WITH (NOLOCK)
			ON (ttg.ID = p.ToTalkgroupID)
		WHERE ((s.SystemID = @systemID)
			AND ((@searchText IS NULL)
                OR (ftg.[Description] LIKE N'%' + @searchText + N'%')
				OR (ttg.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@dateFrom IS NULL)
                OR (p.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (p.[Date] <= @dateTo))
            AND ((@talkgroupIDFrom IS NULL)
                OR (ftg.TalkgroupID >= @talkgroupIDFrom)
                OR (ttg.TalkgroupID >= @talkgroupIDFrom))
            AND ((@talkgroupIDTo IS NULL)
                OR (ftg.TalkgroupID <= @talkgroupIDTo)
                OR (ttg.TalkgroupID <= @talkgroupIDTo))
            AND ((@firstSeenFrom IS NULL)
                OR (p.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (p.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (p.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (p.LastSeen <= @lastSeenTo)))
		GROUP BY ftg.TalkgroupID,
			ftg.[Description],
			ttg.TalkgroupID,
			ttg.[Description]
	)
	SELECT pt.FromTalkgroupID,
		pt.FromTalkgroupDescription,
		pt.ToTalkgroupID,
		pt.ToTalkgroupDescription,
		pt.HitCount,
		pt.FirstSeen,
		pt.LastSeen,
        COUNT(1) OVER() AS RecordCount
	FROM PatchesTotal pt
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_FROM_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pt.FromTalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_FROM_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pt.FromTalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_FROM_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pt.FromTalkgroupDescription END,
		CASE WHEN ((@sortField = @FIELD_FROM_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pt.FromTalkgroupDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_TO_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pt.ToTalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_TO_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pt.ToTalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_TO_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pt.ToTalkgroupDescription END,
		CASE WHEN ((@sortField = @FIELD_TO_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pt.ToTalkgroupDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pt.HitCount END,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pt.HitCount END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pt.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pt.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pt.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pt.LastSeen END DESC,
		pt.FromTalkgroupID
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);

END
GO

/****** Object:  StoredProcedure [dbo].[PatchesInsert]    Script Date: 2/12/2019 2:49:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[PatchesInsert]
(
    @systemID INT,
    @towerID INT,
    @fromTalkgroupID INT,
    @toTalkgroupID INT,
    @date DATETIME2(7),
    @firstSeen DATETIME2(7),
    @lastSeen DATETIME2(7),
    @hitCount INT
)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO dbo.Patches
    (
        SystemID,
        TowerID,
        FromTalkgroupID,
        ToTalkgroupID,
        [Date],
        FirstSeen,
        LastSeen,
        HitCount
    )
    VALUES
    (
        @systemID,
        @towerID,
        @fromTalkgroupID,
        @toTalkgroupID,
        @date,
        @firstSeen,
        @lastSeen,
        @hitCount
    );

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[PatchesUpdate]    Script Date: 2/12/2019 2:49:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[PatchesUpdate]
(
    @id INT,
    @systemID INT,
    @towerID INT,
    @fromTalkgroupID INT,
    @toTalkgroupID INT,
    @date DATETIME2(7),
    @firstSeen DATETIME2(7),
    @lastSeen DATETIME2(7),
    @hitCount INT
)
AS
BEGIN
    SET NOCOUNT ON

    UPDATE dbo.Patches
    SET SystemID = @systemID,
        TowerID = @towerID,
        FromTalkgroupID = @fromTalkgroupID,
        ToTalkgroupID = @toTalkgroupID,
        [Date] = @date,
        FirstSeen = @firstSeen,
        LastSeen = @lastSeen,
        HitCount = @hitCount,
        LastModified = GETDATE();
END
GO

/****** Object:  StoredProcedure [dbo].[ProcessedFilesCheckFileExists]    Script Date: 2/12/2019 2:49:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[ProcessedFilesCheckFileExists]
(
	@systemID int,
	@fileName NVARCHAR(500),
    @fileDate DATETIME2(7) = NULL
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*)
	FROM dbo.ProcessedFiles pf
	WHERE ((pf.SystemID = @systemID)
		AND (pf.[FileName] = @fileName)
        AND ((@fileDate IS NULL)
            OR (pf.FileDate = @fileDate)));
END
GO

/****** Object:  StoredProcedure [dbo].[ProcessedFilesDelete]    Script Date: 2/12/2019 2:49:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[ProcessedFilesDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.ProcessedFiles
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[ProcessedFilesDeleteForSystem]    Script Date: 2/12/2019 2:49:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[ProcessedFilesDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.ProcessedFiles
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[ProcessedFilesGet]    Script Date: 2/12/2019 2:49:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[ProcessedFilesGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT pf.ID,
		pf.SystemID,
		pf.[FileName],
		pf.[Type],
		pf.FileDate,
		pf.DateProcessed,
		pf.[RowCount],
		pf.LastModified
	FROM dbo.ProcessedFiles pf
	WHERE (pf.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[ProcessedFilesGetCountForSystem]    Script Date: 2/12/2019 2:49:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ProcessedFilesGetCountForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*)
	FROM dbo.ProcessedFiles p
	WHERE (p.SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[ProcessedFilesGetForSystem]    Script Date: 2/12/2019 2:49:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[ProcessedFilesGetForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT pf.ID,
		pf.SystemID,
		pf.[FileName],
		pf.[Type],
		pf.FileDate,
		pf.DateProcessed,
		pf.[RowCount],
		pf.LastModified
	FROM dbo.ProcessedFiles pf
	WHERE (pf.SystemID = @systemID)
	ORDER BY pf.DateProcessed DESC,
        pf.[FileName] DESC;
END
GO

/****** Object:  StoredProcedure [dbo].[ProcessedFilesGetForSystemFiltersWithPaging]    Script Date: 2/12/2019 2:49:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ProcessedFilesGetForSystemFiltersWithPaging]
(
	@systemID NVARCHAR(50),
	@searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
	@sortField NVARCHAR(50),
	@sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_FILE_NAME NVARCHAR(50) = N'FileName',
        @FIELD_FILE_DATE NVARCHAR(50) = N'FileDate',
		@FIELD_DATE_PROCESSED NVARCHAR(50) = N'DateProcessed',
		@FIELD_ROW_COUNT NVARCHAR(50) = N'RowCount',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

	SELECT pf.ID,
		pf.SystemID,
		pf.[FileName],
        pf.FileDate,
		pf.DateProcessed,
		pf.[RowCount],
		pf.LastModified,
        COUNT(1) OVER() AS RecordCount
	FROM dbo.ProcessedFiles pf WITH (NOLOCK)
    INNER JOIN dbo.Systems s WITH (NOLOCK)
        ON (s.ID = pf.SystemID)
	WHERE ((s.SystemID = @systemID)
		AND ((@searchText IS NULL)
            OR (pf.[FileName] LIKE N'%' + @searchText + N'%'))
        AND ((@dateFrom IS NULL)
            OR (pf.DateProcessed >= @dateFrom))
        AND ((@dateTo IS NULL)
            OR (pf.DateProcessed <= @dateTo)))
	ORDER BY 
		CASE WHEN ((@sortField = @FIELD_FILE_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pf.[FileName] END,
		CASE WHEN ((@sortField = @FIELD_FILE_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pf.[FileName] END,
		CASE WHEN ((@sortField = @FIELD_FILE_DATE) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pf.[FileDate] END,
		CASE WHEN ((@sortField = @FIELD_FILE_DATE) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pf.[FileDate] END,
		CASE WHEN ((@sortField = @FIELD_DATE_PROCESSED) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pf.DateProcessed END,
		CASE WHEN ((@sortField = @FIELD_DATE_PROCESSED) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pf.DateProcessed END,
		CASE WHEN ((@sortField = @FIELD_ROW_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pf.[RowCount] END,
		CASE WHEN ((@sortField = @FIELD_ROW_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pf.[RowCount] END,
		pf.DateProcessed DESC
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);

END
GO

/****** Object:  StoredProcedure [dbo].[ProcessedFilesGetForSystemWithPaging]    Script Date: 2/12/2019 2:49:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ProcessedFilesGetForSystemWithPaging]
(
	@systemID NVARCHAR(50),
	@sortField NVARCHAR(50),
	@sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_FILE_NAME NVARCHAR(50) = N'FileName',
        @FIELD_FILE_DATE NVARCHAR(50) = N'FileDate',
		@FIELD_DATE_PROCESSED NVARCHAR(50) = N'DateProcessed',
		@FIELD_ROW_COUNT NVARCHAR(50) = N'RowCount',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @recordCount INT;

    SELECT @recordCount = COUNT(*)
    FROM dbo.ProcessedFiles pf WITH (NOLOCK)
    INNER JOIN dbo.Systems s WITH (NOLOCK)
        ON (s.ID = pf.SystemID)
    WHERE (s.SystemID = @systemID);

	SELECT pf.ID,
		pf.SystemID,
		pf.[FileName],
        pf.FileDate,
		pf.DateProcessed,
		pf.[RowCount],
		pf.LastModified,
        @recordCount AS RecordCount
	FROM dbo.ProcessedFiles pf WITH (NOLOCK)
    INNER JOIN dbo.Systems s WITH (NOLOCK)
        ON (s.ID = pf.SystemID)
	WHERE (s.SystemID = @systemID)
	ORDER BY 
		CASE WHEN ((@sortField = @FIELD_FILE_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pf.[FileName] END,
		CASE WHEN ((@sortField = @FIELD_FILE_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pf.[FileName] END DESC,
		CASE WHEN ((@sortField = @FIELD_FILE_DATE) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pf.[FileDate] END,
		CASE WHEN ((@sortField = @FIELD_FILE_DATE) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pf.[FileDate] END DESC,
		CASE WHEN ((@sortField = @FIELD_DATE_PROCESSED) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pf.DateProcessed END,
		CASE WHEN ((@sortField = @FIELD_DATE_PROCESSED) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pf.DateProcessed END DESC,
		CASE WHEN ((@sortField = @FIELD_ROW_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN pf.[RowCount] END,
		CASE WHEN ((@sortField = @FIELD_ROW_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN pf.[RowCount] END DESC,
		pf.DateProcessed DESC
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);

END
GO

/****** Object:  StoredProcedure [dbo].[ProcessedFilesGetForType]    Script Date: 2/12/2019 2:49:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ProcessedFilesGetForType]
(
	@systemID INT,
	@type NVARCHAR(50),
	@fileName NVARCHAR(50) = NULL
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT pf.ID,
		pf.SystemID,
		pf.[FileName],
		pf.[Type],
		pf.FileDate,
		pf.DateProcessed,
		pf.[RowCount],
		pf.LastModified
	FROM dbo.ProcessedFiles pf
	WHERE ((pf.SystemID = @systemID)
		AND (pf.[Type] = @type)
		AND ((@fileName IS NULL)
			OR (pf.[FileName] = @fileName)));
END
GO

/****** Object:  StoredProcedure [dbo].[ProcessedFilesInsert]    Script Date: 2/12/2019 2:49:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[ProcessedFilesInsert]
(
	@systemID INT,
	@fileName NVARCHAR(500),
	@type NVARCHAR(50),
	@fileDate DATETIME2(7),
	@dateProcessed DATETIME2(7),
	@rowCount INT
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.ProcessedFiles
	(
		SystemID,
		[FileName],
		[Type],
		FileDate,
		DateProcessed,
		[RowCount]
	)
	VALUES
	(
		@systemID,
		@fileName,
		@type,
		@fileDate,
		@dateProcessed,
		@rowCount
	);

	SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[ProcessedFilesUpdate]    Script Date: 2/12/2019 2:49:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[ProcessedFilesUpdate]
(
	@id INT,
	@systemID INT,
	@fileName NVARCHAR(500),
	@type NVARCHAR(50),
	@fileDate DATETIME2(7),
	@dateProcessed DATETIME2(7),
	@rowCount INT
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.ProcessedFiles
	SET SystemID = @systemID,
		[FileName] = @fileName,
		[Type] = @type,
		FileDate = @fileDate,
		DateProcessed = @dateProcessed,
		[RowCount] = @rowCount
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[RadioHistoryDelete]    Script Date: 2/12/2019 2:49:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[RadioHistoryDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.RadioHistory
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[RadioHistoryDeleteForSystem]    Script Date: 2/12/2019 2:49:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[RadioHistoryDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.RadioHistory
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[RadioHistoryGet]    Script Date: 2/12/2019 2:49:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[RadioHistoryGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT rh.ID,
		rh.SystemID,
		rh.RadioID,
		rh.[Description],
		rh.FirstSeen,
		rh.LastSeen,
		rh.LastModified
	FROM dbo.RadioHistory rh
	WHERE (rh.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[RadioHistoryGetForRadio]    Script Date: 2/12/2019 2:49:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[RadioHistoryGetForRadio]
(
	@systemID INT,
	@radioID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT rh.ID,
		rh.SystemID,
		rh.RadioID,
		rh.[Description],
		rh.FirstSeen,
		rh.LastSeen,
		rh.LastModified
	FROM dbo.RadioHistory rh WITH (NOLOCK)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = rh.RadioID)
	WHERE ((rh.SystemID = @systemID)
		AND (r.RadioID = @radioID))
	ORDER BY rh.LastSeen DESC,
		rh.FirstSeen DESC;
END
GO

/****** Object:  StoredProcedure [dbo].[RadioHistoryGetForRadioCount]    Script Date: 2/12/2019 2:49:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[RadioHistoryGetForRadioCount]
(
	@systemID INT,
	@radioID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*)
	FROM dbo.RadioHistory rh WITH (NOLOCK)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = rh.RadioID)
	WHERE ((rh.SystemID = @systemID)
		AND (r.RadioID = @radioID));
END
GO

/****** Object:  StoredProcedure [dbo].[RadioHistoryGetForRadioFiltersWithPaging]    Script Date: 2/12/2019 2:49:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[RadioHistoryGetForRadioFiltersWithPaging]
(
	@systemID NVARCHAR(50),
	@radioID INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @radioIDFrom INT = NULL,
    @radioIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @FIELD_DESCRIPTION NVARCHAR(50) = N'Description',
        @FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
        @FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
        @DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
        @DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH RadioHistoryResults ([Description], FirstSeen, LastSeen)
    AS
    (
	    SELECT rh.[Description],
		    rh.FirstSeen,
		    rh.LastSeen
	    FROM dbo.RadioHistory rh WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = rh.SystemID)
	    INNER JOIN dbo.Radios r WITH (NOLOCK)
		    ON (r.ID = rh.RadioID)
	    WHERE ((s.SystemID = @systemID)
		    AND (r.RadioID = @radioID)
			AND ((@searchText IS NULL)
				OR (rh.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (rh.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (rh.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (rh.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenFrom IS NULL)
                OR (rh.LastSeen <= @lastSeenTo)))
    )
    SELECT rhr.[Description], 
        rhr.FirstSeen, 
        rhr.LastSeen, 
        COUNT(1) OVER() AS RecordCount
    FROM RadioHistoryResults rhr
    ORDER BY
		CASE WHEN ((@sortField = @FIELD_DESCRIPTION) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rhr.[Description] END,
		CASE WHEN ((@sortField = @FIELD_DESCRIPTION) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rhr.[Description] END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rhr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rhr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rhr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rhr.LastSeen END DESC,
        rhr.LastSeen DESC,
        rhr.FirstSeen DESC
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[RadioHistoryGetForRadioSearchCount]    Script Date: 2/12/2019 2:49:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[RadioHistoryGetForRadioSearchCount]
(
	@systemID INT,
	@radioID INT,
    @searchText NVARCHAR(200)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*)
	FROM dbo.RadioHistory rh WITH (NOLOCK)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = rh.RadioID)
	WHERE ((rh.SystemID = @systemID)
		AND (r.RadioID = @radioID)
        AND (rh.[Description] LIKE N'%' + @searchText + N'%'));
END
GO

/****** Object:  StoredProcedure [dbo].[RadioHistoryGetForSystem]    Script Date: 2/12/2019 2:49:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[RadioHistoryGetForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT rh.ID,
		rh.SystemID,
		r.RadioID,
		rh.[Description],
		rh.FirstSeen,
		rh.LastSeen,
		rh.LastModified
	FROM dbo.RadioHistory rh WITH (NOLOCK)
    INNER JOIN dbo.Radios r WITH (NOLOCK)
        ON (r.ID = rh.RadioID)
	WHERE (rh.SystemID = @systemID)
	ORDER BY rh.RadioID,
		rh.LastSeen DESC,
		rh.FirstSeen DESC;
END
GO

/****** Object:  StoredProcedure [dbo].[RadioHistoryInsert]    Script Date: 2/12/2019 2:49:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[RadioHistoryInsert]
(
	@systemID INT,
	@radioID INT,
	@description NVARCHAR(100),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @radioIDKey INT = NULL;

	SELECT @radioIDKey = r.ID
	FROM dbo.Radios r
	WHERE ((r.SystemID = @systemID)
		AND (r.RadioID = @radioID));

	IF (@radioIDKey IS NOT NULL)
	BEGIN

		INSERT INTO dbo.RadioHistory
		(
			SystemID,
			RadioID,
			[Description],
			FirstSeen,
			LastSeen
		)
		VALUES
		(
			@systemID,
			@radioIDKey,
			@description,
			@firstSeen,
			@lastSeen
		);

		SELECT SCOPE_IDENTITY() AS ID;
	END
	ELSE
	BEGIN
		SELECT -1 AS ID;
	END
END
GO

/****** Object:  StoredProcedure [dbo].[RadioHistoryUpdate]    Script Date: 2/12/2019 2:49:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[RadioHistoryUpdate]
(
	@id INT,
	@systemID INT,
	@radioID INT,
	@description NVARCHAR(100),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.RadioHistory
	SET SystemID = @systemID,
		RadioID = @radioID,
		[Description] = @description,
		FirstSeen = @firstSeen,
		LastSeen = @lastSeen,
		LastModified = GETDATE()
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[RadiosDelete]    Script Date: 2/12/2019 2:49:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[RadiosDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.Radios
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[RadiosDeleteForSystem]    Script Date: 2/12/2019 2:49:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[RadiosDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.Radios
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[RadiosGet]    Script Date: 2/12/2019 2:49:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[RadiosGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT r.ID,
		r.SystemID,
		r.RadioID,
		r.[Description],
		r.LastSeen,
		r.LastSeenProgram,
		r.LastSeenProgramUnix,
		r.FirstSeen,
		r.FGColor,
		r.BGColor,
		r.HitCount,
        r.PhaseIISeen,
		r.LastModified
	FROM dbo.Radios r
	WHERE (r.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[RadiosGetCountForSystem]    Script Date: 2/12/2019 2:49:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[RadiosGetCountForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*)
	FROM dbo.Radios r
	WHERE (r.SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[RadiosGetDetail]    Script Date: 2/12/2019 2:49:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[RadiosGetDetail]
(
    @systemID INT,
    @radioID INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT r.ID,
        r.RadioID,
        r.[Description],
        r.FirstSeen,
        r.LastSeen,
        r.HitCount,
        r.PhaseIISeen,
        SUM(ISNULL(tr.AffiliationCount, 0)) AS AffiliationCount,
        SUM(ISNULL(tr.DeniedCount, 0)) AS DeniedCount,
        SUM(ISNULL(tr.VoiceGrantCount, 0)) AS VoiceGrantCount,
        SUM(ISNULL(tr.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
        SUM(ISNULL(tr.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
        SUM(ISNULL(tr.DataCount, 0) +
            ISNULL(tr.PrivateDataCount, 0)) AS DataCount,
        COUNT(1) AS RecordCount
    FROM dbo.Radios r WITH (NOLOCK)
    LEFT JOIN TowerRadios tr WITH (NOLOCK)
        ON (tr.RadioID = r.ID)
    WHERE ((r.SystemID = @systemID)
        AND (r.RadioID = @radioID))
    GROUP BY r.ID,
        r.RadioID,
        r.[Description],
        r.FirstSeen,
        r.LastSeen,
        r.HitCount,
        r.PhaseIISeen;
END
GO

/****** Object:  StoredProcedure [dbo].[RadiosGetDetailFilters]    Script Date: 2/12/2019 2:49:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[RadiosGetDetailFilters]
(
    @systemID INT,
    @radioID INT,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT r.ID,
        r.RadioID,
        r.[Description],
        r.FirstSeen,
        r.LastSeen,
        r.HitCount,
        r.PhaseIISeen,
        SUM(ISNULL(tr.AffiliationCount, 0)) AS AffiliationCount,
        SUM(ISNULL(tr.DeniedCount, 0)) AS DeniedCount,
        SUM(ISNULL(tr.VoiceGrantCount, 0)) AS VoiceGrantCount,
        SUM(ISNULL(tr.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
        SUM(ISNULL(tr.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
        SUM(ISNULL(tr.DataCount, 0) +
            ISNULL(tr.PrivateDataCount, 0)) AS DataCount,
        COUNT(1) AS RecordCount
    FROM dbo.Radios r WITH (NOLOCK)
    INNER JOIN TowerRadios tr WITH (NOLOCK)
        ON ((tr.RadioID = r.ID)
            AND ((@dateFrom IS NULL)
                OR (tr.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (tr.[Date] <= @dateTo)))
    WHERE ((r.SystemID = @systemID)
        AND (r.RadioID = @radioID))
    GROUP BY r.ID,
        r.RadioID,
        r.[Description],
        r.FirstSeen,
        r.LastSeen,
        r.HitCount,
        r.PhaseIISeen
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[RadiosGetDetailForSystem]    Script Date: 2/12/2019 2:49:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[RadiosGetDetailForSystem]
(
    @systemID INT
)
AS
    SET NOCOUNT ON;

    DECLARE @recordCount INT;

    SELECT @recordCount = COUNT(*)
    FROM dbo.Radios r
    WHERE (r.SystemID = @systemID);

	SELECT r.ID,
        r.RadioID,
		r.[Description],
		r.FirstSeen,
		r.LastSeen,
        r.PhaseIISeen,
        SUM(ISNULL(tr.AffiliationCount, 0)) AS AffiliationCount,
        SUM(ISNULL(tr.DeniedCount, 0)) AS DeniedCount,
        SUM(ISNULL(tr.VoiceGrantCount, 0)) AS VoiceGrantCount,
        SUM(ISNULL(tr.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
        SUM(ISNULL(tr.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
        SUM(ISNULL(tr.DataCount, 0)) AS DataCount,
        @recordCount AS RecordCount
    FROM dbo.Radios r WITH (NOLOCK)
    LEFT JOIN TowerRadios tr WITH (NOLOCK)
        ON (tr.RadioID = r.ID)
    WHERE (r.SystemID = @systemID)
    GROUP BY r.ID,
        r.RadioID,
        r.[Description],
        r.FirstSeen,
        r.LastSeen,
        r.PhaseIISeen
    ORDER BY r.RadioID;
GO

/****** Object:  StoredProcedure [dbo].[RadiosGetDetailForSystemActiveFiltersWithPaging]    Script Date: 2/12/2019 2:49:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[RadiosGetDetailForSystemActiveFiltersWithPaging]
(
	@systemID NVARCHAR(50),
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @radioIDFrom INT = NULL,
    @radioIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
	@sortField NVARCHAR(50),
	@sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_RADIO_ID NVARCHAR(50) = N'RadioID',
		@FIELD_RADIO_NAME NVARCHAR(50) = N'RadioName',
		@FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
		@FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PHASE_II_SEEN NVARCHAR(50) = N'PhaseIISeen',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @systemIDKey INT;
	
    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

	WITH RadioTotals (ID, RadioID, [Description], FirstSeen, LastSeen, PhaseIISeen, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount)
	AS
	(
		SELECT r.ID,
            r.RadioID,
			r.[Description],
			r.FirstSeen,
			r.LastSeen,
            r.PhaseIISeen,
            SUM(ISNULL(tr.AffiliationCount, 0)) AS AffiliationCount,
            SUM(ISNULL(tr.DeniedCount, 0)) AS DeniedCount,
            SUM(ISNULL(tr.VoiceGrantCount, 0)) AS VoiceGrantCount,
            SUM(ISNULL(tr.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
            SUM(ISNULL(tr.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
            SUM(ISNULL(tr.DataCount, 0)) AS DataCount
        FROM dbo.Radios r WITH (NOLOCK)
        INNER JOIN TowerRadios tr WITH (NOLOCK)
            ON ((tr.RadioID = r.ID)
                AND ((@dateFrom IS NULL)
                    OR (tr.[Date] >= @dateFrom))
                AND ((@dateTo IS NULL)
                    OR (tr.[Date] <= @dateTo)))
		WHERE ((r.SystemID = @systemIDKey)
			AND ((@searchText IS NULL)
				OR (r.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@radioIDFrom IS NULL)
                OR (r.RadioID >= @radioIDFrom))
            AND ((@radioIDTo IS NULL)
                OR (r.RadioID <= @radioIDTo))
            AND ((@firstSeenFrom IS NULL)
                OR (r.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (r.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (r.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (r.LastSeen <= @lastSeenTo)))
		GROUP BY r.ID,
            r.RadioID,
			r.[Description],
			r.FirstSeen,
			r.LastSeen,
            r.PhaseIISeen
	)
	SELECT rt.ID,
        rt.RadioID,
		rt.[Description],
		rt.FirstSeen,
		rt.LastSeen,
        rt.PhaseIISeen,
		rt.AffiliationCount,
		rt.DeniedCount,
		rt.VoiceGrantCount,
		rt.EmergencyVoiceGrantCount,
		rt.EncryptedVoiceGrantCount,
		rt.DataCount,
        COUNT(1) OVER() AS RecordCount
	FROM RadioTotals rt
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.RadioID END,
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.RadioID END DESC,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.[Description] END,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.[Description] END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.DataCount END DESC,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.PhaseIISeen END,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.PhaseIISeen END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.LastSeen END DESC,
		rt.RadioID
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[RadiosGetDetailForSystemActiveWithPaging]    Script Date: 2/12/2019 2:49:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[RadiosGetDetailForSystemActiveWithPaging]
(
	@systemID NVARCHAR(50),
	@sortField NVARCHAR(50),
	@sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
	SET NOCOUNT ON;

	DECLARE @FIELD_RADIO_ID NVARCHAR(50) = N'RadioID',
		@FIELD_RADIO_NAME NVARCHAR(50) = N'RadioName',
		@FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
		@FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
        @FIELD_PHASE_II_SEEN NVARCHAR(50) = N'PhaseIISeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';
	
	WITH RadioTotals (ID, RadioID, [Description], FirstSeen, LastSeen, PhaseIISeen, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount)
	AS
	(
		SELECT r.ID,
            r.RadioID,
			r.[Description],
			r.FirstSeen,
			r.LastSeen,
            r.PhaseIISeen,
            SUM(ISNULL(tr.AffiliationCount, 0)) AS AffiliationCount,
            SUM(ISNULL(tr.DeniedCount, 0)) AS DeniedCount,
            SUM(ISNULL(tr.VoiceGrantCount, 0)) AS VoiceGrantCount,
            SUM(ISNULL(tr.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
            SUM(ISNULL(tr.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
            SUM(ISNULL(tr.DataCount, 0)) AS DataCount
        FROM dbo.Radios r WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = r.SystemID)
        INNER JOIN TowerRadios tr WITH (NOLOCK)
            ON (tr.RadioID = r.ID)
		WHERE (s.SystemID = @systemID)
		GROUP BY r.ID,
            r.RadioID,
			r.[Description],
			r.FirstSeen,
			r.LastSeen,
            r.PhaseIISeen
	)
	SELECT rt.ID,
        rt.RadioID,
		rt.[Description],
		rt.FirstSeen,
		rt.LastSeen,
        rt.PhaseIISeen,
		rt.AffiliationCount,
		rt.DeniedCount,
		rt.VoiceGrantCount,
		rt.EmergencyVoiceGrantCount,
		rt.EncryptedVoiceGrantCount,
		rt.DataCount,
        COUNT(1) OVER() AS RecordCount
	FROM RadioTotals rt
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.RadioID END,
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.RadioID END DESC,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.[Description] END,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.[Description] END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.DataCount END DESC,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.PhaseIISeen END,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.PhaseIISeen END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.LastSeen END DESC,
		rt.RadioID
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
GO

/****** Object:  StoredProcedure [dbo].[RadiosGetDetailForSystemFiltersWithPaging]    Script Date: 2/12/2019 2:49:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[RadiosGetDetailForSystemFiltersWithPaging]
(
	@systemID NVARCHAR(50),
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @radioIDFrom INT = NULL,
    @radioIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
	@sortField NVARCHAR(50),
	@sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_RADIO_ID NVARCHAR(50) = N'RadioID',
		@FIELD_RADIO_NAME NVARCHAR(50) = N'RadioName',
		@FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
		@FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PHASE_II_SEEN NVARCHAR(50) = N'PhaseIISeen',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

	WITH RadioTotals (ID, RadioID, [Description], FirstSeen, LastSeen, PhaseIISeen, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount)
	AS
	(
		SELECT r.ID,
            r.RadioID,
			r.[Description],
			r.FirstSeen,
			r.LastSeen,
            r.PhaseIISeen,
            SUM(ISNULL(tr.AffiliationCount, 0)) AS AffiliationCount,
            SUM(ISNULL(tr.DeniedCount, 0)) AS DeniedCount,
            SUM(ISNULL(tr.VoiceGrantCount, 0)) AS VoiceGrantCount,
            SUM(ISNULL(tr.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
            SUM(ISNULL(tr.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
            SUM(ISNULL(tr.DataCount, 0)) AS DataCount
        FROM dbo.Radios r WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = r.SystemID)
        LEFT JOIN TowerRadios tr WITH (NOLOCK)
            ON ((tr.RadioID = r.ID)
                AND ((@dateFrom IS NULL)
                    OR (tr.[Date] >= @dateFrom))
                AND ((@dateTo IS NULL)
                    OR (tr.[Date] <= @dateTo)))
		WHERE ((s.SystemID = @systemID)
			AND ((@searchText IS NULL)
				OR (r.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@radioIDFrom IS NULL)
                OR (r.RadioID >= @radioIDFrom))
            AND ((@radioIDTo IS NULL)
                OR (r.RadioID <= @radioIDTo))
            AND ((@firstSeenFrom IS NULL)
                OR (r.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (r.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (r.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (r.LastSeen <= @lastSeenTo)))
		GROUP BY r.ID,
            r.RadioID,
			r.[Description],
			r.FirstSeen,
			r.LastSeen,
            r.PhaseIISeen
	)
	SELECT rt.ID,
        rt.RadioID,
		rt.[Description],
		rt.FirstSeen,
		rt.LastSeen,
        rt.PhaseIISeen,
		rt.AffiliationCount,
		rt.DeniedCount,
		rt.VoiceGrantCount,
		rt.EmergencyVoiceGrantCount,
		rt.EncryptedVoiceGrantCount,
		rt.DataCount,
        COUNT(1) OVER() AS RecordCount
	FROM RadioTotals rt
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.RadioID END,
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.RadioID END DESC,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.[Description] END,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.[Description] END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.DataCount END DESC,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.PhaseIISeen END,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.PhaseIISeen END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.LastSeen END DESC,
		rt.RadioID
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[RadiosGetDetailForSystemWithPaging]    Script Date: 2/12/2019 2:49:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[RadiosGetDetailForSystemWithPaging]
(
	@systemID NVARCHAR(50),
	@sortField NVARCHAR(50),
	@sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_RADIO_ID NVARCHAR(50) = N'RadioID',
		@FIELD_RADIO_NAME NVARCHAR(50) = N'RadioName',
		@FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
		@FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PHASE_II_SEEN NVARCHAR(50) = N'PhaseIISeen',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @recordCount INT;

    SELECT @recordCount = COUNT(*)
    FROM dbo.Radios r WITH (NOLOCK)
    INNER JOIN dbo.Systems s WITH (NOLOCK)
        ON (s.ID = r.SystemID)
    WHERE (s.SystemID = @systemID);
	
	WITH RadioTotals (ID, RadioID, [Description], FirstSeen, LastSeen, PhaseIISeen, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount)
	AS
	(
		SELECT r.ID,
            r.RadioID,
			r.[Description],
			r.FirstSeen,
			r.LastSeen,
            r.PhaseIISeen,
            SUM(ISNULL(tr.AffiliationCount, 0)) AS AffiliationCount,
            SUM(ISNULL(tr.DeniedCount, 0)) AS DeniedCount,
            SUM(ISNULL(tr.VoiceGrantCount, 0)) AS VoiceGrantCount,
            SUM(ISNULL(tr.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
            SUM(ISNULL(tr.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
            SUM(ISNULL(tr.DataCount, 0)) AS DataCount
        FROM dbo.Radios r WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = r.SystemID)
        LEFT JOIN TowerRadios tr WITH (NOLOCK)
            ON (tr.RadioID = r.ID)
		WHERE (s.SystemID = @systemID)
		GROUP BY r.ID,
            r.RadioID,
			r.[Description],
			r.FirstSeen,
			r.LastSeen,
            r.PhaseIISeen
	)
	SELECT rt.ID,
        rt.RadioID,
		rt.[Description],
		rt.FirstSeen,
		rt.LastSeen,
        rt.PhaseIISeen,
		rt.AffiliationCount,
		rt.DeniedCount,
		rt.VoiceGrantCount,
		rt.EmergencyVoiceGrantCount,
		rt.EncryptedVoiceGrantCount,
		rt.DataCount,
        @recordCount AS RecordCount
	FROM RadioTotals rt
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.RadioID END,
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.RadioID END DESC,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.[Description] END,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.[Description] END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.DataCount END DESC,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.PhaseIISeen END,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.PhaseIISeen END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rt.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rt.LastSeen END DESC,
		rt.RadioID
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[RadiosGetForSystem]    Script Date: 2/12/2019 2:49:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[RadiosGetForSystem]
(
    @systemID INT
)
AS
    SET NOCOUNT ON;

    SELECT r.ID,
        r.SystemID,
        r.RadioID,
        r.[Description],
        r.LastSeen,
        r.LastSeenProgram,
        r.LastSeenProgramUnix,
        r.FirstSeen,
        r.FGColor,
        r.BGColor,
        r.HitCount,
        r.PhaseIISeen,
        r.LastModified
    FROM dbo.Radios r
    WHERE (r.SystemID = @systemID);
GO

/****** Object:  StoredProcedure [dbo].[RadiosGetForSystemRadio]    Script Date: 2/12/2019 2:49:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[RadiosGetForSystemRadio]
(
    @systemID INT,
    @radioID INT
)
AS
    SET NOCOUNT ON;

    SELECT r.ID,
        r.SystemID,
        r.RadioID,
        r.[Description],
        r.LastSeen,
        r.LastSeenProgram,
        r.LastSeenProgramUnix,
        r.FirstSeen,
        r.FGColor,
        r.BGColor,
        r.HitCount,
        r.PhaseIISeen,
        r.LastModified
    FROM dbo.Radios r
    WHERE (r.SystemID = @systemID);
GO

/****** Object:  StoredProcedure [dbo].[RadiosGetSystemNames]    Script Date: 2/12/2019 2:49:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[RadiosGetSystemNames]
(
    @systemID NVARCHAR(50),
    @searchText NVARCHAR(200) = NULL,
    @radioIDFrom INT = NULL,
    @radioIDTo INT = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
    @row INT,
    @pageSize INT
)
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @FIELD_RADIO_ID NVARCHAR(50) = N'RadioID',
	    @FIELD_RADIO_NAME NVARCHAR(50) = N'RadioName',
	    @DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
	    @DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @systemIDKey INT;

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    WITH RadioResults (RadioID, [Description])
    AS
    (
        SELECT r.RadioID,
            r.[Description]
        FROM dbo.Radios r
        WHERE (((r.SystemID = @systemIDKey)
            AND (r.[Description] NOT LIKE N'TG:%')
            AND (r.[Description] NOT LIKE N'DT:%')
            AND (r.[Description] NOT LIKE N'<Unknown>%'))
            AND ((@searchText IS NULL)
                OR (r.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@radioIDFrom IS NULL)
                OR (r.RadioID >= @radioIDFrom))
            AND ((@radioIDTo IS NULL)
                OR (r.RadioID <= @radioIDTo)))
    )
    SELECT rr.RadioID,
        rr.[Description],
        COUNT(1) OVER() AS RecordCount
    FROM RadioResults rr
    ORDER BY
	    CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rr.RadioID END,
	    CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rr.RadioID END DESC,
	    CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN rr.[Description] END,
	    CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN rr.[Description] END DESC,
        rr.RadioID
    OFFSET ((@row - 1) * @pageSize) ROWS
    FETCH NEXT @pageSize ROWS ONLY;
END
GO

/****** Object:  StoredProcedure [dbo].[RadiosInsert]    Script Date: 2/12/2019 2:49:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[RadiosInsert]
(
    @systemID INT,
    @radioID INT,
    @description NVARCHAR(100),
    @lastSeen DATETIME2(7),
    @lastSeenProgram DATETIME2(7),
    @lastSeenProgramUnix BIGINT,
    @firstSeen DATETIME2(7),
    @fgColor NVARCHAR(50),
    @bgColor NVARCHAR(50),
    @hitCount INT,
    @phaseIISeen BIT
)
AS
BEGIN
    SET NOCOUNT ON;

	INSERT INTO dbo.Radios
    (
        SystemID,
        RadioID,
        [Description],
        LastSeen,
        LastSeenProgram,
        LastSeenProgramUnix,
        FirstSeen,
        FGColor,
        BGColor,
        HitCount,
        PhaseIISeen
    )
    VALUES
    (
        @systemID,
        @radioID,
        @description,
        @lastSeen,
        @lastSeenProgram,
        @lastSeenProgramUnix,
        @firstSeen,
        @fgColor,
        @bgColor,
        @hitCount,
        @phaseIISeen
    );

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[RadiosUpdate]    Script Date: 2/12/2019 2:49:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[RadiosUpdate]
(
    @systemID INT,
    @radioID INT,
    @description NVARCHAR(100),
    @lastSeen DATETIME2(7),
    @lastSeenProgram DATETIME2(7),
    @lastSeenProgramUnix BIGINT,
    @firstSeen DATETIME2(7),
    @fgColor NVARCHAR(50),
    @bgColor NVARCHAR(50),
    @hitCount INT,
    @phaseIISeen BIT
)
AS
    UPDATE dbo.Radios
    SET [Description] = @description,
        LastSeen = @lastSeen,
        LastSeenProgram = @lastSeenProgram,
        LastSeenProgramUnix = @lastSeenProgramUnix,
        FirstSeen = @firstSeen,
        FGColor = @fgColor,
        BGColor = @bgColor,
        HitCount = @hitCount,
        PhaseIISeen = @phaseIISeen,
        LastModified = GETDATE()
    WHERE ((SystemID = @systemID)
        AND (RadioID = @radioID));
GO

/****** Object:  StoredProcedure [dbo].[SystemsDelete]    Script Date: 2/12/2019 2:49:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SystemsDelete]
(
	@systemID NVARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.Systems
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[SystemsGet]    Script Date: 2/12/2019 2:49:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SystemsGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT s.ID,
		s.SystemID,
		s.SystemIDDecimal,
		s.[Description],
		s.WACN,
		s.FirstSeen,
		s.LastSeen,
		s.LastModified
	FROM dbo.Systems s
	WHERE (s.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[SystemsGetCount]    Script Date: 2/12/2019 2:49:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SystemsGetCount]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*)
	FROM Systems;
END
GO

/****** Object:  StoredProcedure [dbo].[SystemsGetForSystem]    Script Date: 2/12/2019 2:49:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SystemsGetForSystem]
(
	@systemID NVARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT s.ID,
		s.SystemID,
		s.SystemIDDecimal,
		s.[Description],
		s.WACN,
		s.FirstSeen,
		s.LastSeen,
		s.LastModified
	FROM dbo.Systems s
	WHERE (s.SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[SystemsGetID]    Script Date: 2/12/2019 2:49:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SystemsGetID]
(
	@systemID NVARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT s.ID
	FROM dbo.Systems s
	WHERE (s.SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[SystemsGetList]    Script Date: 2/12/2019 2:49:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SystemsGetList]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT s.ID,
		s.SystemID,
		s.[Description],
		s.FirstSeen,
		s.LastSeen,
		( SELECT COUNT(*)
		  FROM dbo.Talkgroups t
		  WHERE (t.SystemID = s.ID) ) AS TalkgroupCount,
		( SELECT COUNT(*)
		  FROM dbo.Radios r
		  WHERE (r.SystemID = s.ID) ) AS RadioCount,
		( SELECT COUNT(*)
		  FROM dbo.Towers t
		  WHERE (t.SystemID = s.ID) ) AS TowerCount,
		( SELECT SUM(ISNULL(pf.[RowCount], 0))
		  FROM dbo.ProcessedFiles pf
		  WHERE (pf.SystemID = s.ID) ) AS [RowCount],
		s.LastModified
	FROM dbo.Systems s
	ORDER BY s.SystemID;
END
GO

/****** Object:  StoredProcedure [dbo].[SystemsGetListFilters]    Script Date: 2/12/2019 2:49:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SystemsGetListFilters]
(
    @searchText NVARCHAR(200) = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @FIELD_SYSTEM_ID NVARCHAR(50) = N'SystemID',
        @FIELD_DESCRIPTION NVARCHAR(50) = N'Description',
        @FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
        @FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
        @FIELD_TALKGROUP_COUNT NVARCHAR(50) = N'TalkgroupCount',
        @FIELD_RADIO_COUNT NVARCHAR(50) = N'RadioCount',
        @FIELD_TOWER_COUNT NVARCHAR(50) = N'TowerCount',
        @FIELD_ROW_COUNT NVARCHAR(50) = N'RowCount',
        @DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
        @DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

	SELECT s.ID,
		s.SystemID,
		s.[Description],
		s.FirstSeen,
		s.LastSeen,
		( SELECT COUNT(*)
		  FROM dbo.Talkgroups t
		  WHERE (t.SystemID = s.ID) ) AS TalkgroupCount,
		( SELECT COUNT(*)
		  FROM dbo.Radios r
		  WHERE (r.SystemID = s.ID) ) AS RadioCount,
		( SELECT COUNT(*)
		  FROM dbo.Towers t
		  WHERE (t.SystemID = s.ID) ) AS TowerCount,
		( SELECT SUM(ISNULL(pf.[RowCount], 0))
		  FROM dbo.ProcessedFiles pf
		  WHERE (pf.SystemID = s.ID) ) AS [RowCount],
		s.LastModified
	FROM dbo.Systems s
    WHERE (((@searchText IS NULL)
            OR (s.SystemID LIKE N'%' + @searchText + N'%')
            OR (s.[Description] LIKE N'%' + @searchText + N'%'))
        AND ((@firstSeenFrom IS NULL)
            OR (s.FirstSeen >= @firstSeenFrom))
        AND ((@firstSeenTo IS NULL)
            OR (s.FirstSeen <= @firstSeenTo))
        AND ((@lastSeenFrom IS NULL)
            OR (s.LAstSeen >= @lastSeenFrom))
        AND ((@lastSeenTo IS NULL)
            OR (s.LastSeen <= @lastSeenTo)))
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_SYSTEM_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN s.SystemID END,
		CASE WHEN ((@sortField = @FIELD_SYSTEM_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN s.SystemID END DESC,
		CASE WHEN ((@sortField = @FIELD_DESCRIPTION) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN s.[Description] END,
		CASE WHEN ((@sortField = @FIELD_DESCRIPTION) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN s.[Description] END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN s.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN s.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ( SELECT COUNT(*)
		                                                                                                     FROM dbo.Talkgroups tg
		                                                                                                     WHERE (tg.SystemID = s.ID) ) END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ( SELECT COUNT(*)
		                                                                                                      FROM dbo.Talkgroups tg
		                                                                                                      WHERE (tg.SystemID = s.ID) ) END DESC,
 		CASE WHEN ((@sortField = @FIELD_RADIO_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ( SELECT COUNT(*)
		                                                                                                 FROM dbo.Radios r
		                                                                                                 WHERE (r.SystemID = s.ID) ) END,
		CASE WHEN ((@sortField = @FIELD_RADIO_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ( SELECT COUNT(*)
		                                                                                                  FROM dbo.Radios r
		                                                                                                  WHERE (r.SystemID = s.ID) ) END DESC,
 		CASE WHEN ((@sortField = @FIELD_TOWER_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ( SELECT COUNT(*)
		                                                                                                 FROM dbo.Towers t
		                                                                                                 WHERE (t.SystemID = s.ID) ) END,
		CASE WHEN ((@sortField = @FIELD_TOWER_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ( SELECT COUNT(*)
		                                                                                                  FROM dbo.Towers t
		                                                                                                  WHERE (t.SystemID = s.ID) ) END DESC,
 		CASE WHEN ((@sortField = @FIELD_ROW_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ( SELECT SUM(ISNULL(pf.[RowCount], 0))
		                                                                                               FROM dbo.ProcessedFiles pf
		                                                                                               WHERE (pf.SystemID = s.ID) ) END,
		CASE WHEN ((@sortField = @FIELD_ROW_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ( SELECT SUM(ISNULL(pf.[RowCount], 0))
		                                                                                                FROM dbo.ProcessedFiles pf
		                                                                                                WHERE (pf.SystemID = s.ID) ) END DESC,
       s.SystemID
END
GO

/****** Object:  StoredProcedure [dbo].[SystemsInsert]    Script Date: 2/12/2019 2:49:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SystemsInsert]
(
	@systemID NVARCHAR(50),
	@systemIDDecimal INT,
	@description NVARCHAR(100),
	@wacn NVARCHAR(50),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.Systems
	(
		SystemID,
		SystemIDDecimal,
		[Description],
		WACN,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
		@systemID,
		@systemIDDecimal,
		@description,
		@wacn,
		@firstSeen,
		@lastSeen
	);

	SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[SystemsUpdate]    Script Date: 2/12/2019 2:49:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SystemsUpdate]
(
	@systemID NVARCHAR(50),
	@systemIDDecimal INT,
	@description NVARCHAR(100),
	@wacn NVARCHAR(50),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Systems
	SET SystemIDDecimal = @systemIDDecimal,
		[Description] = @description,
		WACN = @wacn,
		FirstSeen = @firstSeen,
		LastSeen = @lastSeen,
		LastModified = GETDATE()
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupHistoryDelete]    Script Date: 2/12/2019 2:49:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupHistoryDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TalkgroupHistory
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupHistoryDeleteForSystem]    Script Date: 2/12/2019 2:49:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupHistoryDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TalkgroupHistory
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupHistoryGet]    Script Date: 2/12/2019 2:49:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupHistoryGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT th.ID,
		th.SystemID,
		th.TalkgroupID,
		th.[Description],
		th.FirstSeen,
		th.LastSeen,
		th.LastModified
	FROM dbo.TalkgroupHistory th
	WHERE (th.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupHistoryGetForSystem]    Script Date: 2/12/2019 2:49:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupHistoryGetForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tgh.ID,
		tgh.SystemID,
		tg.ID AS TalkgroupIDKey,
		tg.TalkgroupID,
		tgh.[Description],
		tgh.FirstSeen,
		tgh.LastSeen,
		tgh.LastModified
	FROM dbo.TalkgroupHistory tgh WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = tgh.TalkgroupID)
	WHERE (tgh.SystemID = @systemID)
	ORDER BY tgh.TalkgroupID;
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupHistoryGetForTalkgroup]    Script Date: 2/12/2019 2:49:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupHistoryGetForTalkgroup]
(
    @systemID INT,
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tgh.ID,
		tgh.SystemID,
		tgh.TalkgroupID,
		tgh.[Description],
		tgh.FirstSeen,
		tgh.LastSeen,
		tgh.LastModified
	FROM dbo.TalkgroupHistory tgh WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = tgh.TalkgroupID)
	WHERE ((tgh.SystemID = @systemID)
		AND (tg.TalkgroupID = @talkgroupID))
	ORDER BY tgh.LastSeen DESC,
		tgh.FirstSeen DESC;
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupHistoryGetForTalkgroupCount]    Script Date: 2/12/2019 2:49:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TalkgroupHistoryGetForTalkgroupCount]
(
	@systemID INT,
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*)
	FROM dbo.TalkgroupHistory tgh WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = tgh.TalkgroupID)
	WHERE ((tgh.SystemID = @systemID)
		AND (tg.TalkgroupID = @talkgroupID));
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupHistoryGetForTalkgroupFiltersWithPaging]    Script Date: 2/12/2019 2:49:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TalkgroupHistoryGetForTalkgroupFiltersWithPaging]
(
	@systemID NVARCHAR(50),
	@talkgroupID INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @talkgroupIDFrom INT = NULL,
    @talkgroupIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @FIELD_DESCRIPTION NVARCHAR(50) = N'Description',
        @FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
        @FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
        @DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
        @DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH TalkgroupHistoryResults ([Description], FirstSeen, LastSeen)
    AS
    (
	    SELECT tgh.[Description],
		    tgh.FirstSeen,
		    tgh.LastSeen
	    FROM dbo.TalkgroupHistory tgh WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = tgh.SystemID)
	    INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		    ON (tg.ID = tgh.TalkgroupID)
	    WHERE ((s.SystemID = @systemID)
		    AND (tg.TalkgroupID = @talkgroupID)
			AND ((@searchText IS NULL)
				OR (tgh.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (tgh.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (tgh.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (tgh.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenFrom IS NULL)
                OR (tgh.LastSeen <= @lastSeenTo)))
    )
    SELECT tghr.[Description], 
        tghr.FirstSeen, 
        tghr.LastSeen, 
        COUNT(1) OVER() AS RecordCount
    FROM TalkgroupHistoryResults tghr
    ORDER BY
		CASE WHEN ((@sortField = @FIELD_DESCRIPTION) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tghr.[Description] END,
		CASE WHEN ((@sortField = @FIELD_DESCRIPTION) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tghr.[Description] END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tghr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tghr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tghr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tghr.LastSeen END DESC,
        tghr.LastSeen DESC
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupHistoryInsert]    Script Date: 2/12/2019 2:49:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupHistoryInsert]
(
	@systemID INT,
	@talkgroupID INT,
	@description NVARCHAR(100),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @talkgroupIDKey INT = NULL;

	SELECT @talkgroupIDKey = tg.ID
	FROM dbo.Talkgroups tg
	WHERE ((tg.SystemID = @systemID)
		AND (tg.TalkgroupID = @talkgroupID));

	IF (@talkgroupIDKey IS NOT NULL)
	BEGIN

		INSERT dbo.TalkgroupHistory
		(
			SystemID,
			TalkgroupID,
			[Description],
			FirstSeen,
			LastSeen
		)
		VALUES
		(
			@systemID,
			@talkgroupIDKey,
			@description,
			@firstSeen,
			@lastSeen
		);

		SELECT SCOPE_IDENTITY() AS ID;
	END
	ELSE
	BEGIN
		SELECT -1 AS ID;
	END
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupHistoryUpdate]    Script Date: 2/12/2019 2:49:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupHistoryUpdate]
(
	@id INT,
	@systemID INT,
	@talkgroupID INT,
	@description NVARCHAR(100),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.TalkgroupHistory
	SET SystemID = @systemID,
		TalkgroupID = @talkgroupID,
		[Description] = @description,
		FirstSeen = @firstSeen,
		LastSeen = @lastSeen
	WHERE ID = @id;
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosDelete]    Script Date: 2/12/2019 2:49:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupRadiosDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TalkgroupRadios
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosDeleteForSystem]    Script Date: 2/12/2019 2:49:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupRadiosDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TalkgroupRadios
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosGet]    Script Date: 2/12/2019 2:49:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupRadiosGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tgr.ID,
		tgr.SystemID,
		tgr.TalkgroupID,
		tgr.RadioID,
		tgr.[Date],
		tgr.AffiliationCount,
		tgr.DeniedCount,
		tgr.VoiceGrantCount,
		tgr.EmergencyVoiceGrantCount,
		tgr.EncryptedVoiceGrantCount,
		tgr.DataCount,
		tgr.PrivateDataCount,
		tgr.AlertCount,
		tgr.FirstSeen,
		tgr.LastSeen,
		tgr.LastModified
	FROM dbo.TalkgroupRadios tgr
	WHERE (tgr.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosGetForSystem]    Script Date: 2/12/2019 2:49:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupRadiosGetForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tgr.ID,
		tgr.SystemID,
		tgr.TalkgroupID,
		tgr.RadioID,
		tgr.[Date],
		tgr.AffiliationCount,
		tgr.DeniedCount,
		tgr.VoiceGrantCount,
		tgr.EmergencyVoiceGrantCount,
		tgr.EncryptedVoiceGrantCount,
		tgr.DataCount,
		tgr.PrivateDataCount,
		tgr.AlertCount,
		tgr.FirstSeen,
		tgr.LastSeen,
		tgr.LastModified
	FROM dbo.TalkgroupRadios tgr
	WHERE (tgr.SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosGetForSystemDateRange]    Script Date: 2/12/2019 2:49:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupRadiosGetForSystemDateRange]
(
	@systemID INT,
    @date DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tgr.ID,
		tgr.SystemID,
		tgr.TalkgroupID,
		tgr.RadioID,
		tgr.[Date],
		tgr.AffiliationCount,
		tgr.DeniedCount,
		tgr.VoiceGrantCount,
		tgr.EmergencyVoiceGrantCount,
		tgr.EncryptedVoiceGrantCount,
		tgr.DataCount,
		tgr.PrivateDataCount,
		tgr.AlertCount,
		tgr.FirstSeen,
		tgr.LastSeen,
		tgr.LastModified
	FROM dbo.TalkgroupRadios tgr
	WHERE ((tgr.SystemID = @systemID)
        AND (tgr.[Date] >= DATEADD(dd, -1, @date))
        AND (tgr.[Date] <= DATEADD(dd, 1, @date)));
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosGetRadiosForTalkgroup]    Script Date: 2/12/2019 2:49:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupRadiosGetRadiosForTalkgroup]
(
    @systemID INT,
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tg.TalkgroupID,
		tg.[Description] AS TalkgroupDescription,
		r.RadioID,
		r.[Description] AS RadioDescription,
		SUM(tgr.AffiliationCount) AS AffiliationCount,
		SUM(tgr.DeniedCount) AS DeniedCount,
		SUM(tgr.VoiceGrantCount) AS VoiceGrantCount,
		SUM(tgr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		SUM(tgr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		SUM(tgr.DataCount) AS DataCount,
		SUM(tgr.PrivateDataCount) AS PrivateDataCount,
		SUM(tgr.AlertCount) AS AlertCount,
		MIN(tgr.FirstSeen) AS FirstSeen,
		MAX(tgr.LastSeen) AS LastSeen,
        COUNT(1) OVER() AS RecordCount
	FROM dbo.TalkgroupRadios tgr WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = tgr.TalkgroupID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = tgr.RadioID)
	WHERE ((tg.SystemID = @systemID)
        AND (tg.TalkgroupID = @talkgroupID))
	GROUP BY tg.TalkgroupID,
		tg.[Description],
		r.RadioID,
		r.[Description]
	ORDER BY r.RadioID
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosGetRadiosForTalkgroupByDate]    Script Date: 2/12/2019 2:49:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TalkgroupRadiosGetRadiosForTalkgroupByDate]
(
	@systemID INT,
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tg.TalkgroupID,
		tg.[Description] AS TalkgroupDescription,
		r.RadioID,
		r.[Description] AS RadioDescription,
		tgr.[Date],
		SUM(tgr.AffiliationCount) AS AffiliationCount,
		SUM(tgr.DeniedCount) AS DeniedCount,
		SUM(tgr.VoiceGrantCount) AS VoiceGrantCount,
		SUM(tgr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		SUM(tgr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		SUM(tgr.DataCount) AS DataCount,
		SUM(tgr.PrivateDataCount) AS PrivateDataCount,
		SUM(tgr.AlertCount) AS AlertCount,
		MIN(tgr.FirstSeen) AS FirstSeen,
		MAX(tgr.LastSeen) AS LastSeen
	FROM dbo.TalkgroupRadios tgr
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = tgr.TalkgroupID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = tgr.RadioID)
	WHERE ((tgr.SystemID = @systemID)
		AND (tg.TalkgroupID = @talkgroupID))
	GROUP BY tg.TalkgroupID,
		tg.[Description],
		r.RadioID,
		r.[Description],
		tgr.[Date]
	ORDER BY r.RadioID;
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosGetRadiosForTalkgroupCount]    Script Date: 2/12/2019 2:49:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[TalkgroupRadiosGetRadiosForTalkgroupCount]
(
	@systemID INT,
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	WITH TalkgroupRadiosRadios
	AS
	(
		SELECT tgr.TalkgroupID,
			tgr.RadioID
		FROM dbo.TalkgroupRadios tgr WITH (NOLOCK)
		INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
			ON (tg.ID = tgr.TalkgroupID)
		WHERE ((tgr.SystemID = @systemID)
			AND (tg.TalkgroupID = @talkgroupID))
		GROUP BY tgr.TalkgroupID,
			tgr.RadioID
	)
	SELECT COUNT(*)
	FROM TalkgroupRadiosRadios;

END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosGetRadiosForTalkgroupFiltersWithPaging]    Script Date: 2/12/2019 2:49:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TalkgroupRadiosGetRadiosForTalkgroupFiltersWithPaging]
(
	@systemID NVARCHAR(50),
	@talkgroupID INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @radioIDFrom INT = NULL,
    @radioIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_TALKGROUP_ID NVARCHAR(50) = N'TalkgroupID',
		@FIELD_TALKGROUP_NAME NVARCHAR(50) = N'TalkgroupName',
        @FIELD_RADIO_ID NVARCHAR(50) = N'RadioID',
        @FIELD_RADIO_NAME NVARCHAR(50) = N'RadioName',
		@FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PRIVATE_DATA_COUNT NVARCHAR(50) = N'PrivateDataCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH TalkgroupRadioResults (TalkgroupID, TalkgroupDescription, RadioID, RadioDescription, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, PrivateDataCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
	    SELECT tg.TalkgroupID,
		    tg.[Description] AS TalkgroupDescription,
		    r.RadioID,
		    r.[Description] AS RadioDescription,
		    SUM(tgr.AffiliationCount) AS AffiliationCount,
		    SUM(tgr.DeniedCount) AS DeniedCount,
		    SUM(tgr.VoiceGrantCount) AS VoiceGrantCount,
		    SUM(tgr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		    SUM(tgr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		    SUM(tgr.DataCount) AS DataCount,
		    SUM(tgr.PrivateDataCount) AS PrivateDataCount,
		    SUM(tgr.AlertCount) AS AlertCount,
		    MIN(tgr.FirstSeen) AS FirstSeen,
		    MAX(tgr.LastSeen) AS LastSeen
	    FROM dbo.TalkgroupRadios tgr WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = tgr.SystemID)
	    INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		    ON (tg.ID = tgr.TalkgroupID)
	    INNER JOIN dbo.Radios r WITH (NOLOCK)
		    ON (r.ID = tgr.RadioID)
	    WHERE ((s.SystemID = @systemID)
		    AND (tg.TalkgroupID = @talkgroupID)
            AND ((@dateFrom IS NULL)
                OR (tgr.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (tgr.[Date] <= @dateTo))
            AND ((@radioIDFrom IS NULL)
                OR (r.RadioID >= @radioIDFrom))
            AND ((@radioIDTo IS NULL)
                OR (r.RadioID <= @radioIDTo))
			AND ((@searchText IS NULL)
				OR (r.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (r.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (r.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (r.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (r.LastSeen <= @lastSeenTo)))
	    GROUP BY tg.TalkgroupID,
		    tg.[Description],
		    r.RadioID,
		    r.[Description]
    )
    SELECT tgrr.TalkgroupID, 
        tgrr.TalkgroupDescription, 
        tgrr.RadioID, 
        tgrr.RadioDescription, 
        tgrr.AffiliationCount, 
        tgrr.DeniedCount, 
        tgrr.VoiceGrantCount, 
        tgrr.EmergencyVoiceGrantCount, 
        tgrr.EncryptedVoiceGrantCount,
        tgrr.DataCount, 
        tgrr.PrivateDataCount, 
        tgrr.AlertCount, 
        tgrr.FirstSeen, 
        tgrr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TalkgroupRadioResults tgrr
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.TalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.TalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.TalkgroupDescription END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.TalkgroupDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.RadioID END,
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.RadioID END DESC,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.RadioDescription END,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.RadioDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.DataCount END DESC,
		CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.PrivateDataCount END,
		CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.PrivateDataCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.AlertCount END,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.AlertCount END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.LastSeen END DESC,
        tgrr.RadioID
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);

END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosGetRadiosForTalkgroupWithDates]    Script Date: 2/12/2019 2:49:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TalkgroupRadiosGetRadiosForTalkgroupWithDates]
(
	@systemID INT,
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tg.TalkgroupID,
		tg.[Description] AS TalkgroupDescription,
		r.RadioID,
		r.[Description] AS RadioDescription,
		tgr.[Date],
		SUM(tgr.AffiliationCount) AS AffiliationCount,
		SUM(tgr.DeniedCount) AS DeniedCount,
		SUM(tgr.VoiceGrantCount) AS VoiceGrantCount,
		SUM(tgr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		SUM(tgr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		SUM(tgr.DataCount) AS DataCount,
		SUM(tgr.PrivateDataCount) AS PrivateDataCount,
		SUM(tgr.AlertCount) AS AlertCount,
		MIN(tgr.FirstSeen) AS FirstSeen,
		MAX(tgr.LastSeen) AS LastSeen
	FROM dbo.TalkgroupRadios tgr WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = tgr.TalkgroupID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = tgr.RadioID)
	WHERE ((tgr.SystemID = @systemID)
		AND (tg.TalkgroupID = @talkgroupID))
	GROUP BY tg.TalkgroupID,
		tg.[Description],
		r.RadioID,
		r.[Description],
		tgr.[Date]
	ORDER BY r.RadioID,
		tgr.[Date] DESC;
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosGetRadiosForTalkgroupWithPaging]    Script Date: 2/12/2019 2:49:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TalkgroupRadiosGetRadiosForTalkgroupWithPaging]
(
	@systemID INT,
	@talkgroupID INT,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_TALKGROUP_ID NVARCHAR(50) = N'TalkgroupID',
		@FIELD_TALKGROUP_NAME NVARCHAR(50) = N'TalkgroupName',
        @FIELD_RADIO_ID NVARCHAR(50) = N'RadioID',
        @FIELD_RADIO_NAME NVARCHAR(50) = N'RadioName',
		@FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PRIVATE_DATA_COUNT NVARCHAR(50) = N'PrivateDataCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH TalkgroupRadioResults (TalkgroupID, TalkgroupDescription, RadioID, RadioDescription, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, PrivateDataCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
	    SELECT tg.TalkgroupID,
		    tg.[Description] AS TalkgroupDescription,
		    r.RadioID,
		    r.[Description] AS RadioDescription,
		    SUM(tgr.AffiliationCount) AS AffiliationCount,
		    SUM(tgr.DeniedCount) AS DeniedCount,
		    SUM(tgr.VoiceGrantCount) AS VoiceGrantCount,
		    SUM(tgr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		    SUM(tgr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		    SUM(tgr.DataCount) AS DataCount,
		    SUM(tgr.PrivateDataCount) AS PrivateDataCount,
		    SUM(tgr.AlertCount) AS AlertCount,
		    MIN(tgr.FirstSeen) AS FirstSeen,
		    MAX(tgr.LastSeen) AS LastSeen
	    FROM dbo.TalkgroupRadios tgr WITH (NOLOCK)
	    INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		    ON (tg.ID = tgr.TalkgroupID)
	    INNER JOIN dbo.Radios r WITH (NOLOCK)
		    ON (r.ID = tgr.RadioID)
	    WHERE ((tgr.SystemID = @systemID)
		    AND (tg.TalkgroupID = @talkgroupID))
	    GROUP BY tg.TalkgroupID,
		    tg.[Description],
		    r.RadioID,
		    r.[Description]
    )
    SELECT tgrr.TalkgroupID, 
        tgrr.TalkgroupDescription, 
        tgrr.RadioID, 
        tgrr.RadioDescription, 
        tgrr.AffiliationCount, 
        tgrr.DeniedCount, 
        tgrr.VoiceGrantCount, 
        tgrr.EmergencyVoiceGrantCount, 
        tgrr.EncryptedVoiceGrantCount,
        tgrr.DataCount, 
        tgrr.PrivateDataCount, 
        tgrr.AlertCount, 
        tgrr.FirstSeen, 
        tgrr.LastSeen
    FROM TalkgroupRadioResults tgrr
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.TalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.TalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.TalkgroupDescription END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.TalkgroupDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.RadioID END,
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.RadioID END DESC,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.RadioDescription END,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.RadioDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.DataCount END DESC,
		CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.PrivateDataCount END,
		CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.PrivateDataCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.AlertCount END,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.AlertCount END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.LastSeen END DESC,
        tgrr.RadioID
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);

END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosGetTalkgroupsForRadio]    Script Date: 2/12/2019 2:49:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupRadiosGetTalkgroupsForRadio]
(
	@systemID INT,
	@radioID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tg.TalkgroupID,
		tg.[Description] AS TalkgroupDescription,
		r.RadioID,
		r.[Description] AS RadioDescription,
		SUM(tgr.AffiliationCount) AS AffiliationCount,
		SUM(tgr.DeniedCount) AS DeniedCount,
		SUM(tgr.VoiceGrantCount) AS VoiceGrantCount,
		SUM(tgr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		SUM(tgr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		SUM(tgr.DataCount) AS DataCount,
		SUM(tgr.PrivateDataCount) AS PrivateDataCount,
		SUM(tgr.AlertCount) AS AlertCount,
		MIN(tgr.FirstSeen) AS FirstSeen,
		MAX(tgr.LastSeen) AS LastSeen,
        COUNT(1) OVER() AS RecordCount
	FROM dbo.TalkgroupRadios tgr WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = tgr.TalkgroupID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = tgr.RadioID)
	WHERE ((tgr.SystemID = @systemID)
		AND (r.RadioID = @radioID))
	GROUP BY tg.TalkgroupID,
		tg.[Description],
		r.RadioID,
		r.[Description]
	ORDER BY r.RadioID;
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosGetTalkgroupsForRadioByDate]    Script Date: 2/12/2019 2:49:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TalkgroupRadiosGetTalkgroupsForRadioByDate]
(
	@systemID INT,
	@radioID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tg.TalkgroupID,
		tg.[Description] AS TalkgroupDescription,
		r.RadioID,
		r.[Description] AS RadioDescription,
		tgr.[Date],
		SUM(tgr.AffiliationCount) AS AffiliationCount,
		SUM(tgr.DeniedCount) AS DeniedCount,
		SUM(tgr.VoiceGrantCount) AS VoiceGrantCount,
		SUM(tgr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		SUM(tgr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		SUM(tgr.DataCount) AS DataCount,
		SUM(tgr.PrivateDataCount) AS PrivateDataCount,
		SUM(tgr.AlertCount) AS AlertCount,
		MIN(tgr.FirstSeen) AS FirstSeen,
		MAX(tgr.LastSeen) AS LastSeen
	FROM dbo.TalkgroupRadios tgr WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = tgr.TalkgroupID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = tgr.RadioID)
	WHERE ((tgr.SystemID = @systemID)
		AND (r.RadioID = @radioID))
	GROUP BY tg.TalkgroupID,
		tg.[Description],
		r.RadioID,
		r.[Description],
		tgr.[Date]
	ORDER BY r.RadioID;
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosGetTalkgroupsForRadioCount]    Script Date: 2/12/2019 2:49:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupRadiosGetTalkgroupsForRadioCount]
(
	@systemID INT,
	@radioID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	WITH TalkgroupRadiosTalkgroups
	AS
	(
		SELECT tgr.TalkgroupID,
			tgr.RadioID
		FROM dbo.TalkgroupRadios tgr WITH (NOLOCK)
		INNER JOIN dbo.Radios r WITH (NOLOCK)
			ON (r.ID = tgr.RadioID)
		WHERE ((tgr.SystemID = @systemID)
			AND (r.RadioID = @radioID))
		GROUP BY tgr.TalkgroupID,
			tgr.RadioID
	)
	SELECT COUNT(*)
	FROM TalkgroupRadiosTalkgroups;
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosGetTalkgroupsForRadioFiltersWithPaging]    Script Date: 2/12/2019 2:49:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TalkgroupRadiosGetTalkgroupsForRadioFiltersWithPaging]
(
	@systemID NVARCHAR(50),
	@radioID INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @talkgroupIDFrom INT = NULL,
    @talkgroupIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_TALKGROUP_ID NVARCHAR(50) = N'TalkgroupID',
		@FIELD_TALKGROUP_NAME NVARCHAR(50) = N'TalkgroupName',
        @FIELD_RADIO_ID NVARCHAR(50) = N'RadioID',
        @FIELD_RADIO_NAME NVARCHAR(50) = N'RadioName',
		@FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PRIVATE_DATA_COUNT NVARCHAR(50) = N'PrivateDataCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @systemIDKey INT;

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    WITH TalkgroupRadioResults (TalkgroupID, TalkgroupDescription, RadioID, RadioDescription, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, PrivateDataCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
	    SELECT tg.TalkgroupID,
		    tg.[Description] AS TalkgroupDescription,
		    r.RadioID,
		    r.[Description] AS RadioDescription,
		    SUM(tgr.AffiliationCount) AS AffiliationCount,
		    SUM(tgr.DeniedCount) AS DeniedCount,
		    SUM(tgr.VoiceGrantCount) AS VoiceGrantCount,
		    SUM(tgr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		    SUM(tgr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		    SUM(tgr.DataCount) AS DataCount,
		    SUM(tgr.PrivateDataCount) AS PrivateDataCount,
		    SUM(tgr.AlertCount) AS AlertCount,
		    MIN(tgr.FirstSeen) AS FirstSeen,
		    MAX(tgr.LastSeen) AS LastSeen
	    FROM dbo.TalkgroupRadios tgr WITH (NOLOCK)
	    INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		    ON (tg.ID = tgr.TalkgroupID)
	    INNER JOIN dbo.Radios r WITH (NOLOCK)
		    ON (r.ID = tgr.RadioID)
	    WHERE ((r.SystemID = @systemIDKey)
		    AND (r.RadioID = @radioID)
            AND ((@dateFrom IS NULL)
                OR (tgr.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (tgr.[Date] <= @dateTo))
            AND ((@talkgroupIDFrom IS NULL)
                OR (tg.TalkgroupID >= @talkgroupIDFrom))
            AND ((@talkgroupIDTo IS NULL)
                OR (tg.TalkgroupID <= @talkgroupIDTo))
			AND ((@searchText IS NULL)
				OR (tg.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (tgr.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (tgr.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (tgr.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (tgr.LastSeen <= @lastSeenTo)))
	    GROUP BY tg.TalkgroupID,
		    tg.[Description],
		    r.RadioID,
		    r.[Description]
    )
    SELECT tgrr.TalkgroupID, 
        tgrr.TalkgroupDescription, 
        tgrr.RadioID, 
        tgrr.RadioDescription, 
        tgrr.AffiliationCount, 
        tgrr.DeniedCount, 
        tgrr.VoiceGrantCount, 
        tgrr.EmergencyVoiceGrantCount, 
        tgrr.EncryptedVoiceGrantCount,
        tgrr.DataCount, 
        tgrr.PrivateDataCount, 
        tgrr.AlertCount, 
        tgrr.FirstSeen, 
        tgrr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TalkgroupRadioResults tgrr
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.TalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.TalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.TalkgroupDescription END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.TalkgroupDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.RadioID END,
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.RadioID END DESC,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.RadioDescription END,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.RadioDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.DataCount END DESC,
		CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.PrivateDataCount END,
		CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.PrivateDataCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.AlertCount END,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.AlertCount END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgrr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgrr.LastSeen END DESC,
        tgrr.RadioID
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosInsert]    Script Date: 2/12/2019 2:49:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupRadiosInsert]
(
	@systemID INT,
	@talkgroupID INT,
	@radioID INT,
	@date DATETIME2(7),
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TalkgroupRadios
	(
		SystemID,
		TalkgroupID,
		RadioID,
		[Date],
		AffiliationCount,
		DeniedCount,
		VoiceGrantCount,
		EmergencyVoiceGrantCount,
		EncryptedVoiceGrantCount,
		DataCount,
		PrivateDataCount,
		AlertCount,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
		@systemID,
		@talkgroupID,
		@radioID,
		@date,
		@affiliationCount,
		@deniedCount,
		@voiceGrantCount,
		@emergencyVoiceGrantCount,
		@encryptedVoiceGrantCount,
		@dataCount,
		@privateDataCount,
		@alertCount,
		@firstSeen,
		@lastSeen
	);

	SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupRadiosUpdate]    Script Date: 2/12/2019 2:49:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupRadiosUpdate]
(
	@id INT,
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.TalkgroupRadios
	SET AffiliationCount = @affiliationCount,
		DeniedCount = @deniedCount,
		VoiceGrantCount = @voiceGrantCount,
		EmergencyVoiceGrantCount = @emergencyVoiceGrantCount,
		EncryptedVoiceGrantCount = @encryptedVoiceGrantCount,
		DataCount = @dataCount,
		PrivateDataCount = @privateDataCount,
		AlertCount = @alertCount,
		FirstSeen = @firstSeen,
		LastSeen = @lastSeen,
		LastModified = GETDATE()
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsDelete]    Script Date: 2/12/2019 2:49:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupsDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.Talkgroups
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsDeleteForSystem]    Script Date: 2/12/2019 2:49:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupsDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.Talkgroups
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsGet]    Script Date: 2/12/2019 2:49:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupsGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tg.ID,
		tg.SystemID,
		tg.TalkgroupID,
		tg.[Priority],
		tg.[Description],
		tg.LastSeen,
		tg.LastSeenProgram,
		tg.LastSeenProgramUnix,
		tg.FirstSeen,
		tg.FirstSeenProgram,
		tg.FirstSeenProgramUnix,
		tg.FGColor,
		tg.BGColor,
		tg.EncryptionSeen,
		tg.IgnoreEmergencySignal,
		tg.HitCount,
		tg.HitCountProgram,
        tg.PhaseIISeen,
		tg.LastModified
	FROM dbo.Talkgroups tg
	WHERE (tg.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsGetCountForSystem]    Script Date: 2/12/2019 2:49:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupsGetCountForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*)
	FROM dbo.Talkgroups tg
	WHERE (tg.SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsGetDetail]    Script Date: 2/12/2019 2:49:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[TalkgroupsGetDetail]
(
	@systemID INT,
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

    WITH FromTalkgroups (TalkgroupID, PatchCount)
    AS
    (
        SELECT tg.ID,
            SUM(ISNULL(pftg.HitCount, 0)) AS FromTalkgroupCount
        FROM dbo.Talkgroups tg
        LEFT JOIN dbo.Patches pftg
            ON (pftg.FromTalkgroupID = tg.ID)
	    WHERE ((tg.SystemID = @systemID)
		    AND (tg.TalkgroupID = @talkgroupID))
        GROUP BY tg.ID
    ),
    ToTalkgroups (TalkgroupID, PatchCount)
    AS
    (
        SELECT tg.ID,
            SUM(ISNULL(pttg.HitCount, 0)) AS ToTalkgroupCount
        FROM dbo.Talkgroups tg
        LEFT JOIN dbo.Patches pttg
            ON (pttg.ToTalkgroupID = tg.ID)
	    WHERE ((tg.SystemID = @systemID)
		    AND (tg.TalkgroupID = @talkgroupID))
        GROUP BY tg.ID
    )
	SELECT tg.ID,
        tg.TalkgroupID,
		tg.[Description],
		tg.FirstSeen,
		tg.LastSeen,
		tg.EncryptionSeen,
        tg.PhaseIISeen,
		SUM(ISNULL(ttg.AffiliationCount, 0)) AS AffiliationCount,
		SUM(ISNULL(ttg.DeniedCount, 0)) AS DeniedCount,
		SUM(ISNULL(ttg.VoiceGrantCount, 0) +
			ISNULL(ttg.EmergencyVoiceGrantCount, 0) +
			ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS VoiceGrantCount,
		SUM(ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		SUM(ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
        (pftg.PatchCount + pttg.PatchCount) AS PatchCount,
        COUNT(1) AS RecordCount
	FROM Talkgroups tg WITH (NOLOCK)
    INNER JOIN FromTalkgroups pftg WITH (NOLOCK)
        ON (pftg.TalkgroupID = tg.ID)
    INNER JOIN ToTalkgroups pttg WITH (NOLOCK)
        ON (pttg.TalkgroupID = tg.ID)
	LEFT JOIN TowerTalkgroups ttg WITH (NOLOCK)
		ON (ttg.TalkgroupID = tg.ID)
	WHERE ((tg.SystemID = @systemID)
		AND (tg.TalkgroupID = @talkgroupID))
	GROUP BY tg.ID,
        tg.TalkgroupID,
		tg.[Description],
		tg.FirstSeen,
		tg.LastSeen,
		tg.EncryptionSeen,
        tg.PhaseIISeen,
        pftg.PatchCount,
        pttg.PatchCount;
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsGetDetailFilters]    Script Date: 2/12/2019 2:49:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[TalkgroupsGetDetailFilters]
(
	@systemID INT,
	@talkgroupID INT,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tg.ID,
        tg.TalkgroupID,
		tg.[Description],
		tg.FirstSeen,
		tg.LastSeen,
		tg.EncryptionSeen,
        tg.PhaseIISeen,
		SUM(ISNULL(ttg.AffiliationCount, 0)) AS AffiliationCount,
		SUM(ISNULL(ttg.DeniedCount, 0)) AS DeniedCount,
		SUM(ISNULL(ttg.VoiceGrantCount, 0) +
			ISNULL(ttg.EmergencyVoiceGrantCount, 0) +
			ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS VoiceGrantCount,
		SUM(ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		SUM(ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
        COUNT(1) AS RecordCount
	FROM Talkgroups tg WITH (NOLOCK)
	INNER JOIN TowerTalkgroups ttg WITH (NOLOCK)
		ON ((ttg.TalkgroupID = tg.ID)
            AND ((@dateFrom IS NULL)
                OR (ttg.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (ttg.[Date] <= @dateTo)))
	WHERE ((tg.SystemID = @systemID)
		AND (tg.TalkgroupID = @talkgroupID))
	GROUP BY tg.ID,
        tg.TalkgroupID,
		tg.[Description],
		tg.FirstSeen,
		tg.LastSeen,
		tg.EncryptionSeen,
        tg.PhaseIISeen
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsGetDetailForSystem]    Script Date: 2/12/2019 2:49:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[TalkgroupsGetDetailForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

    WITH FromTalkgroups (TalkgroupID, PatchCount)
    AS
    (
        SELECT tg.ID,
            SUM(ISNULL(pftg.HitCount, 0)) AS FromTalkgroupCount
        FROM dbo.Talkgroups tg
        LEFT JOIN dbo.Patches pftg
            ON (pftg.FromTalkgroupID = tg.ID)
        WHERE (tg.SystemID = @systemID)
        GROUP BY tg.ID
    ),
    ToTalkgroups (TalkgroupID, PatchCount)
    AS
    (
        SELECT tg.ID,
            SUM(ISNULL(pttg.HitCount, 0)) AS ToTalkgroupCount
        FROM dbo.Talkgroups tg
        LEFT JOIN dbo.Patches pttg
            ON (pttg.ToTalkgroupID = tg.ID)
        WHERE (tg.SystemID = @systemID)
        GROUP BY tg.ID
    ),
	TalkgroupTotals (ID, TalkgroupID, [Description], FirstSeen, LastSeen, EncryptionSeen, PhaseIISeen, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, PatchCount)
	AS
	(
		SELECT tg.ID,
			tg.TalkgroupID,
			tg.[Description],
			tg.FirstSeen,
			tg.LastSeen,
			tg.EncryptionSeen,
            tg.PhaseIISeen,
			SUM(ISNULL(ttg.AffiliationCount, 0)) AS AffiliationCount,
			SUM(ISNULL(ttg.DeniedCount, 0)) AS DeniedCount,
			SUM(ISNULL(ttg.VoiceGrantCount, 0) +
				ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS VoiceGrantCount,
			SUM(ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
			SUM(ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
            (pftg.PatchCount + pttg.PatchCount) AS PatchCount
	    FROM Talkgroups tg WITH (NOLOCK)
        INNER JOIN FromTalkgroups pftg WITH (NOLOCK)
            ON (pftg.TalkgroupID = tg.ID)
        INNER JOIN ToTalkgroups pttg WITH (NOLOCK)
            ON (pttg.TalkgroupID = tg.ID)
		LEFT JOIN dbo.TowerTalkgroups ttg WITH (NOLOCK)
			ON (ttg.TalkgroupID = tg.ID)
	    WHERE (tg.SystemID = @systemID)
	    GROUP BY tg.ID,
            tg.TalkgroupID,
		    tg.[Description],
		    tg.FirstSeen,
		    tg.LastSeen,
		    tg.EncryptionSeen,
            tg.PhaseIISeen,
            pftg.PatchCount,
            pttg.PatchCount
    )
	SELECT tgt.ID,
		tgt.TalkgroupID,
		tgt.[Description],
		tgt.FirstSeen,
		tgt.LastSeen,
		tgt.EncryptionSeen,
        tgt.PhaseIISeen,
		tgt.AffiliationCount,
		tgt.DeniedCount,
		tgt.VoiceGrantCount,
		tgt.EmergencyVoiceGrantCount,
		tgt.EncryptedVoiceGrantCount,
        tgt.PatchCount,
        COUNT(1) OVER() AS RecordCount
	FROM TalkgroupTotals tgt
    ORDER BY tgt.TalkgroupID;
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsGetDetailForSystemActiveFiltersWithPaging]    Script Date: 2/12/2019 2:49:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TalkgroupsGetDetailForSystemActiveFiltersWithPaging]
(
	@systemID NVARCHAR(50),
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @talkgroupIDFrom INT = NULL,
    @talkgroupIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
	@sortField NVARCHAR(50),
	@sortDirection NVARCHAR(50),
	@pageNumber INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @systemIDKey INT,
        @FIELD_TALKGROUP_ID NVARCHAR(50) = N'TalkgroupID',
		@FIELD_TALKGROUP_NAME NVARCHAR(50) = N'TalkgroupName',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@FIELD_ENCRYPTION_SEEN NVARCHAR(50) = N'EncryptionSeen',
        @FIELD_PHASE_II_SEEN NVARCHAR(50) = N'PhaseIISeen',
		@FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_PATCH_COUNT NVARCHAR(50) = N'PatchCount',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    WITH FromTalkgroups (TalkgroupID, PatchCount)
    AS
    (
        SELECT tg.ID,
            SUM(ISNULL(pftg.HitCount, 0)) AS FromTalkgroupCount
        FROM dbo.Talkgroups tg
        LEFT JOIN dbo.Patches pftg
            ON (pftg.FromTalkgroupID = tg.ID)
        WHERE (tg.SystemID = @systemIDKey)
        GROUP BY tg.ID
    ),
    ToTalkgroups (TalkgroupID, PatchCount)
    AS
    (
        SELECT tg.ID,
            SUM(ISNULL(pttg.HitCount, 0)) AS ToTalkgroupCount
        FROM dbo.Talkgroups tg
        LEFT JOIN dbo.Patches pttg
            ON (pttg.ToTalkgroupID = tg.ID)
        WHERE (tg.SystemID = @systemIDKey)
        GROUP BY tg.ID
    ),
	TalkgroupTotals (ID, TalkgroupID, [Description], FirstSeen, LastSeen, EncryptionSeen, PhaseIISeen, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, PatchCount)
	AS
	(
		SELECT tg.ID,
			tg.TalkgroupID,
			tg.[Description],
			tg.FirstSeen,
			tg.LastSeen,
			tg.EncryptionSeen,
            tg.PhaseIISeen,
			SUM(ISNULL(ttg.AffiliationCount, 0)) AS AffiliationCount,
			SUM(ISNULL(ttg.DeniedCount, 0)) AS DeniedCount,
			SUM(ISNULL(ttg.VoiceGrantCount, 0) +
				ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS VoiceGrantCount,
			SUM(ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
			SUM(ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
            (pftg.PatchCount + pttg.PatchCount) AS PatchCount
	    FROM Talkgroups tg WITH (NOLOCK)
        INNER JOIN FromTalkgroups pftg WITH (NOLOCK)
            ON (pftg.TalkgroupID = tg.ID)
        INNER JOIN ToTalkgroups pttg WITH (NOLOCK)
            ON (pttg.TalkgroupID = tg.ID)
		INNER JOIN dbo.TowerTalkgroups ttg WITH (NOLOCK)
			ON ((ttg.TalkgroupID = tg.ID)
                AND ((@dateFrom IS NULL)
                    OR (ttg.[Date] >= @dateFrom))
                AND ((@dateTo IS NULL)
                    OR (ttg.[Date] <= @dateTo)))
		WHERE (((@talkgroupIDFrom IS NULL)
                OR (tg.TalkgroupID >= @talkgroupIDFrom))
            AND ((@talkgroupIDTo IS NULL)
                OR (tg.TalkgroupID <= @talkgroupIDTo))
			AND ((@searchText IS NULL)
				OR (tg.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (tg.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (tg.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (tg.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenFrom IS NULL)
                OR (tg.LastSeen <= @lastSeenTo)))
		GROUP BY tg.ID,
			tg.TalkgroupID,
			tg.[Description],
			tg.FirstSeen,
			tg.LastSeen,
			tg.EncryptionSeen,
            tg.PhaseIISeen,
            pftg.PatchCount,
            pttg.PatchCount
	)
	SELECT tgt.ID,
		tgt.TalkgroupID,
		tgt.[Description],
		tgt.FirstSeen,
		tgt.LastSeen,
		tgt.EncryptionSeen,
        tgt.PhaseIISeen,
		tgt.AffiliationCount,
		tgt.DeniedCount,
		tgt.VoiceGrantCount,
		tgt.EmergencyVoiceGrantCount,
		tgt.EncryptedVoiceGrantCount,
        tgt.PatchCount,
        COUNT(1) OVER() AS RecordCount
	FROM TalkgroupTotals tgt
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.TalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.TalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.[Description] END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.[Description] END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_PATCH_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.PatchCount END,
		CASE WHEN ((@sortField = @FIELD_PATCH_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.PatchCount END DESC,
        CASE WHEN ((@sortField = @FIELD_ENCRYPTION_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.EncryptionSeen END,
        CASE WHEN ((@sortField = @FIELD_ENCRYPTION_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.EncryptionSeen END DESC,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.PhaseIISeen END,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.PhaseIISeen END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.LastSeen END DESC,
		tgt.TalkgroupID
	OFFSET ((@pageNumber - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsGetDetailForSystemFiltersWithPaging]    Script Date: 2/12/2019 2:49:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TalkgroupsGetDetailForSystemFiltersWithPaging]
(
	@systemID NVARCHAR(50),
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @talkgroupIDFrom INT = NULL,
    @talkgroupIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
	@sortField NVARCHAR(50),
	@sortDirection NVARCHAR(50),
	@pageNumber INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @systemIDKey INT,
        @FIELD_TALKGROUP_ID NVARCHAR(50) = N'TalkgroupID',
		@FIELD_TALKGROUP_NAME NVARCHAR(50) = N'TalkgroupName',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@FIELD_ENCRYPTION_SEEN NVARCHAR(50) = N'EncryptionSeen',
        @FIELD_PHASE_II_SEEN NVARCHAR(50) = N'PhaseIISeen',
		@FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_PATCH_COUNT NVARCHAR(50) = N'PatchCount',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    WITH FromTalkgroups (TalkgroupID, PatchCount)
    AS
    (
        SELECT tg.ID,
            SUM(ISNULL(pftg.HitCount, 0)) AS FromTalkgroupCount
        FROM dbo.Talkgroups tg
        LEFT JOIN dbo.Patches pftg
            ON (pftg.FromTalkgroupID = tg.ID)
        WHERE (tg.SystemID = @systemIDKey)
        GROUP BY tg.ID
    ),
    ToTalkgroups (TalkgroupID, PatchCount)
    AS
    (
        SELECT tg.ID,
            SUM(ISNULL(pttg.HitCount, 0)) AS ToTalkgroupCount
        FROM dbo.Talkgroups tg
        LEFT JOIN dbo.Patches pttg
            ON (pttg.ToTalkgroupID = tg.ID)
        WHERE (tg.SystemID = @systemIDKey)
        GROUP BY tg.ID
    ),
	TalkgroupTotals (ID, TalkgroupID, [Description], FirstSeen, LastSeen, EncryptionSeen, PhaseIISeen, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, PatchCount)
	AS
	(
		SELECT tg.ID,
			tg.TalkgroupID,
			tg.[Description],
			tg.FirstSeen,
			tg.LastSeen,
			tg.EncryptionSeen,
            tg.PhaseIISeen,
			SUM(ISNULL(ttg.AffiliationCount, 0)) AS AffiliationCount,
			SUM(ISNULL(ttg.DeniedCount, 0)) AS DeniedCount,
			SUM(ISNULL(ttg.VoiceGrantCount, 0) +
				ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS VoiceGrantCount,
			SUM(ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
			SUM(ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
            (pftg.PatchCount + pttg.PatchCount) AS PatchCount
	    FROM Talkgroups tg WITH (NOLOCK)
        INNER JOIN FromTalkgroups pftg WITH (NOLOCK)
            ON (pftg.TalkgroupID = tg.ID)
        INNER JOIN ToTalkgroups pttg WITH (NOLOCK)
            ON (pttg.TalkgroupID = tg.ID)
		LEFT JOIN dbo.TowerTalkgroups ttg WITH (NOLOCK)
			ON ((ttg.TalkgroupID = tg.ID)
                AND ((@dateFrom IS NULL)
                    OR (ttg.[Date] >= @dateFrom))
                AND ((@dateTo IS NULL)
                    OR (ttg.[Date] <= @dateTo)))
		WHERE (((@talkgroupIDFrom IS NULL)
                OR (tg.TalkgroupID >= @talkgroupIDFrom))
            AND ((@talkgroupIDTo IS NULL)
                OR (tg.TalkgroupID <= @talkgroupIDTo))
			AND ((@searchText IS NULL)
				OR (tg.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (tg.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (tg.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (tg.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenFrom IS NULL)
                OR (tg.LastSeen <= @lastSeenTo)))
		GROUP BY tg.ID,
			tg.TalkgroupID,
			tg.[Description],
			tg.FirstSeen,
			tg.LastSeen,
			tg.EncryptionSeen,
            tg.PhaseIISeen,
            pftg.PatchCount,
            pttg.PatchCount
	)
	SELECT tgt.ID,
		tgt.TalkgroupID,
		tgt.[Description],
		tgt.FirstSeen,
		tgt.LastSeen,
		tgt.EncryptionSeen,
        tgt.PhaseIISeen,
		tgt.AffiliationCount,
		tgt.DeniedCount,
		tgt.VoiceGrantCount,
		tgt.EmergencyVoiceGrantCount,
		tgt.EncryptedVoiceGrantCount,
        tgt.PatchCount,
        COUNT(1) OVER() AS RecordCount
	FROM TalkgroupTotals tgt
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.TalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.TalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.[Description] END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.[Description] END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_PATCH_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.PatchCount END,
		CASE WHEN ((@sortField = @FIELD_PATCH_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.PatchCount END DESC,
        CASE WHEN ((@sortField = @FIELD_ENCRYPTION_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.EncryptionSeen END,
        CASE WHEN ((@sortField = @FIELD_ENCRYPTION_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.EncryptionSeen END DESC,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.PhaseIISeen END,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.PhaseIISeen END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.LastSeen END DESC,
		tgt.TalkgroupID
	OFFSET ((@pageNumber - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsGetDetailForSystemUnknownFiltersWithPaging]    Script Date: 2/12/2019 2:49:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TalkgroupsGetDetailForSystemUnknownFiltersWithPaging]
(
	@systemID NVARCHAR(50),
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @talkgroupIDFrom INT = NULL,
    @talkgroupIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
	@sortField NVARCHAR(50),
	@sortDirection NVARCHAR(50),
	@pageNumber INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_TALKGROUP_ID NVARCHAR(50) = N'TalkgroupID',
		@FIELD_TALKGROUP_NAME NVARCHAR(50) = N'TalkgroupName',
        @FIELD_TOWERS_NAME NVARCHAR(50) = N'Towers',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@FIELD_ENCRYPTION_SEEN NVARCHAR(50) = N'EncryptionSeen',
        @FIELD_PHASE_II_SEEN NVARCHAR(50) = N'PhaseIISeen',
		@FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_PATCH_COUNT NVARCHAR(50) = N'PatchCount',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @systemIDKey INT;

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    WITH FromTalkgroups (TalkgroupID, PatchCount)
    AS
    (
        SELECT tg.ID,
            SUM(ISNULL(pftg.HitCount, 0)) AS FromTalkgroupCount
        FROM dbo.Talkgroups tg
        LEFT JOIN dbo.Patches pftg
            ON (pftg.FromTalkgroupID = tg.ID)
        WHERE (tg.SystemID = @systemIDKey)
        GROUP BY tg.ID
    ),
    ToTalkgroups (TalkgroupID, PatchCount)
    AS
    (
        SELECT tg.ID,
            SUM(ISNULL(pttg.HitCount, 0)) AS ToTalkgroupCount
        FROM dbo.Talkgroups tg
        LEFT JOIN dbo.Patches pttg
            ON (pttg.ToTalkgroupID = tg.ID)
        WHERE (tg.SystemID = @systemIDKey)
        GROUP BY tg.ID
    ),
	TalkgroupTotals (ID, TalkgroupID, [Description], FirstSeen, LastSeen, EncryptionSeen, PhaseIISeen, Towers, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, PatchCount)
	AS
	(
		SELECT tg.ID,
			tg.TalkgroupID,
			tg.[Description],
			tg.FirstSeen,
			tg.LastSeen,
			tg.EncryptionSeen,
            tg.PhaseIISeen,
            SUBSTRING(( SELECT DISTINCT ', ' + CAST(t.TowerNumber AS NVARCHAR)
                   FROM dbo.TowerTalkgroups ttg 
                   INNER JOIN dbo.Towers t
                       ON (t.ID = ttg.TowerID)
                   WHERE (ttg.TalkgroupID = tg.ID)
                   FOR XML PATH('') ), 2, 2000000) AS Towers,
			SUM(ISNULL(ttg.AffiliationCount, 0)) AS AffiliationCount,
			SUM(ISNULL(ttg.DeniedCount, 0)) AS DeniedCount,
			SUM(ISNULL(ttg.VoiceGrantCount, 0) +
				ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS VoiceGrantCount,
			SUM(ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
			SUM(ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
            (pftg.PatchCount + pttg.PatchCount) AS PatchCount
		FROM Talkgroups tg WITH (NOLOCK)
        INNER JOIN FromTalkgroups pftg WITH (NOLOCK)
            ON (pftg.TalkgroupID = tg.ID)
        INNER JOIN ToTalkgroups pttg WITH (NOLOCK)
            ON (pttg.TalkgroupID = tg.ID)
		LEFT JOIN TowerTalkgroups ttg WITH (NOLOCK)
			ON ((ttg.TalkgroupID = tg.ID)
                AND ((@dateFrom IS NULL)
                    OR (ttg.[Date] >= @dateFrom))
                AND ((@dateTo IS NULL)
                    OR (ttg.[Date] <= @dateTo)))
		WHERE ((tg.SystemID = @systemIDKey)
            AND (tg.[Description] LIKE '<Unknown>%')
            AND ((@talkgroupIDFrom IS NULL)
                OR (tg.TalkgroupID >= @talkgroupIDFrom))
            AND ((@talkgroupIDTo IS NULL)
                OR (tg.TalkgroupID <= @talkgroupIDTo))
            AND ((@firstSeenFrom IS NULL)
                OR (tg.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (tg.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (tg.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenFrom IS NULL)
                OR (tg.LastSeen <= @lastSeenTo)))
		GROUP BY tg.ID,
			tg.TalkgroupID,
			tg.[Description],
			tg.FirstSeen,
			tg.LastSeen,
			tg.EncryptionSeen,
            tg.PhaseIISeen,
            pftg.PatchCount,
            pttg.PatchCount
	)
	SELECT tgt.ID,
		tgt.TalkgroupID,
		tgt.[Description],
		tgt.FirstSeen,
		tgt.LastSeen,
		tgt.EncryptionSeen,
        tgt.PhaseIISeen,
        tgt.Towers,
		tgt.AffiliationCount,
		tgt.DeniedCount,
		tgt.VoiceGrantCount,
		tgt.EmergencyVoiceGrantCount,
		tgt.EncryptedVoiceGrantCount,
        tgt.PatchCount,
        COUNT(1) OVER() AS RecordCount
	FROM TalkgroupTotals tgt
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.TalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.TalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.[Description] END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.[Description] END DESC,
        CASE WHEN ((@sortField = @FIELD_TOWERS_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN
            CASE WHEN (tgt.Towers IS NULL) THEN 1 ELSE 0 END END,
        CASE WHEN ((@sortField = @FIELD_TOWERS_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN
            CASE WHEN (tgt.Towers IS NULL) THEN 0 ELSE 1 END END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.EncryptedVoiceGrantCount END DESC,
        CASE WHEN ((@sortField = @FIELD_ENCRYPTION_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.EncryptionSeen END,
        CASE WHEN ((@sortField = @FIELD_ENCRYPTION_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.EncryptionSeen END DESC,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.PhaseIISeen END,
        CASE WHEN ((@sortField = @FIELD_PHASE_II_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.PhaseIISeen END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tgt.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tgt.FirstSeen END DESC,
        CASE WHEN (@sortDirection = @DIRECTION_ASCENDING) THEN tgt.Towers END,
        CASE WHEN (@sortDirection = @DIRECTION_DESCENDING) THEN tgt.Towers END DESC,
		tgt.TalkgroupID
	OFFSET ((@pageNumber - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsGetForSystem]    Script Date: 2/12/2019 2:49:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupsGetForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tg.ID,
		tg.SystemID,
		tg.TalkgroupID,
		tg.[Priority],
		tg.[Description],
		tg.LastSeen,
		tg.LastSeenProgram,
		tg.LastSeenProgramUnix,
		tg.FirstSeen,
		tg.FirstSeenProgram,
		tg.FirstSeenProgramUnix,
		tg.FGColor,
		tg.BGColor,
		tg.EncryptionSeen,
		tg.IgnoreEmergencySignal,
		tg.HitCount,
		tg.HitCountProgram,
        tg.PhaseIISeen,
		tg.LastModified
	FROM Talkgroups tg
	WHERE (tg.SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsGetForSystemTalkgroup]    Script Date: 2/12/2019 2:49:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupsGetForSystemTalkgroup]
(
	@systemID NVARCHAR(50),
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tg.ID,
		tg.SystemID,
		tg.TalkgroupID,
		tg.[Priority],
		tg.[Description],
		tg.LastSeen,
		tg.LastSeenProgram,
		tg.LastSeenProgramUnix,
		tg.FirstSeen,
		tg.FirstSeenProgram,
		tg.FirstSeenProgramUnix,
		tg.FGColor,
		tg.BGColor,
		tg.EncryptionSeen,
		tg.IgnoreEmergencySignal,
		tg.HitCount,
		tg.HitCountProgram,
        tg.PhaseIISeen,
		tg.LastModified
	FROM dbo.Talkgroups tg WITH (NOLOCK)
	INNER JOIN dbo.Systems s WITH (NOLOCK)
		ON (s.ID = tg.SystemID)
	WHERE ((s.SystemID = @systemID)
		AND (tg.TalkgroupID = @talkgroupID));
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsInsert]    Script Date: 2/12/2019 2:49:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupsInsert]
(
	@systemID INT,
	@talkgroupID INT,
	@priority INT,
	@description NVARCHAR(100),
	@lastSeen DATETIME2(7),
	@lastSeenProgram DATETIME2(7),
	@lastSeenProgramUnix BIGINT,
	@firstSeen DATETIME2(7),
	@firstSeenProgram DATETIME2(7),
	@firstSeenProgramUnix BIGINT,
	@fgColor NVARCHAR(50),
	@bgColor NVARCHAR(50),
	@encryptionSeen BIT,
	@ignoreEmergencySignal BIT,
	@hitCount INT,
	@hitCountProgram INT,
    @phaseIISeen BIT
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.Talkgroups
	(
		SystemID,
		TalkgroupID,
		[Priority],
		[Description],
		LastSeen,
		LastSeenProgram,
		LastSeenProgramUnix,
		FirstSeen,
		FirstSeenProgram,
		FirstSeenProgramUnix,
		BGColor,
		FGColor,
		EncryptionSeen,
		IgnoreEmergencySignal,
		HitCount,
		HitCountProgram,
        PhaseIISeen
	)
	VALUES
	(
		@systemID,
		@talkgroupID,
		@priority,
		@description,
		@lastSeen,
		@lastSeenProgram,
		@lastSeenProgramUnix,
		@firstSeen,
		@firstSeenProgram,
		@firstSeenProgramUnix,
		@fgColor,
		@bgColor,
		@encryptionSeen,
		@ignoreEmergencySignal,
		@hitCount,
		@hitCountProgram,
        @phaseIISeen
	);

	SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TalkgroupsUpdate]    Script Date: 2/12/2019 2:49:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TalkgroupsUpdate]
(
	@systemID INT,
	@talkgroupID INT,
	@priority INT,
	@description NVARCHAR(100),
	@lastSeen DATETIME2(7),
	@lastSeenProgram DATETIME2(7),
	@lastSeenProgramUnix BIGINT,
	@firstSeen DATETIME2(7),
	@firstSeenProgram DATETIME2(7),
	@firstSeenProgramUnix BIGINT,
	@fgColor NVARCHAR(50),
	@bgColor NVARCHAR(50),
	@encryptionSeen BIT,
	@ignoreEmergencySignal BIT,
	@hitCount INT,
	@hitCountProgram INT,
    @phaseIISeen BIT
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Talkgroups
	SET [Priority] = @priority,
		[Description] = @description,
		LastSeen = @lastSeen,
		LastSeenProgram = @lastSeenProgram,
		LastSeenProgramUnix = @lastSeenProgramUnix,
		FirstSeen = @firstSeen,
		FirstSeenProgram = @firstSeenProgram,
		FirstSeenProgramUnix = @firstSeenProgramUnix,
		FGColor = @fgColor,
		BGColor = @bgColor,
		EncryptionSeen = @encryptionSeen,
		IgnoreEmergencySignal = @ignoreEmergencySignal,
		HitCount = @hitCount,
		HitCountProgram = @hitCountProgram,
        PhaseIISeen = @phaseIISeen,
		LastModified = GETDATE()
	WHERE ((SystemID = @systemID)
		AND (TalkgroupID = @talkgroupID));
END
GO

/****** Object:  StoredProcedure [dbo].[TempPatchesInsert]    Script Date: 2/12/2019 2:49:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TempPatchesInsert]
(
	@sessionID UNIQUEIDENTIFIER,
	@systemID INT,
	@towerID INT,
	@fromTalkgroupID INT,
	@toTalkgroupID INT,
	@date DATETIME2(7),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7),
	@hitCount INT
)
AS
BEGIN

	INSERT INTO dbo.TempPatches
	(
		SessionID,
		SystemID,
		TowerID,
		FromTalkgroupID,
		ToTalkgroupID,
		[Date],
		FirstSeen,
		LastSeen,
		HitCount
	)
	VALUES
	(
		@sessionID,
		@systemID,
		@towerID,
		@fromTalkgroupID,
		@toTalkgroupID,
		@date,
		@firstSeen,
		@lastSeen,
		@hitCount
	);

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TempRadioHistoryInsert]    Script Date: 2/12/2019 2:49:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TempRadioHistoryInsert]
(
	@sessionID UNIQUEIDENTIFIER,
	@systemID INT,
	@radioID INT,
	@description NVARCHAR(100),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TempRadioHistory
	(
		SessionID,
		SystemID,
		RadioID,
		[Description],
		FirstSeen,
		LastSeen
	)
	VALUES
	(
		@sessionID,
		@systemID,
		@radioID,
		@description,
		@firstSeen,
		@lastSeen
	);

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TempRadiosInsert]    Script Date: 2/12/2019 2:49:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TempRadiosInsert]
(
	@sessionID UNIQUEIDENTIFIER,
    @systemID INT,
    @radioID INT,
    @description NVARCHAR(100),
    @lastSeen DATETIME2(7),
    @lastSeenProgram DATETIME2(7),
    @lastSeenProgramUnix BIGINT,
    @firstSeen DATETIME2(7),
    @fgColor NVARCHAR(50),
    @bgColor NVARCHAR(50),
    @hitCount INT
)
AS
BEGIN
	INSERT INTO dbo.TempRadios
    (
		SessionID,
        SystemID,
        RadioID,
        [Description],
        LastSeen,
        LastSeenProgram,
        LastSeenProgramUnix,
        FirstSeen,
        FGColor,
        BGColor,
        HitCount
    )
    VALUES
    (
		@sessionID,
        @systemID,
        @radioID,
        @description,
        @lastSeen,
        @lastSeenProgram,
        @lastSeenProgramUnix,
        @firstSeen,
        @fgColor,
        @bgColor,
        @hitCount
    );

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TempSystemsInsert]    Script Date: 2/12/2019 2:49:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TempSystemsInsert]
(
	@sessionID UNIQUEIDENTIFIER,
	@systemID NVARCHAR(50),
	@systemIDDecimal INT,
	@description NVARCHAR(100),
	@wacn NVARCHAR(50),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TempSystems
	(
		SessionID,
		SystemID,
		SystemIDDecimal,
		[Description],
		WACN,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
		@sessionID,
		@systemID,
		@systemIDDecimal,
		@description,
		@wacn,
		@firstSeen,
		@lastSeen
	);

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TempTalkgroupHistoryInsert]    Script Date: 2/12/2019 2:49:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TempTalkgroupHistoryInsert]
(
	@sessionID UNIQUEIDENTIFIER,
	@systemID INT,
	@talkgroupID INT,
	@description NVARCHAR(100),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT dbo.TempTalkgroupHistory
	(
		SessionID,
		SystemID,
		TalkgroupID,
		[Description],
		FirstSeen,
		LastSeen
	)
	VALUES
	(
		@sessionID,
		@systemID,
		@talkgroupID,
		@description,
		@firstSeen,
		@lastSeen
	);

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TempTalkgroupRadiosInsert]    Script Date: 2/12/2019 2:49:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TempTalkgroupRadiosInsert]
(
    @sessionID UNIQUEIDENTIFIER,
	@systemID INT,
	@talkgroupID INT,
	@radioID INT,
	@date DATETIME2(7),
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TempTalkgroupRadios
	(
        SessionID,
		SystemID,
		TalkgroupID,
		RadioID,
		[Date],
		AffiliationCount,
		DeniedCount,
		VoiceGrantCount,
		EmergencyVoiceGrantCount,
		EncryptedVoiceGrantCount,
		DataCount,
		PrivateDataCount,
		AlertCount,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
        @sessionID,
		@systemID,
		@talkgroupID,
		@radioID,
		@date,
		@affiliationCount,
		@deniedCount,
		@voiceGrantCount,
		@emergencyVoiceGrantCount,
		@encryptedVoiceGrantCount,
		@dataCount,
		@privateDataCount,
		@alertCount,
		@firstSeen,
		@lastSeen
	);

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TempTalkgroupsInsert]    Script Date: 2/12/2019 2:49:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TempTalkgroupsInsert]
(
    @sessionID UNIQUEIDENTIFIER,
	@systemID INT,
	@talkgroupID INT,
	@priority INT,
	@description NVARCHAR(100),
	@lastSeen DATETIME2(7),
	@lastSeenProgram DATETIME2(7),
	@lastSeenProgramUnix BIGINT,
	@firstSeen DATETIME2(7),
	@firstSeenProgram DATETIME2(7),
	@firstSeenProgramUnix BIGINT,
	@fgColor NVARCHAR(50),
	@bgColor NVARCHAR(50),
	@encryptionSeen BIT,
	@ignoreEmergencySignal BIT,
	@hitCount INT,
	@hitCountProgram INT
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TempTalkgroups
	(
        SessionID,
		SystemID,
		TalkgroupID,
		[Priority],
		[Description],
		LastSeen,
		LastSeenProgram,
		LastSeenProgramUnix,
		FirstSeen,
		FirstSeenProgram,
		FirstSeenProgramUnix,
		BGColor,
		FGColor,
		EncryptionSeen,
		IgnoreEmergencySignal,
		HitCount,
		HitCountProgram
	)
	VALUES
	(
        @sessionID,
		@systemID,
		@talkgroupID,
		@priority,
		@description,
		@lastSeen,
		@lastSeenProgram,
		@lastSeenProgramUnix,
		@firstSeen,
		@firstSeenProgram,
		@firstSeenProgramUnix,
		@fgColor,
		@bgColor,
		@encryptionSeen,
		@ignoreEmergencySignal,
		@hitCount,
		@hitCountProgram
	);

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TempTowerFrequenciesInsert]    Script Date: 2/12/2019 2:49:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TempTowerFrequenciesInsert]
(
    @sessionID UNIQUEIDENTIFIER,
	@systemID INT,
	@towerID INT,
	@channel NVARCHAR(50),
	@usage NVARCHAR(50),
	@frequency NVARCHAR(50),
	@inputChannel NVARCHAR(50),
	@inputFrequency NVARCHAR(50),
	@inputExplicit INT,
	@hitCountProgram INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TempTowerFrequencies
	(
        SessionID,
		SystemID,
		TowerID,
		Channel,
		Usage,
		Frequency,
		InputChannel,
		InputFrequency,
		InputExplicit,
		HitCount,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
        @sessionID,
		@systemID,
		@towerID,
		@channel,
		@usage,
		@frequency,
		@inputChannel,
		@inputFrequency,
		@inputExplicit,
		@hitCountProgram,
		@firstSeen,
		@lastSeen
	);

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TempTowerFrequencyUsageInsert]    Script Date: 2/12/2019 2:49:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TempTowerFrequencyUsageInsert]
(
    @sessionID UNIQUEIDENTIFIER,
	@systemID INT,
	@towerID INT,
	@channel NVARCHAR(50),
	@frequency NVARCHAR(50),
	@talkgroupID INT,
	@radioID INT,
	@date DATETIME2(7),
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@cwidCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TempTowerFrequencyUsage
	(
        SessionID,
		SystemID,
		TowerID,
		Channel,
		Frequency,
		[Date],
		AffiliationCount,
		DeniedCount,
		VoiceGrantCount,
		EmergencyVoiceGrantCount,
		EncryptedVoiceGrantCount,
		DataCount,
		PrivateDataCount,
		CWIDCount,
		AlertCount,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
        @sessionID,
		@systemID,
		@towerID,
		@channel,
		@frequency,
		@date,
		@affiliationCount,
		@deniedCount,
		@voiceGrantCount,
		@emergencyVoiceGrantCount,
		@encryptedVoiceGrantCount,
		@dataCount,
		@privateDataCount,
		@cwidCount,
		@alertCount,
		@firstSeen,
		@lastSeen
	);

	SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TempTowerRadiosInsert]    Script Date: 2/12/2019 2:49:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TempTowerRadiosInsert]
(
    @sessionID UNIQUEIDENTIFIER,
	@systemID INT,
	@towerNumber INT,
	@radioID INT,
	@date DATETIME2(7),
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TempTowerRadios
	(
        SessionID,
		SystemID,
		TowerID,
		RadioID,
		[Date],
		AffiliationCount,
		DeniedCount,
		VoiceGrantCount,
		EmergencyVoiceGrantCount,
		EncryptedVoiceGrantCount,
		DataCount,
		PrivateDataCount,
		AlertCount,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
        @sessionID,
		@systemID,
		@towerNumber,
		@radioID,
		@date,
		@affiliationCount,
		@deniedCount,
		@voiceGrantCount,
		@emergencyVoiceGrantCount,
		@encryptedVoiceGrantCount,
		@dataCount,
		@privateDataCount,
		@alertCount,
		@firstSeen,
		@lastSeen
	);

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TempTowersInsert]    Script Date: 2/12/2019 2:49:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TempTowersInsert]
(
    @sessionID UNIQUEIDENTIFIER,
	@systemID INT,
	@towerNumber INT,
	@towerNumberHex NVARCHAR(50),
	@description NVARCHAR(100),
	@hitCount INT,
	@wacn NVARCHAR(50),
	@controlCapabilities NVARCHAR(50),
	@flavor NVARCHAR(50),
	@callSigns NVARCHAR(50),
	@timeStamp DATETIME2(7),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TempTowers
	(
        SessionID,
		SystemID,
		TowerNumber,
		TowerNumberHex,
		[Description],
		HitCount,
		WACN,
		ControlCapabilities,
		Flavor,
		CallSigns,
		[TimeStamp],
		FirstSeen,
		LastSeen
	)
	VALUES
	(
        @sessionID,
		@systemID,
		@towerNumber,
		@towerNumberHex,
		@description,
		@hitCount,
		@wacn,
		@controlCapabilities,
		@flavor,
		@callSigns,
		@timeStamp,
		@firstSeen,
		@lastSeen
	);

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TempTowerTalkgroupRadiosInsert]    Script Date: 2/12/2019 2:49:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TempTowerTalkgroupRadiosInsert]
(
    @sessionID UNIQUEIDENTIFIER,
	@systemID INT,
	@towerID INT,
	@talkgroupID INT,
	@radioID INT,
	@date DATETIME2(7),
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TempTowerTalkgroupRadios
	(
        SessionID,
		SystemID,
		TowerID,
		TalkgroupID,
		RadioID,
		[Date],
		AffiliationCount,
		DeniedCount,
		VoiceGrantCount,
		EmergencyVoiceGrantCount,
		EncryptedVoiceGrantCount,
		DataCount,
		PrivateDataCount,
		AlertCount,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
        @sessionID,
		@systemID,
		@towerID,
		@talkgroupID,
		@radioID,
		@date,
		@affiliationCount,
		@deniedCount,
		@voiceGrantCount,
		@emergencyVoiceGrantCount,
		@encryptedVoiceGrantCount,
		@dataCount,
		@privateDataCount,
		@alertCount,
		@firstSeen,
		@lastSeen
	);

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TempTowerTalkgroupsInsert]    Script Date: 2/12/2019 2:49:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TempTowerTalkgroupsInsert]
(
    @sessionID UNIQUEIDENTIFIER,
	@systemID INT,
	@towerNumber INT,
	@talkgroupID INT,
	@date DATETIME2(7),
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TempTowerTalkgroups
	(
        SessionID,
		SystemID,
		TowerID,
		TalkgroupID,
		[Date],
		AffiliationCount,
		DeniedCount,
		VoiceGrantCount,
		EmergencyVoiceGrantCount,
		EncryptedVoiceGrantCount,
		DataCount,
		PrivateDataCount,
		AlertCount,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
        @sessionID,
		@systemID,
		@towerNumber,
		@talkgroupID,
		@date,
		@affiliationCount,
		@deniedCount,
		@voiceGrantCount,
		@emergencyVoiceGrantCount,
		@encryptedVoiceGrantCount,
		@dataCount,
		@privateDataCount,
		@alertCount,
		@firstSeen,
		@lastSeen
	);

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesDelete]    Script Date: 2/12/2019 2:49:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequenciesDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TowerFrequencies
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesDeleteForSystem]    Script Date: 2/12/2019 2:49:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequenciesDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TowerFrequencies
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesGet]    Script Date: 2/12/2019 2:49:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequenciesGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tf.ID,
		tf.SystemID,
		tf.TowerID,
		tf.Channel,
		tf.Usage,
		tf.Frequency,
		tf.InputChannel,
		tf.InputFrequency,
		tf.InputExplicit,
		tf.HitCount,
		tf.FirstSeen,
		tf.LastSeen,
		tf.LastModified
	FROM dbo.TowerFrequencies tf
	WHERE (tf.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesGetForFrequency]    Script Date: 2/12/2019 2:49:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequenciesGetForFrequency]
(
	@systemID INT,
	@towerNumber INT,
	@frequency NVARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tf.ID,
		tf.SystemID,
		tf.TowerID,
		tf.Channel,
		tf.Usage,
		tf.Frequency,
		tf.InputChannel,
		tf.InputFrequency,
		tf.InputExplicit,
		tf.HitCount,
		tf.FirstSeen,
		tf.LastSeen,
		tf.LastModified
	FROM dbo.TowerFrequencies tf WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tf.TowerID)
	WHERE ((tf.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
		AND (tf.Frequency = @frequency));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesGetForTower]    Script Date: 2/12/2019 2:49:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequenciesGetForTower]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tf.ID,
		tf.SystemID,
		tf.TowerID,
		tf.Channel,
		tf.Usage,
		tf.Frequency,
		tf.InputChannel,
		tf.InputFrequency,
		tf.InputExplicit,
		tf.HitCount,
		tf.FirstSeen,
		tf.LastSeen,
		tf.LastModified
	FROM dbo.TowerFrequencies tf WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tf.TowerID)
	WHERE ((tf.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesGetFrequenciesForTower]    Script Date: 2/12/2019 2:49:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequenciesGetFrequenciesForTower]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tf.Frequency,
		tf.Channel,
		tf.Usage,
		SUM(ISNULL(tfu.VoiceGrantCount, 0) +
			ISNULL(tfu.EmergencyVoiceGrantCount, 0) +
			ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS HitCount,
		SUM(ISNULL(tfu.AffiliationCount, 0)) AS AffiliationCount,
		SUM(ISNULL(tfu.DeniedCount, 0)) AS DeniedCount,
		SUM(ISNULL(tfu.VoiceGrantCount, 0)) AS VoiceGrantCount,
		SUM(ISNULL(tfu.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		SUM(ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
		SUM(ISNULL(tfu.DataCount, 0) +
			ISNULL(tfu.PrivateDataCount, 0)) AS DataCount,
		SUM(ISNULL(tfu.PrivateDataCount, 0)) AS PrivateDataCount,
		SUM(ISNULL(tfu.CWIDCount, 0)) AS CWIDCount,
		SUM(ISNULL(tfu.AlertCount, 0)) AS AlertCount,
		MIN(ISNULL(tfu.FirstSeen, GETDATE())) AS FirstSeen,
		MAX(ISNULL(tfu.LastSeen, GETDATE())) AS LastSeen,
        COUNT(1) OVER() AS RecordCount
	FROM dbo.TowerFrequencies tf WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tf.TowerID)
	LEFT JOIN dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
		ON ((tfu.TowerID = tf.TowerID)
			AND (tfu.Frequency = tf.Frequency))
	WHERE ((tf.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber))
	GROUP BY tf.Frequency,
		tf.Channel,
		tf.Usage
	ORDER BY tf.Frequency;

END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesGetFrequenciesForTowerAll]    Script Date: 2/12/2019 2:49:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequenciesGetFrequenciesForTowerAll]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

    WITH CombinedFrequencies (TowerID, Frequency)
    AS
    (
        SELECT tfu.TowerID,
            tfu.Frequency
        FROM dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
        INNER JOIN dbo.Towers t WITH (NOLOCK)
            ON (t.ID = tfu.TowerID)
        WHERE ((t.SystemID = @systemID)
            AND (t.TowerNumber = @towerNumber))
        GROUP BY tfu.TowerID,
            tfu.Frequency
        UNION
            SELECT tf.TowerID,
                tf.Frequency
            FROM dbo.TowerFrequencies tf WITH (NOLOCK)
            INNER JOIN dbo.Towers t WITH (NOLOCK)
                ON (t.ID = tf.TowerID)
            WHERE ((t.SystemID = @systemID)
                AND (t.TowerNumber = @towerNumber))
            GROUP BY tf.TowerID,
                tf.Frequency
    )
    SELECT cf.Frequency,
        ISNULL(tfu.Channel, ISNULL(tf.Channel, N'')) AS Channel,
	    ISNULL(tf.Usage, N'') AS Usage,
	    SUM(ISNULL(tfu.VoiceGrantCount, 0) +
		    ISNULL(tfu.EmergencyVoiceGrantCount, 0) +
		    ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS HitCount,
	    SUM(ISNULL(tfu.AffiliationCount, 0)) AS AffiliationCount,
	    SUM(ISNULL(tfu.DeniedCount, 0)) AS DeniedCount,
	    SUM(ISNULL(tfu.VoiceGrantCount, 0)) AS VoiceGrantCount,
	    SUM(ISNULL(tfu.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
	    SUM(ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
	    SUM(ISNULL(tfu.DataCount, 0) +
		    ISNULL(tfu.PrivateDataCount, 0)) AS DataCount,
	    SUM(ISNULL(tfu.PrivateDataCount, 0)) AS PrivateDataCount,
	    SUM(ISNULL(tfu.CWIDCount, 0)) AS CWIDCount,
	    SUM(ISNULL(tfu.AlertCount, 0)) AS AlertCount,
	    MIN(ISNULL(tfu.FirstSeen, GETDATE())) AS FirstSeen,
	    MAX(ISNULL(tfu.LastSeen, GETDATE())) AS LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM CombinedFrequencies cf WITH (NOLOCK)
    LEFT JOIN dbo.TowerFrequencies tf WITH (NOLOCK)
        ON ((tf.TowerID = cf.TowerID)
            AND (tf.Frequency = cf.Frequency))
    LEFT JOIN dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
        ON ((tfu.TowerID = cf.TowerID)
            AND (tfu.Frequency = cf.Frequency))
    GROUP BY cf.Frequency,
        ISNULL(tfu.Channel, ISNULL(tf.Channel, N'')),
	    ISNULL(tf.Usage, N'');
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesGetFrequenciesForTowerAllCount]    Script Date: 2/12/2019 2:49:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequenciesGetFrequenciesForTowerAllCount]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

    WITH CombinedFrequencies (TowerID, Frequency)
    AS
    (
        SELECT tfu.TowerID,
            tfu.Frequency
        FROM dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
        INNER JOIN dbo.Towers t WITH (NOLOCK)
            ON (t.ID = tfu.TowerID)
        WHERE ((t.SystemID = @systemID)
            AND (t.TowerNumber = @towerNumber))
        GROUP BY tfu.TowerID,
            tfu.Frequency
        UNION
            SELECT tf.TowerID,
                tf.Frequency
            FROM dbo.TowerFrequencies tf WITH (NOLOCK)
            INNER JOIN dbo.Towers t WITH (NOLOCK)
                ON (t.ID = tf.TowerID)
            WHERE ((t.SystemID = @systemID)
                AND (t.TowerNumber = @towerNumber))
            GROUP BY tf.TowerID,
                tf.Frequency
    )
    SELECT COUNT(*)
    FROM CombinedFrequencies cf;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesGetFrequenciesForTowerAllFiltersWithPaging]    Script Date: 2/12/2019 2:49:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerFrequenciesGetFrequenciesForTowerAllFiltersWithPaging]
(
    @systemID NVARCHAR(50),
    @towerNumber INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @FIELD_FREQUENCY NVARCHAR(50) = N'Frequency',
        @FIELD_CHANNEL NVARCHAR(50) = N'Channel',
        @FIELD_USAGE NVARCHAR(50) = N'Usage',
        @FIELD_HIT_COUNT NVARCHAR(50) = N'HitCount',
        @FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
		@FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_CWID_COUNT NVARCHAR(50) = N'CWIDCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH CombinedFrequencies (TowerID, Frequency)
    AS
    (
        SELECT tfu.TowerID,
            tfu.Frequency
        FROM dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
        INNER JOIN dbo.Towers t WITH (NOLOCK)
            ON (t.ID = tfu.TowerID)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = t.SystemID)
        WHERE ((s.SystemID = @systemID)
            AND (t.TowerNumber = @towerNumber))
        GROUP BY tfu.TowerID,
            tfu.Frequency
        UNION
            SELECT tf.TowerID,
                tf.Frequency
            FROM dbo.TowerFrequencies tf WITH (NOLOCK)
            INNER JOIN dbo.Towers t WITH (NOLOCK)
                ON (t.ID = tf.TowerID)
            INNER JOIN dbo.Systems s WITH (NOLOCK)
                ON (s.ID = t.SystemID)
            WHERE ((s.SystemID = @systemID)
                AND (t.TowerNumber = @towerNumber))
            GROUP BY tf.TowerID,
                tf.Frequency
    ),
    CombinedFrequenciesResults (Frequency, Channel, Usage, HitCount, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, PrivateDataCount, CWIDCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
        SELECT cf.Frequency,
            ISNULL(tfu.Channel, ISNULL(tf.Channel, N'')) AS Channel,
	        ISNULL(tf.Usage, N'') AS Usage,
	        SUM(ISNULL(tfu.VoiceGrantCount, 0) +
		        ISNULL(tfu.EmergencyVoiceGrantCount, 0) +
		        ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS HitCount,
	        SUM(ISNULL(tfu.AffiliationCount, 0)) AS AffiliationCount,
	        SUM(ISNULL(tfu.DeniedCount, 0)) AS DeniedCount,
	        SUM(ISNULL(tfu.VoiceGrantCount, 0)) AS VoiceGrantCount,
	        SUM(ISNULL(tfu.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
	        SUM(ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
	        SUM(ISNULL(tfu.DataCount, 0) +
		        ISNULL(tfu.PrivateDataCount, 0)) AS DataCount,
	        SUM(ISNULL(tfu.PrivateDataCount, 0)) AS PrivateDataCount,
	        SUM(ISNULL(tfu.CWIDCount, 0)) AS CWIDCount,
	        SUM(ISNULL(tfu.AlertCount, 0)) AS AlertCount,
	        MIN(ISNULL(tfu.FirstSeen, GETDATE())) AS FirstSeen,
	        MAX(ISNULL(tfu.LastSeen, GETDATE())) AS LastSeen
        FROM CombinedFrequencies cf WITH (NOLOCK)
        LEFT JOIN dbo.TowerFrequencies tf WITH (NOLOCK)
            ON ((tf.TowerID = cf.TowerID)
                AND (tf.Frequency = cf.Frequency))
        LEFT JOIN dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
            ON ((tfu.TowerID = cf.TowerID)
                AND (tfu.Frequency = cf.Frequency))
        WHERE (((@searchText IS NULL)
                OR (tf.Frequency LIKE N'%' + @searchText + N'%'))
            AND ((@dateFrom IS NULL)
                OR (tfu.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (tfu.[Date] <= @dateTo))
            AND ((@firstSeenFrom IS NULL)
                OR (tfu.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (tfu.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (tfu.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (tfu.LastSeen <= @lastSeenTo)))
        GROUP BY cf.Frequency,
            ISNULL(tfu.Channel, ISNULL(tf.Channel, N'')),
	        ISNULL(tf.Usage, N'')
    )
    SELECT cfr.Frequency, 
        cfr.Channel, 
        cfr.Usage, 
        cfr.HitCount, 
        cfr.AffiliationCount, 
        cfr.DeniedCount, 
        cfr.VoiceGrantCount, 
        cfr.EmergencyVoiceGrantCount, 
        cfr.EncryptedVoiceGrantCount, 
        cfr.DataCount, 
        cfr.PrivateDataCount, 
        cfr.CWIDCount, 
        cfr.AlertCount, 
        cfr.FirstSeen, 
        cfr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM CombinedFrequenciesResults cfr
    ORDER BY
		CASE WHEN ((@sortField = @FIELD_FREQUENCY) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.Frequency END,
		CASE WHEN ((@sortField = @FIELD_FREQUENCY) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.Frequency END DESC,
		CASE WHEN ((@sortField = @FIELD_CHANNEL) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.Channel END,
		CASE WHEN ((@sortField = @FIELD_CHANNEL) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.Channel END DESC,
		CASE WHEN ((@sortField = @FIELD_USAGE) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.Usage END,
		CASE WHEN ((@sortField = @FIELD_USAGE) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.Usage END DESC,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.HitCount END,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.HitCount END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.DataCount END DESC,
		CASE WHEN ((@sortField = @FIELD_CWID_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.CWIDCount END,
		CASE WHEN ((@sortField = @FIELD_CWID_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.CWIDCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.AlertCount END,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.AlertCount END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.LastSeen END DESC,
        cfr.Frequency
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);

END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesGetFrequenciesForTowerCount]    Script Date: 2/12/2019 2:49:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerFrequenciesGetFrequenciesForTowerCount]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(DISTINCT tf.Frequency)
	FROM dbo.TowerFrequencies tf WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tf.TowerID)
	WHERE ((tf.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber));
	
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesGetFrequenciesForTowerFiltersWithPaging]    Script Date: 2/12/2019 2:49:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerFrequenciesGetFrequenciesForTowerFiltersWithPaging]
(
	@systemID NVARCHAR(50),
	@towerNumber INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @FIELD_FREQUENCY NVARCHAR(50) = N'Frequency',
        @FIELD_CHANNEL NVARCHAR(50) = N'Channel',
        @FIELD_USAGE NVARCHAR(50) = N'Usage',
        @FIELD_HIT_COUNT NVARCHAR(50) = N'HitCount',
        @FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
		@FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_CWID_COUNT NVARCHAR(50) = N'CWIDCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH CombinedFrequenciesResults (Frequency, Channel, Usage, HitCount, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, PrivateDataCount, CWIDCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
	    SELECT tf.Frequency,
		    tf.Channel,
		    tf.Usage,
		    SUM(ISNULL(tfu.VoiceGrantCount, 0) +
			    ISNULL(tfu.EmergencyVoiceGrantCount, 0) +
			    ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS HitCount,
		    SUM(ISNULL(tfu.AffiliationCount, 0)) AS AffiliationCount,
		    SUM(ISNULL(tfu.DeniedCount, 0)) AS DeniedCount,
		    SUM(ISNULL(tfu.VoiceGrantCount, 0)) AS VoiceGrantCount,
		    SUM(ISNULL(tfu.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		    SUM(ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
		    SUM(ISNULL(tfu.DataCount, 0) +
			    ISNULL(tfu.PrivateDataCount, 0)) AS DataCount,
		    SUM(ISNULL(tfu.PrivateDataCount, 0)) AS PrivateDataCount,
		    SUM(ISNULL(tfu.CWIDCount, 0)) AS CWIDCount,
		    SUM(ISNULL(tfu.AlertCount, 0)) AS AlertCount,
		    MIN(ISNULL(tfu.FirstSeen, GETDATE())) AS FirstSeen,
		    MAX(ISNULL(tfu.LastSeen, GETDATE())) AS LastSeen
	    FROM dbo.TowerFrequencies tf WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = tf.SystemID)
	    INNER JOIN dbo.Towers t WITH (NOLOCK)
		    ON (t.ID = tf.TowerID)
	    LEFT JOIN dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
		    ON ((tfu.TowerID = tf.TowerID)
			    AND (tfu.Frequency = tf.Frequency))
	    WHERE ((s.SystemID = @systemID)
		    AND (t.TowerNumber = @towerNumber)
		    AND ((@searchText IS NULL)
                OR (tf.Frequency LIKE N'%' + @searchText + N'%'))
            AND ((@dateFrom IS NULL)
                OR (tfu.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (tfu.[Date] <= @dateTo))
            AND ((@firstSeenFrom IS NULL)
                OR (tfu.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (tfu.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (tfu.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (tfu.LastSeen <= @lastSeenTo)))
	    GROUP BY tf.Frequency,
		    tf.Channel,
		    tf.Usage
    )
    SELECT cfr.Frequency, 
        cfr.Channel, 
        cfr.Usage, 
        cfr.HitCount, 
        cfr.AffiliationCount, 
        cfr.DeniedCount, 
        cfr.VoiceGrantCount, 
        cfr.EmergencyVoiceGrantCount, 
        cfr.EncryptedVoiceGrantCount, 
        cfr.DataCount, 
        cfr.PrivateDataCount, 
        cfr.CWIDCount, 
        cfr.AlertCount, 
        cfr.FirstSeen, 
        cfr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM CombinedFrequenciesResults cfr
    ORDER BY
		CASE WHEN ((@sortField = @FIELD_FREQUENCY) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.Frequency END,
		CASE WHEN ((@sortField = @FIELD_FREQUENCY) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.Frequency END DESC,
		CASE WHEN ((@sortField = @FIELD_CHANNEL) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.Channel END,
		CASE WHEN ((@sortField = @FIELD_CHANNEL) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.Channel END DESC,
		CASE WHEN ((@sortField = @FIELD_USAGE) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.Usage END,
		CASE WHEN ((@sortField = @FIELD_USAGE) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.Usage END DESC,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.HitCount END,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.HitCount END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.DataCount END DESC,
		CASE WHEN ((@sortField = @FIELD_CWID_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.CWIDCount END,
		CASE WHEN ((@sortField = @FIELD_CWID_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.CWIDCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.AlertCount END,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.AlertCount END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.LastSeen END DESC,
        cfr.Frequency
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);

END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesGetFrequenciesForTowerNotCurrent]    Script Date: 2/12/2019 2:49:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequenciesGetFrequenciesForTowerNotCurrent]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

    WITH CombinedFrequencies (TowerID, Frequency)
    AS
    (
        SELECT tfu.TowerID,
            tfu.Frequency
        FROM dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
        INNER JOIN dbo.Towers t WITH (NOLOCK)
            ON (t.ID = tfu.TowerID)
        WHERE ((t.SystemID = @systemID)
            AND (t.TowerNumber = @towerNumber))
        GROUP BY tfu.TowerID,
            tfu.Frequency
        EXCEPT
            SELECT tf.TowerID,
                tf.Frequency
            FROM dbo.TowerFrequencies tf WITH (NOLOCK)
            INNER JOIN dbo.Towers t WITH (NOLOCK)
                ON (t.ID = tf.TowerID)
            WHERE ((t.SystemID = @systemID)
                AND (t.TowerNumber = @towerNumber))
            GROUP BY tf.TowerID,
                tf.Frequency
    )
    SELECT cf.Frequency,
        ISNULL(tfu.Channel, ISNULL(tf.Channel, N'')) AS Channel,
	    ISNULL(tf.Usage, N'') AS Usage,
	    SUM(ISNULL(tfu.VoiceGrantCount, 0) +
		    ISNULL(tfu.EmergencyVoiceGrantCount, 0) +
		    ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS HitCount,
	    SUM(ISNULL(tfu.AffiliationCount, 0)) AS AffiliationCount,
	    SUM(ISNULL(tfu.DeniedCount, 0)) AS DeniedCount,
	    SUM(ISNULL(tfu.VoiceGrantCount, 0)) AS VoiceGrantCount,
	    SUM(ISNULL(tfu.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
	    SUM(ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
	    SUM(ISNULL(tfu.DataCount, 0) +
		    ISNULL(tfu.PrivateDataCount, 0)) AS DataCount,
	    SUM(ISNULL(tfu.PrivateDataCount, 0)) AS PrivateDataCount,
	    SUM(ISNULL(tfu.CWIDCount, 0)) AS CWIDCount,
	    SUM(ISNULL(tfu.AlertCount, 0)) AS AlertCount,
	    MIN(ISNULL(tfu.FirstSeen, GETDATE())) AS FirstSeen,
	    MAX(ISNULL(tfu.LastSeen, GETDATE())) AS LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM CombinedFrequencies cf WITH (NOLOCK)
    LEFT JOIN dbo.TowerFrequencies tf WITH (NOLOCK)
        ON ((tf.TowerID = cf.TowerID)
            AND (tf.Frequency = cf.Frequency))
    LEFT JOIN dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
        ON ((tfu.TowerID = cf.TowerID)
            AND (tfu.Frequency = cf.Frequency))
    GROUP BY cf.Frequency,
        ISNULL(tfu.Channel, ISNULL(tf.Channel, N'')),
	    ISNULL(tf.Usage, N'');
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesGetFrequenciesForTowerNotCurrentCount]    Script Date: 2/12/2019 2:49:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequenciesGetFrequenciesForTowerNotCurrentCount]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

    WITH CombinedFrequencies (TowerID, Frequency)
    AS
    (
        SELECT tfu.TowerID,
            tfu.Frequency
        FROM dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
        INNER JOIN dbo.Towers t WITH (NOLOCK)
            ON (t.ID = tfu.TowerID)
        WHERE ((t.SystemID = @systemID)
            AND (t.TowerNumber = @towerNumber))
        GROUP BY tfu.TowerID,
            tfu.Frequency
        EXCEPT
            SELECT tf.TowerID,
                tf.Frequency
            FROM dbo.TowerFrequencies tf WITH (NOLOCK)
            INNER JOIN dbo.Towers t WITH (NOLOCK)
                ON (t.ID = tf.TowerID)
            WHERE ((t.SystemID = @systemID)
                AND (t.TowerNumber = @towerNumber))
            GROUP BY tf.TowerID,
                tf.Frequency
    )
    SELECT COUNT(*)
    FROM CombinedFrequencies cf;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesGetFrequenciesForTowerNotCurrentFiltersWithPaging]    Script Date: 2/12/2019 2:49:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerFrequenciesGetFrequenciesForTowerNotCurrentFiltersWithPaging]
(
    @systemID NVARCHAR(50),
    @towerNumber INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @FIELD_FREQUENCY NVARCHAR(50) = N'Frequency',
        @FIELD_CHANNEL NVARCHAR(50) = N'Channel',
        @FIELD_USAGE NVARCHAR(50) = N'Usage',
        @FIELD_HIT_COUNT NVARCHAR(50) = N'HitCount',
        @FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
		@FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_CWID_COUNT NVARCHAR(50) = N'CWIDCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH CombinedFrequencies (TowerID, Frequency)
    AS
    (
        SELECT tfu.TowerID,
            tfu.Frequency
        FROM dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
        INNER JOIN dbo.Towers t WITH (NOLOCK)
            ON (t.ID = tfu.TowerID)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = t.SystemID)
        WHERE ((s.SystemID = @systemID)
            AND (t.TowerNumber = @towerNumber))
        GROUP BY tfu.TowerID,
            tfu.Frequency
        EXCEPT
            SELECT tf.TowerID,
                tf.Frequency
            FROM dbo.TowerFrequencies tf WITH (NOLOCK)
            INNER JOIN dbo.Towers t WITH (NOLOCK)
                ON (t.ID = tf.TowerID)
            INNER JOIN dbo.Systems s
                ON (s.ID = t.SystemID)
            WHERE ((s.SystemID = @systemID)
                AND (t.TowerNumber = @towerNumber))
            GROUP BY tf.TowerID,
                tf.Frequency
    ),
    CombinedFrequenciesResults (Frequency, Channel, Usage, HitCount, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, PrivateDataCount, CWIDCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
        SELECT cf.Frequency,
            ISNULL(tfu.Channel, ISNULL(tf.Channel, N'')) AS Channel,
	        ISNULL(tf.Usage, N'') AS Usage,
	        SUM(ISNULL(tfu.VoiceGrantCount, 0) +
		        ISNULL(tfu.EmergencyVoiceGrantCount, 0) +
		        ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS HitCount,
	        SUM(ISNULL(tfu.AffiliationCount, 0)) AS AffiliationCount,
	        SUM(ISNULL(tfu.DeniedCount, 0)) AS DeniedCount,
	        SUM(ISNULL(tfu.VoiceGrantCount, 0)) AS VoiceGrantCount,
	        SUM(ISNULL(tfu.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
	        SUM(ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
	        SUM(ISNULL(tfu.DataCount, 0) +
		        ISNULL(tfu.PrivateDataCount, 0)) AS DataCount,
	        SUM(ISNULL(tfu.PrivateDataCount, 0)) AS PrivateDataCount,
	        SUM(ISNULL(tfu.CWIDCount, 0)) AS CWIDCount,
	        SUM(ISNULL(tfu.AlertCount, 0)) AS AlertCount,
	        MIN(ISNULL(tfu.FirstSeen, GETDATE())) AS FirstSeen,
	        MAX(ISNULL(tfu.LastSeen, GETDATE())) AS LastSeen
        FROM CombinedFrequencies cf WITH (NOLOCK)
        LEFT JOIN dbo.TowerFrequencies tf WITH (NOLOCK)
            ON ((tf.TowerID = cf.TowerID)
                AND (tf.Frequency = cf.Frequency))
        LEFT JOIN dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
            ON ((tfu.TowerID = cf.TowerID)
                AND (tfu.Frequency = cf.Frequency))
        WHERE (((@searchText IS NULL)
                OR (tf.Frequency LIKE N'%' + @searchText + N'%'))
            AND ((@dateFrom IS NULL)
                OR (tfu.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (tfu.[Date] <= @dateTo))
            AND ((@firstSeenFrom IS NULL)
                OR (tfu.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (tfu.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (tfu.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (tfu.LastSeen <= @lastSeenTo)))
        GROUP BY cf.Frequency,
            ISNULL(tfu.Channel, ISNULL(tf.Channel, N'')),
	        ISNULL(tf.Usage, N'')
    )
    SELECT cfr.Frequency, 
        cfr.Channel, 
        cfr.Usage, 
        cfr.HitCount, 
        cfr.AffiliationCount, 
        cfr.DeniedCount, 
        cfr.VoiceGrantCount, 
        cfr.EmergencyVoiceGrantCount, 
        cfr.EncryptedVoiceGrantCount, 
        cfr.DataCount, 
        cfr.PrivateDataCount, 
        cfr.CWIDCount, 
        cfr.AlertCount, 
        cfr.FirstSeen, 
        cfr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM CombinedFrequenciesResults cfr
    ORDER BY
		CASE WHEN ((@sortField = @FIELD_FREQUENCY) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.Frequency END,
		CASE WHEN ((@sortField = @FIELD_FREQUENCY) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.Frequency END DESC,
		CASE WHEN ((@sortField = @FIELD_CHANNEL) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.Channel END,
		CASE WHEN ((@sortField = @FIELD_CHANNEL) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.Channel END DESC,
		CASE WHEN ((@sortField = @FIELD_USAGE) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.Usage END,
		CASE WHEN ((@sortField = @FIELD_USAGE) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.Usage END DESC,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.HitCount END,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.HitCount END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.DataCount END DESC,
		CASE WHEN ((@sortField = @FIELD_CWID_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.CWIDCount END,
		CASE WHEN ((@sortField = @FIELD_CWID_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.CWIDCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.AlertCount END,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.AlertCount END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN cfr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN cfr.LastSeen END DESC,
        cfr.Frequency
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);

END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesInsert]    Script Date: 2/12/2019 2:49:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequenciesInsert]
(
	@systemID INT,
	@towerID INT,
	@channel NVARCHAR(50),
	@usage NVARCHAR(50),
	@frequency NVARCHAR(50),
	@inputChannel NVARCHAR(50),
	@inputFrequency NVARCHAR(50),
	@inputExplicit INT,
	@hitCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @towerIDKey INT;

	SELECT @towerIDKey = t.ID
	FROM dbo.Towers t
	WHERE ((t.SystemID = @systemID)
		AND (t.TowerNumber = @towerID));

	INSERT INTO dbo.TowerFrequencies
	(
		SystemID,
		TowerID,
		Channel,
		Usage,
		Frequency,
		InputChannel,
		InputFrequency,
		InputExplicit,
		HitCount,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
		@systemID,
		@towerIDKey,
		@channel,
		@usage,
		@frequency,
		@inputChannel,
		@inputFrequency,
		@inputExplicit,
		@hitCount,
		@firstSeen,
		@lastSeen
	);

	SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequenciesUpdate]    Script Date: 2/12/2019 2:49:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequenciesUpdate]
(
	@id INT,
	@channel NVARCHAR(50),
	@usage NVARCHAR(50),
	@inputChannel NVARCHAR(50),
	@inputFrequency NVARCHAR(50),
	@inputExplicit INT,
	@hitCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.TowerFrequencies
	SET Channel = @channel,
		Usage = @usage,
		InputChannel = @inputChannel,
		InputFrequency = @inputFrequency,
		InputExplicit = @inputExplicit,
		HitCount = @hitCount,
		FirstSeen = @firstSeen,
		LastSeen = @lastSeen,
		LastModified = GETDATE()
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyGetSummaryForFrequency]    Script Date: 2/12/2019 2:49:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[TowerFrequencyGetSummaryForFrequency]
(
    @systemID NVARCHAR(50),
    @towerNumber INT,
    @frequency NVARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON

	SELECT tfu.Frequency,
        tf.Channel,
        tf.InputFrequency,
		tf.InputChannel,
        tf.Usage,
		SUM(tfu.AffiliationCount) AS AffiliationCount,
		SUM(tfu.DeniedCount) AS DeniedCount,
		SUM(tfu.VoiceGrantCount) AS VoiceGrantCount,
		SUM(tfu.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		SUM(tfu.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		SUM(tfu.DataCount) AS DataCount,
		SUM(tfu.PrivateDataCount) AS PrivateDataCount,
		SUM(tfu.CWIDCount) AS CWIDCount,
		SUM(tfu.AlertCount) AS AlertCount,
		MIN(tfu.FirstSeen) AS FirstSeen,
		MAX(tfu.LastSeen) AS LastSeen
	FROM dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
    INNER JOIN dbo.Systems s WITH (NOLOCK)
        ON (s.ID = tfu.SystemID)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tfu.TowerID)
    LEFT JOIN dbo.TowerFrequencies tf WITH (NOLOCK)
        ON ((tf.TowerID = tfu.TowerID)
            AND (tf.Frequency = tfu.Frequency))
	WHERE ((s.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
		AND (tfu.Frequency = @frequency))
    GROUP BY tfu.Frequency,
        tf.Channel,
        tf.InputFrequency,
		tf.InputChannel,
        tf.Usage;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyRadiosGet]    Script Date: 2/12/2019 2:49:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[TowerFrequencyRadiosGet]
(
    @id INT
)
AS
BEGIN
    SET NOCOUNT ON

    SELECT tfr.ID,
        tfr.SystemID,
        tfr.TowerID,
        tfr.Frequency,
        tfr.RadioID,
        tfr.[Date],
        tfr.AffiliationCount,
        tfr.DeniedCount,
        tfr.VoiceGrantCount,
        tfr.EmergencyVoiceGrantCount,
        tfr.EncryptedVoiceGrantCount,
        tfr.DataCount,
        tfr.PrivateDataCount,
        tfr.AlertCount,
        tfr.FirstSeen,
        tfr.LastSeen,
        tfr.LastModified
    FROM dbo.TowerFrequencyRadios tfr
    WHERE (tfr.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyRadiosGetForTowerDate]    Script Date: 2/12/2019 2:49:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[TowerFrequencyRadiosGetForTowerDate]
(
    @systemID INT,
    @towerNumber INT,
    @date DATETIME2(7)
)
AS
BEGIN
    SET NOCOUNT ON

	SELECT tfr.ID,
		tfr.SystemID,
		tfr.TowerID,
        tfr.Frequency,
		r.RadioID AS RadioID,
		tfr.[Date],
		tfr.AffiliationCount,
		tfr.DeniedCount,
		tfr.VoiceGrantCount,
		tfr.EmergencyVoiceGrantCount,
		tfr.EncryptedVoiceGrantCount,
		tfr.DataCount,
		tfr.PrivateDataCount,
		tfr.AlertCount,
		tfr.FirstSeen,
		tfr.LastSeen,
		tfr.LastModified
	FROM dbo.TowerFrequencyRadios tfr WITH (NOLOCK)
    INNER JOIN dbo.Towers t WITH (NOLOCK)
        ON (t.ID = tfr.TowerID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = tfr.RadioID)
	WHERE ((tfr.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
        AND (tfr.[Date] >= DATEADD(dd, -1, @date))
        AND (tfr.[Date] <= DATEADD(dd, 1, @date)))
    ORDER BY tfr.Frequency,
        r.RadioID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyRadiosGetRadiosForFrequenciesWithPaging]    Script Date: 2/12/2019 2:49:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[TowerFrequencyRadiosGetRadiosForFrequenciesWithPaging]
(
    @systemID NVARCHAR(50),
	@towerNumber INT,
    @frequency NVARCHAR(50),
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @radioIDFrom INT = NULL,
    @radioIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @FIELD_RADIO_ID NVARCHAR(50) = N'RadioID',
	    @FIELD_RADIO_NAME NVARCHAR(50) = N'RadioName',
        @FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
	    @FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
	    @FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
	    @FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
	    @FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PRIVATE_DATA_COUNT NVARCHAR(50) = N'PrivateDataCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
	    @FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
	    @FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
	    @DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
	    @DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @systemIDKey INT;

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    WITH TowerFrequencyRadioResults (RadioID, RadioDescription, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, PrivateDataCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
        SELECT r.RadioID,
            r.[Description] AS RadioDescription,
            SUM(tfr.AffiliationCount) AS AffiliationCount,
            SUM(tfr.DeniedCount) AS DeniedCount,
            SUM(tfr.VoiceGrantCount) AS VoiceGrantCount,
            SUM(tfr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
            SUM(tfr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
            SUM(tfr.DataCount) AS DataCount,
            SUM(tfr.PrivateDataCount) AS PrivateDataCount,
            SUM(tfr.AlertCount) AS AlertCount,
            MIN(tfr.FirstSeen) AS FirstSeen,
            MAX(tfr.LastSeen) AS LastSeen
        FROM dbo.TowerFrequencyRadios tfr (NOLOCK)
        INNER JOIN dbo.Towers t (NOLOCK)
            ON (t.ID = tfr.TowerID)
        INNER JOIN dbo.Radios r (NOLOCK)
            ON (r.ID = tfr.RadioID)
        WHERE ((tfr.SystemID = @systemIDKey) 
            AND (t.TowerNumber = @towerNumber)
            AND (tfr.Frequency = @frequency)
            AND ((@dateFrom IS NULL)
                OR (tfr.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (tfr.[Date] <= @dateTo))
            AND ((@radioIDFrom IS NULL)
                OR (r.RadioID >= @radioIDFrom))
            AND ((@radioIDTo IS NULL)
                OR (r.RadioID <= @radioIDTo))
	        AND ((@searchText IS NULL)
                OR (r.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (tfr.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (tfr.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (tfr.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (tfr.LastSeen <= @lastSeenTo)))
        GROUP BY r.RadioID,
            r.[Description]
    )
    SELECT tfrr.RadioID,
        tfrr.RadioDescription,
        tfrr.AffiliationCount,
        tfrr.DeniedCount,
        tfrr.VoiceGrantCount,
        tfrr.EmergencyVoiceGrantCount,
        tfrr.EncryptedVoiceGrantCount,
        tfrr.DataCount,
        tfrr.PrivateDataCount,
        tfrr.AlertCount,
        tfrr.FirstSeen,
        tfrr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TowerFrequencyRadioResults tfrr
    ORDER BY
	    CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tfrr.RadioID END,
	    CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tfrr.RadioID END DESC,
	    CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tfrr.RadioDescription END,
	    CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tfrr.RadioDescription END DESC,
	    CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tfrr.AffiliationCount END,
	    CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tfrr.AffiliationCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tfrr.DeniedCount END,
	    CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tfrr.DeniedCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tfrr.VoiceGrantCount END,
	    CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tfrr.VoiceGrantCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tfrr.EmergencyVoiceGrantCount END,
	    CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tfrr.EmergencyVoiceGrantCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tfrr.EncryptedVoiceGrantCount END,
	    CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tfrr.EncryptedVoiceGrantCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tfrr.DataCount END,
	    CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tfrr.DataCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tfrr.PrivateDataCount END,
	    CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tfrr.PrivateDataCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tfrr.AlertCount END,
	    CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tfrr.AlertCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tfrr.FirstSeen END,
	    CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tfrr.FirstSeen END DESC,
	    CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tfrr.LastSeen END,
	    CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tfrr.LastSeen END DESC,
        tfrr.RadioID
    OFFSET ((@row - 1) * @pageSize) ROWS
    FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyRadiosInsert]    Script Date: 2/12/2019 2:49:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[TowerFrequencyRadiosInsert]
(
    @systemID INT,
    @towerID INT,
    @frequency NVARCHAR(50),
    @radioID INT,
    @date DATETIME2(7),
    @affiliationCount INT,
    @deniedCount INT,
    @voiceGrantCount INT,
    @emergencyVoiceGrantCount INT,
    @encryptedVoiceGrantCount INT,
    @dataCount INT,
    @privateDataCount INT,
    @alertCount INT,
    @firstSeen DATETIME2(7),
    @lastSeen DATETIME2(7)
)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO TowerFrequencyRadios
    (
        SystemID,
        TowerID,
        Frequency,
        RadioID,
        [Date],
        AffiliationCount,
        DeniedCount,
        VoiceGrantCount,
        EmergencyVoiceGrantCount,
        EncryptedVoiceGrantCount,
        DataCount,
        PrivateDataCount,
        AlertCount,
        FirstSeen,
        LastSeen    
    )
    VALUES
    (
        @systemID,
        @towerID,
        @frequency,
        @radioID,
        @date,
        @affiliationCount,
        @deniedCount,
        @voiceGrantCount,
        @emergencyVoiceGrantCount,
        @encryptedVoiceGrantCount,
        @dataCount,
        @privateDataCount,
        @alertCount,
        @firstSeen,
        @lastSeen
    );

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyRadiosUpdate]    Script Date: 2/12/2019 2:49:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[TowerFrequencyRadiosUpdate]
(
    @id INT,
    @systemID INT,
    @towerID INT,
    @frequency NVARCHAR(50),
    @radioID INT,
    @date DATETIME2(7),
    @affiliationCount INT,
    @deniedCount INT,
    @voiceGrantCount INT,
    @emergencyVoiceGrantCount INT,
    @encryptedVoiceGrantCount INT,
    @dataCount INT,
    @privateDataCount INT,
    @alertCount INT,
    @firstSeen DATETIME2(7),
    @lastSeen DATETIME2(7)
)
AS
BEGIN
    SET NOCOUNT ON

    UPDATE TowerFrequencyRadios
    SET SystemID = @systemID,
        TowerID = @towerID,
        Frequency = @frequency,
        RadioID = @radioID,
        [Date] = @date,
        AffiliationCount = @affiliationCount,
        DeniedCount = @deniedCount,
        VoiceGrantCount = @voiceGrantCount,
        EmergencyVoiceGrantCount = @emergencyVoiceGrantCount,
        EncryptedVoiceGrantCount = @encryptedVoiceGrantCount,
        DataCount = @dataCount,
        PrivateDataCount = @privateDataCount,
        AlertCount = @alertCount,
        FirstSeen = @firstSeen,
        LastSeen = @lastSeen,
        LastModified = GETDATE()
    WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyTalkgroupsGet]    Script Date: 2/12/2019 2:49:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[TowerFrequencyTalkgroupsGet]
(
    @id INT
)
AS
BEGIN
    SET NOCOUNT ON

    SELECT tft.ID,
        tft.SystemID,
        tft.TowerID,
        tft.Frequency,
        tft.TalkgroupID,
        tft.[Date],
        tft.AffiliationCount,
        tft.DeniedCount,
        tft.VoiceGrantCount,
        tft.EmergencyVoiceGrantCount,
        tft.EncryptedVoiceGrantCount,
        tft.DataCount,
        tft.PrivateDataCount,
        tft.AlertCount,
        tft.FirstSeen,
        tft.LastSeen,
        tft.LastModified
    FROM dbo.TowerFrequencyTalkgroups tft
    WHERE (tft.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyTalkgroupsGetForTowerDate]    Script Date: 2/12/2019 2:49:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[TowerFrequencyTalkgroupsGetForTowerDate]
(
    @systemID INT,
    @towerNumber INT,
    @date DATETIME2(7)
)
AS
BEGIN
    SET NOCOUNT ON

	SELECT tftg.ID,
		tftg.SystemID,
		tftg.TowerID,
        tftg.Frequency,
		tg.TalkgroupID AS TalkgroupID,
		tftg.[Date],
		tftg.AffiliationCount,
		tftg.DeniedCount,
		tftg.VoiceGrantCount,
		tftg.EmergencyVoiceGrantCount,
		tftg.EncryptedVoiceGrantCount,
		tftg.DataCount,
		tftg.PrivateDataCount,
		tftg.AlertCount,
		tftg.FirstSeen,
		tftg.LastSeen,
		tftg.LastModified
	FROM dbo.TowerFrequencyTalkgroups tftg WITH (NOLOCK)
    INNER JOIN dbo.Towers t WITH (NOLOCK)
        ON (t.ID = tftg.TowerID)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = tftg.TalkgroupID)
	WHERE ((tftg.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
        AND (tftg.[Date] >= DATEADD(dd, -1, @date))
        AND (tftg.[Date] <= DATEADD(dd, 1, @date)))
    ORDER BY tftg.Frequency,
        tg.TalkgroupID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyTalkgroupsGetTalkgroupsForFrequenciesWithPaging]    Script Date: 2/12/2019 2:49:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[TowerFrequencyTalkgroupsGetTalkgroupsForFrequenciesWithPaging]
(
    @systemID NVARCHAR(50),
	@towerNumber INT,
    @frequency NVARCHAR(50),
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @talkgroupIDFrom INT = NULL,
    @talkgroupIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @FIELD_TALKGROUP_ID NVARCHAR(50) = N'TalkgroupID',
	    @FIELD_TALKGROUP_NAME NVARCHAR(50) = N'TalkgroupName',
        @FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
	    @FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
	    @FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
	    @FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
	    @FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PRIVATE_DATA_COUNT NVARCHAR(50) = N'PrivateDataCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
	    @FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
	    @FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
	    @DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
	    @DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @systemIDKey INT;

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    WITH TowerFrequencyTalkgroupResults (TalkgroupID, TalkgroupDescription, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, PrivateDataCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
        SELECT tg.TalkgroupID,
            tg.[Description] AS TalkgroupDescription,
            SUM(tftg.AffiliationCount) AS AffiliationCount,
            SUM(tftg.DeniedCount) AS DeniedCount,
            SUM(tftg.VoiceGrantCount) AS VoiceGrantCount,
            SUM(tftg.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
            SUM(tftg.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
            SUM(tftg.DataCount) AS DataCount,
            SUM(tftg.PrivateDataCount) AS PrivateDataCount,
            SUM(tftg.AlertCount) AS AlertCount,
            MIN(tftg.FirstSeen) AS FirstSeen,
            MAX(tftg.LastSeen) AS LastSeen
        FROM dbo.TowerFrequencyTalkgroups tftg (NOLOCK)
        INNER JOIN dbo.Towers t (NOLOCK)
            ON (t.ID = tftg.TowerID)
        INNER JOIN dbo.Talkgroups tg (NOLOCK)
            ON (tg.ID = tftg.TalkgroupID)
        WHERE ((tftg.SystemID = @systemIDKey) 
            AND (t.TowerNumber = @towerNumber)
            AND (tftg.Frequency = @frequency)
            AND ((@dateFrom IS NULL)
                OR (tftg.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (tftg.[Date] <= @dateTo))
            AND ((@talkgroupIDFrom IS NULL)
                OR (tg.TalkgroupID >= @talkgroupIDFrom))
            AND ((@talkgroupIDTo IS NULL)
                OR (tg.TalkgroupID <= @talkgroupIDTo))
	        AND ((@searchText IS NULL)
                OR (tg.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (tftg.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (tftg.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (tftg.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (tftg.LastSeen <= @lastSeenTo)))
        GROUP BY tg.TalkgroupID,
            tg.[Description]
    )
    SELECT tftgr.TalkgroupID,
        tftgr.TalkgroupDescription,
        tftgr.AffiliationCount,
        tftgr.DeniedCount,
        tftgr.VoiceGrantCount,
        tftgr.EmergencyVoiceGrantCount,
        tftgr.EncryptedVoiceGrantCount,
        tftgr.DataCount,
        tftgr.PrivateDataCount,
        tftgr.AlertCount,
        tftgr.FirstSeen,
        tftgr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TowerFrequencyTalkgroupResults tftgr
    ORDER BY
	    CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tftgr.TalkgroupID END,
	    CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tftgr.TalkgroupID END DESC,
	    CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tftgr.TalkgroupDescription END,
	    CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tftgr.TalkgroupDescription END DESC,
	    CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tftgr.AffiliationCount END,
	    CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tftgr.AffiliationCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tftgr.DeniedCount END,
	    CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tftgr.DeniedCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tftgr.VoiceGrantCount END,
	    CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tftgr.VoiceGrantCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tftgr.EmergencyVoiceGrantCount END,
	    CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tftgr.EmergencyVoiceGrantCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tftgr.EncryptedVoiceGrantCount END,
	    CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tftgr.EncryptedVoiceGrantCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tftgr.DataCount END,
	    CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tftgr.DataCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tftgr.PrivateDataCount END,
	    CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tftgr.PrivateDataCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tftgr.AlertCount END,
	    CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tftgr.AlertCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tftgr.FirstSeen END,
	    CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tftgr.FirstSeen END DESC,
	    CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tftgr.LastSeen END,
	    CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tftgr.LastSeen END DESC,
        tftgr.TalkgroupID
    OFFSET ((@row - 1) * @pageSize) ROWS
    FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyTalkgroupsInsert]    Script Date: 2/12/2019 2:49:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[TowerFrequencyTalkgroupsInsert]
(
    @systemID INT,
    @towerID INT,
    @frequency NVARCHAR(50),
    @talkgroupID INT,
    @date DATETIME2(7),
    @affiliationCount INT,
    @deniedCount INT,
    @voiceGrantCount INT,
    @emergencyVoiceGrantCount INT,
    @encryptedVoiceGrantCount INT,
    @dataCount INT,
    @privateDataCount INT,
    @alertCount INT,
    @firstSeen DATETIME2(7),
    @lastSeen DATETIME2(7)
)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO TowerFrequencyTalkgroups
    (
        SystemID,
        TowerID,
        Frequency,
        TalkgroupID,
        [Date],
        AffiliationCount,
        DeniedCount,
        VoiceGrantCount,
        EmergencyVoiceGrantCount,
        EncryptedVoiceGrantCount,
        DataCount,
        PrivateDataCount,
        AlertCount,
        FirstSeen,
        LastSeen    
    )
    VALUES
    (
        @systemID,
        @towerID,
        @frequency,
        @talkgroupID,
        @date,
        @affiliationCount,
        @deniedCount,
        @voiceGrantCount,
        @emergencyVoiceGrantCount,
        @encryptedVoiceGrantCount,
        @dataCount,
        @privateDataCount,
        @alertCount,
        @firstSeen,
        @lastSeen
    );

    SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyTalkgroupsUpdate]    Script Date: 2/12/2019 2:49:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[TowerFrequencyTalkgroupsUpdate]
(
    @id INT,
    @systemID INT,
    @towerID INT,
    @frequency NVARCHAR(50),
    @talkgroupID INT,
    @date DATETIME2(7),
    @affiliationCount INT,
    @deniedCount INT,
    @voiceGrantCount INT,
    @emergencyVoiceGrantCount INT,
    @encryptedVoiceGrantCount INT,
    @dataCount INT,
    @privateDataCount INT,
    @alertCount INT,
    @firstSeen DATETIME2(7),
    @lastSeen DATETIME2(7)
)
AS
BEGIN
    SET NOCOUNT ON

    UPDATE TowerFrequencyTalkgroups
    SET SystemID = @systemID,
        TowerID = @towerID,
        Frequency = @frequency,
        TalkgroupID = @talkgroupID,
        [Date] = @date,
        AffiliationCount = @affiliationCount,
        DeniedCount = @deniedCount,
        VoiceGrantCount = @voiceGrantCount,
        EmergencyVoiceGrantCount = @emergencyVoiceGrantCount,
        EncryptedVoiceGrantCount = @encryptedVoiceGrantCount,
        DataCount = @dataCount,
        PrivateDataCount = @privateDataCount,
        AlertCount = @alertCount,
        FirstSeen = @firstSeen,
        LastSeen = @lastSeen,
        LastModified = GETDATE()
    WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyUsageDelete]    Script Date: 2/12/2019 2:49:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequencyUsageDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TowerFrequencyUsage
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyUsageDeleteForSystem]    Script Date: 2/12/2019 2:49:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequencyUsageDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TowerFrequencyUsage
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyUsageGet]    Script Date: 2/12/2019 2:49:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequencyUsageGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tfu.ID,
		tfu.SystemID,
		tfu.TowerID,
		tfu.Channel,
		tfu.Frequency,
		tfu.TalkgroupID,
		tfu.RadioID,
		tfu.[Date],
		tfu.AffiliationCount,
		tfu.DeniedCount,
		tfu.VoiceGrantCount,
		tfu.EmergencyVoiceGrantCount,
		tfu.EncryptedVoiceGrantCount,
		tfu.DataCount,
		tfu.PrivateDataCount,
		tfu.CWIDCount,
		tfu.AlertCount,
		tfu.FirstSeen,
		tfu.LastSeen,
		tfu.LastModified
	FROM dbo.TowerFrequencyUsage tfu
	WHERE (tfu.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyUsageGetForFrequency]    Script Date: 2/12/2019 2:49:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequencyUsageGetForFrequency]
(
	@systemID INT,
	@towerNumber INT,
	@frequency NVARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tfu.ID,
		tfu.SystemID,
		tfu.TowerID,
		tfu.Channel,
		tfu.Frequency,
		tfu.TalkgroupID,
		tfu.RadioID,
		tfu.[Date],
		tfu.AffiliationCount,
		tfu.DeniedCount,
		tfu.VoiceGrantCount,
		tfu.EmergencyVoiceGrantCount,
		tfu.EncryptedVoiceGrantCount,
		tfu.DataCount,
		tfu.PrivateDataCount,
		tfu.CWIDCount,
		tfu.AlertCount,
		tfu.FirstSeen,
		tfu.LastSeen,
		tfu.LastModified
	FROM dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tfu.TowerID)
	WHERE ((tfu.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
		AND (tfu.Frequency = @frequency));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyUsageGetFrequenciesForTower]    Script Date: 2/12/2019 2:49:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequencyUsageGetFrequenciesForTower]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tfu.Frequency,
		tfu.Channel,
		tg.TalkgroupID,
		r.RadioID,
		tfu.[Date],
		SUM(ISNULL(tfu.VoiceGrantCount, 0) +
			ISNULL(tfu.EmergencyVoiceGrantCount, 0) +
			ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS HitCount,
		SUM(ISNULL(tfu.AffiliationCount, 0)) AS AffiliationCount,
		SUM(ISNULL(tfu.DeniedCount, 0)) AS DeniedCount,
		SUM(ISNULL(tfu.VoiceGrantCount, 0)) AS VoiceGrantCount,
		SUM(ISNULL(tfu.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		SUM(ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
		SUM(ISNULL(tfu.DataCount, 0) +
			ISNULL(tfu.PrivateDataCount, 0)) AS DataCount,
		SUM(ISNULL(tfu.PrivateDataCount, 0)) AS PrivateDataCount,
		SUM(ISNULL(tfu.CWIDCount, 0)) AS CWIDCount,
		SUM(ISNULL(tfu.AlertCount, 0)) AS AlertCount,
		MIN(ISNULL(tfu.FirstSeen, GETDATE())) AS FirstSeen,
		MAX(ISNULL(tfu.LastSeen, GETDATE())) AS LastSeen
	FROM dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tfu.TowerID)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = tfu.TalkgroupID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = tfu.RadioID)
	WHERE ((tfu.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber))
	GROUP BY tfu.Frequency,
		tfu.Channel,
		tg.TalkgroupID,
		r.RadioID,
		tfu.[Date]
	ORDER BY tfu.Frequency;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyUsageGetFrequenciesForTowerForDate]    Script Date: 2/12/2019 2:49:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerFrequencyUsageGetFrequenciesForTowerForDate]
(
	@systemID INT,
	@towerNumber INT,
	@date DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tfu.Frequency,
		tfu.Channel,
		tg.TalkgroupID,
		r.RadioID,
		tfu.[Date],
		SUM(ISNULL(tfu.VoiceGrantCount, 0) +
			ISNULL(tfu.EmergencyVoiceGrantCount, 0) +
			ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS HitCount,
		SUM(ISNULL(tfu.AffiliationCount, 0)) AS AffiliationCount,
		SUM(ISNULL(tfu.DeniedCount, 0)) AS DeniedCount,
		SUM(ISNULL(tfu.VoiceGrantCount, 0)) AS VoiceGrantCount,
		SUM(ISNULL(tfu.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		SUM(ISNULL(tfu.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
		SUM(ISNULL(tfu.DataCount, 0) +
			ISNULL(tfu.PrivateDataCount, 0)) AS DataCount,
		SUM(ISNULL(tfu.PrivateDataCount, 0)) AS PrivateDataCount,
		SUM(ISNULL(tfu.CWIDCount, 0)) AS CWIDCount,
		SUM(ISNULL(tfu.AlertCount, 0)) AS AlertCount,
		MIN(ISNULL(tfu.FirstSeen, GETDATE())) AS FirstSeen,
		MAX(ISNULL(tfu.LastSeen, GETDATE())) AS LastSeen
	FROM dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tfu.TowerID)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = tfu.TalkgroupID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = tfu.RadioID)
	WHERE ((tfu.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
		AND ((tfu.[Date] >= DATEADD(dd, -1, @date))
			AND (tfu.[Date] <= DATEADD(dd, 1, @date))))
	GROUP BY tfu.Frequency,
		tfu.Channel,
		tg.TalkgroupID,
		r.RadioID,
		tfu.[Date]
	ORDER BY tfu.Frequency;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyUsageInsert]    Script Date: 2/12/2019 2:49:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequencyUsageInsert]
(
	@systemID INT,
	@towerID INT,
	@channel NVARCHAR(50),
	@frequency NVARCHAR(50),
	@talkgroupID INT,
	@radioID INT,
	@date DATETIME2(7),
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@cwidCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @towerIDKey INT;

	SELECT @towerIDKey = t.ID
	FROM dbo.Towers t
	WHERE ((t.SystemID = @systemID)
		AND (t.TowerNumber = @towerID));

	INSERT INTO dbo.TowerFrequencyUsage
	(
		SystemID,
		TowerID,
		Channel,
		Frequency,
		TalkgroupID,
		RadioID,
		[Date],
		AffiliationCount,
		DeniedCount,
		VoiceGrantCount,
		EmergencyVoiceGrantCount,
		EncryptedVoiceGrantCount,
		DataCount,
		PrivateDataCount,
		CWIDCount,
		AlertCount,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
		@systemID,
		@towerIDKey,
		@channel,
		@frequency,
		@talkgroupID,
		@radioID,
		@date,
		@affiliationCount,
		@deniedCount,
		@voiceGrantCount,
		@emergencyVoiceGrantCount,
		@encryptedVoiceGrantCount,
		@dataCount,
		@privateDataCount,
		@cwidCount,
		@alertCount,
		@firstSeen,
		@lastSeen
	);

	SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerFrequencyUsageUpdate]    Script Date: 2/12/2019 2:49:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerFrequencyUsageUpdate]
(
	@id INT,
	@systemID INT,
	@towerID INT,
	@channel NVARCHAR(50),
	@frequency NVARCHAR(50),
	@talkgroupID INT,
	@radioID INT,
	@date DATETIME2(7),
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@cwidCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.TowerFrequencyUsage
	SET SystemID = @systemID,
		TowerID = @towerID,
		Channel = @channel,
		Frequency = @frequency,
		TalkgroupID = @talkgroupID,
		RadioID = @radioID,
		[Date] = @date,
		AffiliationCount = @affiliationCount,
		DeniedCount = @deniedCount,
		VoiceGrantCount = @voiceGrantCount,
		EmergencyVoiceGrantCount = @emergencyVoiceGrantCount,
		EncryptedVoiceGrantCount = @encryptedVoiceGrantCount,
		DataCount = @dataCount,
		PrivateDataCount = @privateDataCount,
		CWIDCount = @cwidCount,
		AlertCount = @alertCount,
		FirstSeen = @firstSeen,
		LastSeen = @lastSeen
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerNeighborsDelete]    Script Date: 2/12/2019 2:49:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerNeighborsDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TowerNeighbors
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerNeighborsDeleteForSystem]    Script Date: 2/12/2019 2:49:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerNeighborsDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TowerNeighbors
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerNeighborsDeleteForTower]    Script Date: 2/12/2019 2:49:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerNeighborsDeleteForTower]
(
    @systemID INT,
    @towerNumber INT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @towerID INT;

    SELECT @towerID = t.ID
    FROM dbo.Towers t
    WHERE ((t.SystemID = @systemID)
        AND (t.TowerNumber = @towerNumber));

    DELETE
    FROM dbo.TowerNeighbors
    WHERE (TowerID = @towerID);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerNeighborsGet]    Script Date: 2/12/2019 2:49:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerNeighborsGet]
(
	@id INT
)
AS
BEGIN
	SELECT tn.ID,
		tn.SystemID,
		tn.TowerID,
		tn.NeighborSystemID,
		tn.NeighborTowerID,
		tn.NeighborTowerNumberHex,
		tn.NeighborChannel,
		tn.NeighborFrequency,
		tn.NeighborTowerName,
		tn.FirstSeen,
		tn.LastSeen,
		tn.LastModified
	FROM dbo.TowerNeighbors tn
	WHERE (tn.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerNeighborsGetForSystemTower]    Script Date: 2/12/2019 2:49:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerNeighborsGetForSystemTower]
(
	@systemID INT,
	@towerID INT,
	@neighborSystemID INT,
	@neighborTowerNumber INT
)
AS
BEGIN
	SELECT tn.ID,
		tn.SystemID,
		tn.TowerID,
		tn.NeighborSystemID,
		tn.NeighborTowerID,
		tn.NeighborTowerNumberHex,
		tn.NeighborChannel,
		tn.NeighborFrequency,
		tn.NeighborTowerName,
		tn.FirstSeen,
		tn.LastSeen,
		tn.LastModified
	FROM dbo.TowerNeighbors tn WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tn.NeighborTowerID)
	WHERE ((tn.SystemID = @systemID)
		AND (tn.TowerID = @towerID)
		AND (tn.NeighborSystemID = @neighborSystemID)
		AND (t.TowerNumber = @neighborTowerNumber));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerNeighborsGetForSystemTowerNumber]    Script Date: 2/12/2019 2:49:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerNeighborsGetForSystemTowerNumber]
(
	@systemID INT,
	@towerNumber INT,
	@neighborSystemID INT,
	@neighborTowerNumber INT
)
AS
BEGIN
	SELECT tn.ID,
		tn.SystemID,
		tn.TowerID,
		tn.NeighborSystemID,
		tn.NeighborTowerID,
		tn.NeighborTowerNumberHex,
		tn.NeighborChannel,
		tn.NeighborFrequency,
		tn.NeighborTowerName,
		tn.FirstSeen,
		tn.LastSeen,
		tn.LastModified
	FROM dbo.TowerNeighbors tn WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tn.TowerID)
	INNER JOIN dbo.Towers nt WITH (NOLOCK)
		ON (nt.ID = tn.NeighborTowerID)
	WHERE ((tn.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
		AND (tn.NeighborSystemID = @neighborSystemID)
		AND (nt.TowerNumber = @neighborTowerNumber));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerNeighborsGetNeighborsForSystemTowerFiltersWithPaging]    Script Date: 2/12/2019 2:49:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[TowerNeighborsGetNeighborsForSystemTowerFiltersWithPaging]
(
    @systemID NVARCHAR(50),
    @towerNumber INT,
    @searchText NVARCHAR(200) = NULL,
    @towerNumberFrom INT = NULL,
    @towerNumberTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @FIELD_NEIGHBOR_TOWER_NUMBER NVARCHAR(50) = N'NeighborTowerNumber',
        @FIELD_NEIGHBOR_TOWER_NAME NVARCHAR(50) = N'NeighborTowerName',
        @FIELD_NEIGHBOR_CONTROL_CHANNEL NVARCHAR(50) = N'NeighborControlChannel',
        @FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
        @FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH TowerNeighborResults (NeighborTowerNumber, NeighborTowerName, NeighborFrequency, FirstSeen, LastSeen)
    AS
    (
	    SELECT tnt.TowerNumber AS NeighborTowerNumber,
		    tnt.[Description] AS NeighborTowerName,
            tn.NeighborFrequency,
		    tn.FirstSeen,
		    tn.LastSeen
	    FROM dbo.TowerNeighbors tn WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = tn.SystemID)
	    INNER JOIN dbo.Towers t WITH (NOLOCK)
		    ON (t.ID = tn.TowerID)
	    INNER JOIN dbo.Towers tnt WITH (NOLOCK)
		    ON (tnt.ID = tn.NeighborTowerID)
	    WHERE ((s.SystemID = @systemID)
		    AND (t.TowerNumber = @towerNumber)
            AND ((@searchText IS NULL)
                OR (tnt.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@towerNumberFrom IS NULL)
                OR (tnt.TowerNumber >= @towerNumberFrom))
            AND ((@towerNumberTo IS NULL)
                OR (tnt.TowerNumber <= @towerNumberTo))
            AND ((@firstSeenFrom IS NULL)
                OR (tn.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (tn.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (tn.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (tn.LastSeen <= @lastSeenTo)))
    )
    SELECT tnr.NeighborTowerNumber, 
        tnr.NeighborTowerName, 
        tnr.NeighborFrequency, 
        tnr.FirstSeen, 
        tnr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TowerNeighborResults tnr
    ORDER BY
		CASE WHEN ((@sortField = @FIELD_NEIGHBOR_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tnr.NeighborTowerNumber END,
		CASE WHEN ((@sortField = @FIELD_NEIGHBOR_TOWER_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tnr.NeighborTowerNumber END DESC,
		CASE WHEN ((@sortField = @FIELD_NEIGHBOR_TOWER_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tnr.NeighborTowerName END,
		CASE WHEN ((@sortField = @FIELD_NEIGHBOR_TOWER_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tnr.NeighborTowerName END DESC,
		CASE WHEN ((@sortField = @FIELD_NEIGHBOR_CONTROL_CHANNEL) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tnr.NeighborFrequency END,
		CASE WHEN ((@sortField = @FIELD_NEIGHBOR_CONTROL_CHANNEL) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tnr.NeighborFrequency END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tnr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tnr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tnr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tnr.LastSeen END DESC,
        tnr.NeighborTowerNumber
    OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerNeighborsGetNeighborsForTower]    Script Date: 2/12/2019 2:49:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerNeighborsGetNeighborsForTower]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SELECT tnt.TowerNumber AS NeighborTowerNumber,
		tnt.[Description] AS NeighborTowerName,
        tn.NeighborFrequency,
		tn.FirstSeen,
		tn.LastSeen,
        COUNT(1) OVER() AS RecordCount
	FROM dbo.TowerNeighbors tn WITH (NOLOCK)
    INNER JOIN dbo.Systems s WITH (NOLOCK)
        ON (s.ID = tn.SystemID)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tn.TowerID)
	INNER JOIN dbo.Towers tnt WITH (NOLOCK)
		ON (tnt.ID = tn.NeighborTowerID)
	WHERE ((s.ID = @systemID)
		AND (t.TowerNumber = @towerNumber));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerNeighborsInsert]    Script Date: 2/12/2019 2:49:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerNeighborsInsert]
(
	@systemID INT,
	@towerID INT,
	@neighborSystemID INT,
	@neighborTowerID INT,
	@neighborTowerNumberHex NVARCHAR(50),
	@neighborChannel NVARCHAR(50),
	@neighborFrequency NVARCHAR(50),
	@neighborTowerName NVARCHAR(50),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @towerIDKey INT,
		@neighborTowerIDKey INT;

	SELECT @towerIDKey = t.ID
	FROM dbo.Towers t
	WHERE ((t.SystemID= @systemID)
		AND (t.TowerNumber = @towerID));

	SELECT @neighborTowerIDKey = t.ID
	FROM dbo.Towers t
	WHERE ((t.SystemID = @systemID)
		AND (t.TowerNumber = @neighborTowerID));

	INSERT INTO dbo.TowerNeighbors
	(
		SystemID,
		TowerID,
		NeighborSystemID,
		NeighborTowerID,
		NeighborTowerNumberHex,
		NeighborChannel,
		NeighborFrequency,
		NeighborTowerName,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
		@systemID,
		@towerIDKey,
		@neighborSystemID,
		@neighborTowerIDKey,
		@neighborTowerNumberHex,
		@neighborChannel,
		@neighborFrequency,
		@neighborTowerName,
		@firstSeen,
		@lastSeen
	);

	SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerNeighborsUpdate]    Script Date: 2/12/2019 2:49:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerNeighborsUpdate]
(
	@id INT,
	@neighborTowerNumberHex NVARCHAR(50),
	@neighborChannel NVARCHAR(50),
	@neighborFrequency NVARCHAR(50),
	@neighborTowerName NVARCHAR(50),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.TowerNeighbors
	SET NeighborTowerNumberHex = @neighborTowerNumberHex,
		NeighborChannel = @neighborChannel,
		NeighborFrequency = @neighborFrequency,
		NeighborTowerName = @neighborTowerName,
		FirstSeen = @firstSeen,
		LastSeen = @lastSeen,
		LastModified = GETDATE()
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosDelete]    Script Date: 2/12/2019 2:49:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerRadiosDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TowerRadios
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosDeleteForSystem]    Script Date: 2/12/2019 2:49:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerRadiosDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TowerRadios
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosGet]    Script Date: 2/12/2019 2:49:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerRadiosGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tr.ID,
		tr.SystemID,
		tr.TowerID,
		tr.RadioID,
		tr.[Date],
		tr.AffiliationCount,
		tr.DeniedCount,
		tr.VoiceGrantCount,
		tr.EmergencyVoiceGrantCount,
		tr.EncryptedVoiceGrantCount,
		tr.DataCount,
		tr.PrivateDataCount,
		tr.AlertCount,
		tr.FirstSeen,
		tr.LastSeen,
		tr.LastModified
	FROM dbo.TowerRadios tr
	WHERE (tr.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosGetDateListForTowerRadio]    Script Date: 2/12/2019 2:49:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[TowerRadiosGetDateListForTowerRadio]
(
    @systemID NVARCHAR(50),
    @radioID INT,
    @towerNumber INT,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL
)
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @systemIDKey INT,
        @towerIDKey INT,
        @radioIDKey INT;

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    SELECT @towerIDKey = t.ID
    FROM dbo.Towers t
    WHERE((t.SystemID = @systemIDKey)
        AND (t.TowerNumber = @towerNumber));

    SELECT @radioIDKey = r.ID
    FROM dbo.Radios r
    WHERE ((r.SystemID = @systemIDKey)
        AND (r.RadioID = @radioID));

    SELECT tr.[Date]
    FROM dbo.TowerRadios tr
    WHERE ((tr.TowerID = @towerIDKey)
        AND (tr.RadioID = @radioIDKey)
        AND ((@dateFrom IS NULL)
            OR (tr.[Date] >= @dateFrom))
        AND ((@dateTo IS NULL)
            OR (tr.[Date] <= @dateTo)))
    GROUP BY tr.[Date];
END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosGetForTower]    Script Date: 2/12/2019 2:49:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerRadiosGetForTower]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tr.ID,
		tr.SystemID,
		t.TowerNumber AS TowerID,
		r.RadioID AS RadioID,
		tr.[Date],
		tr.AffiliationCount,
		tr.DeniedCount,
		tr.VoiceGrantCount,
		tr.EmergencyVoiceGrantCount,
		tr.EncryptedVoiceGrantCount,
		tr.DataCount,
		tr.PrivateDataCount,
		tr.AlertCount,
		tr.FirstSeen,
		tr.LastSeen,
		tr.LastModified
	FROM dbo.TowerRadios tr WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tr.TowerID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = tr.RadioID)
	WHERE ((tr.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber))
    ORDER BY tr.[Date],
        t.TowerNumber,
        r.RadioID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosGetForTowerDateRange]    Script Date: 2/12/2019 2:49:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerRadiosGetForTowerDateRange]
(
	@systemID INT,
	@towerNumber INT,
    @date DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tr.ID,
		tr.SystemID,
		tr.TowerID,
		r.RadioID AS RadioID,
		tr.[Date],
		tr.AffiliationCount,
		tr.DeniedCount,
		tr.VoiceGrantCount,
		tr.EmergencyVoiceGrantCount,
		tr.EncryptedVoiceGrantCount,
		tr.DataCount,
		tr.PrivateDataCount,
		tr.AlertCount,
		tr.FirstSeen,
		tr.LastSeen,
		tr.LastModified
	FROM dbo.TowerRadios tr WITH (NOLOCK)
    INNER JOIN dbo.Towers t WITH (NOLOCK)
        ON (t.ID = tr.TowerID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = tr.RadioID)
	WHERE ((tr.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
        AND (tr.[Date] >= DATEADD(dd, -1, @date))
        AND (tr.[Date] <= DATEADD(dd, 1, @date)))
    ORDER BY r.RadioID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosGetRadiosForTower]    Script Date: 2/12/2019 2:49:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerRadiosGetRadiosForTower]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		r.RadioID,
		r.[Description] AS RadioDescription,
		SUM(tr.AffiliationCount) AS AffiliationCount,
		SUM(tr.DeniedCount) AS DeniedCount,
		SUM(tr.VoiceGrantCount) AS VoiceGrantCount,
		SUM(tr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		SUM(tr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		SUM(tr.DataCount) AS DataCount,
		SUM(tr.PrivateDataCount) AS PrivateDataCount,
		SUM(tr.AlertCount) AS AlertCount,
		MIN(tr.FirstSeen) AS FirstSeen,
		MAX(tr.LastSeen) AS LastSeen,
        COUNT(1) OVER() AS RecordCount
	FROM dbo.TowerRadios tr WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tr.TowerID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = tr.RadioID)
	WHERE ((tr.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber))
	GROUP BY t.TowerNumber,
		t.[Description],
		r.RadioID,
		r.[Description];
END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosGetRadiosForTowerByDate]    Script Date: 2/12/2019 2:49:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerRadiosGetRadiosForTowerByDate]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		r.RadioID,
		r.[Description] AS RadioDescription,
		tr.[Date],
		SUM(tr.AffiliationCount) AS AffiliationCount,
		SUM(tr.DeniedCount) AS DeniedCount,
		SUM(tr.VoiceGrantCount) AS VoiceGrantCount,
		SUM(tr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		SUM(tr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		SUM(tr.DataCount) AS DataCount,
		SUM(tr.PrivateDataCount) AS PrivateDataCount,
		SUM(tr.AlertCount) AS AlertCount,
		MIN(tr.FirstSeen) AS FirstSeen,
		MAX(tr.LastSeen) AS LastSeen
	FROM dbo.TowerRadios tr WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tr.TowerID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = tr.RadioID)
	WHERE ((tr.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber))
	GROUP BY t.TowerNumber,
		t.[Description],
		r.RadioID,
		r.[Description],
		tr.[Date]
	ORDER BY t.TowerNumber,
		r.RadioID,
		tr.[Date] DESC;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosGetRadiosForTowerCount]    Script Date: 2/12/2019 2:49:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerRadiosGetRadiosForTowerCount]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*)
	FROM dbo.TowerRadios tr WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tr.TowerID)
	WHERE ((tr.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber))
	GROUP BY tr.SystemID,
		tr.TowerID;

END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosGetRadiosForTowerFiltersWithPaging]    Script Date: 2/12/2019 2:49:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerRadiosGetRadiosForTowerFiltersWithPaging]
(
	@systemID NVARCHAR(50),
	@towerNumber INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @radioIDFrom INT = NULL,
    @radioIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @FIELD_TOWER_NUMBER NVARCHAR(50) = N'TowerNumber',
        @FIELD_TOWER_NAME NVARCHAR(50) = N'TowerName',
        @FIELD_RADIO_ID NVARCHAR(50) = N'RadioID',
        @FIELD_RADIO_NAME NVARCHAR(50) = N'RadioName',
        @FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
        @FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
        @FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
        @FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
        @FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PRIVATE_DATA_COUNT NVARCHAR(50) = N'PrivateDataCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
        @FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
        @FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
        @DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
        @DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @systemIDKey INT,
        @towerIDKey INT;

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    SELECT @towerIDKey = t.ID
    FROM dbo.Towers t
    WHERE ((t.SystemID = @systemIDKey)
        AND (t.TowerNumber = @towerNumber));

    WITH TowerResults (TowerNumber, TowerDescription, RadioID, RadioDescription, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, PrivateDataCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
	    SELECT t.TowerNumber,
		    t.[Description] AS TowerDescription,
		    r.RadioID,
		    r.[Description] AS RadioDescription,
		    SUM(tr.AffiliationCount) AS AffiliationCount,
		    SUM(tr.DeniedCount) AS DeniedCount,
		    SUM(tr.VoiceGrantCount) AS VoiceGrantCount,
		    SUM(tr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		    SUM(tr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		    SUM(tr.DataCount) AS DataCount,
		    SUM(tr.PrivateDataCount) AS PrivateDataCount,
		    SUM(tr.AlertCount) AS AlertCount,
		    MIN(tr.FirstSeen) AS FirstSeen,
		    MAX(tr.LastSeen) AS LastSeen
	    FROM dbo.TowerRadios tr WITH (NOLOCK)
	    INNER JOIN dbo.Towers t WITH (NOLOCK)
		    ON (t.ID = tr.TowerID)
	    INNER JOIN dbo.Radios r WITH (NOLOCK)
		    ON (r.ID = tr.RadioID)
	    WHERE ((t.SystemID = @systemIDKey)
		    AND (t.ID = @towerIDKey)
            AND ((@dateFrom IS NULL)
                OR (tr.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (tr.[Date] <= @dateTo))
            AND ((@radioIDFrom IS NULL)
                OR (r.RadioID >= @radioIDFrom))
            AND ((@radioIDTo IS NULL)
                OR (r.RadioID <= @radioIDTo))
		    AND ((@searchText IS NULL)
                OR (r.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (tr.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (tr.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (tr.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (tr.LastSeen <= @lastSeenTo)))
	    GROUP BY t.TowerNumber,
		    t.[Description],
		    r.RadioID,
		    r.[Description]
    )
    SELECT TowerNumber, 
        TowerDescription, 
        RadioID, 
        RadioDescription, 
        AffiliationCount, 
        DeniedCount, 
        VoiceGrantCount, 
        EmergencyVoiceGrantCount, 
        EncryptedVoiceGrantCount, 
        DataCount, 
        PrivateDataCount, 
        AlertCount, 
        FirstSeen, 
        LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TowerResults tr
	ORDER BY
    	CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.TowerNumber END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.TowerNumber END DESC,
    	CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.TowerDescription END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.TowerDescription END DESC,
    	CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.RadioID END,
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.RadioID END DESC,
    	CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.RadioDescription END,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.RadioDescription END DESC,
    	CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.AffiliationCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.DeniedCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.VoiceGrantCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.EmergencyVoiceGrantCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.EncryptedVoiceGrantCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.DataCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.PrivateDataCount END,
		CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.PrivateDataCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.AlertCount END,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.AlertCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.FirstSeen END DESC,
    	CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.LastSeen END DESC,
        tr.TowerNumber
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);

END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosGetRadiosForTowerWithPaging]    Script Date: 2/12/2019 2:49:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerRadiosGetRadiosForTowerWithPaging]
(
	@systemID INT,
	@towerNumber INT,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @FIELD_TOWER_NUMBER NVARCHAR(50) = N'TowerNumber',
        @FIELD_TOWER_NAME NVARCHAR(50) = N'TowerName',
        @FIELD_RADIO_ID NVARCHAR(50) = N'RadioID',
        @FIELD_RADIO_NAME NVARCHAR(50) = N'RadioName',
        @FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
        @FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
        @FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
        @FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
        @FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PRIVATE_DATA_COUNT NVARCHAR(50) = N'PrivateDataCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
        @FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
        @FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
        @DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
        @DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH TowerResults (TowerNumber, TowerDescription, RadioID, RadioDescription, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, PrivateDataCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
	    SELECT t.TowerNumber,
		    t.[Description] AS TowerDescription,
		    r.RadioID,
		    r.[Description] AS RadioDescription,
		    SUM(tr.AffiliationCount) AS AffiliationCount,
		    SUM(tr.DeniedCount) AS DeniedCount,
		    SUM(tr.VoiceGrantCount) AS VoiceGrantCount,
		    SUM(tr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		    SUM(tr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		    SUM(tr.DataCount) AS DataCount,
		    SUM(tr.PrivateDataCount) AS PrivateDataCount,
		    SUM(tr.AlertCount) AS AlertCount,
		    MIN(tr.FirstSeen) AS FirstSeen,
		    MAX(tr.LastSeen) AS LastSeen
	    FROM dbo.TowerRadios tr WITH (NOLOCK)
	    INNER JOIN dbo.Towers t WITH (NOLOCK)
		    ON (t.ID = tr.TowerID)
	    INNER JOIN dbo.Radios r WITH (NOLOCK)
		    ON (r.ID = tr.RadioID)
	    WHERE ((tr.SystemID = @systemID)
		    AND (t.TowerNumber = @towerNumber))
	    GROUP BY t.TowerNumber,
		    t.[Description],
		    r.RadioID,
		    r.[Description]
    )
    SELECT TowerNumber, 
        TowerDescription, 
        RadioID, 
        RadioDescription, 
        AffiliationCount, 
        DeniedCount, 
        VoiceGrantCount, 
        EmergencyVoiceGrantCount, 
        EncryptedVoiceGrantCount, 
        DataCount, 
        PrivateDataCount, 
        AlertCount, 
        FirstSeen, 
        LastSeen
    FROM TowerResults tr
	ORDER BY
    	CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.TowerNumber END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.TowerNumber END DESC,
    	CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.TowerDescription END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.TowerDescription END DESC,
    	CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.RadioID END,
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.RadioID END DESC,
    	CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.RadioDescription END,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.RadioDescription END DESC,
    	CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.AffiliationCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.DeniedCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.VoiceGrantCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.EmergencyVoiceGrantCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.EncryptedVoiceGrantCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.DataCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.PrivateDataCount END,
		CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.PrivateDataCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.AlertCount END,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.AlertCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.FirstSeen END DESC,
    	CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.LastSeen END DESC,
        tr.TowerNumber
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY;

END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosGetTowerListForRadio]    Script Date: 2/12/2019 2:49:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[TowerRadiosGetTowerListForRadio]
(
    @systemID NVARCHAR(50),
    @radioID INT,
    @dateFrom DATETIME2(7),
    @dateTo DATETIME2(7)
)
AS
BEGIN
    SET NOCOUNT ON

    SELECT t.TowerNumber,
        t.[Description] AS TowerDescription
    FROM dbo.TowerRadios tr WITH (NOLOCK)
    INNER JOIN dbo.Systems s WITH (NOLOCK)
        ON (s.ID = tr.SystemID)
    INNER JOIN dbo.Towers t WITH (NOLOCK)
        ON (t.ID = tr.TowerID)
    INNER JOIN dbo.Radios r WITH (NOLOCK)
        ON (r.ID = tr.RadioID)
    WHERE ((s.SystemID = @systemID)
        AND (r.RadioID = @radioID)
        AND ((@dateFrom IS NULL)
            OR (tr.[Date] >= @dateFrom))
        AND ((@dateTo IS NULL)
            OR (tr.[Date] <= @dateTo)))
    GROUP BY t.TowerNumber,
        t.[Description];
END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosGetTowersForRadio]    Script Date: 2/12/2019 2:49:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerRadiosGetTowersForRadio]
(
    @systemID INT,
	@radioID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		r.RadioID,
		r.[Description] AS RadioDescription,
		SUM(tr.AffiliationCount) AS AffiliationCount,
		SUM(tr.DeniedCount) AS DeniedCount,
		SUM(tr.VoiceGrantCount) AS VoiceGrantCount,
		SUM(tr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		SUM(tr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		SUM(tr.DataCount) AS DataCount,
		SUM(tr.PrivateDataCount) AS PrivateDataCount,
		SUM(tr.AlertCount) AS AlertCount,
		MIN(tr.FirstSeen) AS FirstSeen,
		MAX(tr.LastSeen) AS LastSeen,
        COUNT(1) OVER() AS RecordCount
	FROM dbo.TowerRadios tr WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tr.TowerID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = tr.RadioID)
	WHERE ((r.SystemID = @systemID)
        AND (r.RadioID = @radioID))
	GROUP BY t.TowerNumber,
		t.[Description],
		r.RadioID,
		r.[Description];
END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosGetTowersForRadioCount]    Script Date: 2/12/2019 2:49:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[TowerRadiosGetTowersForRadioCount]
(
	@systemID INT,
	@radioID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	WITH TowerRadiosTowers
	AS
	(
		SELECT tr.TowerID,
			tr.RadioID
		FROM dbo.TowerRadios tr WITH (NOLOCK)
		INNER JOIN dbo.Radios r WITH (NOLOCK)
			ON (r.ID = tr.RadioID)
		WHERE ((tr.SystemID = @systemID)
			AND (r.RadioID = @radioID))
		GROUP BY tr.TowerID,
			tr.RadioID
	)
	SELECT COUNT(*)
	FROM TowerRadiosTowers;

END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosGetTowersForRadioFiltersWithPaging]    Script Date: 2/12/2019 2:49:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerRadiosGetTowersForRadioFiltersWithPaging]
(
	@systemID NVARCHAR(50),
	@radioID INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @towerNumberFrom INT = NULL,
    @towerNumberTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @FIELD_TOWER_NUMBER NVARCHAR(50) = N'TowerNumber',
        @FIELD_TOWER_NAME NVARCHAR(50) = N'TowerName',
        @FIELD_RADIO_ID NVARCHAR(50) = N'RadioID',
        @FIELD_RADIO_NAME NVARCHAR(50) = N'RadioName',
        @FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
        @FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
        @FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
        @FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
        @FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PRIVATE_DATA_COUNT NVARCHAR(50) = N'PrivateDataCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
        @FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
        @FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
        @DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
        @DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @systemIDKey INT;

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    WITH TowerResults (TowerNumber, TowerDescription, RadioID, RadioDescription, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, PrivateDataCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
	    SELECT t.TowerNumber,
		    t.[Description] AS TowerDescription,
		    r.RadioID,
		    r.[Description] AS RadioDescription,
		    SUM(tr.AffiliationCount) AS AffiliationCount,
		    SUM(tr.DeniedCount) AS DeniedCount,
		    SUM(tr.VoiceGrantCount) AS VoiceGrantCount,
		    SUM(tr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		    SUM(tr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		    SUM(tr.DataCount) AS DataCount,
		    SUM(tr.PrivateDataCount) AS PrivateDataCount,
		    SUM(tr.AlertCount) AS AlertCount,
		    MIN(tr.FirstSeen) AS FirstSeen,
		    MAX(tr.LastSeen) AS LastSeen
	    FROM dbo.TowerRadios tr WITH (NOLOCK)
	    INNER JOIN dbo.Towers t WITH (NOLOCK)
		    ON (t.ID = tr.TowerID)
	    INNER JOIN dbo.Radios r WITH (NOLOCK)
		    ON (r.ID = tr.RadioID)
	    WHERE ((r.SystemID = @systemIDKey)
		    AND (r.RadioID = @radioID)
            AND ((@dateFrom IS NULL)
                OR (tr.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (tr.[Date] <= @dateTo))
            AND ((@towerNumberFrom IS NULL)
                OR (t.TowerNumber >= @towerNumberFrom))
            AND ((@towerNumberTo IS NULL)
                OR (t.TowerNumber <= @towerNumberTo))
		    AND ((@searchText IS NULL)
                OR (t.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (tr.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (tr.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (tr.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (tr.LastSeen <= @lastSeenTo)))
	    GROUP BY t.TowerNumber,
		    t.[Description],
		    r.RadioID,
		    r.[Description]
    )
    SELECT TowerNumber, 
        TowerDescription, 
        RadioID, 
        RadioDescription, 
        AffiliationCount, 
        DeniedCount, 
        VoiceGrantCount, 
        EmergencyVoiceGrantCount, 
        EncryptedVoiceGrantCount, 
        DataCount, 
        PrivateDataCount, 
        AlertCount, 
        FirstSeen, 
        LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TowerResults tr
	ORDER BY
    	CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.TowerNumber END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.TowerNumber END DESC,
    	CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.TowerDescription END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.TowerDescription END DESC,
    	CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.RadioID END,
		CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.RadioID END DESC,
    	CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.RadioDescription END,
		CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.RadioDescription END DESC,
    	CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.AffiliationCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.DeniedCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.VoiceGrantCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.EmergencyVoiceGrantCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.EncryptedVoiceGrantCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.DataCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.PrivateDataCount END,
		CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.PrivateDataCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.AlertCount END,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.AlertCount END DESC,
    	CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.FirstSeen END DESC,
    	CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.LastSeen END DESC,
        tr.TowerNumber
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);

END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosInsert]    Script Date: 2/12/2019 2:49:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerRadiosInsert]
(
	@systemID INT,
	@towerNumber INT,
	@radioID INT,
	@date DATETIME2(7),
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TowerRadios
	(
		SystemID,
		TowerID,
		RadioID,
		[Date],
		AffiliationCount,
		DeniedCount,
		VoiceGrantCount,
		EmergencyVoiceGrantCount,
		EncryptedVoiceGrantCount,
		DataCount,
		PrivateDataCount,
		AlertCount,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
		@systemID,
		@towerNumber,
		@radioID,
		@date,
		@affiliationCount,
		@deniedCount,
		@voiceGrantCount,
		@emergencyVoiceGrantCount,
		@encryptedVoiceGrantCount,
		@dataCount,
		@privateDataCount,
		@alertCount,
		@firstSeen,
		@lastSeen
	);

	SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerRadiosUpdate]    Script Date: 2/12/2019 2:49:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerRadiosUpdate]
(
	@id INT,
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.TowerTalkgroupRadios
	SET AffiliationCount = @affiliationCount,
		DeniedCount = @deniedCount,
		VoiceGrantCount = @voiceGrantCount,
		EmergencyVoiceGrantCount = @emergencyVoiceGrantCount,
		EncryptedVoiceGrantCount = @encryptedVoiceGrantCount,
		DataCount = @dataCount,
		PrivateDataCount = @privateDataCount,
		AlertCount = @alertCount,
		FirstSeen = @firstSeen,
		LastSeen = @lastSeen,
		LastModified = GETDATE()
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowersDelete]    Script Date: 2/12/2019 2:49:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowersDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.Towers
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowersDeleteForSystem]    Script Date: 2/12/2019 2:49:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowersDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.Towers
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TowersGet]    Script Date: 2/12/2019 2:49:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowersGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.ID,
		t.SystemID,
		t.TowerNumber,
		t.TowerNumberHex,
		t.[Description],
		t.HitCount,
		t.WACN,
		t.ControlCapabilities,
		t.Flavor,
		t.CallSigns,
		t.[TimeStamp],
		t.FirstSeen,
		t.LastSeen,
		t.LastModified
	FROM dbo.Towers t
	WHERE (t.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowersGetCountForSystem]    Script Date: 2/12/2019 2:49:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowersGetCountForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*)
	FROM dbo.Towers t
	WHERE (t.SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TowersGetForSystem]    Script Date: 2/12/2019 2:49:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowersGetForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT t.ID,
		t.SystemID,
		t.TowerNumber,
		t.TowerNumberHex,
		t.[Description],
		t.WACN,
		t.ControlCapabilities,
		t.Flavor,
		t.CallSigns,
		t.[TimeStamp],
		t.FirstSeen,
		t.LastSeen,
		t.LastModified,
		( SUM(ISNULL(tfu.VoiceGrantCount, 0))
		    + SUM(ISNULL(tfu.EmergencyVoiceGrantCount, 0))
			+ SUM(ISNULL(tfu.EncryptedVoiceGrantCount, 0))
			+ SUM(ISNULL(tfu.AlertCount, 0)) ) AS HitCount
	FROM dbo.Towers t WITH (NOLOCK)
    LEFT JOIN dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
        ON (tfu.TowerID = t.ID)
	WHERE (t.SystemID = @systemID)
    GROUP BY t.ID,
		t.SystemID,
		t.TowerNumber,
		t.TowerNumberHex,
		t.[Description],
		t.WACN,
		t.ControlCapabilities,
		t.Flavor,
		t.CallSigns,
		t.[TimeStamp],
		t.FirstSeen,
		t.LastSeen,
		t.LastModified
	ORDER BY t.TowerNumber;
END
GO

/****** Object:  StoredProcedure [dbo].[TowersGetForSystemActiveFiltersWithPaging]    Script Date: 2/12/2019 2:49:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowersGetForSystemActiveFiltersWithPaging]
(
	@systemID NVARCHAR(50),
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @towerNumberFrom INT = NULL,
    @towerNumberTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_TOWER_NUMBER NVARCHAR(50) = N'TowerNumber',
		@FIELD_TOWER_NAME NVARCHAR(50) = N'TowerName',
        @FIELD_HIT_COUNT NVARCHAR(50) = N'HitCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH TowerResults (TowerNumber, [Description], HitCount, FirstSeen, LastSeen)
    AS
    (
	    SELECT t.TowerNumber,
		    t.[Description],
		    SUM(ISNULL(tfu.VoiceGrantCount, 0))
			    + SUM(ISNULL(tfu.EmergencyVoiceGrantCount, 0))
			    + SUM(ISNULL(tfu.EncryptedVoiceGrantCount, 0))
			    + SUM(ISNULL(tfu.AlertCount, 0)) AS HitCount,
		    t.FirstSeen,
		    t.LastSeen
	    FROM dbo.Towers t WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = t.SystemID)
	    INNER JOIN dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
	        ON ((tfu.TowerID = t.ID)
                AND ((@dateFrom IS NULL)
                    OR (tfu.[Date] >= @dateFrom))
                AND ((@dateTo IS NULL)
                    OR (tfu.[Date] <= @dateTo)))
	    WHERE ((s.SystemID = @systemID)
            AND ((@towerNumberFrom IS NULL)
                OR (t.TowerNumber >= @towerNumberFrom))
            AND ((@towerNumberTo IS NULL)
                OR (t.TowerNumber <= @towerNumberTo))
		    AND ((@searchText IS NULL)
                OR (t.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (t.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (t.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (t.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (t.LastSeen <= @lastSeenTo)))
        GROUP BY t.TowerNumber,
            t.[Description],
            t.FirstSeen,
            t.LastSeen
    )
    SELECT tr.TowerNumber,
        tr.[Description],
        tr.HitCount,
        tr.FirstSeen,
        tr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TowerResults tr
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.TowerNumber END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.TowerNumber END DESC,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.[Description] END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.[Description] END DESC,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.HitCount END,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.HitCount END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.LastSeen END DESC,
        tr.TowerNumber
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TowersGetForSystemActiveWithPaging]    Script Date: 2/12/2019 2:49:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowersGetForSystemActiveWithPaging]
(
	@systemID NVARCHAR(50),
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_TOWER_NUMBER NVARCHAR(50) = N'TowerNumber',
		@FIELD_TOWER_NAME NVARCHAR(50) = N'TowerName',
        @FIELD_HIT_COUNT NVARCHAR(50) = N'HitCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH TowerResults (TowerNumber, [Description], HitCount, FirstSeen, LastSeen)
    AS
    (
	    SELECT t.TowerNumber,
		    t.[Description],
		    SUM(ISNULL(tfu.VoiceGrantCount, 0))
			    + SUM(ISNULL(tfu.EmergencyVoiceGrantCount, 0))
			    + SUM(ISNULL(tfu.EncryptedVoiceGrantCount, 0))
			    + SUM(ISNULL(tfu.AlertCount, 0)) AS HitCount,
		    t.FirstSeen,
		    t.LastSeen
	    FROM dbo.Towers t WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = t.SystemID)
        LEFT JOIN dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
            ON (tfu.TowerID = t.ID)
	    WHERE (s.SystemID = @systemID)
        GROUP BY t.TowerNumber,
            t.[Description],
            t.FirstSeen,
            t.LastSeen
    )
    SELECT tr.TowerNumber,
        tr.[Description],
        tr.HitCount,
        tr.FirstSeen,
        tr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TowerResults tr
    WHERE (tr.HitCount > 0)
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.TowerNumber END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.TowerNumber END DESC,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.[Description] END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.[Description] END DESC,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.HitCount END,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.HitCount END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.LastSeen END DESC,
        tr.TowerNumber
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TowersGetForSystemFiltersWithPaging]    Script Date: 2/12/2019 2:49:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowersGetForSystemFiltersWithPaging]
(
	@systemID NVARCHAR(50),
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @towerNumberFrom INT = NULL,
    @towerNumberTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_TOWER_NUMBER NVARCHAR(50) = N'TowerNumber',
		@FIELD_TOWER_NAME NVARCHAR(50) = N'TowerName',
        @FIELD_HIT_COUNT NVARCHAR(50) = N'HitCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH TowerResults (TowerNumber, [Description], HitCount, FirstSeen, LastSeen)
    AS
    (
	    SELECT t.TowerNumber,
		    t.[Description],
		    SUM(ISNULL(tfu.VoiceGrantCount, 0))
			    + SUM(ISNULL(tfu.EmergencyVoiceGrantCount, 0))
			    + SUM(ISNULL(tfu.EncryptedVoiceGrantCount, 0))
			    + SUM(ISNULL(tfu.AlertCount, 0)) AS HitCount,
		    t.FirstSeen,
		    t.LastSeen
	    FROM dbo.Towers t WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = t.SystemID)
	    LEFT JOIN dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
	        ON ((tfu.TowerID = t.ID)
                AND ((@dateFrom IS NULL)
                    OR (tfu.[Date] >= @dateFrom))
                AND ((@dateTo IS NULL)
                    OR (tfu.[Date] <= @dateTo)))
	    WHERE ((s.SystemID = @systemID)
            AND ((@towerNumberFrom IS NULL)
                OR (t.TowerNumber >= @towerNumberFrom))
            AND ((@towerNumberTo IS NULL)
                OR (t.TowerNumber <= @towerNumberTo))
		    AND ((@searchText IS NULL)
                OR (t.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (t.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (t.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (t.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (t.LastSeen <= @lastSeenTo)))
        GROUP BY t.TowerNumber,
            t.[Description],
            t.FirstSeen,
            t.LastSeen
    )
    SELECT tr.TowerNumber,
        tr.[Description],
        tr.HitCount,
        tr.FirstSeen,
        tr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TowerResults tr
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.TowerNumber END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.TowerNumber END DESC,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.[Description] END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.[Description] END DESC,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.HitCount END,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.HitCount END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.LastSeen END DESC,
        tr.TowerNumber
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TowersGetForSystemTower]    Script Date: 2/12/2019 2:49:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowersGetForSystemTower]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.ID,
		t.SystemID,
		t.TowerNumber,
		t.TowerNumberHex,
		t.[Description],
		t.HitCount,
		t.WACN,
		t.ControlCapabilities,
		t.Flavor,
		t.CallSigns,
		t.[TimeStamp],
		t.FirstSeen,
		t.LastSeen,
		t.LastModified
	FROM dbo.Towers t
	WHERE ((t.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber));
END
GO

/****** Object:  StoredProcedure [dbo].[TowersGetForSystemWithPaging]    Script Date: 2/12/2019 2:49:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowersGetForSystemWithPaging]
(
	@systemID NVARCHAR(50),
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FIELD_TOWER_NUMBER NVARCHAR(50) = N'TowerNumber',
		@FIELD_TOWER_NAME NVARCHAR(50) = N'TowerName',
        @FIELD_HIT_COUNT NVARCHAR(50) = N'HitCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @recordCount INT;

    SELECT @recordCount = COUNT(*)
    FROM dbo.Towers t WITH (NOLOCK)
    INNER JOIN dbo.Systems s WITH (NOLOCK)
        ON (s.ID = t.SystemID)
    WHERE (s.SystemID = @systemID);

    WITH TowerResults (TowerNumber, [Description], HitCount, FirstSeen, LastSeen)
    AS
    (
	    SELECT t.TowerNumber,
		    t.[Description],
		    SUM(ISNULL(tfu.VoiceGrantCount, 0))
			    + SUM(ISNULL(tfu.EmergencyVoiceGrantCount, 0))
			    + SUM(ISNULL(tfu.EncryptedVoiceGrantCount, 0))
			    + SUM(ISNULL(tfu.AlertCount, 0)) AS HitCount,
		    t.FirstSeen,
		    t.LastSeen
	    FROM dbo.Towers t WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = t.SystemID)
        LEFT JOIN dbo.TowerFrequencyUsage tfu WITH (NOLOCK)
            ON (tfu.TowerID = t.ID)
	    WHERE (s.SystemID = @systemID)
        GROUP BY t.TowerNumber,
            t.[Description],
            t.FirstSeen,
            t.LastSeen
    )
    SELECT tr.TowerNumber,
        tr.[Description],
        tr.HitCount,
        tr.FirstSeen,
        tr.LastSeen,
        @recordCount AS RecordCount
    FROM TowerResults tr
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.TowerNumber END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.TowerNumber END DESC,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.[Description] END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.[Description] END DESC,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.HitCount END,
		CASE WHEN ((@sortField = @FIELD_HIT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.HitCount END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN tr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN tr.LastSeen END DESC,
        tr.TowerNumber
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TowersInsert]    Script Date: 2/12/2019 2:49:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowersInsert]
(
	@systemID INT,
	@towerNumber INT,
	@towerNumberHex NVARCHAR(50),
	@description NVARCHAR(100),
	@hitCount INT,
	@wacn NVARCHAR(50),
	@controlCapabilities NVARCHAR(50),
	@flavor NVARCHAR(50),
	@callSigns NVARCHAR(50),
	@timeStamp DATETIME2(7),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.Towers
	(
		SystemID,
		TowerNumber,
		TowerNumberHex,
		[Description],
		HitCount,
		WACN,
		ControlCapabilities,
		Flavor,
		CallSigns,
		[TimeStamp],
		FirstSeen,
		LastSeen,
		LastModified
	)
	VALUES
	(
		@systemID,
		@towerNumber,
		@towerNumberHex,
		@description,
		@hitCount,
		@wacn,
		@controlCapabilities,
		@flavor,
		@callSigns,
		@timeStamp,
		@firstSeen,
		@lastSeen,
		GETDATE()
	);

	SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowersUpdate]    Script Date: 2/12/2019 2:49:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowersUpdate]
(
	@id INT,
	@towerNumberHex NVARCHAR(50),
	@description NVARCHAR(100),
	@hitCount INT,
	@wacn NVARCHAR(50),
	@controlCapabilities NVARCHAR(50),
	@flavor NVARCHAR(50),
	@callSigns NVARCHAR(50),
	@timeStamp DATETIME2(7),
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Towers
	SET TowerNumberHex = @towerNumberHex,
		[Description] = @description,
		HitCount = @hitCount,
		WACN = @wacn,
		ControlCapabilities = @controlCapabilities,
		Flavor = @flavor,
		CallSigns = @callSigns,
		[TimeStamp] = @timeStamp,
		FirstSeen = @firstSeen,
		LastSeen = @lastSeen
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTablesDelete]    Script Date: 2/12/2019 2:49:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTablesDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TowerTables
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTablesDeleteForSystem]    Script Date: 2/12/2019 2:49:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTablesDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TowerTables
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTablesDeleteForTower]    Script Date: 2/12/2019 2:49:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTablesDeleteForTower]
(
    @systemID INT,
    @towerNumber INT
)
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @towerID INT;

    SELECT @towerID = t.ID
    FROM dbo.Towers t
    WHERE ((t.SystemID = @systemID)
        AND (t.TowerNumber = @towerNumber));

    DELETE
    FROM dbo.TowerTables
    WHERE (TowerID = @towerID);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTablesGet]    Script Date: 2/12/2019 2:49:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTablesGet]
(
	@id INT
)
AS
BEGIN

	SELECT tt.ID,
		tt.SystemID,
		tt.TowerID,
		tt.TableID,
		tt.BaseFrequency,
		tt.Spacing,
		tt.InputOffset,
		tt.AssumedConfirmed,
		tt.Bandwidth,
		tt.Slots,
		tt.LastModified
	FROM dbo.TowerTables tt
	WHERE (tt.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTablesGetForTower]    Script Date: 2/12/2019 2:49:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTablesGetForTower]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN

	SELECT tt.ID,
		tt.SystemID,
		tt.TowerID,
		tt.TableID,
		tt.BaseFrequency,
		tt.Spacing,
		tt.InputOffset,
		tt.AssumedConfirmed,
		tt.Bandwidth,
		tt.Slots,
		tt.LastModified
	FROM dbo.TowerTables tt WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tt.TowerID)
	WHERE ((tt.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTablesGetForTowerTable]    Script Date: 2/12/2019 2:49:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTablesGetForTowerTable]
(
	@systemID INT,
	@towerNumber INT,
	@tableID INT
)
AS
BEGIN

	SELECT tt.ID,
		tt.SystemID,
		tt.TowerID,
		tt.TableID,
		tt.BaseFrequency,
		tt.Spacing,
		tt.InputOffset,
		tt.AssumedConfirmed,
		tt.Bandwidth,
		tt.Slots,
		tt.LastModified
	FROM dbo.TowerTables tt WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = tt.TowerID)
	WHERE ((tt.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
		AND (tt.TableID = @tableID));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTablesInsert]    Script Date: 2/12/2019 2:49:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTablesInsert]
(
	@systemID INT,
	@towerID INT,
	@tableID INT,
	@baseFrequency NVARCHAR(50),
	@spacing NVARCHAR(50),
	@inputOffset NVARCHAR(50),
	@assumedConfirmed NVARCHAR(50),
	@bandwidth NVARCHAR(50),
	@slots INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @towerIDKey INT;

	SELECT @towerIDKey = t.ID
	FROM dbo.Towers t
	WHERE ((t.SystemID = @systemID)
		AND (t.TowerNumber = @towerID));

	INSERT INTO dbo.TowerTables
	(
		SystemID,
		TowerID,
		TableID,
		BaseFrequency,
		Spacing,
		InputOffset,
		AssumedConfirmed,
		Bandwidth,
		Slots
	)
	VALUES
	(
		@systemID,
		@towerIDKey,
		@tableID,
		@baseFrequency,
		@spacing,
		@inputOffset,
		@assumedConfirmed,
		@bandwidth,
		@slots
	);

	SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTablesUpdate]    Script Date: 2/12/2019 2:49:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTablesUpdate]
(
	@id INT,
	@baseFrequency NVARCHAR(50),
	@spacing NVARCHAR(50),
	@inputOffset NVARCHAR(50),
	@assumedConfirmed NVARCHAR(50),
	@bandwidth NVARCHAR(50),
	@slots INT
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.TowerTables
	SET BaseFrequency = @baseFrequency,
		Spacing = @spacing,
		InputOffset = @inputOffset,
		AssumedConfirmed = @assumedConfirmed,
		Bandwidth = @bandwidth,
		Slots = @slots,
		LastModified = GETDATE()
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosDelete]    Script Date: 2/12/2019 2:49:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TowerTalkgroupRadios
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosDeleteForSystem]    Script Date: 2/12/2019 2:49:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TowerTalkgroupRadios
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosGet]    Script Date: 2/12/2019 2:49:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ttgr.ID,
		ttgr.SystemID,
		ttgr.TowerID,
		ttgr.TalkgroupID,
		ttgr.RadioID,
		ttgr.[Date],
		ttgr.AffiliationCount,
		ttgr.DeniedCount,
		ttgr.VoiceGrantCount,
		ttgr.EmergencyVoiceGrantCount,
		ttgr.EncryptedVoiceGrantCount,
		ttgr.DataCount,
		ttgr.PrivateDataCount,
		ttgr.AlertCount,
		ttgr.FirstSeen,
		ttgr.LastSeen,
		ttgr.LastModified
	FROM dbo.TowerTalkgroupRadios ttgr
	WHERE (ttgr.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosGetForTower]    Script Date: 2/12/2019 2:49:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosGetForTower]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ttgr.ID,
		ttgr.SystemID,
		ttgr.TowerID,
		ttgr.TalkgroupID,
		ttgr.RadioID,
		ttgr.[Date],
		ttgr.AffiliationCount,
		ttgr.DeniedCount,
		ttgr.VoiceGrantCount,
		ttgr.EmergencyVoiceGrantCount,
		ttgr.EncryptedVoiceGrantCount,
		ttgr.DataCount,
		ttgr.PrivateDataCount,
		ttgr.AlertCount,
		ttgr.FirstSeen,
		ttgr.LastSeen,
		ttgr.LastModified
	FROM dbo.TowerTalkgroupRadios ttgr WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = ttgr.TowerID)
	WHERE ((ttgr.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosGetForTowerDateRange]    Script Date: 2/12/2019 2:49:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosGetForTowerDateRange]
(
	@systemID INT,
	@towerNumber INT,
    @date DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ttgr.ID,
		ttgr.SystemID,
		ttgr.TowerID,
		tg.TalkgroupID,
		r.RadioID,
		ttgr.[Date],
		ttgr.AffiliationCount,
		ttgr.DeniedCount,
		ttgr.VoiceGrantCount,
		ttgr.EmergencyVoiceGrantCount,
		ttgr.EncryptedVoiceGrantCount,
		ttgr.DataCount,
		ttgr.PrivateDataCount,
		ttgr.AlertCount,
		ttgr.FirstSeen,
		ttgr.LastSeen,
		ttgr.LastModified
	FROM dbo.TowerTalkgroupRadios ttgr WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = ttgr.TowerID)
    INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
        ON (tg.ID = ttgr.TalkgroupID)
    INNER JOIN dbo.Radios r WITH (NOLOCK)
        ON (r.ID = ttgr.RadioID)
	WHERE ((ttgr.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
        AND (ttgr.[Date] >= DATEADD(dd, -1, @date))
        AND (ttgr.[Date] <= DATEADD(dd, 1, @date)));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosGetRadiosForTalkgroup]    Script Date: 2/12/2019 2:50:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosGetRadiosForTalkgroup]
(
	@systemID INT,
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		tg.TalkgroupID,
		tg.[Description] AS TalkgroupDescription,
		r.RadioID,
		r.[Description] AS RadioDescription,
		SUM(ISNULL(ttgr.AffiliationCount, 0)) AS AffiliationCount,
		SUM(ISNULL(ttgr.DeniedCount, 0)) AS DeniedCount,
		SUM(ISNULL(ttgr.VoiceGrantCount, 0)) AS VoiceGrantCount,
		SUM(ISNULL(ttgr.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		SUM(ISNULL(ttgr.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
		SUM(ISNULL(ttgr.DataCount, 0)) AS DataCount,
		SUM(ISNULL(ttgr.PrivateDataCount, 0)) AS PrivateDataCount,
		SUM(ISNULL(ttgr.AlertCount, 0)) AS AlertCount,
		MIN(ttgr.FirstSeen) AS FirstSeen,
		MAX(ttgr.LastSeen) AS LastSeen
	FROM dbo.TowerTalkgroupRadios ttgr WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = ttgr.TowerID)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = ttgr.TalkgroupID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = ttgr.RadioID)
	WHERE ((ttgr.SystemID = @systemID)
		AND (tg.TalkgroupID = @talkgroupID))
	GROUP BY t.TowerNumber,
		t.[Description],
		tg.TalkgroupID,
		tg.[Description],
		r.RadioID,
		r.[Description]
	ORDER BY r.RadioID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosGetRadiosForTalkgroupWithDates]    Script Date: 2/12/2019 2:50:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosGetRadiosForTalkgroupWithDates]
(
	@systemID INT,
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		tg.TalkgroupID,
		tg.[Description] AS TalkgroupDescription,
		r.RadioID,
		r.[Description] AS RadioDescription,
		ttgr.[Date],
		SUM(ISNULL(ttgr.AffiliationCount, 0)) AS AffiliationCount,
		SUM(ISNULL(ttgr.DeniedCount, 0)) AS DeniedCount,
		SUM(ISNULL(ttgr.VoiceGrantCount, 0)) AS VoiceGrantCount,
		SUM(ISNULL(ttgr.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		SUM(ISNULL(ttgr.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
		SUM(ISNULL(ttgr.DataCount, 0)) AS DataCount,
		SUM(ISNULL(ttgr.PrivateDataCount, 0)) AS PrivateDataCount,
		SUM(ISNULL(ttgr.AlertCount, 0)) AS AlertCount,
		MIN(ttgr.FirstSeen) AS FirstSeen,
		MAX(ttgr.LastSeen) AS LastSeen
	FROM dbo.TowerTalkgroupRadios ttgr WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = ttgr.TowerID)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = ttgr.TalkgroupID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = ttgr.RadioID)
	WHERE ((ttgr.SystemID = @systemID)
		AND (tg.TalkgroupID = @talkgroupID))
	GROUP BY t.TowerNumber,
		t.[Description],
		tg.TalkgroupID,
		tg.[Description],
		r.RadioID,
		r.[Description],
		ttgr.[Date]
	ORDER BY r.RadioID,
		ttgr.[Date] DESC;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosGetRadiosForTowerTalkgroup]    Script Date: 2/12/2019 2:50:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosGetRadiosForTowerTalkgroup]
(
	@systemID INT,
	@towerNumber INT,
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		tg.TalkgroupID,
		tg.[Description] AS TalkgroupDescription,
		r.RadioID,
		r.[Description] AS RadioDescription,
		SUM(ttgr.AffiliationCount) AS AffiliationCount,
		SUM(ttgr.DeniedCount) AS DeniedCount,
		SUM(ttgr.VoiceGrantCount) AS VoiceGrantCount,
		SUM(ttgr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		SUM(ttgr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		SUM(ttgr.DataCount) AS DataCount,
		SUM(ttgr.PrivateDataCount) AS PrivateDataCount,
		SUM(ttgr.AlertCount) AS AlertCount,
		MIN(ttgr.FirstSeen) AS FirstSeen,
		MAX(ttgr.LastSeen) AS LastSeen
	FROM dbo.TowerTalkgroupRadios ttgr WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = ttgr.TowerID)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = ttgr.TalkgroupID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = ttgr.RadioID)
	WHERE ((ttgr.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
		AND (tg.TalkgroupID = @talkgroupID))
	GROUP BY t.TowerNumber,
		t.[Description],
		tg.TalkgroupID,
		tg.[Description],
		r.RadioID,
		r.[Description]
	ORDER BY r.RadioID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosGetRadiosForTowerTalkgroupFiltersWithPaging]    Script Date: 2/12/2019 2:50:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosGetRadiosForTowerTalkgroupFiltersWithPaging]
(
    @systemID NVARCHAR(50),
    @talkgroupID INT,
	@towerNumber INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @radioIDFrom INT = NULL,
    @radioIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @systemIDKey INT,
        @towerIDKey INT,
        @talkgroupIDKey INT,
        @FIELD_TOWER_NUMBER NVARCHAR(50) = N'TowerNumber',
        @FIELD_TOWER_NAME NVARCHAR(50) = N'TowerName',
        @FIELD_RADIO_ID NVARCHAR(50) = N'RadioID',
	    @FIELD_RADIO_NAME NVARCHAR(50) = N'RadioName',
        @FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
	    @FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
	    @FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
	    @FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
	    @FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PRIVATE_DATA_COUNT NVARCHAR(50) = N'PrivateDataCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
	    @FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
	    @FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
	    @DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
	    @DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    SELECT @towerIDKey = t.ID
    FROM dbo.Towers t
    WHERE ((t.SystemID = @systemIDKey)
        AND (t.TowerNumber = @towerNumber));

    SELECT @talkgroupIDKey = tg.ID
    FROM dbo.Talkgroups tg
    WHERE ((tg.SystemID = @systemIDKey)
        AND (tg.TalkgroupID = @talkgroupID));

    WITH TowerTalkgroupRadioResults (RadioID, RadioDescription, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
        SELECT r.RadioID,
            r.[Description] AS RadioDescription,
	        SUM(ISNULL(ttgr.AffiliationCount, 0)) AS AffiliationCount,
	        SUM(ISNULL(ttgr.DeniedCount, 0)) AS DeniedCount,
	        SUM(ISNULL(ttgr.VoiceGrantCount, 0)) AS VoiceGrantCount,
	        SUM(ISNULL(ttgr.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
	        SUM(ISNULL(ttgr.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
	        SUM(ISNULL(ttgr.DataCount, 0))
	            + SUM(ISNULL(ttgr.PrivateDataCount, 0)) AS DataCount,
	        SUM(ISNULL(ttgr.AlertCount, 0)) AS AlertCount,
	        MIN(ttgr.FirstSeen) AS FirstSeen,
	        MAX(ttgr.LastSeen) AS LastSeen
        FROM dbo.TowerTalkgroupRadios ttgr WITH (NOLOCK)
        INNER JOIN dbo.Radios r WITH (NOLOCK)
            ON (r.ID = ttgr.RadioID)
        WHERE ((ttgr.TalkgroupID = @talkgroupIDKey)
            AND (ttgr.TowerID = @towerIDKey)
            AND ((@dateFrom IS NULL)
                OR (ttgr.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (ttgr.[Date] <= @dateTo))
            AND ((@radioIDFrom IS NULL)
                OR (r.RadioID >= @radioIDFrom))
            AND ((@radioIDTo IS NULL)
                OR (r.RadioID <= @radioIDTo))
		    AND ((@searchText IS NULL)
                OR (r.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (ttgr.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (ttgr.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (ttgr.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (ttgr.LastSeen <= @lastSeenTo)))
        GROUP BY r.RadioID,
            r.[Description]
    )
    SELECT ttgrr.RadioID, 
        ttgrr.RadioDescription, 
        ttgrr.AffiliationCount, 
        ttgrr.DeniedCount, 
        ttgrr.VoiceGrantCount, 
        ttgrr.EmergencyVoiceGrantCount, 
        ttgrr.EncryptedVoiceGrantCount, 
        ttgrr.DataCount, 
        ttgrr.AlertCount, 
        ttgrr.FirstSeen, 
        ttgrr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TowerTalkgroupRadioResults ttgrr
    ORDER BY
	    CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.RadioID END,
	    CASE WHEN ((@sortField = @FIELD_RADIO_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.RadioID END DESC,
	    CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.RadioDescription END,
	    CASE WHEN ((@sortField = @FIELD_RADIO_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.RadioDescription END DESC,
	    CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.AffiliationCount END,
	    CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.AffiliationCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.DeniedCount END,
	    CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.DeniedCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.VoiceGrantCount END,
	    CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.VoiceGrantCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.EmergencyVoiceGrantCount END,
	    CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.EmergencyVoiceGrantCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.EncryptedVoiceGrantCount END,
	    CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.EncryptedVoiceGrantCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.DataCount END,
	    CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.DataCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.AlertCount END,
	    CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.AlertCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.FirstSeen END,
	    CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.FirstSeen END DESC,
	    CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.LastSeen END,
	    CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.LastSeen END DESC,
        ttgrr.RadioID
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosGetTalkgroupsForRadio]    Script Date: 2/12/2019 2:50:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosGetTalkgroupsForRadio]
(
	@systemID INT,
	@radioID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tg.TalkgroupID,
		tg.[Description] AS TalkgroupDescription,
		r.RadioID,
		r.[Description] AS RadioDescription,
		SUM(ttgr.AffiliationCount) AS AffiliationCount,
		SUM(ttgr.DeniedCount) AS DeniedCount,
		SUM(ttgr.VoiceGrantCount) AS VoiceGrantCount,
		SUM(ttgr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		SUM(ttgr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		SUM(ttgr.DataCount) AS DataCount,
		SUM(ttgr.PrivateDataCount) AS PrivateDataCount,
		SUM(ttgr.AlertCount) AS AlertCount,
		MIN(ttgr.FirstSeen) AS FirstSeen,
		MAX(ttgr.LastSeen) AS LastSeen,
        COUNT(1) OVER() AS RecordCount
	FROM dbo.TowerTalkgroupRadios ttgr WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = ttgr.TalkgroupID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = ttgr.RadioID)
	WHERE ((ttgr.SystemID = @systemID)
		AND (r.RadioID = @radioID))
	GROUP BY tg.TalkgroupID,
		tg.[Description],
		r.RadioID,
		r.[Description]
	ORDER BY tg.TalkgroupID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosGetTalkgroupsForRadioWithDates]    Script Date: 2/12/2019 2:50:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosGetTalkgroupsForRadioWithDates]
(
	@systemID INT,
	@radioID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		tg.TalkgroupID,
		tg.[Description] AS TalkgroupDescription,
		r.RadioID,
		r.[Description] AS RadioDescription,
		ttgr.[Date],
		SUM(ttgr.AffiliationCount) AS AffiliationCount,
		SUM(ttgr.DeniedCount) AS DeniedCount,
		SUM(ttgr.VoiceGrantCount) AS VoiceGrantCount,
		SUM(ttgr.EmergencyVoiceGrantCount) AS EmergencyVoiceGrantCount,
		SUM(ttgr.EncryptedVoiceGrantCount) AS EncryptedVoiceGrantCount,
		SUM(ttgr.DataCount) AS DataCount,
		SUM(ttgr.PrivateDataCount) AS PrivateDataCount,
		SUM(ttgr.AlertCount) AS AlertCount,
		MIN(ttgr.FirstSeen) AS FirstSeen,
		MAX(ttgr.LastSeen) AS LastSeen
	FROM dbo.TowerTalkgroupRadios ttgr
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = ttgr.TowerID)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = ttgr.TalkgroupID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = ttgr.RadioID)
	WHERE ((ttgr.SystemID = @systemID)
		AND (r.RadioID = @radioID))
	GROUP BY t.TowerNumber,
		t.[Description],
		tg.TalkgroupID,
		tg.[Description],
		r.RadioID,
		r.[Description],
		ttgr.[Date]
	ORDER BY tg.TalkgroupID,
		ttgr.[Date] DESC;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosGetTalkgroupsForTowerRadio]    Script Date: 2/12/2019 2:50:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosGetTalkgroupsForTowerRadio]
(
	@systemID INT,
	@towerNumber INT,
	@radioID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ttgr.SystemID,
		ttgr.TowerID,
		ttgr.TalkgroupID,
		ttgr.RadioID,
		ttgr.[Date],
		ttgr.AffiliationCount,
		ttgr.DeniedCount,
		ttgr.VoiceGrantCount,
		ttgr.EmergencyVoiceGrantCount,
		ttgr.EncryptedVoiceGrantCount,
		ttgr.DataCount,
		ttgr.PrivateDataCount,
		ttgr.AlertCount,
		ttgr.FirstSeen,
		ttgr.LastSeen,
		ttgr.LastModified
	FROM dbo.TowerTalkgroupRadios ttgr WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = ttgr.TowerID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = ttgr.RadioID)
	WHERE ((ttgr.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
		AND (r.RadioID = @radioID));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosGetTalkgroupsForTowerRadioFiltersWithPaging]    Script Date: 2/12/2019 2:50:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosGetTalkgroupsForTowerRadioFiltersWithPaging]
(
    @systemID NVARCHAR(50),
    @radioID INT,
	@towerNumber INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @talkgroupIDFrom INT = NULL,
    @talkgroupIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @FIELD_TOWER_NUMBER NVARCHAR(50) = N'TowerNumber',
        @FIELD_TOWER_NAME NVARCHAR(50) = N'TowerName',
        @FIELD_TALKGROUP_ID NVARCHAR(50) = N'TalkgroupID',
	    @FIELD_TALKGROUP_NAME NVARCHAR(50) = N'TalkgroupName',
        @FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
	    @FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
	    @FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
	    @FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
	    @FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PRIVATE_DATA_COUNT NVARCHAR(50) = N'PrivateDataCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
	    @FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
	    @FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
	    @DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
	    @DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @systemIDKey INT,
        @towerIDKey INT,
        @radioIDKey INT;

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    SELECT @towerIDKey = t.ID
    FROM dbo.Towers t
    WHERE ((t.SystemID = @systemIDKey)
        AND (t.TowerNumber = @towerNumber));

    SELECT @radioIDKey = r.ID
    FROM dbo.Radios r
    WHERE ((r.SystemID = @systemIDKey)
        AND (r.RadioID = @radioID));

    WITH TowerTalkgroupRadioResults (TalkgroupID, TalkgroupDescription, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
        SELECT tg.TalkgroupID,
            tg.[Description] AS TalkgroupDescription,
	        SUM(ISNULL(ttgr.AffiliationCount, 0)) AS AffiliationCount,
	        SUM(ISNULL(ttgr.DeniedCount, 0)) AS DeniedCount,
	        SUM(ISNULL(ttgr.VoiceGrantCount, 0)) AS VoiceGrantCount,
	        SUM(ISNULL(ttgr.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
	        SUM(ISNULL(ttgr.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
	        SUM(ISNULL(ttgr.DataCount, 0))
	            + SUM(ISNULL(ttgr.PrivateDataCount, 0)) AS DataCount,
	        SUM(ISNULL(ttgr.AlertCount, 0)) AS AlertCount,
	        MIN(ttgr.FirstSeen) AS FirstSeen,
	        MAX(ttgr.LastSeen) AS LastSeen
        FROM dbo.TowerTalkgroupRadios ttgr WITH (NOLOCK)
        INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
            ON (tg.ID = ttgr.TalkgroupID)
        WHERE ((ttgr.RadioID = @radioIDKey)
            AND (ttgr.TowerID = @towerIDKey)
            AND ((@dateFrom IS NULL)
                OR (ttgr.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (ttgr.[Date] <= @dateTo))
            AND ((@talkgroupIDFrom IS NULL)
                OR (tg.TalkgroupID >= @talkgroupIDFrom))
            AND ((@talkgroupIDTo IS NULL)
                OR (tg.TalkgroupID <= @talkgroupIDTo))
		    AND ((@searchText IS NULL)
                OR (tg.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (ttgr.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (ttgr.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (ttgr.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (ttgr.LastSeen <= @lastSeenTo)))
        GROUP BY tg.TalkgroupID,
            tg.[Description]
    )
    SELECT ttgrr.TalkgroupID, 
        ttgrr.TalkgroupDescription, 
        ttgrr.AffiliationCount, 
        ttgrr.DeniedCount, 
        ttgrr.VoiceGrantCount, 
        ttgrr.EmergencyVoiceGrantCount, 
        ttgrr.EncryptedVoiceGrantCount, 
        ttgrr.DataCount, 
        ttgrr.AlertCount, 
        ttgrr.FirstSeen, 
        ttgrr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TowerTalkgroupRadioResults ttgrr
    ORDER BY
	    CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.TalkgroupID END,
	    CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.TalkgroupID END DESC,
	    CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.TalkgroupDescription END,
	    CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.TalkgroupDescription END DESC,
	    CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.AffiliationCount END,
	    CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.AffiliationCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.DeniedCount END,
	    CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.DeniedCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.VoiceGrantCount END,
	    CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.VoiceGrantCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.EmergencyVoiceGrantCount END,
	    CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.EmergencyVoiceGrantCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.EncryptedVoiceGrantCount END,
	    CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.EncryptedVoiceGrantCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.DataCount END,
	    CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.DataCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.AlertCount END,
	    CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.AlertCount END DESC,
	    CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.FirstSeen END,
	    CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.FirstSeen END DESC,
	    CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgrr.LastSeen END,
	    CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgrr.LastSeen END DESC,
        ttgrr.TalkgroupID
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosGetTowersForTalkgroupRadio]    Script Date: 2/12/2019 2:50:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosGetTowersForTalkgroupRadio]
(
	@systemID INT,
	@talkgroupID INT,
	@radioID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ttgr.SystemID,
		ttgr.TowerID,
		ttgr.TalkgroupID,
		ttgr.RadioID,
		ttgr.[Date],
		ttgr.AffiliationCount,
		ttgr.DeniedCount,
		ttgr.VoiceGrantCount,
		ttgr.EmergencyVoiceGrantCount,
		ttgr.EncryptedVoiceGrantCount,
		ttgr.DataCount,
		ttgr.PrivateDataCount,
		ttgr.AlertCount,
		ttgr.FirstSeen,
		ttgr.LastSeen,
		ttgr.LastModified
	FROM dbo.TowerTalkgroupRadios ttgr WITH (NOLOCK)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = ttgr.TalkgroupID)
	INNER JOIN dbo.Radios r WITH (NOLOCK)
		ON (r.ID = ttgr.RadioID)
	WHERE ((ttgr.SystemID = @systemID)
		AND (tg.TalkgroupID = @talkgroupID)
		AND (r.RadioID = @radioID));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosInsert]    Script Date: 2/12/2019 2:50:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosInsert]
(
	@systemID INT,
	@towerID INT,
	@talkgroupID INT,
	@radioID INT,
	@date DATETIME2(7),
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TowerTalkgroupRadios
	(
		SystemID,
		TowerID,
		TalkgroupID,
		RadioID,
		[Date],
		AffiliationCount,
		DeniedCount,
		VoiceGrantCount,
		EmergencyVoiceGrantCount,
		EncryptedVoiceGrantCount,
		DataCount,
		PrivateDataCount,
		AlertCount,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
		@systemID,
		@towerID,
		@talkgroupID,
		@radioID,
		@date,
		@affiliationCount,
		@deniedCount,
		@voiceGrantCount,
		@emergencyVoiceGrantCount,
		@encryptedVoiceGrantCount,
		@dataCount,
		@privateDataCount,
		@alertCount,
		@firstSeen,
		@lastSeen
	);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupRadiosUpdate]    Script Date: 2/12/2019 2:50:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupRadiosUpdate]
(
	@id INT,
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.TowerTalkgroupRadios
	SET AffiliationCount = @affiliationCount,
		DeniedCount = @deniedCount,
		VoiceGrantCount = @voiceGrantCount,
		EmergencyVoiceGrantCount = @emergencyVoiceGrantCount,
		EncryptedVoiceGrantCount = @encryptedVoiceGrantCount,
		DataCount = @dataCount,
		PrivateDataCount = @privateDataCount,
		AlertCount = @alertCount,
		FirstSeen = @firstSeen,
		LastSeen = @lastSeen,
		LastModified = GETDATE()
	WHERE (ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsDelete]    Script Date: 2/12/2019 2:50:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupsDelete]
(
	@systemID NVARCHAR(50),
	@towerNumber INT,
	@talkgroupID INT,
	@date DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @systemIDKey INT,
		@towerID INT,
		@talkgroupIDKey INT;

	SELECT @systemIDKey = s.ID,
		@towerID = t.ID,
		@talkgroupIDKey = tg.ID
	FROM dbo.Systems s WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.SystemID = s.ID)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.SystemID = s.ID)
	WHERE ((s.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
		AND (tg.TalkgroupID = @talkgroupID));

	DELETE
	FROM dbo.TowerTalkgroups
	WHERE ((SystemID = @systemIDKey)
		AND (TowerID = @towerID)
		AND (TalkgroupID = @talkgroupIDKey)
		AND ([Date] = @date));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsDeleteForSystem]    Script Date: 2/12/2019 2:50:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupsDeleteForSystem]
(
	@systemID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM dbo.TowerTalkgroups
	WHERE (SystemID = @systemID);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsGet]    Script Date: 2/12/2019 2:50:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupsGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ttg.ID,
		ttg.SystemID,
		ttg.TowerID,
		ttg.TalkgroupID,
		ttg.[Date],
		ttg.AffiliationCount,
		ttg.DeniedCount,
		ttg.VoiceGrantCount,
		ttg.EmergencyVoiceGrantCount,
		ttg.EncryptedVoiceGrantCount,
		ttg.DataCount,
		ttg.PrivateDataCount,
		ttg.AlertCount,
		ttg.FirstSeen,
		ttg.LastSeen,
		ttg.LastModified
	FROM dbo.TowerTalkgroups ttg
	WHERE (ttg.ID = @id);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsGetDateListForTowerTalkgroup]    Script Date: 2/12/2019 2:50:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[TowerTalkgroupsGetDateListForTowerTalkgroup]
(
    @systemID NVARCHAR(50),
    @talkgroupID INT,
    @towerNumber INT,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL
)
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @systemIDKey INT,
        @towerIDKey INT,
        @talkgroupIDKey INT;

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    SELECT @towerIDKey = t.ID
    FROM dbo.Towers t
    WHERE((t.SystemID = @systemIDKey)
        AND (t.TowerNumber = @towerNumber));

    SELECT @talkgroupIDKey = tg.ID
    FROM dbo.Talkgroups tg
    WHERE ((tg.SystemID = @systemIDKey)
        AND (tg.TalkgroupID = @talkgroupID));

    SELECT ttg.[Date]
    FROM dbo.TowerTalkgroups ttg
    WHERE ((ttg.TowerID = @towerIDKey)
        AND (ttg.TalkgroupID = @talkgroupIDKey)
        AND ((@dateFrom IS NULL)
            OR (ttg.[Date] >= @dateFrom))
        AND ((@dateTo IS NULL)
            OR (ttg.[Date] <= @dateTo)))
    GROUP BY ttg.[Date];
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsGetForTower]    Script Date: 2/12/2019 2:50:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupsGetForTower]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ttg.ID,
		ttg.SystemID,
		ttg.TowerID,
		ttg.TalkgroupID,
		ttg.[Date],
		ttg.AffiliationCount,
		ttg.DeniedCount,
		ttg.VoiceGrantCount,
		ttg.EmergencyVoiceGrantCount,
		ttg.EncryptedVoiceGrantCount,
		ttg.DataCount,
		ttg.PrivateDataCount,
		ttg.AlertCount,
		ttg.FirstSeen,
		ttg.LastSeen,
		ttg.LastModified
	FROM dbo.TowerTalkgroups ttg WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = ttg.TowerID)
	WHERE ((ttg.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsGetForTowerImport]    Script Date: 2/12/2019 2:50:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupsGetForTowerImport]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ttg.ID,
		ttg.SystemID,
		ttg.TowerID,
		tg.TalkgroupID,
		ttg.[Date],
		ttg.AffiliationCount,
		ttg.DeniedCount,
		ttg.VoiceGrantCount,
		ttg.EmergencyVoiceGrantCount,
		ttg.EncryptedVoiceGrantCount,
		ttg.DataCount,
		ttg.PrivateDataCount,
		ttg.AlertCount,
		ttg.FirstSeen,
		ttg.LastSeen,
		ttg.LastModified
	FROM dbo.TowerTalkgroups ttg WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = ttg.TowerID)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = ttg.TalkgroupID)
	WHERE ((ttg.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber));
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsGetForTowerImportDateRange]    Script Date: 2/12/2019 2:50:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupsGetForTowerImportDateRange]
(
	@systemID INT,
	@towerNumber INT,
    @date DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ttg.ID,
		ttg.SystemID,
		ttg.TowerID,
		tg.TalkgroupID,
		ttg.[Date],
		ttg.AffiliationCount,
		ttg.DeniedCount,
		ttg.VoiceGrantCount,
		ttg.EmergencyVoiceGrantCount,
		ttg.EncryptedVoiceGrantCount,
		ttg.DataCount,
		ttg.PrivateDataCount,
		ttg.AlertCount,
		ttg.FirstSeen,
		ttg.LastSeen,
		ttg.LastModified
	FROM dbo.TowerTalkgroups ttg WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = ttg.TowerID)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = ttg.TalkgroupID)
	WHERE ((ttg.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber)
        AND (ttg.[Date] >= DATEADD(dd, -1, @date))
        AND (ttg.[Date] <= DATEADD(dd, 1, @date)))
    ORDER BY ttg.[Date],
        ttg.TowerID,
        ttg.TalkgroupID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsGetTalkgroupsForTower]    Script Date: 2/12/2019 2:50:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupsGetTalkgroupsForTower]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		tg.TalkgroupID,
		tg.[Description] AS TalkgroupDescription,
		SUM(ISNULL(ttg.AffiliationCount, 0)) AS AffiliationCount,
		SUM(ISNULL(ttg.DeniedCount, 0)) AS DeniedCount,
		SUM(ISNULL(ttg.VoiceGrantCount, 0)) AS VoiceGrantCount,
		SUM(ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		SUM(ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
		SUM(ISNULL(ttg.DataCount, 0)) AS DataCount,
		SUM(ISNULL(ttg.PrivateDataCount, 0)) AS PrivateDataCount,
		SUM(ISNULL(ttg.AlertCount, 0)) AS AlertCount,
		MIN(ttg.FirstSeen) AS FirstSeen,
		MAX(ttg.LastSeen) AS LastSeen,
        COUNT(1) OVER() AS RecordCount
	FROM dbo.TowerTalkgroups ttg WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = ttg.TowerID)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = ttg.TalkgroupID)
	WHERE ((ttg.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber))
	GROUP BY t.TowerNumber,
		t.[Description],
		tg.TalkgroupID,
		tg.[Description]
	ORDER BY tg.TalkgroupID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsGetTalkgroupsForTowerByDate]    Script Date: 2/12/2019 2:50:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerTalkgroupsGetTalkgroupsForTowerByDate]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		tg.TalkgroupID,
		tg.[Description] AS TalkgroupDescription,
		ttg.[Date],
		SUM(ISNULL(ttg.AffiliationCount, 0)) AS AffiliationCount,
		SUM(ISNULL(ttg.DeniedCount, 0)) AS DeniedCount,
		SUM(ISNULL(ttg.VoiceGrantCount, 0)) AS VoiceGrantCount,
		SUM(ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		SUM(ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
		SUM(ISNULL(ttg.DataCount, 0)) AS DataCount,
		SUM(ISNULL(ttg.PrivateDataCount, 0)) AS PrivateDataCount,
		SUM(ISNULL(ttg.AlertCount, 0)) AS AlertCount,
		MIN(ttg.FirstSeen) AS FirstSeen,
		MAX(ttg.LastSeen) AS LastSeen
	FROM dbo.TowerTalkgroups ttg WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = ttg.TowerID)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = ttg.TalkgroupID)
	WHERE ((ttg.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber))
	GROUP BY t.TowerNumber,
		t.[Description],
		tg.TalkgroupID,
		tg.[Description],
		ttg.[Date];
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsGetTalkgroupsForTowerCount]    Script Date: 2/12/2019 2:50:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerTalkgroupsGetTalkgroupsForTowerCount]
(
	@systemID INT,
	@towerNumber INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*)
	FROM dbo.TowerTalkgroups ttg
	INNER JOIN dbo.Towers t
		ON (t.ID = ttg.TowerID)
	WHERE ((ttg.SystemID = @systemID)
		AND (t.TowerNumber = @towerNumber))
	GROUP BY ttg.SystemID,
		ttg.TowerID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsGetTalkgroupsForTowerFiltersWithPaging]    Script Date: 2/12/2019 2:50:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerTalkgroupsGetTalkgroupsForTowerFiltersWithPaging]
(
	@systemID NVARCHAR(50),
	@towerNumber INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @talkgroupIDFrom INT = NULL,
    @talkgroupIDTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE  @FIELD_TOWER_NUMBER NVARCHAR(50) = N'TowerNumber',
        @FIELD_TOWER_NAME NVARCHAR(50) = N'TowerName',
        @FIELD_TALKGROUP_ID NVARCHAR(50) = N'TalkgroupID',
		@FIELD_TALKGROUP_NAME NVARCHAR(50) = N'TalkgroupName',
       	@FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PRIVATE_DATA_COUNT NVARCHAR(50) = N'PrivateDataCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending',
        @systemIDKey INT;

    SELECT @systemIDKey = s.ID
    FROM dbo.Systems s
    WHERE (s.SystemID = @systemID);

    WITH TowerTalkgroupResults (TowerNumber, TowerDescription, TalkgroupID, TalkgroupDescription, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, PrivateDataCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
	    SELECT t.TowerNumber,
		    t.[Description] AS TowerDescription,
		    tg.TalkgroupID,
		    tg.[Description] AS TalkgroupDescription,
		    SUM(ISNULL(ttg.AffiliationCount, 0)) AS AffiliationCount,
		    SUM(ISNULL(ttg.DeniedCount, 0)) AS DeniedCount,
		    SUM(ISNULL(ttg.VoiceGrantCount, 0)) AS VoiceGrantCount,
		    SUM(ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		    SUM(ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
		    SUM(ISNULL(ttg.DataCount, 0)) AS DataCount,
		    SUM(ISNULL(ttg.PrivateDataCount, 0)) AS PrivateDataCount,
		    SUM(ISNULL(ttg.AlertCount, 0)) AS AlertCount,
		    MIN(ttg.FirstSeen) AS FirstSeen,
		    MAX(ttg.LastSeen) AS LastSeen
	    FROM dbo.TowerTalkgroups ttg WITH (NOLOCK)
	    INNER JOIN dbo.Towers t WITH (NOLOCK)
		    ON (t.ID = ttg.TowerID)
	    INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		    ON (tg.ID = ttg.TalkgroupID)
	    WHERE ((tg.SystemID = @systemIDKey)
		    AND (t.TowerNumber = @towerNumber)
            AND ((@dateFrom IS NULL)
                OR (ttg.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (ttg.[Date] <= @dateTo))
            AND ((@talkgroupIDFrom IS NULL)
                OR (tg.TalkgroupID >= @talkgroupIDFrom))
            AND ((@talkgroupIDTo IS NULL)
                OR (tg.TalkgroupID <= @talkgroupIDTo))
		    AND ((@searchText IS NULL)
                OR (tg.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (ttg.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (ttg.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (ttg.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (ttg.LastSeen <= @lastSeenTo)))
	    GROUP BY t.TowerNumber,
		    t.[Description],
		    tg.TalkgroupID,
		    tg.[Description]
    )
    SELECT ttgr.TowerNumber,
        ttgr.TowerDescription, 
        ttgr.TalkgroupID, 
        ttgr.TalkgroupDescription, 
        ttgr.AffiliationCount, 
        ttgr.DeniedCount, 
        ttgr.VoiceGrantCount, 
        ttgr.EmergencyVoiceGrantCount, 
        ttgr.EncryptedVoiceGrantCount, 
        ttgr.DataCount, 
        ttgr.PrivateDataCount, 
        ttgr.AlertCount, 
        ttgr.FirstSeen, 
        ttgr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TowerTalkgroupResults ttgr
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.TowerNumber END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.TowerNumber END DESC,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.TowerDescription END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.TowerDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.TalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.TalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.TalkgroupDescription END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.TalkgroupDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.DataCount END DESC,
		CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.PrivateDataCount END,
		CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.PrivateDataCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.AlertCount END,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.AlertCount END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.LastSeen END DESC,
        ttgr.TowerNumber
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);

END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsGetTowerListForTalkgroup]    Script Date: 2/12/2019 2:50:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[TowerTalkgroupsGetTowerListForTalkgroup]
(
    @systemID NVARCHAR(50),
    @talkgroupID INT,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL
)
AS
BEGIN
    SET NOCOUNT ON

    SELECT t.TowerNumber,
        t.[Description] AS TowerDescription
    FROM dbo.TowerTalkgroups ttg WITH (NOLOCK)
    INNER JOIN dbo.Systems s WITH (NOLOCK)
        ON (s.ID = ttg.SystemID)
    INNER JOIN dbo.Towers t WITH (NOLOCK)
        ON (t.ID = ttg.TowerID)
    INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
        ON (tg.ID = ttg.TalkgroupID)
    WHERE ((s.SystemID = @systemID)
        AND (tg.TalkgroupID = @talkgroupID)
        AND ((@dateFrom IS NULL)
            OR (ttg.[Date] >= @dateFrom))
        AND ((@dateTo IS NULL)
            OR (ttg.[Date] <= @dateTo)))
    GROUP BY t.TowerNumber,
        t.[Description];
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsGetTowersForTalkgroup]    Script Date: 2/12/2019 2:50:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupsGetTowersForTalkgroup]
(
    @systemID INT,
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.TowerNumber,
		t.[Description] AS TowerDescription,
		tg.TalkgroupID,
		tg.[Description] AS TalkgroupDescription,
		SUM(ISNULL(ttg.AffiliationCount, 0)) AS AffiliationCount,
		SUM(ISNULL(ttg.DeniedCount, 0)) AS DeniedCount,
		SUM(ISNULL(ttg.VoiceGrantCount, 0)) AS VoiceGrantCount,
		SUM(ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		SUM(ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
		SUM(ISNULL(ttg.DataCount, 0)) AS DataCount,
		SUM(ISNULL(ttg.PrivateDataCount, 0)) AS PrivateDataCount,
		SUM(ISNULL(ttg.AlertCount, 0)) AS AlertCount,
		MIN(ttg.FirstSeen) AS FirstSeen,
		MAX(ttg.LastSeen) AS LastSeen,
        COUNT(1) OVER() AS RecordCount
	FROM dbo.TowerTalkgroups ttg WITH (NOLOCK)
	INNER JOIN dbo.Towers t WITH (NOLOCK)
		ON (t.ID = ttg.TowerID)
	INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		ON (tg.ID = ttg.TalkgroupID)
	WHERE ((tg.SystemID = @systemID)
        AND (tg.TalkgroupID = @talkgroupID))
	GROUP BY t.TowerNumber,
		t.[Description],
		tg.TalkgroupID,
		tg.[Description];
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsGetTowersForTalkgroupCount]    Script Date: 2/12/2019 2:50:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerTalkgroupsGetTowersForTalkgroupCount]
(
	@systemID INT,
	@talkgroupID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	WITH TowerTalkgroupTowers
	AS
	(
		SELECT ttg.TowerID,
			ttg.TalkgroupID
		FROM dbo.TowerTalkgroups ttg WITH (NOLOCK)
		INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
			ON (tg.ID = ttg.TalkgroupID)
		WHERE ((ttg.SystemID = @systemID)
			AND (tg.TalkgroupID = @talkgroupID))
		GROUP BY ttg.TowerID,
			ttg.TalkgroupID
	)
	SELECT COUNT(*)
	FROM TowerTalkgroupTowers;

END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsGetTowersForTalkgroupFiltersWithPaging]    Script Date: 2/12/2019 2:50:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[TowerTalkgroupsGetTowersForTalkgroupFiltersWithPaging]
(
	@systemID NVARCHAR(50),
	@talkgroupID INT,
    @searchText NVARCHAR(200) = NULL,
    @dateFrom DATETIME2(7) = NULL,
    @dateTo DATETIME2(7) = NULL,
    @towerNumberFrom INT = NULL,
    @towerNumberTo INT = NULL,
    @firstSeenFrom DATETIME2(7) = NULL,
    @firstSeenTo DATETIME2(7) = NULL,
    @lastSeenFrom DATETIME2(7) = NULL,
    @lastSeenTo DATETIME2(7) = NULL,
    @sortField NVARCHAR(50),
    @sortDirection NVARCHAR(50),
	@row INT,
	@pageSize INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE  @FIELD_TOWER_NUMBER NVARCHAR(50) = N'TowerNumber',
        @FIELD_TOWER_NAME NVARCHAR(50) = N'TowerName',
        @FIELD_TALKGROUP_ID NVARCHAR(50) = N'TalkgroupID',
		@FIELD_TALKGROUP_NAME NVARCHAR(50) = N'TalkgroupName',
       	@FIELD_AFFILIATION_COUNT NVARCHAR(50) = N'AffiliationCount',
		@FIELD_DENIED_COUNT NVARCHAR(50) = N'DeniedCount',
		@FIELD_VOICE_COUNT NVARCHAR(50) = N'VoiceCount',
		@FIELD_EMERGENCY_COUNT NVARCHAR(50) = N'EmergencyCount',
		@FIELD_ENCRYPTED_COUNT NVARCHAR(50) = N'EncryptedCount',
        @FIELD_DATA_COUNT NVARCHAR(50) = N'DataCount',
        @FIELD_PRIVATE_DATA_COUNT NVARCHAR(50) = N'PrivateDataCount',
        @FIELD_ALERT_COUNT NVARCHAR(50) = N'AlertCount',
		@FIELD_FIRST_SEEN NVARCHAR(50) = N'FirstSeen',
		@FIELD_LAST_SEEN NVARCHAR(50) = N'LastSeen',
		@DIRECTION_ASCENDING NVARCHAR(50) = N'Ascending',
		@DIRECTION_DESCENDING NVARCHAR(50) = N'Descending';

    WITH TowerTalkgroupResults (TowerNumber, TowerDescription, TalkgroupID, TalkgroupDescription, AffiliationCount, DeniedCount, VoiceGrantCount, EmergencyVoiceGrantCount, EncryptedVoiceGrantCount, DataCount, PrivateDataCount, AlertCount, FirstSeen, LastSeen)
    AS
    (
	    SELECT t.TowerNumber,
		    t.[Description] AS TowerDescription,
		    tg.TalkgroupID,
		    tg.[Description] AS TalkgroupDescription,
		    SUM(ISNULL(ttg.AffiliationCount, 0)) AS AffiliationCount,
		    SUM(ISNULL(ttg.DeniedCount, 0)) AS DeniedCount,
		    SUM(ISNULL(ttg.VoiceGrantCount, 0)) AS VoiceGrantCount,
		    SUM(ISNULL(ttg.EmergencyVoiceGrantCount, 0)) AS EmergencyVoiceGrantCount,
		    SUM(ISNULL(ttg.EncryptedVoiceGrantCount, 0)) AS EncryptedVoiceGrantCount,
		    SUM(ISNULL(ttg.DataCount, 0)) AS DataCount,
		    SUM(ISNULL(ttg.PrivateDataCount, 0)) AS PrivateDataCount,
		    SUM(ISNULL(ttg.AlertCount, 0)) AS AlertCount,
		    MIN(ttg.FirstSeen) AS FirstSeen,
		    MAX(ttg.LastSeen) AS LastSeen
	    FROM dbo.TowerTalkgroups ttg WITH (NOLOCK)
        INNER JOIN dbo.Systems s WITH (NOLOCK)
            ON (s.ID = ttg.SystemID)
	    INNER JOIN dbo.Towers t WITH (NOLOCK)
		    ON (t.ID = ttg.TowerID)
	    INNER JOIN dbo.Talkgroups tg WITH (NOLOCK)
		    ON (tg.ID = ttg.TalkgroupID)
	    WHERE ((s.SystemID = @systemID)
		    AND (tg.TalkgroupID = @talkgroupID)
            AND ((@dateFrom IS NULL)
                OR (ttg.[Date] >= @dateFrom))
            AND ((@dateTo IS NULL)
                OR (ttg.[Date] <= @dateTo))
            AND ((@towerNumberFrom IS NULL)
                OR (t.TowerNumber >= @towerNumberFrom))
            AND ((@towerNumberTo IS NULL)
                OR (t.TowerNumber <= @towerNumberTo))
		    AND ((@searchText IS NULL)
                OR (t.[Description] LIKE N'%' + @searchText + N'%'))
            AND ((@firstSeenFrom IS NULL)
                OR (ttg.FirstSeen >= @firstSeenFrom))
            AND ((@firstSeenTo IS NULL)
                OR (ttg.FirstSeen <= @firstSeenTo))
            AND ((@lastSeenFrom IS NULL)
                OR (ttg.LastSeen >= @lastSeenFrom))
            AND ((@lastSeenTo IS NULL)
                OR (ttg.LastSeen <= @lastSeenTo)))
	    GROUP BY t.TowerNumber,
		    t.[Description],
		    tg.TalkgroupID,
		    tg.[Description]
    )
    SELECT ttgr.TowerNumber,
        ttgr.TowerDescription, 
        ttgr.TalkgroupID, 
        ttgr.TalkgroupDescription, 
        ttgr.AffiliationCount, 
        ttgr.DeniedCount, 
        ttgr.VoiceGrantCount, 
        ttgr.EmergencyVoiceGrantCount, 
        ttgr.EncryptedVoiceGrantCount, 
        ttgr.DataCount, 
        ttgr.PrivateDataCount, 
        ttgr.AlertCount, 
        ttgr.FirstSeen, 
        ttgr.LastSeen,
        COUNT(1) OVER() AS RecordCount
    FROM TowerTalkgroupResults ttgr
	ORDER BY
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.TowerNumber END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NUMBER) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.TowerNumber END DESC,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.TowerDescription END,
		CASE WHEN ((@sortField = @FIELD_TOWER_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.TowerDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.TalkgroupID END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_ID) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.TalkgroupID END DESC,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.TalkgroupDescription END,
		CASE WHEN ((@sortField = @FIELD_TALKGROUP_NAME) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.TalkgroupDescription END DESC,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.AffiliationCount END,
		CASE WHEN ((@sortField = @FIELD_AFFILIATION_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.AffiliationCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.DeniedCount END,
		CASE WHEN ((@sortField = @FIELD_DENIED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.DeniedCount END DESC,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.VoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_VOICE_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.VoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.EmergencyVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_EMERGENCY_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.EmergencyVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.EncryptedVoiceGrantCount END,
		CASE WHEN ((@sortField = @FIELD_ENCRYPTED_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.EncryptedVoiceGrantCount END DESC,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.DataCount END,
		CASE WHEN ((@sortField = @FIELD_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.DataCount END DESC,
		CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.PrivateDataCount END,
		CASE WHEN ((@sortField = @FIELD_PRIVATE_DATA_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.PrivateDataCount END DESC,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.AlertCount END,
		CASE WHEN ((@sortField = @FIELD_ALERT_COUNT) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.AlertCount END DESC,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.FirstSeen END,
		CASE WHEN ((@sortField = @FIELD_FIRST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.FirstSeen END DESC,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_ASCENDING)) THEN ttgr.LastSeen END,
		CASE WHEN ((@sortField = @FIELD_LAST_SEEN) AND (@sortDirection = @DIRECTION_DESCENDING)) THEN ttgr.LastSeen END DESC,
        ttgr.TowerNumber
	OFFSET ((@row - 1) * @pageSize) ROWS
	FETCH NEXT @pageSize ROWS ONLY
    OPTION (RECOMPILE);
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsInsert]    Script Date: 2/12/2019 2:50:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupsInsert]
(
	@systemID INT,
	@towerNumber INT,
	@talkgroupID INT,
	@date DATETIME2(7),
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.TowerTalkgroups
	(
		SystemID,
		TowerID,
		TalkgroupID,
		[Date],
		AffiliationCount,
		DeniedCount,
		VoiceGrantCount,
		EmergencyVoiceGrantCount,
		EncryptedVoiceGrantCount,
		DataCount,
		PrivateDataCount,
		AlertCount,
		FirstSeen,
		LastSeen
	)
	VALUES
	(
		@systemID,
		@towerNumber,
		@talkgroupID,
		@date,
		@affiliationCount,
		@deniedCount,
		@voiceGrantCount,
		@emergencyVoiceGrantCount,
		@encryptedVoiceGrantCount,
		@dataCount,
		@privateDataCount,
		@alertCount,
		@firstSeen,
		@lastSeen
	);

	SELECT SCOPE_IDENTITY() AS ID;
END
GO

/****** Object:  StoredProcedure [dbo].[TowerTalkgroupsUpdate]    Script Date: 2/12/2019 2:50:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TowerTalkgroupsUpdate]
(
	@id INT,
	@date DATETIME2(7),
	@affiliationCount INT,
	@deniedCount INT,
	@voiceGrantCount INT,
	@emergencyVoiceGrantCount INT,
	@encryptedVoiceGrantCount INT,
	@dataCount INT,
	@privateDataCount INT,
	@alertCount INT,
	@firstSeen DATETIME2(7),
	@lastSeen DATETIME2(7)
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.TowerTalkgroups
	SET AffiliationCount = @affiliationCount,
		DeniedCount = @deniedCount,
		VoiceGrantCount = @voiceGrantCount,
		EmergencyVoiceGrantCount = @emergencyVoiceGrantCount,
		EncryptedVoiceGrantCount = @encryptedVoiceGrantCount,
		DataCount = @dataCount,
		PrivateDataCount = @privateDataCount,
		AlertCount = @alertCount,
		FirstSeen = @firstSeen,
		LastSeen = @lastSeen,
		LastModified = GETDATE()
	WHERE (ID = @id);
END
GO


