USE GhostPhotoDBTest;
GO

IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'DbReset')
BEGIN
   DROP PROCEDURE DbReset
END
GO

CREATE PROCEDURE DbReset AS
BEGIN
	DELETE FROM PostHashtag;
	DELETE FROM PostComment;
		DBCC CHECKIDENT ('PostComment', RESEED, 1)
	DELETE FROM BlogPost;
		DBCC CHECKIDENT ('BlogPost', RESEED, 1)
	DELETE FROM Comment;
		DBCC CHECKIDENT ('Comment', RESEED, 1)
	DELETE FROM Hashtag;
		DBCC CHECKIDENT ('Hashtag', RESEED, 1)
	DELETE FROM PostType;
		DBCC CHECKIDENT ('PostType', RESEED, 1)
	DELETE FROM PostStatus;
		DBCC CHECKIDENT ('PostStatus', RESEED, 1)
	DELETE FROM AspNetUserRoles
	WHERE (UserId = '00000000-0000-0000-0000-000000000000' AND RoleId = '00000000-0000-0000-0000-777777777777')
		  OR
		  (UserId = '11111111-1111-1111-1111-111111111111' AND RoleId = '11111111-1111-1111-1111-888888888888')
		  OR
		  (UserId = '22222222-2222-2222-2222-222222222222' AND RoleId = '22222222-2222-2222-2222-999999999999')
		  OR
		  (UserId = '33333333-3333-3333-3333-333333333333' AND RoleId = '33333333-3333-3333-3333-666666666666');

	DELETE FROM AspNetRoles
	WHERE Id IN ('00000000-0000-0000-0000-777777777777',
				 '11111111-1111-1111-1111-888888888888',
				 '22222222-2222-2222-2222-999999999999',
				 '33333333-3333-3333-3333-666666666666');

	DELETE FROM AspNetUsers
	WHERE Id IN ('00000000-0000-0000-0000-000000000000',
				 '11111111-1111-1111-1111-111111111111',
				 '22222222-2222-2222-2222-222222222222',
				 '33333333-3333-3333-3333-333333333333');


		

	--Populate PostStatus
	-- AspNetUsers
	INSERT INTO AspNetUsers(Id, EmailConfirmed, PhoneNumberConfirmed, Email, TwoFactorEnabled,
				LockoutEnabled, AccessFailedCount, UserName) VALUES
		('00000000-0000-0000-0000-000000000000', 0, 0, 'test1@test.com', 0, 0, 0, 'Doug'),
		('11111111-1111-1111-1111-111111111111', 0, 0, 'test2@test.com', 0, 0, 0, 'Richard'),
		('22222222-2222-2222-2222-222222222222', 0, 0, 'test3@test.com', 0, 0, 0, 'Sam'),
		('33333333-3333-3333-3333-333333333333', 0, 0, 'test4@test.com', 0, 0, 0, 'Matt');

	 --AspNetRoles
	INSERT INTO AspNetRoles(Id, [Name]) VALUES
		('00000000-0000-0000-0000-777777777777', 'Admin'),
		('11111111-1111-1111-1111-888888888888', 'Writer'),
		('22222222-2222-2222-2222-999999999999', 'Guest'),
		('33333333-3333-3333-3333-666666666666', 'Disabled');

	-- AspNetUserRoles
	INSERT INTO AspNetUserRoles(UserId, RoleId) VALUES
		('00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-777777777777'),
		('11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-888888888888'),
		('22222222-2222-2222-2222-222222222222', '22222222-2222-2222-2222-999999999999'),
		('33333333-3333-3333-3333-333333333333', '33333333-3333-3333-3333-666666666666');
	
	SET IDENTITY_INSERT PostStatus ON

	INSERT INTO PostStatus(Id, StatusName, StatusDescription)
	VALUES (1, 'Pending', 'Post is Pending Approval'),
			(2, 'Approved', 'Post has been Approved'),
			(3,'Expired','Post has Expired');

	SET IDENTITY_INSERT PostStatus OFF

	--Populate PostType
	SET IDENTITY_INSERT PostType ON

	INSERT INTO PostType(Id, PostType)
	VALUES (1, 'Static'),
			(2, 'Blog'),
			(3,'Admin');

	SET IDENTITY_INSERT PostType OFF

	--Populate Hashtag
	SET IDENTITY_INSERT Hashtag ON

	INSERT INTO Hashtag(Id, HashtagName)
	VALUES (1, '#Ghost'),
			(2, '#Photo'),
			(3,'#GhostPhoto'),
			(4,'#GhostPhotography'),
			(5,'#Photography'),
			(6,'#Louisville'),
			(7,'#Waverly'),
			(8, '#Scary'),
			(9,'#Frightened');

	SET IDENTITY_INSERT Hashtag OFF

	--Populate Comment
	SET IDENTITY_INSERT Comment ON

	INSERT INTO Comment(Id, Comment, CommentDate, UserId)
	VALUES (1, 'I Like Ghosts!', '7/20/17','11111111-1111-1111-1111-111111111111'),
		   (2, 'I too Like Ghosts', '7/20/17','00000000-0000-0000-0000-000000000000'),
		   (3,'Your Photos are great!','7/20/17','22222222-2222-2222-2222-222222222222'),
		   (4,'That is Creepy, I would never go in there!', '7/20/17','22222222-2222-2222-2222-222222222222'),
		   (5,'You are the worst photographer on the Internet! Bad!', '7/20/17','33333333-3333-3333-3333-333333333333'),
		   (6,'Whoever made that last comment is an idiot!', '7/20/17','33333333-3333-3333-3333-333333333333'),
		   (7,'Wow! Amazing!', '7/20/17','11111111-1111-1111-1111-111111111111');

	SET IDENTITY_INSERT Comment OFF

	--Populate BlogPost
	SET IDENTITY_INSERT BlogPost ON

	INSERT INTO BlogPost(Id, PostTypeId, Title, PostContent, PostImage, AuthorId, DisplayAuthorId, PostStatusId, DateCreated, ModifiedDate, ScheduleDate, ExpirationDate)
		VALUES 
		(1, 2, 'Ghosts Are Very Scary!', '<p> This is a test page!</p>',NULL,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',2,'7/20/17',NULL,'7/20/17',NULL),
		(2, 2, 'Ghosts Are Very Very Scary!', '<p> This is a test page too!</p>',NULL,'00000000-0000-0000-0000-000000000000','11111111-1111-1111-1111-111111111111',2,'7/20/17',NULL,'7/20/17',NULL),
		(3, 2, 'This Ghost Made Me Poop My Pants!', '<p> These pants are stained!</p>',NULL,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',1,'7/20/17',NULL,'7/20/17',NULL),
		(4, 2, 'Spooky!', '<p>Ghosts are Spooky</p>',NULL,'11111111-1111-1111-1111-111111111111','11111111-1111-1111-1111-111111111111',1,'7/20/17',NULL,'7/20/17',NULL);

	SET IDENTITY_INSERT BlogPost OFF
	
	--Populate PostComment
	SET IDENTITY_INSERT PostComment ON

	INSERT INTO PostComment(Id, PostId, CommentId)
	VALUES (1, 1,1),(2, 1,2),(3,1,3),(4,1,4),(5,1,5),(6,1,6),(7,1,7),
	(8,2,1),(9,2,2),(10,2,3),(11,2,4),(12,2,5),(13,2,6),(14,2,7),
	(15,3,1),(16,3,2),(17,3,3),(18,4,4),(19,4,5),(20,4,6),(21,4,7);

	SET IDENTITY_INSERT PostComment OFF

	--Populate PostHashtag

	INSERT INTO PostHashtag(PostId, HashtagId)
	VALUES (1,1),(1,2),(1,3),(1,4),(1,5),(1,6),(1,7),
	(1,8),(1,9),(2,1),(2,2),(2,3),(2,4),(3,1),
	(3,2),(3,3),(3,5),(3,6),(4,1),(4,2),(4,7),(4,8);

	
END
GO

EXEC DbReset
