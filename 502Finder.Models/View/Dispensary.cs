namespace _502Finder.Mappers
{
    public class Dispensary
    {
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string CityStateZip { get; private set; }
        public string Telephone { get; private set; }

        public Dispensary(string name, string address, string cityStateZip, string telephone)
        {
            Name = name;
            Address = address;
            CityStateZip = cityStateZip;
            Telephone = telephone;
        }
    }
}
