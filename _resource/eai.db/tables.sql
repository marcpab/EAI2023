/*

IF OBJECT_ID('dbo.tStage')		IS NOT NULL BEGIN DROP TABLE dbo.tStage		END
IF OBJECT_ID('dbo.tLogLevel')	IS NOT NULL BEGIN DROP TABLE dbo.tLogLevel	END
IF OBJECT_ID('dbo.tService')	IS NOT NULL BEGIN DROP TABLE dbo.tService	END
IF OBJECT_ID('dbo.tProcess')	IS NOT NULL BEGIN DROP TABLE dbo.tProcess	END
IF OBJECT_ID('dbo.tLog')		IS NOT NULL BEGIN DROP TABLE dbo.tLog		END
IF OBJECT_ID('dbo.tMessage')	IS NOT NULL BEGIN DROP TABLE dbo.tMessage	END
IF OBJECT_ID('dbo.tException')	IS NOT NULL BEGIN DROP TABLE dbo.tException	END

*/


CREATE TABLE dbo.tStage
(
	Id					TINYINT				NOT NULL	IDENTITY (1, 1),
	Name				NVARCHAR(20)	NOT NULL,
	Description			NVARCHAR(200)	NOT NULL	CONSTRAINT DF_tStage_Description	DEFAULT(''),
	CreatedOnUTC		DATETIME		NOT NULL,

	CONSTRAINT PK_tStage PRIMARY KEY	(id),
	CONSTRAINT UK_tStage UNIQUE			(Name)
)
GO

CREATE TABLE dbo.tLogLevel
(
	Id					TINYINT			NOT NULL	IDENTITY (1, 1),
	Name				NVARCHAR(100)	NOT NULL,
	Description			NVARCHAR(200)	NOT NULL	CONSTRAINT DF_tLogLevel_Description	DEFAULT(''),
	CreatedOnUTC		DATETIME		NOT NULL,

	CONSTRAINT PK_tLogLevel PRIMARY KEY (id),
	CONSTRAINT UK_tLogLevel UNIQUE		(Name)
)
GO

CREATE TABLE dbo.tService
(
	Id					SMALLINT		NOT NULL	IDENTITY (1, 1),
	Name				NVARCHAR(100)	NOT NULL,
	Description			NVARCHAR(200)	NOT NULL,
	CreatedOnUTC		DATETIME		NOT NULL,

	CONSTRAINT PK_tService PRIMARY KEY	(Id),
	CONSTRAINT UK_tService UNIQUE		(Name)
)
GO

CREATE TABLE dbo.tProcess
(
	Id					NVARCHAR(50)	NOT NULL,
	Id_Stage			TINYINT			NOT NULL,

	Id_ParentProcess	NVARCHAR(50)	NULL,
	Id_ParentStage		TINYINT			NULL,

	Id_Service			SMALLINT		NOT NULL,

	MessageKey			NVARCHAR(100)	NULL,

	IsSuccess			BIT				NOT NULL	CONSTRAINT DF_tProcess_IsSuccess	DEFAULT(0),
	IsFailed			BIT				NOT NULL	CONSTRAINT DF_tProcess_IsFailed		DEFAULT(0),
	
	CreatedOnUTC		DATETIME		NOT NULL,
	FinishedOnUTC		DATETIME		NULL,

	CONSTRAINT PK_tProcess	PRIMARY KEY		(Id, Id_Stage),
	--CONSTRAINT FK_tProcess_tProcess
	--						FOREIGN KEY		(Id_ParentProcess, Id_ParentStage)	
	--													REFERENCES	dbo.tProcess	(Id, Id_Stage),
	CONSTRAINT FK_tProcess_tStage
							FOREIGN KEY		(Id_Stage)	REFERENCES	dbo.tStage		(Id),

	CONSTRAINT FK_tProcess_tStage_Parent
							FOREIGN KEY		(Id_ParentStage)
														REFERENCES	dbo.tStage		(Id),
	CONSTRAINT FK_tProcess_tStervice
							FOREIGN KEY		(Id_Service)
														REFERENCES	dbo.tService	(Id),
)
GO

CREATE TABLE dbo.tMessage
(
	Id					BIGINT			NOT NULL	IDENTITY (1, 1),
	Type				NVARCHAR(250)	NOT NULL,
	Hash				BINARY(8)		NOT NULL,
	Checksum			INT				NOT NULL,
	ContentType			NVARCHAR(4)		NOT NULL,
	Content				NVARCHAR(MAX)	NOT NULL,

	CreatedOnUTC		DATETIME		NOT NULL,

	CONSTRAINT PK_tMessage PRIMARY KEY	(Id),
	CONSTRAINT UK_tMessage UNIQUE		(Hash, Checksum)
)
GO

CREATE TABLE dbo.tLog
(
	Id					BIGINT			NOT NULL	IDENTITY (1, 1),
	Id_Process			NVARCHAR(50)	NOT NULL,
	Id_Stage			TINYINT			NOT NULL,
	Id_LogLevel			TINYINT			NOT NULL,
	MessageKey			NVARCHAR(100)	NULL,
	LogText				NVARCHAR(3000)	NOT NULL,
	MessageName			NVARCHAR(100)	NULL,
	Id_Message			BIGINT			NULL,

	CreatedOnUTC		DATETIME		NOT NULL,

	CONSTRAINT PK_tLog PRIMARY KEY	(Id),
	CONSTRAINT FK_tLog_tProcess
							FOREIGN KEY		(Id_Process, Id_Stage)	
														REFERENCES	dbo.tProcess	(Id, Id_Stage),
	CONSTRAINT FK_tLog_tStage
							FOREIGN KEY		(Id_Stage)	REFERENCES	dbo.tStage		(Id),
	CONSTRAINT FK_tLog_tLogLevel
							FOREIGN KEY		(Id_LogLevel)
														REFERENCES	dbo.tLogLevel	(Id),
	CONSTRAINT FK_tLog_tMessage
							FOREIGN KEY		(Id_Message)
														REFERENCES	dbo.tMessage	(Id),
)
GO


CREATE TABLE dbo.tException
(
	Id					BIGINT			NOT NULL	IDENTITY (1, 1),
	Id_Log				BIGINT			NOT NULL,

	Level				INT				NOT NULL,


	Type				NVARCHAR(250)	NOT NULL,
	Message				NVARCHAR(1000)	NOT NULL,
	Source				NVARCHAR(250)	NOT NULL,
	StackTrace			NVARCHAR(MAX)	NOT NULL,
	TargetSite			NVARCHAR(250)	NOT NULL,
	HResult				INT				NOT NULL,

	CreatedOnUTC		DATETIME		NOT NULL,

	CONSTRAINT PK_tException PRIMARY KEY	(Id),
	CONSTRAINT FK_tException_tLog			
							FOREIGN KEY		(Id_Log)	REFERENCES	dbo.tLog	(Id)
)
GO

