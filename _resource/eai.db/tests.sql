DECLARE @Id_Process INT;
DECLARE @Id_Log INT;

EXEC dbo.up_AddProcess
		@Id_ParentProcess	= NULL
		,@Stage				= 'DEV'
		,@MessageKey		= 'Test:1234'
		,@InvocationId		= '1234567890ABCDEF1234567890ABCDEF'
		,@Id_Process		= @Id_Process OUTPUT;


EXEC dbo.up_UpdateProcess
		@Id_Process			= @Id_Process
		,@Id_ParentProcess	= @Id_Process
		,@Stage				= NULL
		,@MessageKey		= NULL	
		,@InvocationId		= NULL;

EXEC dbo.up_AddLog
		@Id_Process			= @Id_Process		
		,@LogLevel			= 'Info'
		,@MessageKey		= 'Test:1234'
		,@LogText			= 'Start'
		,@MessageType		= NULL
		,@MessageHash		= NULL	
		,@MessageContent	= NULL	
		,@Id_Log			= @Id_Log OUTPUT;

EXEC dbo.up_AddLog
		@Id_Process			= @Id_Process		
		,@LogLevel			= 'Debug'
		,@MessageKey		= 'Test:1234'
		,@LogText			= 'Log Test'
		,@MessageType		= NULL
		,@MessageHash		= NULL	
		,@MessageContent	= NULL	
		,@Id_Log			= @Id_Log OUTPUT;

EXEC dbo.up_AddLog
		@Id_Process			= @Id_Process		
		,@LogLevel			= 'Debug'
		,@MessageKey		= 'Test:1234'
		,@LogText			= 'Message Test'
		,@MessageType		= 'Message.Test'
		,@MessageHash		= 0x48656C6C6F20576F
		,@MessageContent	= '{''json'':''Message''}'
		,@Id_Log			= @Id_Log OUTPUT;

EXEC dbo.up_SetProcessFinished
		@Id_Process			= @Id_Process		
		,@IsSuccess			= 1
		,@IsFailed			= 0;



/*

SELECT	*	FROM	dbo.tStage		
SELECT	*	FROM	dbo.tLogLevel	
SELECT	*	FROM	dbo.tService	
SELECT	*	FROM	dbo.tProcess	
SELECT	*	FROM	dbo.tLog		
SELECT	*	FROM	dbo.tMessage	

*/


CREATE VIEW
	dbo.vProcess
AS
	SELECT	p.CreatedOnUTC
			,p.Id
			,st.Name	Stage
			,se.Name	Service
			,p.MessageKey
			,p.IsFailed
			,p.IsSuccess
			,p.FinishedOnUTC
	FROM	dbo.tProcess p
			JOIN	dbo.tStage st
				ON	st.Id = p.Id_Stage
			JOIN	dbo.tService se
				ON	se.Id = p.Id_Service
GO

ALTER VIEW
	dbo.vLog
AS
	SELECT	p.CreatedOnUTC
			,l.Id
			,p.Id		Id_Process
			,st.Name	Stage
			,se.Name	Service
			,ll.Name	LogLevel
			,l.MessageKey
			,l.LogText
			,m.Type		MessageType
			,m.Content	MessageContent
	FROM	dbo.tProcess p
			JOIN	dbo.tStage st
				ON	st.Id = p.Id_Stage
			JOIN	dbo.tService se
				ON	se.Id = p.Id_Service
			LEFT JOIN	(
					dbo.tLog l
					JOIN	dbo.tLogLevel ll
						ON	ll.Id = l.Id_LogLevel
					LEFT JOIN
							dbo.tMessage m
						ON	m.Id = l.Id_Message
				)
				ON	l.Id_Process = p.Id
				AND	l.Id_Stage = p.Id_Stage
			
GO


SELECT	*
FROM	dbo.vProcess p
ORDER BY
		p.CreatedOnUTC DESC


SELECT	*
FROM	dbo.vLog l
ORDER BY
		l.Id DESC



SELECT	*	
FROM	dbo.tProcess	
ORDER BY
		CreatedOnUTC DESC
