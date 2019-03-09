
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace test
{
    public class CountryCodeRepository
    {
        //Sets the dataPath

        private string dataPath = Path.Combine(
        System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "Interview-Exercise");

        //private string dataPath = "/Users/Abraxas/Desktop/Output";

        //Checks to see if Country already in repository.  If not, adds country to repository
        public void Add(Country country)
        {
            //Checks that country has Code and Name
            if (country.Code == null || country.Name == null) 
            {
                Console.WriteLine("Country must contain a Name and a Code");
                return;
            }

            //Checks the format of the Country Code
            string check = CheckCountryCode(country.Code);
            if ( check != "success") 
            {
                Console.WriteLine(check);
                return;
            }

            //Makes sure output directory exists.  If not, it creates it.
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            //Get first letter of Country Code and create filepath string
            string initial = country.Code.Substring(0, 1);
            string filePath = dataPath + "/" + initial + ".cvs";
            bool found = false;

            //Create file if does not exist
            if (!File.Exists(filePath))
            {
                var myFile = File.Create(filePath);
                myFile.Close();
            }

            //split file string into array and check for matching country code.  If country code found, notify and end method
                string results = File.ReadAllText(filePath);
                if (results != null)
                {
                    var countryArray = results.Split(',');
                    foreach (string c in countryArray)
                    {
                        if (c.Length > 0)
                        {
                            var newItem = new Country();
                            var values = c.Split(';');
                            newItem.Code = values[0];
                            newItem.Name = values[1];
                            if (newItem.Code == country.Code) 
                            { 
                                found = true;
                                Console.WriteLine(country.Code + " already exists!");
                                return;
                            }
                            if (newItem.Name == country.Name)
                            {
                                Console.WriteLine(country.Name + " already exists!");
                                return;
                            }
                    }

                    }
                }
                //If country code not found in file append Country to end of file
                    if (!found) {
                        string newCountryStr = string.Format(
                        "{0};{1}",
                        country.Code,
                        country.Name) + ",";
                        File.AppendAllText(filePath, newCountryStr);
                    }

        }

        //Update existing Country
        public void Update(Country country)
        {
            string initial = country.Code.Substring(0, 1);
            string filePath = dataPath + "/" + initial + ".cvs";

            //Create dictionary from file string with code as key and name as value
            Dictionary<string, string> cDict =  DictCreator(filePath);

            //Check is code exists in dictionary. If not, notify and exit.
            if (!cDict.ContainsKey(country.Code)) {
                Console.WriteLine("No country with country code " + country.Code + " found.");
                return;
            }

            //If code exists in dictionary, update the country name
            cDict[country.Code] = country.Name;

            //Build string with updated information from dictionary
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (KeyValuePair<string, string> kvp in cDict)
            {
                string newCountryStr = string.Format(
                        "{0};{1},",
                        kvp.Key,
                        kvp.Value);
                sb.Append(newCountryStr);
            }

            //Overwrite file with updated string
            File.WriteAllText(filePath, sb.ToString());
        }

        //Delete existing country
        public void Delete(Country country)
        {
            string initial = country.Code.Substring(0, 1);
            string filePath = dataPath + "/" + initial + ".cvs";

            //Create dictionary from file string with code as key and name as value
            Dictionary<string, string> cDict = DictCreator(filePath);

            //Check is code exists in dictionary. If not, notify and exit.
            if (!cDict.ContainsKey(country.Code))
            {
                Console.WriteLine("No country with country code " + country.Code + " found.");
                return;
            }

            //If code exists in Dictionary, remove relevant entry from dictionary
            cDict.Remove(country.Code);

            //Build string with updated information from dictionary
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (KeyValuePair<string, string> kvp in cDict)
            {
                string newCountryStr = string.Format(
                        "{0};{1}",
                        kvp.Key,
                        kvp.Value) + ",";
                sb.Append(newCountryStr);
            }

            //Overwrite file with updated string
            File.WriteAllText(filePath, sb.ToString());
        }

        //Gets a country from a country code
        public Country Get(string countryCode)
        {
            countryCode = countryCode.ToUpper();
            string initial = countryCode.Substring(0, 1);
            string filePath = dataPath + "/" + initial + ".cvs";

            //Create dictionary from file string with code as key and name as value
            Dictionary<string, string> cDict = DictCreator(filePath);

            //Create an empty country
            Country country = new Country();

            //Check is code exists in dictionary. If not, notify and exit.
            if (!cDict.ContainsKey(countryCode))
            {
                Console.WriteLine("No country with country code " + countryCode + " found.");
                return country;
            }

            //If code exists in dictionary, add country data to country to create Country object
            country.Code = countryCode;
            country.Name = cDict[countryCode];

            //return country object
            return country;
        }


        //Deletes all repository files
        public void Clear()
        {
            DirectoryInfo di = new DirectoryInfo(dataPath);
            foreach (FileInfo f in di.GetFiles())
            {
                f.Delete();
            }
        }

        //Creates a string from a csv file and breaks it down into a dictionary with country code as Key and country name as Value
        private Dictionary<string, string> DictCreator(string filePath)
        {
            Dictionary<string, string> CodeDict = new Dictionary<string, string>();

            //First checks if csv file exists, if not notifies and exits
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File " + filePath + " not found.");
                return CodeDict;
            }

            string results = File.ReadAllText(filePath);

            //if file is not empty, turns file to array and then breaks array down into dictionary
            if (results != null)
            {
                var countryArray = results.Split(',');
                foreach (string c in countryArray)
                {
                    if (c != "")
                    {
                        var newItem = new Country();
                        var values = c.Split(';');
                        newItem.Code = values[0];
                        newItem.Name = values[1];
                        CodeDict.Add(newItem.Code, newItem.Name);
                    }
                }
            }
            else
            {
                Console.WriteLine("File " + filePath + " is empty.");
                return CodeDict;
            }
            return CodeDict;
        }

        //Checks country code string is within parameters
        private string CheckCountryCode(String code)
        {

            //checks length
            if (code.Length != 3)
            {
                return("Country Code must be 3 letter");
            }

            //checks that code is not within the following ranges: AAA to AAZ, QMA to QZZ, XAA to XZZ, and ZZA to ZZZ
            if ((string.Compare(code, "AAA", StringComparison.Ordinal) >= 0 && string.Compare(code, "AAZ", StringComparison.Ordinal) <= 0) ||
            (string.Compare(code, "QMA", StringComparison.Ordinal) >= 0 && string.Compare(code, "QZZ", StringComparison.Ordinal) <= 0) ||
                (string.Compare(code, "XAA", StringComparison.Ordinal) >= 0 && string.Compare(code, "XZZ", StringComparison.Ordinal) <= 0) ||
            (string.Compare(code, "ZZA", StringComparison.Ordinal) >= 0 && string.Compare(code, "ZZZ", StringComparison.Ordinal) <= 0))
            {
                return ("User-defined country code " + code + " not allowed");
            }
            else
            {
                return ("success");
            }
        }
    }
}
