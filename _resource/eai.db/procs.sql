IF OBJECT_ID('dbo.up_GetStage') IS NULL
BEGIN
	EXEC('
		
		CREATE PROCEDURE
			dbo.up_GetStage
		AS
			PRINT ''up_GetStage'';

	');
END
GO

ALTER PROCEDURE
	dbo.up_GetStage
		@Name				NVARCHAR(20)
		,@CreatedOnUTC		DATETIME		
		,@Id_Stage			INT					OUTPUT
AS
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_GetStage/Start'; 

	INSERT INTO
			dbo.tStage
			(Name, Description, CreatedOnUTC)
	SELECT	@Name
			,@Name
			,@CreatedOnUTC
	WHERE	@Name IS NOT NULL
		AND	NOT EXISTS (
				SELECT	*
				FROM	dbo.tStage s
				WHERE	s.Id > 0
					AND	s.Name = @Name
		);

	SELECT	@Id_Stage = ISNULL(
				SCOPE_IDENTITY(), 				
				(
					SELECT	MAX(s.Id)
					FROM	dbo.tStage s
					WHERE	s.Id > 0
						AND	s.Name = @Name
						AND @Name IS NOT NULL
				));

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_GetStage/@Id_Stage = ' + ISNULL(LTRIM(@Id_Stage), 'NULL'); 
GO

IF OBJECT_ID('dbo.up_GetLogLevel') IS NULL
BEGIN
	EXEC('
		
		CREATE PROCEDURE
			dbo.up_GetLogLevel
		AS
			PRINT ''up_GetLogLevel'';

	');
END
GO

ALTER PROCEDURE
	dbo.up_GetLogLevel
		@Name				NVARCHAR(20)
		,@CreatedOnUTC		DATETIME		
		,@Id_LogLevel		INT					OUTPUT
AS
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_GetLogLevel/Start'; 

	INSERT INTO
			dbo.tLogLevel
			(Name, Description, CreatedOnUTC)
	SELECT	@Name
			,@Name
			,@CreatedOnUTC
	WHERE	@Name IS NOT NULL
		AND	NOT EXISTS (
				SELECT	*
				FROM	dbo.tLogLevel s
				WHERE	s.Id > 0
					AND	s.Name = @Name
		);

	SELECT	@Id_LogLevel = ISNULL(
				SCOPE_IDENTITY(), 				
				(
					SELECT	MAX(s.Id)
					FROM	dbo.tLogLevel s
					WHERE	s.Id > 0
						AND	s.Name = @Name
						AND @Name IS NOT NULL
				));

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_GetLogLevel/@Id_LogLevel = ' + ISNULL(LTRIM(@Id_LogLevel), 'NULL'); 
GO

IF OBJECT_ID('dbo.up_GetService') IS NULL
BEGIN
	EXEC('
		
		CREATE PROCEDURE
			dbo.up_GetService
		AS
			PRINT ''up_GetService'';

	');
END
GO

ALTER PROCEDURE
	dbo.up_GetService
		@Name				NVARCHAR(100)
		,@CreatedOnUTC		DATETIME		
		,@Id_Service		INT					OUTPUT
AS
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_GetService/Start'; 

	INSERT INTO
			dbo.tService
			(Name, Description, CreatedOnUTC)
	SELECT	@Name
			,@Name
			,@CreatedOnUTC
	WHERE	@Name IS NOT NULL
		AND	NOT EXISTS (
				SELECT	*
				FROM	dbo.tService s
				WHERE	s.Id > 0
					AND	s.Name = @Name
		);

	SELECT	@Id_Service = ISNULL(
				SCOPE_IDENTITY(), 				
				(
					SELECT	MAX(s.Id)
					FROM	dbo.tService s
					WHERE	s.Id > 0
						AND	s.Name = @Name
						AND @Name IS NOT NULL
				));

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_GetService/@Id_Service = ' + ISNULL(LTRIM(@Id_Service), 'NULL'); 
GO


IF OBJECT_ID('dbo.up_AddProcess') IS NULL
BEGIN
	EXEC('
		
		CREATE PROCEDURE
			dbo.up_AddProcess
		AS
			PRINT ''up_AddProcess'';

	');
END
GO

ALTER PROCEDURE
	dbo.up_AddProcess
		@Id					NVARCHAR(50)
		,@Stage				NVARCHAR(20)
		,@Id_ParentProcess	NVARCHAR(50)
		,@ParentStage		NVARCHAR(20)
		,@Service			NVARCHAR(250)
		,@MessageKey		NVARCHAR(100)
		,@CreatedOnUTC		DATETIME		
AS
	
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddProcess/start'; 

	DECLARE @Id_Stage		TINYINT;
	DECLARE @Id_ParentStage	TINYINT;
	DECLARE @Id_Service		SMALLINT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddProcess/Stage'; 

	EXEC dbo.up_GetStage
			@Name		= @Stage
			,@CreatedOnUTC
						= @CreatedOnUTC
			,@Id_Stage	= @Id_Stage OUTPUT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddProcess/ParentStage'; 

	EXEC dbo.up_GetStage
			@Name		= @ParentStage
			,@CreatedOnUTC
						= @CreatedOnUTC
			,@Id_Stage	= @Id_ParentStage OUTPUT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddProcess/Service'; 

	EXEC dbo.up_GetService
			@Name		= @Service
			,@CreatedOnUTC
						= @CreatedOnUTC
			,@Id_Service= @Id_Service OUTPUT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddProcess/Insert'; 

	INSERT INTO
			dbo.tProcess
			(Id, Id_Stage, Id_ParentProcess, Id_ParentStage, MessageKey, Id_Service, CreatedOnUTC)
	SELECT	@Id
			,@Id_Stage
			,@Id_ParentProcess
			,@Id_ParentStage
			,@MessageKey
			,@Id_Service
			,@CreatedOnUTC;
	
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddProcess/finish'; 
GO

IF OBJECT_ID('dbo.up_UpdateProcess') IS NULL
BEGIN
	EXEC('
		
		CREATE PROCEDURE
			dbo.up_UpdateProcess
		AS
			PRINT ''up_UpdateProcess'';

	');
END
GO

ALTER PROCEDURE
	dbo.up_UpdateProcess
		@Id					NVARCHAR(50)
		,@Stage				NVARCHAR(20)
		,@Id_ParentProcess	NVARCHAR(50)
		,@ParentStage		NVARCHAR(20)
		,@MessageKey		NVARCHAR(100)
		,@CreatedOnUTC		DATETIME		

AS
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_UpdateProcess/start'; 

	DECLARE @Id_Stage		TINYINT;
	DECLARE @Id_ParentStage	TINYINT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_UpdateProcess/Stage'; 

	EXEC dbo.up_GetStage
			@Name		= @Stage
			,@CreatedOnUTC
						= @CreatedOnUTC
			,@Id_Stage	= @Id_Stage OUTPUT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_UpdateProcess/ParentStage'; 

	EXEC dbo.up_GetStage
			@Name		= @ParentStage
			,@CreatedOnUTC
						= @CreatedOnUTC
			,@Id_Stage	= @Id_ParentStage OUTPUT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_UpdateProcess/Update'; 

	UPDATE	p
	SET		Id_ParentProcess = ISNULL(@Id_ParentProcess, p.Id_ParentProcess),
			Id_ParentStage = ISNULL(@Id_Stage, p.Id_ParentStage),
			MessageKey = ISNULL(@MessageKey, p.MessageKey)
	FROM	dbo.tProcess p
	WHERE	p.Id = @Id
		AND	p.Id_Stage = @Id_Stage;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_UpdateProcess/finish'; 

GO

IF OBJECT_ID('dbo.up_SetProcessFinished') IS NULL
BEGIN
	EXEC('
		
		CREATE PROCEDURE
			dbo.up_SetProcessFinished
		AS
			PRINT ''up_SetProcessFinished'';

	');
END
GO

ALTER PROCEDURE
	dbo.up_SetProcessFinished
		@Id					NVARCHAR(50)
		,@Stage				NVARCHAR(20)
		,@IsSuccess			BIT
		,@IsFailed			BIT
		,@FinishedOnUTC		DATETIME		

AS
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_SetProcessFinished/start'; 

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_SetProcessFinished/start'; 

	DECLARE @Id_Stage		TINYINT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_SetProcessFinished/Stage'; 

	EXEC dbo.up_GetStage
			@Name		= @Stage
			,@CreatedOnUTC
						= @FinishedOnUTC
			,@Id_Stage	= @Id_Stage OUTPUT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_SetProcessFinished/Update'; 

	UPDATE	p
	SET		IsSuccess = @IsSuccess,
			IsFailed = @IsFailed,
			FinishedOnUTC = @FinishedOnUTC
	FROM	dbo.tProcess p
	WHERE	p.Id = @Id
		AND	p.Id_Stage = @Id_Stage;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_SetProcessFinished/finish'; 

GO


IF OBJECT_ID('dbo.up_AddLog') IS NULL
BEGIN
	EXEC('
		
		CREATE PROCEDURE
			dbo.up_AddLog
		AS
			PRINT ''up_AddLog'';

	');
END
GO

ALTER PROCEDURE
	dbo.up_AddLog
		@Id_Process			NVARCHAR(50)
		,@Stage				NVARCHAR(20)
		,@LogLevel			NVARCHAR(20)
		,@MessageKey		NVARCHAR(100)
		,@LogText			NVARCHAR(500)
		,@MessageName		NVARCHAR(100)
		,@MessageType		NVARCHAR(250)
		,@MessageHash		BINARY(8)
		,@MessageContentType
							NVARCHAR(5)
		,@MessageContent	NVARCHAR(MAX)
		,@CreatedOnUTC		DATETIME		

		,@Id_Log			INT					OUTPUT
AS
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddLog/start'; 

	DECLARE @Id_Stage		TINYINT;
	DECLARE @Id_LogLevel	TINYINT;
	DECLARE @Id_Message		BIGINT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddLog/Stage'; 

	EXEC dbo.up_GetStage
			@Name		= @Stage
			,@CreatedOnUTC
						= @CreatedOnUTC
			,@Id_Stage	= @Id_Stage OUTPUT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddLog/LogLevel'; 

	EXEC dbo.up_GetLogLevel
			@Name		= @LogLevel
			,@CreatedOnUTC
						= @CreatedOnUTC
			,@Id_LogLevel= @Id_LogLevel OUTPUT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddLog/@Id_LogLevel = ' + ISNULL(LTRIM(@Id_LogLevel), 'NULL'); 

	if @MessageContent IS NOT NULL
	BEGIN

		PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddLog/Message'; 

		DECLARE @MessageChecksum INT = BINARY_CHECKSUM(@MessageContent);

		INSERT INTO
				dbo.tMessage
				(Type, Hash, Checksum, ContentType, Content, CreatedOnUTC)
		SELECT	ISNULL(@MessageType, '')
				,@MessageHash
				,@MessageChecksum
				,@MessageContentType
				,@MessageContent
				,@CreatedOnUTC
		WHERE	NOT EXISTS (
					SELECT	m.Id
					FROM	dbo.tMessage m
					WHERE	m.Id > 0
						AND	m.Hash = @MessageHash
						AND	m.Checksum = @MessageChecksum
				);

		SELECT	@Id_Message = ISNULL(
					SCOPE_IDENTITY(), 				
					(
						SELECT	MAX(m.Id)
						FROM	dbo.tMessage m
					WHERE	m.Id > 0
						AND	m.Hash = @MessageHash
						AND	m.Checksum = @MessageChecksum
					));

		PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddLog/@Id_Message = ' + ISNULL(LTRIM(@Id_Message), 'NULL'); 

	END


	INSERT INTO
			dbo.tLog
			(Id_Process, Id_Stage, Id_LogLevel, MessageKey, LogText, MessageName, Id_Message, CreatedOnUTC)
	SELECT	@Id_Process
			,@Id_Stage
			,@Id_LogLevel
			,@MessageKey
			,@LogText
			,@MessageName
			,@Id_Message
			,@CreatedOnUTC;

	SELECT	@Id_Log = SCOPE_IDENTITY();
	
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddLog/finish'; 
GO

IF OBJECT_ID('dbo.up_AddException') IS NULL
BEGIN
	EXEC('
		
		CREATE PROCEDURE
			dbo.up_AddException
		AS
			PRINT ''up_AddException'';

	');
END
GO

ALTER PROCEDURE
	dbo.up_AddException
		@Id_Log				BIGINT
		,@Level				INT				
		,@Type				NVARCHAR(250)	
		,@Message			NVARCHAR(1000)	
		,@Source			NVARCHAR(250)	
		,@StackTrace		NVARCHAR(MAX)	
		,@TargetSite		NVARCHAR(250)	
		,@HResult			INT				
		,@CreatedOnUTC		DATETIME		
AS

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddLog/start'; 

	INSERT INTO
		dbo.tException
		(Id_Log, Level, Type, Message, Source, StackTrace, TargetSite, HResult, CreatedOnUTC)
	SELECT	@Id_Log			
			,@Level			
			,@Type			
			,@Message		
			,@Source		
			,@StackTrace	
			,@TargetSite	
			,@HResult		
			,@CreatedOnUTC;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddLog/finish'; 
