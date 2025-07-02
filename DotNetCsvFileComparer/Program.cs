using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCsvFileComparer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Specify the paths of your two CSV files here
            string file1Path = @"C:\your_path\file01.csv";
            string file2Path = @"C:\your_path\file02.csv";

            // 2. Call the comparison function
            var comparisonResult = CompareCsvFiles(file1Path, file2Path);

            // 2. Specify the output file name
            string outputFilePath = "C:\\your_path\\comparison_results.txt";

            // 3. Write the result to a .txt file with UTF-8 Encoding
            File.WriteAllText(outputFilePath, comparisonResult, Encoding.UTF8);

            // 4. (Optional) Inform the user that saving is complete
            Console.WriteLine($"Comparison results successfully saved to: {outputFilePath}");
        }
 
        public static string CompareCsvFiles(string file1Path, string file2Path)
        {
            // Configure CsvHelper to support Tab as a delimiter
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t", // Or change to "," if your file uses commas
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                MissingFieldFound = null // Better handling of mismatched columns
            };

            // Read data from both files
            var records1 = ReadCsvFile(file1Path, config);
            var records2 = ReadCsvFile(file2Path, config);

            // Get Headers
            var headers = GetHeaders(file1Path, config);

            // Create a Dictionary from the first file for faster lookup
            var records1Dict = records1.ToDictionary(r => r["id_no"]);

            var differences = new StringBuilder();
            var checkedIds = new HashSet<string>();

            differences.AppendLine("## CSV File Comparison Results");
            differences.AppendLine("---");

            // --- Compare data from File 2 with File 1 ---
            foreach (var record2 in records2)
            {
                var id = record2["id_no"];
                checkedIds.Add(id);

                if (records1Dict.TryGetValue(id, out var record1))
                {
                    // Same ID found, compare data in each column
                    var rowDifferences = new List<string>();
                    foreach (var header in headers)
                    {
                        // Check if the column exists in both records
                        var value1Exists = record1.TryGetValue(header, out var value1);
                        var value2Exists = record2.TryGetValue(header, out var value2);

                        // Treat NULL or empty values as empty strings for comparison
                        value1 = value1 ?? string.Empty;
                        value2 = value2 ?? string.Empty;

                        if (!value1.Equals(value2))
                        {
                            rowDifferences.Add($"`{header}`: `{value1}` (File 1) vs `{value2}` (File 2)");
                        }
                    }

                    if (rowDifferences.Any())
                    {
                        differences.AppendLine($"### ❗️ ID `{id}` has discrepancies:");
                        foreach (var diff in rowDifferences)
                        {
                            differences.AppendLine($"- {diff}");
                        }
                        differences.AppendLine();
                    }
                }
                else
                {
                    // This ID is not found in File 1
                    differences.AppendLine($"### ➕ ID `{id}` is new (found only in File 2)\n");
                }
            }

            // --- Check for IDs present in File 1 but not in File 2 ---
            var missingIds = records1Dict.Keys.Where(id => !checkedIds.Contains(id)).ToList();
            if (missingIds.Any())
            {
                foreach (var id in missingIds)
                {
                    differences.AppendLine($"### ➖ ID `{id}` deleted (found only in File 1)\n");
                }
            }

            if (differences.Length == 0)
            {
                differences.AppendLine("Data in both files are identical.");
            }

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(differences.ToString());
            return differences.ToString();
        }

        // Function to read data from a CSV file
        private static List<Dictionary<string, string>> ReadCsvFile(string filePath, CsvConfiguration config)
        {
             var reader = new StreamReader(filePath, Encoding.UTF8);
             var csv = new CsvReader(reader, config);
            var records = new List<Dictionary<string, string>>();
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var record = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var header in csv.HeaderRecord)
                {
                    record[header] = csv.GetField(header);
                }
                records.Add(record);
            }
            return records;
        }

        // Function to get column headers
        private static string[] GetHeaders(string filePath, CsvConfiguration config)
        {
             var reader = new StreamReader(filePath, Encoding.UTF8);
             var csv = new CsvReader(reader, config);
            csv.Read();
            csv.ReadHeader();
            return csv.HeaderRecord;
        }

     


    }
}
