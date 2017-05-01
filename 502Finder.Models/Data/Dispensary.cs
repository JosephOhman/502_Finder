using System;

namespace _502Finder.Models.Data
{
    public class Dispensary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string Telephone { get; set; }
        public Uri WebsiteUrl { get; set; }

        public Dispensary(string name, Address address, string telephone, Uri websiteUrl)
        {
            Name = name;
            Address = address;
            Telephone = telephone;
            WebsiteUrl = websiteUrl;
            DateTime DateCreated = DateTime.UtcNow;
            DateTime DateUpdated = DateTime.UtcNow;
        }
    }
}
