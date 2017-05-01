using System.Collections.Generic;

namespace _502Finder.Scraper.Models
{
    public class Pagination
    {
        public int Start { get; private set; }
        public int End { get; private set; }
        public List<PaginationItem> Items { get; private set; }

        public Pagination(int start, int end)
        {
            Start = start;
            End = end;

            var items = new List<PaginationItem>();
        }
    }
}
