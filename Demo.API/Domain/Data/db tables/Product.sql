CREATE TABLE Candidate (
	[ID] bigint IDENTITY(1,1),
	[Name] NVARCHAR(50) NOT NULL,    	
	[FileID] varchar(50) NULL,
);

ALTER TABLE Candidate ADD CONSTRAINT PK_Candidate PRIMARY KEY(ID)

-----------------------------------------

CREATE TABLE Job (
	[ID] bigint IDENTITY(1,1),
	[Title] NVARCHAR(50) NOT NULL,    	
	[Description] varchar(Max) NULL,
);

ALTER TABLE Job ADD CONSTRAINT PK_Job PRIMARY KEY(ID)

----------------------------------------

CREATE TABLE CandidateJob
(
	[ID] bigint IDENTITY(1,1),
	[CandidateID] bigint NOT NULL,
	[JobID] bigint NOT NULL,
)

ALTER TABLE CandidateJob Add Constraint PK_CandidateJob Primary Key(ID)
ALTER TABLE CandidateJob Add Constraint FK_CandidateJob_Candidate Foreign Key(CandidateID) References Candidate(ID)
ALTER TABLE CandidateJob Add Constraint FK_CandidateJob_Job Foreign Key(JobID) References Job(ID)

--DROP TABLE CandidateJob
--DROP TABLE Job
--DROP TABLE Candidate
