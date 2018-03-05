/****** Script for SelectTopNRows command from SSMS  ******/
  SELECT  SUBSTRING([IssueNumber], 6, 500) + ' ' + SUBSTRING([IssueNumber], 0, 4)
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '[0-9][0-9][0-9] –%'

  Update dbo.Issues
  set IssueNumber = SUBSTRING([IssueNumber], 6, 500) + ' ' + SUBSTRING([IssueNumber], 0, 4) where [IssueNumber] like '[0-9][0-9][0-9] –%'

  SELECT SUBSTRING([IssueNumber], 6, 500) + ' ' + SUBSTRING([IssueNumber], 0, 4)
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '[0-9][0-9]-%' and IssueNumber not like '%Revolution%' 
  order by IssueNumber

  Update dbo.Issues
  set ComicId = 5 where [IssueNumber] like '[0-9][0-9]-%' and IssueNumber not like '%Revolution%'

  SELECT RTRIM(replace(IssueNumber, '.', ''))
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '%.' and [IssueNumber] not like '%.__%'

  Update dbo.Issues
  set IssueNumber = RTRIM(replace(IssueNumber, '.', '')) where [IssueNumber] like '%.' and [IssueNumber] not like '%.__%'

  SELECT SUBSTRING([IssueNumber], 6, 500) + ' ' + SUBSTRING([IssueNumber], 0, 4)
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '[0-9][0-9][0-9]: %'

  Update dbo.Issues
  set IssueNumber = SUBSTRING([IssueNumber], 6, 500) + ' ' + SUBSTRING([IssueNumber], 0, 4) where [IssueNumber] like '[0-9][0-9][0-9]: %'

  SELECT LTRIM( [IssueNumber])
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like ' %'

  Update dbo.Issues
  set IssueNumber = LTRIM( [IssueNumber]) where [IssueNumber] like ' %'

  SELECT SUBSTRING([IssueNumber], 8, 500) + ' ' + SUBSTRING([IssueNumber], 0, 6)
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '[0-9][0-9][0-9]:[0-9]%'

  Update dbo.Issues
  set IssueNumber = SUBSTRING([IssueNumber], 8, 500) + ' ' + SUBSTRING([IssueNumber], 0, 6) where [IssueNumber] like '[0-9][0-9][0-9]:[0-9]%'

  SELECT  SUBSTRING([IssueNumber], 6, 500) + ' ' + SUBSTRING([IssueNumber], 0, 4), IssueNumber
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '[0-9][0-9] –%'

  Update dbo.Issues
  set IssueNumber = SUBSTRING([IssueNumber], 6, 500) + ' ' + SUBSTRING([IssueNumber], 0, 4) where [IssueNumber] like '[0-9][0-9] –%'

  SELECT SUBSTRING([IssueNumber], 5, 500) + ' ' + SUBSTRING([IssueNumber], 0, 3), IssueNumber
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '[0-9][0-9]. %'

  Update dbo.Issues
  set IssueNumber = SUBSTRING([IssueNumber], 5, 500) + ' ' + SUBSTRING([IssueNumber], 0, 3) where [IssueNumber] like '[0-9][0-9]. %'

  SELECT [IssueNumber]
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '[0-9][0-9]%' and ComicId = 2 and IssueNumber = '39 Puzzle Book- Find Asterix'
  order by IssueNumber

  Update dbo.Issues
  set ComicId = 5 where [IssueNumber] like '[0-9][0-9]%' and ComicId = 2 and IssueNumber = '39 Puzzle Book- Find Asterix'
