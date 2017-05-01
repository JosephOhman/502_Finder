using System.Collections.Generic;
using _502Finder.Data;
using _502Finder.Data.Repositories;
using _502Finder.Scraper.Models;

namespace _502Finder.App
{
    public class StrainService : IStrainService
    {
        public Strain GetStrain(int id)
        {
            using (var context = DataContext.Create())
            {
                var strains = new List<Strain>();
                var repository = new StrainRepository(context);

                return ToStrainModel(repository.GetById(id));
            }
        }

        public List<Strain> GetTop(int count)
        {
            using (var context = DataContext.Create())
            {
                var strains = new List<Strain>();
                var repository = new StrainRepository(context);
                return ToStrainModel(repository.GetTop(count));
            }
        }

        public List<Strain> GetAll()
        {
            using (var context = DataContext.Create())
            {
                var strains = new List<Strain>();
                var repository = new StrainRepository(context);
                return ToStrainModel(repository.GetAll());
            }
        }

        public List<Strain> StrainSearch(string query)
        {
            using (var context = DataContext.Create())
            {
                var strains = new List<Strain>();
                var repository = new StrainRepository(context);

                return ToStrainModel(repository.StrainSearch(query));
            }
        }

        public List<Strain> StrainAutoComplete(string query)
        {
            using (var context = DataContext.Create())
            {
                var strains = new List<Strain>();
                var repository = new StrainRepository(context);

                return ToStrainModel(repository.StrainAutoComplete(query));
            }
        }
        private Strain ToStrainModel(Models.Data.Strain strain)
        {
            return ToStrainModel(new List<Models.Data.Strain> { strain })[0];
        }

        private List<Strain> ToStrainModel(IEnumerable<Models.Data.Strain> strains)
        {
            var list = new List<Strain>();

            foreach (Models.Data.Strain strain in strains)
            {
                list.Add(new Strain(
                    strain.Id,
                    strain.Name,
                    strain.Description,
                    strain.Type,
                    strain.Rating,
                    strain.THC,
                    strain.CBD,
                    strain.CBN,
                    strain.Url));
            }

            return list;
        }
    }
}
