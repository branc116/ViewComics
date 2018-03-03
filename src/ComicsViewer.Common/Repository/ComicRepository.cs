﻿using ComicsViewer.Common.Context;
using ComicsViewer.Common.Resources;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Collections.Generic;
using System.Linq;

namespace ComicsViewer.Common.Repository
{
    public class ComicRepository : IComicRepository
    {
        private readonly ComicDbContext _context;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ComicRepository(ComicDbContext context)
        {
            _context = context;
        }

        public void AddNewIssue(Issue issue)
        {
            if (_context.Issues.Include(i => i.Comic).Any(i => i.IssueNumber == issue.IssueNumber && i.Comic.Name == i.Comic.Name))
            {
                _logger.Error($"adding issues with same issue name {issue.IssueNumber} {issue.Comic.Name}");
                return;
            }

            _context.Issues.Add(issue);
            _context.SaveChanges();
        }

        public void AddNewIssues(List<Issue> issues, bool checkIfExists = true)
        {
            foreach (var issue in issues)
            {
                if (checkIfExists && _context.Issues.Include(i => i.Comic).Any(i => i.IssueNumber == issue.IssueNumber && i.Comic.Name == i.Comic.Name))
                {
                    _logger.Error($"adding issues with same issue name {issue.IssueNumber} {issue.Comic.Name}");
                    continue;
                }
                _context.Issues.Add(issue);
            }
            _context.SaveChanges();
        }

        public List<Issue> GetAllIssue(string comicName)
        {
            return _context.Comics
                .Include(i => i.Issues)
                .FirstOrDefault(i => i.Name == comicName)
                ?.Issues
                .ToList();
        }
        public List<string> GetAllIssueNames(string comicName)
        {
            var comic = GetComic(comicName);
            var id = comic.Id;
            return _context.Issues.Where(i => i.Comic.Id == id).Select(i => i.IssueNumber).ToList();
        }

        public Comic GetComic(string comicName)
        {
            return _context.Comics
                .Include(i => i.Issues)
                .FirstOrDefault(i => i.Name == comicName);
        }

        public bool Contains(string issueName)
        {
            return _context.Issues
                .Any(i => i.IssueNumber == issueName);
        }

        public List<Issue> StartsWith(string issuePrefix)
        {
            return _context.Issues.Where(i => i.IssueNumber.StartsWith(issuePrefix)).ToList();
        }

        public List<Comic> GetAllComics()
        {
            return _context.Comics.ToList();
        }
    }
}
