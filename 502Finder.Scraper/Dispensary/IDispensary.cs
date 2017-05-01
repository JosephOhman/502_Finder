using _502Finder.Scraper.Models;

namespace _502Finder.Scraper.Dispensary
{
    public interface IDispensary
    {
        ParseDispensaryResult Parse(string state);
    }
}
