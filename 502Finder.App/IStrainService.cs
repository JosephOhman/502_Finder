using System.Collections.Generic;
using _502Finder.Scraper.Models;

namespace _502Finder.App
{
    public interface IStrainService
    {
        Strain GetStrain(int id);
        List<Strain> GetTop(int count);
        List<Strain> GetAll();
        List<Strain> StrainSearch(string query);
        List<Strain> StrainAutoComplete(string query);
    }
}
