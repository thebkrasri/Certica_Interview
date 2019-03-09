using System;
using System.Collections.Generic;
using System.IO;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
;
        Country country1 = new Country("Uga", "Uganda");
            Country country2 = new Country("MEX", "Mexico");
            Country country3 = new Country("MOZ", "Mozambique");
            Country country4 = new Country("ARG", "Argentina");
            Country country5 = new Country("ALB", "Albania");
            Country country6 = new Country("ALQ", "Algeria");
            List<Country> cList = new List<Country>();
            cList.Add(country1);
            cList.Add(country2);
            cList.Add(country3);
            cList.Add(country4);
            cList.Add(country5);
            cList.Add(country6);


            CountryCodeRepository repo = new CountryCodeRepository();
            repo.Clear();
            foreach (Country c in cList) {
              repo.Add(c);
            }
            repo.Delete(country6);

            Console.WriteLine("Task Complete!");
        }
    }
}
