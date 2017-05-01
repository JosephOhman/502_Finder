using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using _502Finder.Scraper.Models;
using _502Finder.Scraper.Strains;
using DataModels = _502Finder.Models.Data;

namespace _502Finder.Scraper.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            var tasks = new List<Task>(2);
            tasks.Add(Task.Factory.StartNew(CrawlStrains));
            //tasks.Add(Task.Factory.StartNew(CrawlDispensaries));

            stopWatch.Start();
            Task.WaitAll(tasks.ToArray());
            stopWatch.Stop();

            System.Console.WriteLine($"Crawled strains. Took {stopWatch}");
            System.Console.ReadLine();
        }

        static void CrawlStrains()
        {
            IStrain parser = new ParseWeedMaps();
            ParseStrainResult weedMapsReslt = parser.Parse();

            parser = new ParseThcFinder();
            ParseStrainResult thcFinderResult = parser.Parse();

            var strains = new List<Models.Strain>(weedMapsReslt.Strains.Count + thcFinderResult.Strains.Count);
            strains.AddRange(weedMapsReslt.Strains);
            strains.AddRange(thcFinderResult.Strains);

            using (var context = Data.DataContext.Create())
            {
                foreach (Models.Strain strain in strains.OrderBy(s => s.Name))
                {
                    var s = context.Strains.FirstOrDefault(x => x.Name == strain.Name);
                    if (s != null)
                    {
                        if (s.Type != strain.Type)
                            s.Type = strain.Type;

                        if (s.Description != strain.Description)
                            s.Description = strain.Description;

                        if (s.Rating != strain.Rating)
                            s.Rating = strain.Rating;

                        if (s.THC != strain.THC)
                            s.THC = strain.THC;

                        if (s.CBD != strain.CBD)
                            s.CBD = strain.CBD;

                        if (s.CBN != strain.CBN)
                            s.CBN = strain.CBN;

                        System.Console.WriteLine($"Updated {strain.Name}");
                    }
                    else
                    {
                        
                        context.Strains.Add(new DataModels.Strain(
                            strain.Id,
                            strain.Name,
                            strain.Description,
                            strain.Type,
                            strain.Rating,
                            strain.THC,
                            strain.CBD,
                            strain.CBN,
                            strain.Url));

                        System.Console.WriteLine($"Saving {strain.Name}");
                    }
                }

                context.SaveChanges();
            }
        }

        static void CrawlDispensaries()
        {
            Dispensary.IDispensary parser = new Dispensary.ParsePotGuide();
            string[] states = new[] { "Washington", "Oregon", "Nevada", "Massachusetts", "Arizona", "Colorado" };

            foreach (string state in states)
            {
                ParseDispensaryResult result = parser.Parse(state);
            }
        }
    }
}
