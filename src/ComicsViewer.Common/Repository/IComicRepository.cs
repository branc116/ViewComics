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

        List<Issue> GetAllIssue(string comicName);

        Comic GetComic(string comicName);
        List<Comic> GetAllComics();

        bool Contains(string issueName);

        List<Issue> StartsWith(string issuePrefix);
    }
}
