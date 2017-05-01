using System;
using System.IO;
using System.Collections.Generic;
using _502Finder.Scraper.Models;

namespace _502Finder.Scraper
{
    public static class LoadGeoLocations
    {
        private const string DataDir = @"Data\";
        private const string DataFilename = "us_postal_codes.csv";
        private static List<GeoLocation> Locations;

        public static GeoLocation[] GetLocations()
        {
            if (Locations != null)
                return Locations.ToArray();

            Locations = new List<GeoLocation>();

            foreach (string line in File.ReadAllLines(GetFilePath()))
            {
                try
                {
                    string[] parts = line.Split(
                        new[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length != 7)
                        continue;

                    double tmp;
                    double latitude = 0.0;
                    double longitude = 0.0;

                    if (double.TryParse(parts[5], out tmp))
                        latitude = tmp;
                    else
                        throw new Exception($"Unable to parse latitude string.");

                    if (double.TryParse(parts[6], out tmp))
                        longitude = tmp;
                    else
                        throw new Exception($"Unable to parse longitude string.");

                    Locations.Add(new GeoLocation(
                        parts[0],
                        parts[1],
                        parts[2],
                        parts[3],
                        parts[4],
                        latitude,
                        longitude));
                }
                catch (Exception ex)
                {
                    var message = $"Geolocation Loading Error: {ex.Message} line={line}";
                    throw new Exception(message, ex);
                }
            }

            return Locations.ToArray();
        }

        private static string GetFilePath()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            return $@"{baseDir}{DataDir}{DataFilename}";
        }
    }
}
