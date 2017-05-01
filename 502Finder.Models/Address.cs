namespace _502Finder.Models
{
    public class Address
    {
        public string Address1 { get; private set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; private set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Address(string address1, string address2, string city, string state, string postalCode, double latitude = 0, double longitude = 0)
        {
            Address1 = address1;
            Address2 = address2;
            City = city;
            State = state;
            PostalCode = postalCode;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
