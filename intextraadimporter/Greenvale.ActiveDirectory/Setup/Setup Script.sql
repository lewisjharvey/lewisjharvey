-- =======================================================
-- = Removes users and directory entries                 =
-- = that were added from Active Directory               =
-- =									                 =
-- = Author: Lewis Harvey (lewis.harvey@greenvale.co.uk) =
-- = Date: 13 Aug 2010                                   =
-- =======================================================
USE ix33_greenvale;

-- Tidy up the extranet users
DELETE
FROM awsFRM_SubmissionUserRecipient
WHERE UserID IN (25,114,1999,2005,2006,2014,2015,2016,2019,2137,2164,2198,2405,2425)
GO 

DELETE
FROM awsFRM_Submission
WHERE SubmitterID IN (25,114,1999,2005,2006,2014,2015,2016,2019,2137,2164,2198,2405,2425)
GO 

DELETE
FROM awsUsers 
WHERE UserID IN (25,114,1999,2005,2006,2014,2015,2016,2019,2137,2164,2198,2405,2425)
GO

UPDATE awsUsers
SET DefaultURL = 'Extranet'
WHERE UserID = 95
Go

-- Delete all the users that are not in AD (use the GUID)
--UPDATE awsUsers
--SET [Disabled] = 1
--WHERE ActiveDirectoryGUID IS NOT NULL

-- Now delete all the attribute people directory entry IDs
DELETE
FROM awsAttribute
WHERE AttributeID IN (
	SELECT AttributeID
	FROM awsdDirectoryEntryAttribute
	WHERE DirectoryEntryID IN (
			SELECT DirectoryEntryID
			FROM awsdDirectoryEntry
			WHERE DirectoryID <> 2
		)
	)
GO

-- Now delete all the directory entry attributes
DELETE
FROM awsdDirectoryEntryAttribute
WHERE DirectoryEntryID IN (
		SELECT DirectoryEntryID
		FROM awsdDirectoryEntry
		WHERE DirectoryID <> 2
	)
GO
	
-- Now delete all the directory entry attributes
DELETE
FROM awsdDirectoryEntry
WHERE DirectoryID <> 2
GO

-- Add the new attributes to the directory
SET IDENTITY_INSERT awsAttributeType ON

INSERT INTO awsAttributeType (AttributeTypeID, AttributeGroupID, name, 
	description, type, sort, MinLength, MaxLength, ValidationExpr, 
	CreatedOn, LastModifiedOn, CreatedByUserID, LastModifiedByUserID)
VALUES (69, 1, 'Site', 'The site the user works at', 'List', 16, 0, 250, '', GETDATE(), GETDATE(), 1, 1)

SET IDENTITY_INSERT awsAttributeType OFF

INSERT INTO awsdListAttributeTypeData (AttributeTypeID, ItemValue)
VALUES (69, 'Burrelton')
INSERT INTO awsdListAttributeTypeData (AttributeTypeID, ItemValue)
VALUES (69, 'March')
INSERT INTO awsdListAttributeTypeData (AttributeTypeID, ItemValue)
VALUES (69, 'Colton')
INSERT INTO awsdListAttributeTypeData (AttributeTypeID, ItemValue)
VALUES (69, 'Duns')
INSERT INTO awsdListAttributeTypeData (AttributeTypeID, ItemValue)
VALUES (69, 'Floods Ferry')
INSERT INTO awsdListAttributeTypeData (AttributeTypeID, ItemValue)
VALUES (69, 'Nacton')
INSERT INTO awsdListAttributeTypeData (AttributeTypeID, ItemValue)
VALUES (69, 'Swancote')
INSERT INTO awsdListAttributeTypeData (AttributeTypeID, ItemValue)
VALUES (69, 'Tern Hill')

SET IDENTITY_INSERT awsAttributeType ON

INSERT INTO awsAttributeType (AttributeTypeID, AttributeGroupID, name, 
	description, type, sort, MinLength, MaxLength, ValidationExpr, 
	CreatedOn, LastModifiedOn, CreatedByUserID, LastModifiedByUserID)
VALUES (70, 1, 'Department', 'The department the user works at', 'Text', 17, 0, 250, '', GETDATE(), GETDATE(), 1, 1)

SET IDENTITY_INSERT awsAttributeType OFF
GO
-- Alter trigger to add created by
ALTER trigger [dbo].[TRG_awsUsers_awsDirectoryEntry_parity] on [dbo].[awsUsers]
for insert
as

	if exists (select * from sysobjects where id = object_id(N'awsdDirectory')) 
	begin
		--If there is a directory
		declare @id int
		declare @firstname varchar(255)
		declare @surname varchar(255)
		declare @created_by int

		--get the userid
		select @id = userid from inserted
		
		select @firstname = firstname, @surname = surname, @created_by = CreatedBy from inserted

		--add new record
		insert into awsdDirectoryEntry (DirectoryID, userid, CreatedByUserID) values (1, @id, @created_by)

		set @id = @@identity
		
		--add firstname and surname to entry
		exec awsdAddDirectoryEntryAttribute @id, 2, @firstname, Null
		exec awsdAddDirectoryEntryAttribute @id, 4, @surname, Null

	end
GO


CREATE PROCEDURE EnsureDirectoryEntryAttribute 
	@AttributeTypeID int, 
	@Value nvarchar(255), 
	@DirectoryEntryID int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @AttributeId int

    IF EXISTS (
		SELECT *
		FROM awsAttribute AS a
			INNER JOIN awsdDirectoryEntryAttribute AS dea
				ON a.AttributeID = dea.AttributeID
		WHERE AttributeTypeID = @AttributeTypeID
			AND DirectoryEntryID = @DirectoryEntryID )
		BEGIN
			SET @AttributeId = (
					SELECT a.AttributeID
					FROM awsAttribute AS a
						INNER JOIN awsdDirectoryEntryAttribute AS dea
							ON a.AttributeID = dea.AttributeID
					WHERE AttributeTypeID = @AttributeTypeID
						AND DirectoryEntryID = @DirectoryEntryID
				)
			UPDATE awsAttribute
			SET [value] = @Value,
				LastModifiedOn = GETDATE(),
				LastModifiedByUserID = @UserId
			WHERE AttributeID = @AttributeId
		END
	ELSE
		BEGIN
			INSERT INTO awsAttribute (AttributeTypeID, [value], CreatedOn, LastModifiedOn, CreatedByUserID, LastModifiedByUserID)
			VALUES (@AttributeTypeID, @Value, GETDATE(), GETDATE(), @UserId, @UserId)
			SET @AttributeId = SCOPE_IDENTITY();
		END
	PRINT @AttributeId
	IF EXISTS (
		SELECT *
		FROM awsdDirectoryEntryAttribute
		WHERE DirectoryEntryID = @DirectoryEntryID
			AND AttributeID = @AttributeId )
		BEGIN 
			UPDATE awsdDirectoryEntryAttribute
			SET LastModifiedOn = GETDATE(),
				LastModifiedByUserID = @UserId
			WHERE DirectoryEntryID = @DirectoryEntryID
				AND AttributeID = @AttributeId
		END
	ELSE
		BEGIN 
			INSERT INTO awsdDirectoryEntryAttribute (DirectoryEntryID, AttributeID, CreatedOn, LastModifiedOn, CreatedByUserID, LastModifiedByUserID)
            VALUES (@DirectoryEntryID, @AttributeID, GETDATE(), GETDATE(), @UserId, @UserId)
		END
END
GO



UPDATE awsUsers
SET Disabled = 1
WHERE UserName LIKE '%.sc'