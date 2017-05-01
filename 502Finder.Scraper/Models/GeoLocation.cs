namespace _502Finder.Scraper.Models
{
    public class GeoLocation
    {
        public string PostalCode { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string StateAbbr { get; private set; }
        public string County { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public GeoLocation(
            string postalCode,
            string city,
            string state,
            string stateAbbr,
            string county,
            double latitude,
            double longitude)
        {
            PostalCode = postalCode;
            City = city;
            State = state;
            StateAbbr = stateAbbr;
            County = county;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
