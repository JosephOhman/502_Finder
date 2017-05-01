using _502Finder.Models.Enums;

namespace _502Finder.Models.View
{
    public class Strain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public StrainType Type { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public double THC { get; set; }
        public double CBD { get; set; }
        public double CBN { get; set; }

        public Strain(int id, string name, StrainType type, string description, double rating, double thc, double cbd, double cbn)
        {
            Id = id;
            Name = name;
            Type = type;
            Description = description;
            Rating = rating;
            THC = thc;
            CBD = cbd;
            CBN = cbn;
        }
    }
}
