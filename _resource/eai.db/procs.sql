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

IF OBJECT_ID('dbo.up_AddMessage') IS NULL
BEGIN
	EXEC('
		
		CREATE PROCEDURE
			dbo.up_AddMessage
		AS
			PRINT ''up_AddMessage'';

	');
END
GO

ALTER PROCEDURE
	dbo.up_AddMessage
		@MessageType		NVARCHAR(250)
		,@MessageHash		BINARY(8)
		,@MessageContentType
							NVARCHAR(5)
		,@MessageContent	NVARCHAR(MAX)
		,@CreatedOnUTC		DATETIME		

		,@Id_Message		INT					OUTPUT

AS

		PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddMessage/Message'; 

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

		PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddMessage/@Id_Message = ' + ISNULL(LTRIM(@Id_Message), 'NULL'); 
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

	if @MessageContent IS NOT NULL
	BEGIN

		EXEC dbo.up_AddMessage 
				@MessageType		 = @MessageType		
				,@MessageHash		 = @MessageHash		
				,@MessageContentType = @MessageContentType
				,@MessageContent	 = @MessageContent	
				,@CreatedOnUTC		 = @CreatedOnUTC		
				,@Id_Message		 = @Id_Message		OUTPUT;

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
GO

IF OBJECT_ID('dbo.up_AddQueue') IS NULL
BEGIN
	EXEC('
		
		CREATE PROCEDURE
			dbo.up_AddQueue
		AS
			PRINT ''up_AddQueue'';

	');
END
GO

ALTER PROCEDURE
	dbo.up_AddQueue
		@Id_Process			NVARCHAR(50)
		,@Stage				NVARCHAR(20)
		,@EndpointName		NVARCHAR(100)
		,@Id_Status			TINYINT
		,@MessageKey		NVARCHAR(100)
		,@MessageHash		BINARY(8)
		,@MessageType		NVARCHAR(250)
		,@MessageContentType
							NVARCHAR(5)
		,@MessageContent	NVARCHAR(MAX)
		,@CreatedOnUTC		DATETIME		

		,@Id_Queue			INT					OUTPUT
AS
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddQueue/start'; 

	DECLARE @Id_Stage		TINYINT;
	DECLARE @Id_Message		BIGINT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddQueue/Stage'; 

	EXEC dbo.up_GetStage
			@Name		= @Stage
			,@CreatedOnUTC
						= @CreatedOnUTC
			,@Id_Stage	= @Id_Stage OUTPUT;


	if @MessageContent IS NOT NULL
	BEGIN

		EXEC dbo.up_AddMessage 
				@MessageType		 = @MessageType		
				,@MessageHash		 = @MessageHash		
				,@MessageContentType = @MessageContentType
				,@MessageContent	 = @MessageContent	
				,@CreatedOnUTC		 = @CreatedOnUTC		
				,@Id_Message		 = @Id_Message		OUTPUT;

	END

	INSERT INTO
			dbo.tQueue
			(Id_Process, Id_Stage, EndpointName, Id_Status, MessageType, MessageKey, MessageHash, Id_Message, Id_PrevDuplicateKey, Id_PrevDuplicateHash, CreatedOnUTC)
	SELECT	@Id_Process
			,@Id_Stage
			,@EndpointName
			,@Id_Status
			,@MessageType
			,@MessageKey
			,@MessageHash
			,@Id_Message
			,(
				SELECT	MAX(q.Id)
				FROM	dbo.tQueue q
				WHERE	q.Id > 0
					AND	q.EndpointName = @EndpointName
					AND	q.Id_Stage = @Id_Stage
					AND	q.MessageKey = @MessageKey
			)
			,(
				SELECT	MAX(q.Id)
				FROM	dbo.tQueue q
				WHERE	q.Id > 0
					AND	q.EndpointName = @EndpointName
					AND	q.Id_Stage = @Id_Stage
					AND	q.MessageHash = @MessageHash
			)
			,@CreatedOnUTC;

	SELECT	@Id_Queue = SCOPE_IDENTITY();
	
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_AddQueue/finish'; 
GO

IF OBJECT_ID('dbo.up_UpdateQueueStatus') IS NULL
BEGIN
	EXEC('
		
		CREATE PROCEDURE
			dbo.up_UpdateQueueStatus
		AS
			PRINT ''up_UpdateQueueStatus'';

	');
END
GO

ALTER PROCEDURE
	dbo.up_UpdateQueueStatus
		@Id_Queue			BIGINT
		,@Id_Status			TINYINT

AS
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_UpdateQueueStatus/start'; 

	UPDATE	q
	SET		Id_Status = @Id_Status
	FROM	dbo.tQueue q
	WHERE	q.Id = @Id_Queue;
	
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_UpdateQueueStatus/finish'; 
GO

IF OBJECT_ID('dbo.up_Dequeue') IS NULL
BEGIN
	EXEC('
		
		CREATE PROCEDURE
			dbo.up_Dequeue
		AS
			PRINT ''up_Dequeue'';

	');
END
GO

ALTER PROCEDURE
	dbo.up_Dequeue
		@EndpointName		NVARCHAR(100)
		,@Stage				NVARCHAR(20)
		,@Id_StatusEnqueued	TINYINT
		,@Id_StatusTimeout	TINYINT
		,@Id_StatusProcessing
							TINYINT
		,@CreatedOnUTC		DATETIME		


AS
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_Dequeue/start'; 

	DECLARE @Id_Stage		TINYINT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_Dequeue/Stage'; 

	EXEC dbo.up_GetStage
			@Name		= @Stage
			,@CreatedOnUTC
						= @CreatedOnUTC
			,@Id_Stage	= @Id_Stage OUTPUT;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_Dequeue/timeout'; 

	UPDATE	q
	SET		Id_Status = @Id_StatusTimeout
	FROM	dbo.tQueue q
			JOIN	dbo.tQueueConfiguration qc
				ON	qc.EndpointName = q.EndpointName
	WHERE	q.Id > 0
		AND	q.Id_Status = @Id_StatusProcessing
		AND	q.EndpointName = @EndpointName
		AND	q.Id_Stage = @Id_Stage
		AND	DATEDIFF(SECOND, q.DequeuedOnUTC, GETUTCDATE()) > qc.TimeoutSeconds;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_Dequeue/prepare'; 

	IF OBJECT_ID('tempdb..#q') IS NULL
	BEGIN

		CREATE TABLE
			#q
		(
			Id				INT		NOT NULL PRIMARY KEY,
			MaxTickets		INT		NOT NULL,
			TimeoutSeconds	INT		NOT NULL,
			CurrentTickets	INT		NOT NULL
		)

	END

	INSERT INTO
			#q
			(Id,  MaxTickets, TimeoutSeconds, CurrentTickets)

	SELECT	1,
			qc.MaxTickets,
			qc.TimeoutSeconds,
			ISNULL((
					SELECT	COUNT(*)
					FROM	dbo.tQueue q
					WHERE	q.Id > 0
						AND	q.EndpointName = qc.EndpointName
						AND q.Id_Status = @Id_StatusProcessing
			), 0)	CurrentTickets
	FROM	dbo.tQueueConfiguration qc
	WHERE	qc.EndpointName = @EndpointName
		AND qc.Id_Stage = @Id_Stage;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_Dequeue/dequeue'; 

	DECLARE @MaxTickets		TINYINT = 0;

	SELECT	@MaxTickets = q.MaxTickets - q.CurrentTickets
	FROM	#q q
	WHERE	q.Id = 1;

	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/up_Dequeue/@MaxTickets = ' + LTRIM(@MaxTickets); 

	WITH dequeue AS (
		SELECT	TOP (@MaxTickets)
				q.Id
		FROM	dbo.tQueue q
		WHERE	q.Id > 0
			AND	q.Id_Status = @Id_StatusEnqueued
			AND	q.EndpointName = @EndpointName
			AND	q.Id_Stage = @Id_Stage
			AND	(
					SELECT	q_key.Id
					FROM	dbo.tQueue q_key
					WHERE	q_key.Id = q.Id_PrevDuplicateKey
						AND	q_key.Id_Status IN (@Id_StatusEnqueued, @Id_StatusProcessing)
				) IS NULL
			AND	(
					SELECT	q_hash.Id
					FROM	dbo.tQueue q_hash
					WHERE	q_hash.Id = q.Id_PrevDuplicateHash
						AND	q_hash.Id_Status IN (@Id_StatusEnqueued, @Id_StatusProcessing)
				) IS NULL
		ORDER BY
				q.Id ASC
	)
	UPDATE	q_upd
	SET		Id_Status = @Id_StatusProcessing
			,DequeuedOnUTC = @CreatedOnUTC
	OUTPUT	inserted.Id
			,inserted.Id_Process
			,s.Name	Stage
			,inserted.EndpointName
			,inserted.Id_Status
			,inserted.MessageKey
			,inserted.MessageHash
			,inserted.MessageType
			,m.ContentType
			,m.Content
			,inserted.CreatedOnUTC
	FROM	dbo.tQueue q_upd
			JOIN	dbo.tMessage m
				ON	m.Id = q_upd.Id_Message
			JOIN	dbo.tStage s
				ON	s.Id = q_upd.Id_Stage
	WHERE	q_upd.Id > 0
		AND	q_upd.Id IN (
				SELECT	d.Id
				FROM	dequeue d
			)
	
	PRINT CONVERT(NVARCHAR(29), GETUTCDATE(), 120) + '/@EndpointName/finish'; 
GO

