/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '[0-9][0-9][0-9]:%'

  SELECT *
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '[0-9][0-9]-%'
  order by IssueNumber

  SELECT *
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '%..'

  SELECT *
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] not like '%).' and [IssueNumber] not like '%) .' and  [IssueNumber] not like '%..' and [IssueNumber] like '%.'

  SELECT *
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '%.'

  SELECT RTRIM(replace(IssueNumber, '.', ''))
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '%.' and [IssueNumber] not like '%.__%'

  select issueNumber
  from [ComicsViewer].[dbo].[Issues]
  where len(issueNumber) = 100