USE GhostPhotoDBTest;
GO

-- Get HashtagInfo ************************
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

-- Post Comment ************************
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
	SELECT pc.PostId, c.Comment, c.CommentDate, anu.UserName
	FROM PostComment AS pc
	INNER JOIN Comment as c on pc.CommentId = c.Id
	INNER JOIN AspNetUsers AS anu ON UserId = anu.Id
	WHERE pc.PostId = @PostId
END
GO

-- Post Comment ************************
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
	SELECT bp.Title, bp.PostContent, bp.PostImage, anu.UserName AS [Author], anu2.UserName AS [DisplayAuthor], pt.PostType, ps.StatusName
	FROM BlogPost AS bp
	INNER JOIN PostType AS pt ON bp.PostTypeId = pt.Id
	INNER JOIN AspNetUsers AS anu ON bp.AuthorId = anu.Id
	INNER JOIN AspNetUsers AS anu2 ON bp.DisplayAuthorId = anu2.Id
	INNER JOIN PostStatus AS ps ON bp.PostStatusId = ps.Id
	WHERE ps.Id = 1
END
GO
