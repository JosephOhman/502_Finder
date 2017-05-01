using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using HtmlAgilityPack;
using _502Finder.Common;
using _502Finder.Scraper.Models;

namespace _502Finder.Scraper.Strains
{
    public class ParseWeedMaps : IStrain
    {
        private readonly List<string> _removeList = new List<string>
        {
            "AK 47",
            "Bubbleishous",
            "Grape Krush",
            "Jilly Bean (Not Different Than Jelly Bean)",
            "Kush Bubba (Variations/Complexity Run In) <--(Bubba)",
            "Marleys Collie"
        };

        private readonly Dictionary<string, string> _renameDict = new Dictionary<string, string>
        {
            ["AK47"] = "AK-47",
            ["AK49"] = "AK-49",
            ["A Train"] = "A-Train",
            ["BLUE CRACK"] = "Blue Crack",
            ["Buddhas Sister"] = "Buddha's Sister",
            ["Cinex ?"] = "Cinex",
            ["Blackberry (Not Kush?) Ehh"] = "Blackberry",
            ["Goo (Afgoo?_"] = "Goo",
            ["Bubblegum (Fruit Fam)"] = "Bubblegum",
            ["Kush Bubba (Variations/Complexity Run In) <--(Bubba)"] = "Bubba Kush",
            ["Kush Cotton Candy (Diff Than CC? Ehh)"] = "Cotton Candy Kush",
            ["Kush Platinum (Not Bubba"] = "Platinum Kush",
            ["Purple Grandaddy (Alt: Purple Granddaddy)"] = "Grandaddy Purple (GDP)",
            ["Skunk #1                   XX"] = "Skunk #1",
            ["Sweet God&#X27;S"] = "Sweet God's",
            ["Haze Silver (Not Super)"] = "Silver Haze",
            ["MAUI WAUI        (Maui Wowie)"] = "MAUI WAUI (Maui Wowie)",
            ["Snowcap             XX"] = "Snowcap",
            ["Xj 13"] = "XJ-13",
            ["Boss&#X27;S Sister"] = "Boss's Sister",
            ["California Dream&#X27;N"] = "California Dream'N",
            ["Chem Dawg (Or Chemdawg) (Or Chem Dog)"] = "Chem Dawg (Chem Dog)",
            ["Cinderella 99            XX"] = "Cinderella 99",
            ["Diesel Purple (Purple Diesel) &Lt;--Tweener"] = "Purple Diesel",
            ["Girl Scout Cookies   (Duplicate: Cookie)"] = "Girl Scout Cookies",
            ["Lamb&#X27;S Breath?"] = "Lamb's Breath",
            ["Lambs Bread"] = "Lamb's Bread",
            ["OG Fire                            XX"] = "Fire OG",
            ["OG LA (Not Larry?)"] = "OG LA",
            ["Mango      (Fruit Family)"] = "Mango"
        };

        private readonly Dictionary<string, string> _replaceDict = new Dictionary<string, string>
        {
            ["  "] = " ",
            ["â€™"] = "'"
        };

        public ParseStrainResult Parse()
        {
            int page = 1;
            var baseUrl = "https://weedmaps.com";
            var result = new ParseStrainResult();

            while (true)
            {
                string html;

                try
                {
                    Uri url = new Uri($"{baseUrl}/strains?filter=%2A&page={page}");
                    html = new WebClient().DownloadString(url);
                    if (!html.Contains("strain-cell"))
                        break;

                    page++;
                }
                catch
                {
                    break;
                }

                var htmlDoc = new HtmlDocument();
                htmlDoc.OptionFixNestedTags = true;
                htmlDoc.LoadHtml(html);

                if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Any())
                {
                    var sb = new StringBuilder(512);
                    foreach (var error in htmlDoc.ParseErrors) {
                        sb.AppendLine(error.ToString());
                    }

                    result.Error = true;
                    result.Message = sb.ToString();

                    return result;
                }

                if (htmlDoc.DocumentNode != null)
                {
                    ParseStrains(result.Strains, htmlDoc, baseUrl, "Hybrid");
                    ParseStrains(result.Strains, htmlDoc, baseUrl, "Indica");
                    ParseStrains(result.Strains, htmlDoc, baseUrl, "Sativa");
                }
            }

            var options = new ParallelOptions { MaxDegreeOfParallelism = 5 };
            Parallel.ForEach(result.Strains, options, (strain) =>
            {
                Console.WriteLine($"Getting description for {strain.Name}: {strain.Url.AbsoluteUri}");
                strain.Description = GetDescription(strain.Url.AbsoluteUri);
            });

            return result;
        }

        private void ParseStrains(List<Strain> strains, HtmlDocument htmlDoc, string baseUrl, string category)
        {
            string expression = $"//div[contains(@class, 'strain-cell') and contains(@class, '{category}')]";

            try
            {
                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes(expression))
                {
                    string strainName = GetName(HtmlEntity.DeEntitize(node.GetAttributeValue("data-name", string.Empty)));
                    if (strainName == string.Empty)
                        continue;

                    string strainType = HtmlEntity.DeEntitize(node.GetAttributeValue("data-category", string.Empty)).Trim();
                    string strainRating = HtmlEntity.DeEntitize(node.GetAttributeValue("data-rating", string.Empty)).Trim();
                    string strainThc = HtmlEntity.DeEntitize(node.GetAttributeValue("data-thc", string.Empty)).Trim();
                    string strainCbd = HtmlEntity.DeEntitize(node.GetAttributeValue("data-cbd", string.Empty)).Trim();
                    string strainCbn = HtmlEntity.DeEntitize(node.GetAttributeValue("data-cbn", string.Empty)).Trim();

                    string name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(strainName);
                    string type = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(strainType);
                    double rating = double.Parse(strainRating);
                    double thc = double.Parse(strainThc);
                    double cbd = double.Parse(strainCbd);
                    double cbn = double.Parse(strainCbn);

                    HtmlNode anchorNode = node.ChildNodes.Single(n => n.Name == "a");
                    string strainUri = anchorNode.GetAttributeValue("href", string.Empty);

                    if (strains.Any(x => x.Name == name))
                    {
                        Strain strain = strains.Single(x => x.Name == name);

                        if (rating > strain.Rating)
                            strain.Rating = rating;

                        if (thc > strain.THC)
                            strain.THC = thc;

                        if (cbd > strain.CBD)
                            strain.CBD = cbd;

                        if (cbn > strain.CBN)
                            strain.CBN = cbn;
                    }
                    else
                    {
                        var strain = new Strain(name, type.ToStrainType(), rating, thc, cbd, cbn, new Uri(strainUri));
                        strains.Add(strain);

                        Console.WriteLine($"Added {strain}");
                    }
                }
            }
            catch
            {
            }
        }

        private string GetName(string name)
        {
            if (_removeList.Contains(name))
                return string.Empty;

            if (_renameDict.ContainsKey(name))
                return _renameDict[name];

            return name;
        }

        private string GetDescription(string url)
        {
            try
            {
                string html = new WebClient().DownloadString(new Uri(url));

                var htmlDoc = new HtmlDocument();
                htmlDoc.OptionFixNestedTags = true;
                htmlDoc.LoadHtml(html);

                if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Any())
                    return string.Empty;

                string expression = "//div[contains(@class, 'content-a')]" +
                                    "//div[contains(@class, 'content-b')]" +
                                    "//div[contains(@class, 'container')]" +
                                    "//p";

                HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode(expression);
                string description = HtmlEntity.DeEntitize(node.InnerText);
                if (description == "i")
                    return string.Empty;

                foreach (KeyValuePair<string, string> kvp in _replaceDict)
                    description = description.Replace(kvp.Key, kvp.Value);

                return description;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
