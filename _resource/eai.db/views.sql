/*

DROP VIEW dbo.vProcess
DROP VIEW dbo.vLog

*/


IF OBJECT_ID('dbo.vProcess') IS NULL
BEGIN
	EXEC('
		
		CREATE VIEW
			dbo.vProcess
		AS
			SELECT ''Hello wordl!'' HelloWorld;

	');
END
GO

ALTER VIEW
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

IF OBJECT_ID('dbo.vLog') IS NULL
BEGIN
	EXEC('
		
		CREATE VIEW
			dbo.vLog
		AS
			SELECT ''Hello wordl!'' HelloWorld;

	');
END
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
			,l.MessageName
			,m.Type		MessageType
			,CAST(m.Content AS XML)
						MessageContent
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



/*

SELECT	*
FROM	dbo.tException
WHERE	Id_Log = 196


SELECT	TOP 100
		*
FROM	dbo.vProcess p
ORDER BY
		p.CreatedOnUTC DESC


SELECT	TOP 100
		*
FROM	dbo.vLog l
WHERE	l.Id > 0
ORDER BY
		l.Id DESC



SELECT	*	
FROM	dbo.tProcess	
ORDER BY
		CreatedOnUTC DESC

*/