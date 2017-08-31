USE GhostPhotoDB;
GO

IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'GetAllUsers')
BEGIN
   DROP PROCEDURE GetAllUsers
END
GO
CREATE PROCEDURE GetAllUsers
AS
SELECT ur.UserId, u.UserName, ur.RoleId, r.[Name] AS RoleName
FROM AspNetUsers u
	JOIN AspNetUserRoles AS ur ON ur.UserId = u.Id
	JOIN AspNetRoles AS r ON r.Id = ur.RoleId
GO

-- Get HashtagInfo ************************
-- The returns hashtag name and count of times used
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'GetHashtagInfo')
BEGIN
   DROP PROCEDURE GetHashtagInfo
END
GO
CREATE PROCEDURE GetHashtagInfo
AS
	SELECT h.HashtagName, Count(ph.HashtagId) AS [Count]
		FROM Hashtag AS h
		INNER JOIN PostHashtag as ph
			ON ph.HashtagId = h.Id
		GROUP BY h.HashtagName
		ORDER BY h.HashtagName
GO

-- Get Comment By Id ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'GetCommentById')
BEGIN
   DROP PROCEDURE GetCommentById
END
GO
CREATE PROCEDURE GetCommentById (
	@Id INT
)
AS
BEGIN
	SELECT c.Id, c.PostId, c.Comment, c.CommentDate, anu.UserName, anu.DisplayName
	FROM Comment AS c
		INNER JOIN AspNetUsers AS anu ON UserId = anu.Id
	WHERE c.Id = @Id
END
GO

-- Get Comments for a Post ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'GetCommentsByPostId')
BEGIN
   DROP PROCEDURE GetCommentsByPostId
END
GO
CREATE PROCEDURE GetCommentsByPostId (
	@PostId INT
)
AS
BEGIN
	SELECT c.Id, c.PostId, c.Comment, c.CommentDate, anu.UserName, anu.DisplayName
	FROM Comment AS c
		INNER JOIN AspNetUsers AS anu ON UserId = anu.Id
	WHERE c.PostId = @PostId
	ORDER BY c.CommentDate DESC
END
GO

-- Add Comment to a Post ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'NewPostComment')
BEGIN
   DROP PROCEDURE NewPostComment
END
GO
CREATE PROCEDURE NewPostComment
	@PostId INT,
	@Comment NVARCHAR(500),
	@UserId NVARCHAR(128)
AS
	DECLARE @CommentId INT
	DECLARE @CommentDate DATETIME2 = GETDATE()
BEGIN
	INSERT INTO Comment (PostId, Comment, CommentDate, UserId)
		VALUES (@PostId, @Comment, @CommentDate, @UserId)
		SET @CommentId = SCOPE_IDENTITY()

	EXEC GetCommentById @CommentId
END
GO

-- Delete Comment (deprecated 7/26/2017 - drop kept to remove from databases) ************************
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES
		WHERE ROUTINE_NAME = 'DeleteComment')
	DROP PROCEDURE DeleteComment
GO
-- Delete Comment By Id ************************
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES
		WHERE ROUTINE_NAME = 'DeleteCommentById')
	DROP PROCEDURE DeleteCommentById
GO
CREATE PROCEDURE DeleteCommentById
	@Id INT
AS
BEGIN
	DECLARE @PostId INT
	SET @PostId = (SELECT bp.Id FROM BlogPost AS bp
	               JOIN Comment AS c ON c.PostId = bp.Id
			       WHERE c.Id = @Id)

	DELETE Comment WHERE Id = @Id
END
GO

-- Get Page of Approved Posts ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'GetPageOfApprovedPosts')
BEGIN
   DROP PROCEDURE GetPageOfApprovedPosts
END
GO
CREATE PROCEDURE GetPageOfApprovedPosts
	@PageNum INT
AS
BEGIN
	DECLARE @PageSize INT = 10;
	DECLARE @PostNum INT = (@PageNum - 1) * @PageSize;

	SELECT bp.Id, bp.Title, bp.PostContent, bp.PostImage, anu1.UserName AS Author, anu2.UserName AS DisplayAuthor, bp.DateCreated, bp.ScheduleDate
	FROM BlogPost AS bp 
	INNER JOIN PostType AS pt ON bp.PostTypeId = pt.Id
	INNER JOIN AspNetUsers AS anu1 ON bp.AuthorId = anu1.Id
	INNER JOIN AspNetUsers AS anu2 ON bp.DisplayAuthorId = anu2.Id
	INNER JOIN PostStatus AS ps ON bp.PostStatusId = ps.Id
	WHERE ps.Id = 2  -- 2=Approved
	ORDER BY bp.DateCreated DESC
	OFFSET @PostNum ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

-- Get Number of Pages of Approved Posts ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'GetApprovedPostsPageCount')
BEGIN
   DROP PROCEDURE GetApprovedPostsPageCount
END
GO
CREATE PROCEDURE GetApprovedPostsPageCount
AS
BEGIN
	SELECT ((COUNT(*) + 9) / 10) AS PageCount
	FROM BlogPost AS bp
	INNER JOIN PostStatus AS ps ON bp.PostStatusId = ps.Id
	WHERE ps.Id = 2  -- 2=Approved
END
GO

-- Get Pending Posts ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'GetPendingPosts')
BEGIN
   DROP PROCEDURE GetPendingPosts
END
GO
CREATE PROCEDURE GetPendingPosts
AS
BEGIN
	SELECT bp.Id, bp.Title, bp.PostContent, bp.PostImage, anu1.UserName AS Author, anu2.UserName AS DisplayAuthor, bp.DateCreated, bp.ScheduleDate
	FROM BlogPost AS bp
	INNER JOIN PostType AS pt ON bp.PostTypeId = pt.Id
    INNER JOIN AspNetUsers AS anu1 ON bp.AuthorId = anu1.Id
	INNER JOIN AspNetUsers AS anu2 ON bp.DisplayAuthorId = anu2.Id
	INNER JOIN PostStatus AS ps ON bp.PostStatusId = ps.Id
	WHERE ps.Id = 1  -- 1=Pending
END
GO

-- Get Posts By Hashtag Id ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'GetPostsByHashtagId')
BEGIN
   DROP PROCEDURE GetPostsByHashtagId
END
GO
CREATE PROCEDURE GetPostsByHashtagId
	@HashtagId INT
AS
BEGIN
	SELECT bp.Id, bp.Title, bp.PostContent, bp.PostImage, anu1.UserName AS Author, anu2.UserName AS DisplayAuthor, bp.DateCreated, bp.ScheduleDate
	FROM BlogPost AS bp
	INNER JOIN PostType AS pt ON pt.Id = bp.PostTypeId
	INNER JOIN PostStatus AS ps ON ps.Id = bp.PostStatusId
	INNER JOIN PostHashtag AS ph ON ph.PostId = bp.Id AND ph.HashtagId = @HashtagId
    INNER JOIN AspNetUsers AS anu1 ON bp.AuthorId = anu1.Id  -- for return data only
	INNER JOIN AspNetUsers AS anu2 ON bp.DisplayAuthorId = anu2.Id  -- for return data only
    WHERE pt.Id = 2 AND ps.Id = 2
END
GO

-- Get Hashtags By Post Id ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'GetHashtagsByPostId')
BEGIN
   DROP PROCEDURE GetHashtagsByPostId
END
GO
CREATE PROCEDURE GetHashtagsByPostId
	@PostId INT
AS
BEGIN
	SELECT h.Id, h.HashtagName
	FROM Hashtag AS h
        JOIN PostHashtag AS ph ON ph.HashtagId = h.Id
        JOIN BlogPost AS bp ON bp.Id = ph.PostId
        WHERE bp.Id = @PostId
END
GO

-- Relate Hashtag to Blog Post ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'RelateTagToPost')
BEGIN
   DROP PROCEDURE RelateTagToPost
END
GO
CREATE PROCEDURE RelateTagToPost
	@PostId INT,
	@HashtagName VARCHAR(100)
AS
BEGIN
	DECLARE @HashtagId INT
	BEGIN
		IF NOT EXISTS (SELECT h.Id FROM Hashtag AS h WHERE h.HashtagName = @HashtagName)
			BEGIN
				INSERT INTO Hashtag(HashtagName) VALUES (@HashtagName)
				SET @HashtagId = SCOPE_IDENTITY()
			END
		ELSE
			-- TOP 1 should not be needed, just added for safety to ensure only 1 result is returned
			SET @HashtagId = (SELECT TOP 1 h.Id FROM Hashtag AS h WHERE h.HashtagName = @HashtagName)
	END

	IF NOT EXISTS (SELECT ph.PostId FROM PostHashtag AS ph WHERE ph.PostId = @PostId AND ph.HashtagId = @HashtagId)
		BEGIN
			INSERT INTO PostHashtag(PostId, HashtagId) VALUES (@PostId, @HashtagId)
		END
END
GO

-- Get Post By ID ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'GetPostById')
BEGIN
   DROP PROCEDURE GetPostById
END
GO
CREATE PROCEDURE GetPostById(
	@Id INT
)
AS
BEGIN
	SELECT
		bp.Id,
		pt.PostType,
		bp.Title,
		bp.PostContent,
		bp.PostImage,
		anu1.UserName AS Author,
		anu2.UserName AS DisplayAuthor,
		ps.StatusName,
		bp.DateCreated,
		bp.ModifiedDate,
		bp.ScheduleDate,
		bp.ExpirationDate
	FROM BlogPost AS bp
		JOIN AspNetUsers AS anu1 ON bp.AuthorId = anu1.Id
		JOIN AspNetUsers AS anu2 ON bp.DisplayAuthorId = anu2.Id
		JOIN PostStatus AS ps ON ps.Id = bp.PostStatusId
		JOIN PostType AS pt ON pt.Id = bp.PostTypeId 
	WHERE bp.Id = @Id
END
GO

-- Get Post Body By ID ************************
-- This returns only the content (i.e. the body) of the post
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'GetPostBodyById')
BEGIN
   DROP PROCEDURE GetPostBodyById
END
GO
CREATE PROCEDURE GetPostBodyById
	@Id INT
AS
BEGIN
	SELECT
		PostContent
	FROM BlogPost
	WHERE Id = @Id
END
GO

-- Get Approved Post By ID ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'GetApprovedPostById')
BEGIN
   DROP PROCEDURE GetApprovedPostById
END
GO
CREATE PROCEDURE GetApprovedPostById(
	@Id INT
)
AS
BEGIN
	SELECT bp.Id, bp.Title, bp.PostContent, bp.PostImage, anu1.UserName AS Author, anu2.UserName AS DisplayAuthor, bp.DateCreated, bp.ScheduleDate
	FROM BlogPost AS bp
	INNER JOIN AspNetUsers AS anu1 ON bp.DisplayAuthorId = anu1.Id
	INNER JOIN AspNetUsers AS anu2 ON bp.DisplayAuthorId = anu2.Id
	WHERE (bp.Id = @Id) AND (bp.PostStatusId = 2) AND (bp.PostTypeId = 2)
END
GO

-- Search Blog Posts By Any (i.e. Title, Content, or Hashtag) ************************
-- delete old
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'SearchBlogPosts')
BEGIN
   DROP PROCEDURE SearchBlogPosts
END
GO
-- new
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'SearchPostsByAny')
BEGIN
   DROP PROCEDURE SearchPostsByAny
END
GO
CREATE PROCEDURE SearchPostsByAny
	@Search NVARCHAR(60)
AS
BEGIN
	SELECT bp.Id, bp.Title, bp.PostContent, bp.PostImage, anu1.UserName AS Author, anu2.UserName AS DisplayAuthor, bp.DateCreated, bp.ScheduleDate
	FROM BlogPost AS bp
	INNER JOIN PostType AS pt ON bp.PostTypeId = 2
	INNER JOIN AspNetUsers AS anu1 ON bp.DisplayAuthorId = anu1.Id
	INNER JOIN AspNetUsers AS anu2 ON bp.DisplayAuthorId = anu2.Id
	INNER JOIN PostStatus AS ps ON bp.PostStatusId = 2
	INNER JOIN PostHashtag AS ph ON  bp.Id = ph.PostId
	INNER JOIN Hashtag AS h ON ph.HashtagId = h.Id
	WHERE (bp.Title LIKE '%' + @Search + '%') OR
			(bp.PostContent LIKE '%' + @Search + '%') OR
			(h.HashtagName LIKE '%' + @Search + '%')
	GROUP BY bp.Id, bp.Title, bp.PostContent, bp.PostImage, anu1.UserName, anu2.UserName, bp.DateCreated, h.HashtagName, bp.ScheduleDate
	ORDER BY bp.DateCreated DESC
END
GO

-- Search Blog Posts by Title ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'SearchPostsByTitle')
BEGIN
   DROP PROCEDURE SearchPostsByTitle
END
GO
CREATE PROCEDURE SearchPostsByTitle
	@Search NVARCHAR(60)
AS
BEGIN
	SELECT bp.Id, bp.Title, bp.PostContent, bp.PostImage, anu1.UserName AS Author, anu2.UserName AS DisplayAuthor, bp.DateCreated, bp.ScheduleDate
	FROM BlogPost AS bp
	INNER JOIN PostType AS pt ON bp.PostTypeId = 2
	INNER JOIN AspNetUsers AS anu1 ON bp.DisplayAuthorId = anu1.Id
	INNER JOIN AspNetUsers AS anu2 ON bp.DisplayAuthorId = anu2.Id
	INNER JOIN PostStatus AS ps ON bp.PostStatusId = 2
	INNER JOIN PostHashtag AS ph ON  bp.Id = ph.PostId
	INNER JOIN Hashtag AS h ON ph.HashtagId = h.Id
	WHERE (bp.Title LIKE '%' + @Search + '%')
	GROUP BY bp.Id, bp.Title, bp.PostContent, bp.PostImage, anu1.UserName, anu2.UserName, bp.DateCreated, h.HashtagName, bp.ScheduleDate
	ORDER BY bp.DateCreated DESC
END
GO

-- Search Blog Posts By Post Content ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'SearchPostsByContent')
BEGIN
   DROP PROCEDURE SearchPostsByContent
END
GO
CREATE PROCEDURE SearchPostsByContent
	@Search NVARCHAR(60)
AS
BEGIN
	SELECT bp.Id, bp.Title, bp.PostContent, bp.PostImage, anu1.UserName AS Author, anu2.UserName AS DisplayAuthor, bp.DateCreated, bp.ScheduleDate
	FROM BlogPost AS bp
	INNER JOIN PostType AS pt ON bp.PostTypeId = 2
	INNER JOIN AspNetUsers AS anu1 ON bp.DisplayAuthorId = anu1.Id
	INNER JOIN AspNetUsers AS anu2 ON bp.DisplayAuthorId = anu2.Id
	INNER JOIN PostStatus AS ps ON bp.PostStatusId = 2
	INNER JOIN PostHashtag AS ph ON  bp.Id = ph.PostId
	INNER JOIN Hashtag AS h ON ph.HashtagId = h.Id
	WHERE (bp.PostContent LIKE '%' + @Search + '%')
	GROUP BY bp.Id, bp.Title, bp.PostContent, bp.PostImage, anu1.UserName, anu2.UserName, bp.DateCreated, h.HashtagName, bp.ScheduleDate
	ORDER BY bp.DateCreated DESC
END
GO

-- Search Blog Posts By Hashtag Name ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'SearchPostsByHashtag')
BEGIN
   DROP PROCEDURE SearchPostsByHashtag
END
GO
CREATE PROCEDURE SearchPostsByHashtag
	@Search NVARCHAR(60)
AS
BEGIN
	SELECT bp.Id, bp.Title, bp.PostContent, bp.PostImage, anu1.UserName AS Author, anu2.UserName AS DisplayAuthor, bp.DateCreated, bp.ScheduleDate
	FROM BlogPost AS bp
	INNER JOIN PostType AS pt ON bp.PostTypeId = 2
	INNER JOIN AspNetUsers AS anu1 ON bp.DisplayAuthorId = anu1.Id
	INNER JOIN AspNetUsers AS anu2 ON bp.DisplayAuthorId = anu2.Id
	INNER JOIN PostStatus AS ps ON bp.PostStatusId = 2
	INNER JOIN PostHashtag AS ph ON  bp.Id = ph.PostId
	INNER JOIN Hashtag AS h ON ph.HashtagId = h.Id
	WHERE (h.HashtagName LIKE '%' + @Search + '%')
	GROUP BY bp.Id, bp.Title, bp.PostContent, bp.PostImage, anu1.UserName, anu2.UserName, bp.DateCreated, h.HashtagName, bp.ScheduleDate
	ORDER BY bp.DateCreated DESC
END
GO

-- Create Blog Post ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'CreateBlogPost')
BEGIN
   DROP PROCEDURE CreateBlogPost
END
GO
CREATE PROCEDURE CreateBlogPost(
	@Id INT OUTPUT,
	@PostType VARCHAR(20),
	@Title NVARCHAR(80),
	@PostContent NVARCHAR(max),
	@PostImage VARCHAR(50),
	@AuthorID NVARCHAR(128),
	@DisplayAuthorId NVARCHAR(128),
	@StatusName VARCHAR(20),
	@DateCreated DATETIME2,
	@ScheduleDate DATETIME2,
	@ExpirationDate DATETIME2
)
AS
BEGIN
	DECLARE @PostTypeId INT
	SELECT TOP 1 @PostTypeId=pt.Id FROM PostType AS pt WHERE pt.PostType = @PostType

	DECLARE @PostStatusId INT
	SELECT TOP 1 @PostStatusId=ps.Id FROM PostStatus AS ps WHERE ps.StatusName = @StatusName

	INSERT INTO BlogPost (PostTypeId, Title, PostContent, PostImage, AuthorId, DisplayAuthorId, PostStatusId, DateCreated, ModifiedDate, ScheduleDate, ExpirationDate)
		VALUES (@PostTypeId, @Title, @PostContent, @PostImage, @AuthorID, @DisplayAuthorId, @PostStatusId, @DateCreated, NULL, @ScheduleDate, @ExpirationDate)

	SET @Id = SCOPE_IDENTITY()
END
GO

-- Edit Blog Post ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'EditBlogPost')
BEGIN
   DROP PROCEDURE EditBlogPost
END
GO
CREATE PROCEDURE EditBlogPost(
	@Id INT,
	@PostType VARCHAR(20),
	@Title NVARCHAR(80),
	@PostContent NVARCHAR(max),
	@PostImage VARCHAR(50),
	@AuthorID NVARCHAR(128),
	@DisplayAuthorId NVARCHAR(128),
	@StatusName VARCHAR(20),
	@DateCreated DATETIME2,
	@ModifiedDate DATETIME2,
	@ScheduleDate DATETIME2,
	@ExpirationDate DATETIME2
)
AS
BEGIN
	DECLARE @PostTypeId INT
	SELECT TOP 1 @PostTypeId=pt.Id FROM PostType AS pt WHERE pt.PostType = @PostType

	DECLARE @PostStatusId INT
	SELECT TOP 1 @PostStatusId=ps.Id FROM PostStatus AS ps WHERE ps.StatusName = @StatusName

	UPDATE BlogPost
	SET PostTypeId = @PostTypeId,
		Title = @Title,
		PostContent = @PostContent,
		PostImage = @PostImage,
		AuthorId = @AuthorID,
		DisplayAuthorId = @DisplayAuthorId,
		PostStatusId = @PostStatusId,
		DateCreated = @DateCreated,
		ModifiedDate = @ModifiedDate,
		ScheduleDate = @ScheduleDate,
		ExpirationDate = @ExpirationDate
	WHERE Id = @Id
END
GO

-- Update New Registration ************************
IF EXISTS(
   SELECT ROUTINE_NAME
   FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'UpdateNewRegistration')
BEGIN
   DROP PROCEDURE UpdateNewRegistration
END
GO
CREATE PROCEDURE UpdateNewRegistration
	@Email NVARCHAR(256)
AS
BEGIN
    DECLARE @guid nvarchar(128)
    DECLARE @randId INT

    SET @randId = (SELECT CONVERT(INT, (999999+1)*RAND()))

    SET @guid = (SELECT Id FROM AspNetUsers WHERE Email = @Email)
    UPDATE AspNetUsers
    SET DisplayName = 'Guest' + CAST(@randId AS NVARCHAR(10))
    WHERE Id = @guid
    INSERT INTO AspNetUserRoles(UserId, RoleId) VALUES (@guid, '22222222-2222-2222-2222-999999999999')

END
GO
