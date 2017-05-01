namespace _502Finder.Models
{
    public class DispensaryModel
    {
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string CityStateZip { get; private set; }
        public string Telephone { get; private set; }

        public DispensaryModel(string name, string address, string cityStateZip, string telephone)
        {
            Name = name;
            Address = address;
            CityStateZip = cityStateZip;
            Telephone = telephone;
        }

        public override string ToString()
        {
            return $"{Name}: {Address} {CityStateZip} {Telephone}";
        }
    }
}
