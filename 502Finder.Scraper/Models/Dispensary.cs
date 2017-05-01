using System;
using _502Finder.Models;

namespace _502Finder.Scraper.Models
{
    public class Dispensary
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Address Address { get; private set; }
        public string Telephone { get; private set; }
        public Uri Url { get; private set; }

        public Dispensary(string name, string description, Address address, string telephone, string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
                Url = new Uri(url);

            Name = name;
            Description = description;
            Address = address;
            Telephone = telephone;
        }

        public override string ToString()
        {
            return $"{Name} - {Address}: {Url}";
        }
    }
}
