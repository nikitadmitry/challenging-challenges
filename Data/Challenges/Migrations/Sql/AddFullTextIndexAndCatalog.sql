CREATE FULLTEXT CATALOG Challenges_FullTextCatalog
WITH ACCENT_SENSITIVITY = OFF;

CREATE FULLTEXT STOPLIST Challenges_StopList 
FROM SYSTEM STOPLIST; 

CREATE FULLTEXT INDEX ON dbo.Challenges(Title, PreviewText, Condition)
KEY INDEX [PK_dbo.Challenges]
ON Challenges_FullTextCatalog
WITH STOPLIST = Challenges_StopList;