using System.Collections.Generic;
using _502Finder.Models.Data;

namespace _502Finder.Data.Repositories
{
    public interface IStrainRepository
    {
        List<Strain> GetTop(int count);
        List<Strain> StrainSearch(string query);
        List<Strain> StrainAutoComplete(string query);
    }
}
