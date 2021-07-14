using System;
using System.Linq;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Windows;
using CsvHelper;
using CsvHelper.Configuration;
using ProfilesCalculator.Core.Models;

namespace ProfilesCalculator.Shared.Helpers
{
    public class ExcelFileReader
    {
        private Excel.Application _profilesExcelApp;
        private Excel.Workbook _myBook;
        private Excel.Worksheet _mySheet;
        private readonly string _path;

        public string NewPath { get; private set; }

        public ExcelFileReader(string path = @"C:\Users\rafal\Downloads\DANE_WYJŚCIOWE1.XLSX")
        {
            _path = path;
        }

        public void InitExcel()
        {
            _profilesExcelApp = new Excel.Application
            {
                Visible = false
            };
            _myBook = _profilesExcelApp.Workbooks.Open(_path);
            _mySheet = (Excel.Worksheet)_myBook.Sheets[1];
        }

        public IEnumerable<Profile> GetData()
        {
            try
            {
                int lastRow = _mySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;

                List<Profile> profileList = new();
                for (int index = 4; index <= lastRow; index++)
                {
                    Array data = (Array)_mySheet.get_Range("A" +
                       index.ToString(), "C" + index.ToString()).Cells.Value;

                    profileList.Add(new Profile
                    {
                        Name = data.GetValue(1, 1).ToString(),
                        Length = float.Parse(data.GetValue(1, 2).ToString()),
                        Quantity = int.Parse(data.GetValue(1, 3).ToString())
                    });
                }
                var sortedList = profileList.OrderByDescending(o => o.Length).ToArray();
                return sortedList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //MessageBox.Show("Cannot read Excel file.", "Oops, something went wrong: (", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public void StoreInCsvFile(IEnumerable<NewProfile> sortedNewProfiles)
        {
            int lastIndex = _path.LastIndexOf(Path.DirectorySeparatorChar) + 1;
            string substring = _path.Substring(lastIndex);

            Console.WriteLine(substring);

            NewPath = _path.Replace(substring, "profile.csv");

            Console.WriteLine(NewPath);
            using StreamWriter writer = new(NewPath);
            using CsvWriter csv = new(writer);
            csv.Configuration.Delimiter = ",";
            _ = csv.Configuration.RegisterClassMap<NewProfileMap>();
            _ = csv.Configuration.RegisterClassMap<ProfileMap>();
            csv.Configuration.UseNewObjectForNullReferenceMembers = true;
            writer.WriteLine("sep=" + csv.Configuration.Delimiter);

            foreach (var newProfile in sortedNewProfiles)
            {
                Console.WriteLine($"{newProfile.NewLenght}, {newProfile.WasteLength}, {newProfile.Max}");
                csv.WriteHeader(newProfile.GetType());
                csv.NextRecord();
                csv.WriteRecord(newProfile);
                csv.NextRecord();
                csv.WriteHeader(typeof(Profile));
                csv.NextRecord();
                csv.WriteRecords(newProfile.PartsOfNewProfile);
                writer.Flush();
            }
        }

        private sealed class NewProfileMap : ClassMap<NewProfile>
        {
            public NewProfileMap()
            {
                AutoMap();
            }
        }

        private sealed class ProfileMap : ClassMap<Profile>
        {
            public ProfileMap()
            {
                AutoMap();
                _ = Map(m => m.Quantity).Ignore();
            }
        }
    }
}
