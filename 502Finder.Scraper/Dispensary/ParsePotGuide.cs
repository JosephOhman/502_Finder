using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using _502Finder.Logging;
using _502Finder.Logging.Extensions;
using _502Finder.Models;
using _502Finder.Scraper.Models;
using HtmlAgilityPack;

namespace _502Finder.Scraper.Dispensary
{
    public class ParsePotGuide : IDispensary
    {
        Dictionary<Uri, string> dispensaries = new Dictionary<Uri, string>
        {
            [new Uri("https://potguide.com/washington/marijuana-stores/")] = "Washington",
            [new Uri("https://potguide.com/oregon/marijuana-dispensaries/")] = "Oregon",
            [new Uri("https://potguide.com/nevada/marijuana-dispensaries/")] = "Nevada",
            [new Uri("https://potguide.com/massachusetts/marijuana-dispensaries/")] = "Massachusetts",
            [new Uri("https://potguide.com/arizona/marijuana-dispensaries/")] = "Arizona",
            [new Uri("https://www.coloradopotguide.com/where-to-buy-marijuana/")] = "Colorado"
        };

        public ParseDispensaryResult Parse(string state)
        {
            var html = string.Empty;
            var htmlMap = new Dictionary<Uri, string>();
            var result = new ParseDispensaryResult();

            foreach (KeyValuePair<Uri, string> kvp in dispensaries)
            {
                try
                {
                    html = new WebClient().DownloadString(kvp.Key);
                    htmlMap.Add(kvp.Key, html);
                }
                catch (Exception ex)
                {
                    Logger<ParsePotGuide>.Error(ex.GetMessage());
                }

                var htmlDoc = new HtmlDocument();
                htmlDoc.OptionCheckSyntax = false;
                htmlDoc.OptionFixNestedTags = true;
                htmlDoc.OptionAutoCloseOnEnd = true;
                htmlDoc.OptionWriteEmptyNodes = true;
                htmlDoc.LoadHtml(html);

                if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Any())
                {
                    var sb = new StringBuilder(512);

                    foreach (var error in htmlDoc.ParseErrors)
                    {
                        string msg = $"Line {error.Line.ToString("N0")} at position " +
                                     $"{error.LinePosition.ToString("N0")}, Reason={error.Reason}.";

                        sb.AppendLine(msg);
                    }

                    result.Error = true;
                    result.ErrorMessage = sb.ToString();
                }

                if (htmlDoc.DocumentNode != null)
                {
                    for (int i = 0; i < 10; i++)
                        ParseDispensaries(dispensaries, htmlDoc);
                }
            }

            return result;
        }

        private int ParseDispensaries(Dictionary<Uri, string> htmlMap, HtmlDocument htmlDoc)
        {
            int added = 0;
            Address address;
            string dispensaryName;

            string xpath = "//div[@class=\"basic-listing\"]";

            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes(xpath))
            {
                DispensaryModel model = ParseDispensary(node.InnerText);

                Console.WriteLine(model);

                HtmlNode anchorNode = node.SelectSingleNode("//h4/a");
                if (anchorNode == null)
                    continue;

                dispensaryName = HtmlEntity.DeEntitize(anchorNode.InnerText);
                address = GetAddress(anchorNode.InnerText);

                HtmlNode tmpNode = node.SelectSingleNode("a");
                string url = tmpNode.Attributes["href"].Value;;
            }

            return added;
        }

        private DispensaryModel ParseDispensary(string source)
        {
            var words = new List<string>();
            var options = StringSplitOptions.RemoveEmptyEntries;

            foreach (string word in source.Split(new [] { '\t' }, options))
            {
                char r = word[0];
                char n = word[1];

                if (r.Equals('\r') && n.Equals('\n'))
                    continue;

                words.Add(word);
            }

            var remove = StringSplitOptions.RemoveEmptyEntries;
            string[] parts = words[0].Split(new[] { "\r\n\r\n" }, remove);
            for (int i = 0, len = parts.Length; i < len; i++)
                parts[i] = parts[i].Trim();

            if (parts.Length != 2)
                return null;

            string businessName = parts[0];
            string address = parts[1];
            string cityStateZip = words[1].Replace("\r\n", "").Trim();
            string telephone = words[2].Replace("Phone: ", "").Trim();

            return new DispensaryModel(businessName, address, cityStateZip, telephone);
        }

        private Address GetAddress(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                if (character != '<')
                {
                }
            }

            return null;
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
    }
}
