USE GhostPhotoDB;
GO

-- update credentials
-- Admin    = '00000000-0000-0000-0000-777777777777'
-- Writer   = '11111111-1111-1111-1111-888888888888'
-- Guest    = '22222222-2222-2222-2222-999999999999'
-- Disabled = '33333333-3333-3333-3333-666666666666'
BEGIN TRAN
update AspNetUsers
set DisplayName = UserName
where UserName in ('Doug', 'Richard', 'Sam', 'Matt')

Declare @guid varchar(36);

SET @guid = (select Id from AspNetUsers where Email = 'zfantastic@ghosts.com')
UPDATE AspNetUsers
-- Note that Identity is using the UserName instead of the Email as labeled to login, though UserName must fit email format!?
SET DisplayName = 'Zee Fantastic'
WHERE Id = @guid
INSERT INTO AspNetUserRoles(UserId, RoleId) VALUES (@guid, '00000000-0000-0000-0000-777777777777')

SET @guid = (select Id from AspNetUsers where Email = 'ckeene@ghostwriter.com')
UPDATE AspNetUsers
-- Note that Identity is using the UserName instead of the Email as labeled to login, though UserName must fit email format!?
SET DisplayName = 'Carolyn Keene'
WHERE Id = @guid
INSERT INTO AspNetUserRoles(UserId, RoleId) VALUES (@guid, '11111111-1111-1111-1111-888888888888')

SET @guid = (select Id from AspNetUsers where Email = 'wnovak@ghostwriter.com')
UPDATE AspNetUsers
-- Note that Identity is using the UserName instead of the Email as labeled to login, though UserName must fit email format!?
SET DisplayName = 'William Novak'
WHERE Id = @guid
INSERT INTO AspNetUserRoles(UserId, RoleId) VALUES (@guid, '11111111-1111-1111-1111-888888888888')

SET @guid = (select Id from AspNetUsers where Email = 'lvincent@ghostwriter.com')
UPDATE AspNetUsers
-- Note that Identity is using the UserName instead of the Email as labeled to login, though UserName must fit email format!?
SET DisplayName = 'Lynn Vincent'
WHERE Id = @guid
INSERT INTO AspNetUserRoles(UserId, RoleId) VALUES (@guid, '11111111-1111-1111-1111-888888888888')

SET @guid = (select Id from AspNetUsers where Email = 'aneiderman@ghostwriter.com')
UPDATE AspNetUsers
-- Note that Identity is using the UserName instead of the Email as labeled to login, though UserName must fit email format!?
SET DisplayName = 'Andrew Neiderman'
WHERE Id = @guid
INSERT INTO AspNetUserRoles(UserId, RoleId) VALUES (@guid, '11111111-1111-1111-1111-888888888888')

select u.Id, Email, UserName, DisplayName, r.[Name] as RoleName
from AspNetUsers as u
left join AspNetUserRoles as ur ON ur.UserId = u.Id
left join AspNetRoles as r ON r.Id = ur.RoleId
ROLLBACK TRAN
GO
