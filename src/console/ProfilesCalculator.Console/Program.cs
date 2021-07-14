using ProfilesCalculator.Core;
using ProfilesCalculator.Core.Models;
using ProfilesCalculator.Shared;
using ProfilesCalculator.Shared.Helpers;
using System.Linq;

namespace ProfilesCalculator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string param in args)
            {
                System.Console.WriteLine("param: " + param);
            }
            //ReadFromExcelFile(args);
            SaveToExcelFile(args);
        }

        private static void SaveToExcelFile(string[] args)
        {
            var reader = new ExcelFileReader(@"C:\Users\razn\Downloads\DANE_WYJŚCIOWE1.XLSX");
            reader.InitExcel();

            var profileList = reader.GetData();
            IProfileGenerator generator = new CutProfiles(ref profileList, 12000, 5);
            var newProfiles = generator.GenerateNewProfiles();

            var sortedNewProfiles = newProfiles.OrderBy(x => x.WasteLength).ToArray();

            reader.StoreInCsvFile(sortedNewProfiles);
            System.Console.ReadKey();
        }

        private static void ReadFromExcelFile(string[] args)
        {
            //var reader = new ExcelFileReader(args[0]);
            var reader = new ExcelFileReader();
            reader.InitExcel();

            var profileList = reader.GetData();
            IProfileGenerator generator = new CutProfiles(ref profileList, 12000, 5);
            var newProfiles = generator.GenerateNewProfiles();

            System.Console.WriteLine("==================================");
            System.Console.WriteLine("profileList size: " + profileList.Count());
            System.Console.WriteLine("newProfiles size: " + newProfiles.Count());
            System.Console.WriteLine("==================================");

            var sortedNewProfiles = newProfiles.OrderBy(x => x.WasteLength).ToList();
            int count = 0;
            foreach (NewProfile newProfileItem in sortedNewProfiles)
            {
                System.Console.WriteLine("*************************************************************");
                System.Console.WriteLine("new profile: " + newProfileItem);
                count += newProfileItem.PartsOfNewProfile.Count;
                foreach (Profile newProfilePart in newProfileItem.PartsOfNewProfile)
                {
                    System.Console.WriteLine("\tNP: " + newProfilePart.Name + " : " + newProfilePart.Quantity + " : " + newProfilePart.Length);
                }
            }
            System.Console.WriteLine("==================================");
            System.Console.WriteLine("All profiles created: " + count);
            System.Console.ReadKey();
        }
    }
}
