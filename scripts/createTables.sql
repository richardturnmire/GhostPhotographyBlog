USE GhostPhotoDB;
GO

-- Create Tables anew

IF EXISTS(SELECT * FROM sys.tables WHERE name='PostHashtag')
	DROP TABLE PostHashtag
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name='PostComment')
	DROP TABLE PostComment
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name='BlogPost')
	DROP TABLE BlogPost
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name='Hashtag')
	DROP TABLE Hashtag
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name='PostType')
	DROP TABLE PostType
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name='PostStatus')
	DROP TABLE PostStatus
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name='Comment')
	DROP TABLE Comment
GO

CREATE TABLE Comment (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	PostId INT NOT NULL,
	Comment NVARCHAR(500) NOT NULL,
	CommentDate DATETIME2 DEFAULT GETDATE(),
	UserId NVARCHAR(128) FOREIGN KEY REFERENCES AspNetUsers(Id) NOT NULL
)

CREATE TABLE PostStatus (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	StatusName VARCHAR(20) NOT NULL,
	StatusDescription VARCHAR(50) NOT NULL
)

CREATE TABLE PostType (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	PostType VARCHAR(20) NOT NULL,
)

CREATE TABLE Hashtag (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	HashtagName VARCHAR(100) NOT NULL
)

CREATE TABLE BlogPost (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	PostTypeId INT FOREIGN KEY REFERENCES PostType(Id) NOT NULL,
	Title NVARCHAR(80) NOT NULL,
	PostContent NVARCHAR(max) NOT NULL,
	PostImage VARCHAR(50) NULL,
	AuthorId NVARCHAR(128) FOREIGN KEY REFERENCES AspNetUsers(ID) NOT NULL,
	DisplayAuthorId NVARCHAR(128) FOREIGN KEY REFERENCES AspNetUsers(ID) NOT NULL,
	PostStatusId INT FOREIGN KEY REFERENCES PostStatus(Id) NOT NULL,
	DateCreated DATETIME2 NOT NULL DEFAULT GETDATE(),
	ModifiedDate DATETIME2 NULL,
	ScheduleDate DATETIME2 NOT NULL DEFAULT GETDATE(),
	ExpirationDate DATETIME2 NULL
)

CREATE TABLE PostHashtag (
	PostId INT FOREIGN KEY REFERENCES BlogPost(Id) NOT NULL,
	HashtagId INT FOREIGN KEY REFERENCES Hashtag(Id) NOT NULL,
	CONSTRAINT PK_PostHashtag PRIMARY KEY(PostId, HashtagId)
)

--AspNetRoles created by Identity
-- Id (PK, nvarchar(128))
-- Name (nvarchar(256))

--AspNetUsers created by Identity
-- Id (PK, nvarchar(128), not null)  (e.g. 12345678-1234-1234-1234-123456789012)
-- Email (nvarchar(256), null)
-- EmailConfirmed (bit, not null)
-- PasswordHash (nvarchar(max), null)
-- SecurityStamp (nvarchar(max), null)
-- PhoneNumber (nvarchar(max), null)
-- PhoneNumberConfirmed (bit, not null)
-- TwoFactorEnabled (bit, not null)
-- LockoutEndDateUtc (datetime, null)
-- AccessFailedCount (int, not null)
-- UserName (nvarchar(256), not null)

-- AspNetUserRoles created by Identity
-- UserId (PK, FK, nvarchar(128), not null)
-- RoleId (PK, FK, nvarchar(128), not null)
