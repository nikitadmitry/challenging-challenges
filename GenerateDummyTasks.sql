DECLARE @ChallengeId UNIQUEIDENTIFIER
DECLARE @Iterator INT = 1000

WHILE @Iterator > 0
begin

SET @ChallengeId = NEWID()

INSERT INTO CC_Challenges.dbo.Challenges
        ( Id ,
          AuthorId ,
          Title ,
          PreviewText ,
          Condition ,
          Difficulty ,
          Section ,
          Language ,
          Rating ,
          NumberOfVotes ,
          TimesSolved ,
          TimeCreated
        )
VALUES  ( @ChallengeId , -- Id - uniqueidentifier
          'E233D0E8-650A-E611-BDCF-8C89A50B69B1' , -- AuthorId - uniqueidentifier
          N'Generated Challenge' , -- Title - nvarchar(max)
          N'This is a Generated Challenge. No further assistance required.' , -- PreviewText - nvarchar(max)
          N'This is a Generated Challenge. One of the answers is ''answer''.' , -- Condition - nvarchar(max)
          0 , -- Difficulty - tinyint
          0 , -- Section - int
          0 , -- Language - int
          3.0 , -- Rating - float
          1 , -- NumberOfVotes - int
          0 , -- TimesSolved - int
          GETDATE()  -- TimeCreated - datetime
        )

INSERT INTO dbo.Answers
        ( Id, Value, Challenge_Id )
VALUES  ( NEWID(), -- Id - uniqueidentifier
          N'answer', -- Value - nvarchar(max)
          @ChallengeId  -- Challenge_Id - uniqueidentifier
          )

SET @Iterator = @Iterator - 1
end