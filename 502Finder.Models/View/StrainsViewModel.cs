using System.Collections.Generic;

namespace _502Finder.Models.View
{
    public class StrainsViewModel
    {
        public List<Strain> StrainList { get; }

        public StrainsViewModel(List<Strain> strains)
        {
            StrainList = strains;
        }
    }
}
