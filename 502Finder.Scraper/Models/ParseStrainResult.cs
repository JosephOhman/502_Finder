using System.Collections.Generic;

namespace _502Finder.Scraper.Models
{
    public class ParseStrainResult
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public List<Strain> Strains { get; set; }

        public ParseStrainResult()
        {
            Strains = new List<Strain>();
        }
    }
}
