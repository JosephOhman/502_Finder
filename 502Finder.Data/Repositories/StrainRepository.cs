using System.Collections.Generic;
using System.Linq;
using _502Finder.Models.Data;

namespace _502Finder.Data.Repositories
{
    public class StrainRepository : Repository<Strain>, IStrainRepository
    {
        private readonly DataContext _context;

        public StrainRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public new List<Strain> GetTop(int count)
        {
            List<Strain> strains = _dbSet.OrderByDescending(d => d.THC).Take(count).ToList();
            return strains;
        }

        public List<Strain> StrainSearch(string query)
        {
            List<Strain> strains = _dbSet.Where(s => s.Name.Contains(query)).OrderByDescending(d => d.THC).ToList();
            return strains;
        }

        public List<Strain> StrainAutoComplete(string query)
        {
            List<Strain> strains = _dbSet.Where(s => s.Name.StartsWith(query)).OrderByDescending(d => d.Name).ToList();
            return strains;
        }
    }
}
