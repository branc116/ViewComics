using ComicsViewer.Common.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComicsViewer.Common.Repository
{
    public interface IComicRepository
    {

        void AddNewIssue(Issue issue);

        void AddNewIssues(List<Issue> issues, bool checkIfExists = true);
        bool Contains(string issueName);
        Comic GetComic(string comicName);
        List<Issue> GetAllIssue(string comicName);
        List<string> GetAllIssueNames(string comicName);
        List<Comic> GetAllComics();
        List<Issue> StartsWith(string issuePrefix);
    }
}
