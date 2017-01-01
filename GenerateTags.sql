DECLARE @cnt INT = 0;

WHILE @cnt < 100
BEGIN
   INSERT INTO dbo.Tags
        ( Id, Value )
VALUES  ( NEWID(), -- Id - uniqueidentifier
          char(rand()*26+65)+char(rand()*26+65)+char(rand()*26+65)+char(rand()*26+65)+char(rand()*26+65)
          )
   SET @cnt = @cnt + 1;
END;

-----------

DECLARE @AccountID UNIQUEIDENTIFIER
DECLARE @getAccountID CURSOR
SET @getAccountID = CURSOR FOR
SELECT Id
FROM dbo.Challenges
OPEN @getAccountID
FETCH NEXT
FROM @getAccountID INTO @AccountID
WHILE @@FETCH_STATUS = 0
BEGIN

INSERT INTO dbo.TagChallenges
        ( Tag_Id, Challenge_Id )
SELECT Id, @AccountID FROM dbo.Tags
  WHERE (ABS(CAST(
  (BINARY_CHECKSUM(*) *
  RAND()) as int)) % 100) < 10

FETCH NEXT
FROM @getAccountID INTO @AccountID
END
CLOSE @getAccountID
DEALLOCATE @getAccountID