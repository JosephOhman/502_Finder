using System;
using _502Finder.Models.Enums;

namespace _502Finder.Scraper.Models
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
        public Uri Url { get; set; }

        public Strain(int id, string name, string description, StrainType type, double rating, double thc, double cbd, double cbn, Uri url)
        {
            Id = id;
            Name = name;
            Description = description;
            Type = type;
            Rating = rating;
            THC = thc;
            CBD = cbd;
            CBN = cbn;
            Url = url;
        }

        public Strain(string name, StrainType type, double rating, double thc, double cbd, double cbn, Uri url)
        {
            Name = name;
            Type = type;
            Rating = rating;
            THC = thc;
            CBD = cbd;
            CBN = cbn;
            Url = url;
        }

        public Strain(string url, string name, StrainType type)
        {
            if (!string.IsNullOrWhiteSpace(url))
                Url = new Uri(url);

            Name = name;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Name} - {Type} - {THC}% THC - {CBD}% CBD - {CBN}% CBN";
        }
    }
}
