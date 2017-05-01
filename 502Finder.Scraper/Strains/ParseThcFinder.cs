using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using _502Finder.Common;
using _502Finder.Scraper.Models;
using HtmlAgilityPack;

namespace _502Finder.Scraper.Strains
{
    public class ParseThcFinder : IStrain
    {
        private readonly List<string> _removeList = new List<string>
        {
        };

        private readonly Dictionary<string, string> _renameDict = new Dictionary<string, string>
        {
        };

        private readonly Dictionary<string, string> _replaceDict = new Dictionary<string, string>
        {
        };

        public ParseStrainResult Parse()
        {
            int page = 1;
            var baseUrl = "http://www.thcfinder.com";
            var result = new ParseStrainResult();

            while (page <= 5142)
            {
                string html;

                try
                {
                    Uri url = new Uri($"{baseUrl}/strains/page/{page}");
                    html = new WebClient().DownloadString(url);
                    page++;
                }
                catch
                {
                    break;
                }

                var htmlDoc = new HtmlDocument();
                htmlDoc.OptionFixNestedTags = true;
                htmlDoc.OptionCheckSyntax = false;
                htmlDoc.OptionAutoCloseOnEnd = true;
                htmlDoc.OptionWriteEmptyNodes = true;
                htmlDoc.LoadHtml(html);

                if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Any())
                {
                    var sb = new StringBuilder(512);
                    foreach (var error in htmlDoc.ParseErrors)
                        sb.AppendLine(error.Reason.ToString());

                    result.Error = true;
                    result.Message = sb.ToString();
                }

                if (htmlDoc.DocumentNode != null)
                {
                    int added = ParseStrains(result.Strains, htmlDoc, baseUrl);
                    UpdateStrainInfo(result.Strains, added);
                }
            }

            return result;
        }

        private int ParseStrains(List<Strain> strains, HtmlDocument htmlDoc, string baseUrl)
        {
            int added = 0;
            string expression = "//div/div/div/div/div/div/div/"+
                                "table/tr/td/div/div/table/tr/td/a";

            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes(expression))
            {
                try
                {
                    string href = HtmlEntity.DeEntitize(node.GetAttributeValue("href", string.Empty));
                    string[] parts = href.Split(new[] { '/' });

                    if (parts.Length == 4 && href.ToLower().Contains("/strains/"))
                    {
                        if (string.IsNullOrWhiteSpace(parts[3]))
                            continue;

                        string type = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parts[2].Replace("_", " ")).Trim();
                        string name = GetName(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parts[3].Replace("_", " "))).Trim();

                        if (!strains.Any(s => s.Name.ToLower() == name.ToLower()))
                        {
                            var strain = new Strain($"{baseUrl}{href}", name, type.ToStrainType());
                            strains.Add(strain);
                            ++added;

                            Console.WriteLine($"Found {strain.Name}.");
                        }
                    }
                }
                catch
                {
                }
            }

            return added;
        }

        private string GetName(string name)
        {
            string[] parts = name.Split(new[] { ' ' });

            if (parts.Length > 0)
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].Length == 2 || parts[i].Length == 3)
                    {
                        parts[i] = parts[i].ToUpper();
                    }
                }

                name = string.Join(" ", parts);
            }

            parts = name.Split(new[] { '-' });
            if (parts.Length > 0)
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].Length == 2 || parts[i].Length == 3)
                    {
                        parts[i] = parts[i].ToUpper();
                    }
                }

                name = string.Join("-", parts);
            }

            return name;
        }

        private void UpdateStrainInfo(List<Strain> strains, int maxDegreeOfParallelism)
        {
            var options = new ParallelOptions {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };

            Parallel.ForEach(strains, options, (strain) =>
            {
                try
                {
                    string html = new WebClient().DownloadString(strain.Url);

                    var htmlDoc = new HtmlDocument();
                    htmlDoc.OptionFixNestedTags = true;
                    htmlDoc.LoadHtml(html);

                    string expression = "//span[contains(@itemprop, 'description')]";
                    HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode(expression);
                    if (node != null)
                    {
                        strain.Description = HtmlEntity.DeEntitize(node.InnerText);

                        expression = "//div//div//table//tbody//tr//td//div//div[1]//table//tbody//tr//td[2]//span";
                        node = htmlDoc.DocumentNode.SelectSingleNode(expression);
                        double thc = double.Parse(HtmlEntity.DeEntitize(node.InnerText.Replace("%", string.Empty)));

                        expression = "//div//div//table//tbody//tr//td//div//div[3]//table//tbody//tr//td[2]//span";
                        node = htmlDoc.DocumentNode.SelectSingleNode(expression);
                        double cbd = double.Parse(HtmlEntity.DeEntitize(node.InnerText.Replace("%", string.Empty)));

                        expression = "//div//div//table//tbody//tr//td//div//div[2]//table//tbody//tr//td[2]//span";
                        node = htmlDoc.DocumentNode.SelectSingleNode(expression);
                        double cbn = double.Parse(HtmlEntity.DeEntitize(node.InnerText.Replace("%", string.Empty)));

                        if (thc > strain.THC)
                            strain.THC = thc;

                        if (cbd > strain.CBD)
                            strain.CBD = cbd;

                        if (cbn > strain.CBN)
                            strain.CBN = cbn;

                        Console.WriteLine($"Updated {strain.Name}.");
                    }
                }
                catch
                {
                }
            });
        }
    }
}
