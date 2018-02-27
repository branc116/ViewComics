using ComicsViewer.Common.Context;
using ComicsViewer.Common.Repository;
using System;
using System.Collections.Generic;

namespace ComicsViewer.NameFixer.Helper
{
    public static class DataHelper
    {
        public static IEnumerable<string> ComicViewerPages()
        {
            for (int i = 1; i < 10000; i++)
            {
                yield return $"http://viewcomic.com/page/{i}";
            }
        }

        public static ComicDbContext GetContext()
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            var dbcontext = new ComicDbContext(connectionString);
            return dbcontext;
        }

        public static ComicRepository GetRepository()
        {
            return new ComicRepository(GetContext());
        }
    }
}
