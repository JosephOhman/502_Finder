using System.Collections.Generic;

namespace _502Finder.Scraper.Models
{
    public class ParseDispensaryResult
    {
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
        public List<GeoLocation> Locations { get; set; }

        public ParseDispensaryResult()
        {
            Locations = new List<GeoLocation>();
        }
    }
}
