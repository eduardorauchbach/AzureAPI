CREATE TABLE Candidate (
	[ID] bigint IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,    	
	[FileID] varchar(50) NULL,
	CONSTRAINT [PK_Candidate] PRIMARY KEY CLUSTERED
	(
		[ID]
	)
);

-----------------------------------------

CREATE TABLE Job (
	[ID] bigint IDENTITY(1,1) NOT NULL,
	[Title] NVARCHAR(50) NOT NULL,    	
	[Description] varchar(Max) NULL,
	CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED
	(
		[ID]
	)
);

----------------------------------------

CREATE TABLE CandidateJob
(
	[CandidateID] bigint NOT NULL,
	[JobID] bigint NOT NULL
	CONSTRAINT [PK_CandidateJob] PRIMARY KEY CLUSTERED 
	(
		[CandidateID] ASC,
		[JobID] ASC
	)
)

ALTER TABLE CandidateJob Add Constraint FK_CandidateJob_Candidate Foreign Key(CandidateID) References Candidate(ID)
ALTER TABLE CandidateJob Add Constraint FK_CandidateJob_Job Foreign Key(JobID) References Job(ID)

--DROP TABLE CandidateJob
--DROP TABLE Job
--DROP TABLE Candidate
