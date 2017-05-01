namespace _502Finder.Models.View
{
    public class StrainDetailViewModel
    {
        public Strain Strain { get; }

        public StrainDetailViewModel(Strain strain)
        {
            Strain = strain;
        }
    }
}
