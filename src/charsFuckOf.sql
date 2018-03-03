/****** Script for SelectTopNRows command from SSMS  ******/
  --'&#038;' -> '&'
  Update dbo.Issues
  set IssueNumber = REPLACE(IssueNumber,'&#038;','&') where IssueNumber like '%&#038;%'
  --'&#8211;' -> '–'
  Update dbo.Issues
  set IssueNumber = REPLACE(IssueNumber,'&#8211;','–') where IssueNumber like '%&#8211;%'
  --&#8217; -> '
  Update dbo.Issues
  set IssueNumber = REPLACE(IssueNumber,'&#8217;','’')
  --&#8216; -> ‘
  Update dbo.Issues
  set IssueNumber = REPLACE(IssueNumber,'&#8216;','‘')
  --&#8230; ->  
  Update dbo.Issues
  set IssueNumber = REPLACE(IssueNumber,'&#8230;','')

  --[email&#160;protected] – ->
  Update dbo.Issues
  set IssueNumber = REPLACE(IssueNumber,'[email&#160;protected] – ','')
  --&#8242; -> ′
  Update dbo.Issues
  set IssueNumber = REPLACE(IssueNumber,'&#8242;','′')

  SELECT [IssueNumber]
  FROM [ComicsViewer].[dbo].[Issues]
  where [IssueNumber] like '%&#%'