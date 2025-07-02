# DotNetCsvComparisonTool
A C# utility to compare two CSV files based on a specified key column (e.g., serial number) and report discrepancies, new records, and deleted records.

## ✨ Features

* Compares two CSV files based on a configurable key column (`id_no`).
* Identifies discrepancies in field values for matching records.
* Detects records present in File 2 but not in File 1 (new records).
* Detects records present in File 1 but not in File 2 (deleted records).
* Outputs a detailed comparison report to the console and a text file.
* Supports configurable delimiters (default is Tab, can be changed to comma).

---

## 🚀 Prerequisites

* .NET 6.0 SDK or higher
* .NET Framework 4.8
* [CsvHelper](https://joshclose.github.io/CsvHelper/) library (already referenced in the code)

---

## 💡 Usage

1.  Open the project in your preferred IDE (e.g., Visual Studio, VS Code).
2.  Locate the `TestFile_FindsDifferencesCorrectly` method (or your main entry point) in the `CompareCsvFiles` class.
3.  Update the `file1Path` and `file2Path` variables to point to your CSV files.
4.  (Optional) Adjust the `Delimiter` in `CsvConfiguration` if your files use a different delimiter (e.g., `,`).
5.  Run the application. The comparison results will be printed to the console and saved to `comparison_results.txt` in your specified download directory.

**Example Code Snippet for calling the comparison:**

```csharp
string file1Path = @"C:\path\to\your\file1.csv";
string file2Path = @"C:\path\to\your\file2.csv";
var comparisonResult = CompareCsvFiles(file1Path, file2Path);
File.WriteAllText("comparison_results.txt", comparisonResult, Encoding.UTF8);
````

-----

## 📂 Example Files

Here are examples of `file01.csv` (File 1) and `file02.csv` (File 2) used for demonstration.

### `file01.csv` (File 1)

```csv
id_no      item_name  quantity status
SN001	Product A	10	Active
SN002	Product B	25	Inactive
SN003	Product C	5	Active
SN004	Product D	12	Active
```

### `P_SEARCH_STOCK_WS_RESPONSE_LOG_DBA.csv` (File 2)

```csv
id_no      item_name  quantity   status
SN001	Product A	10	Active
SN002	Product B	20	Active
SN005	Product E	8	Active
SN003	Product C	5	Active
```

-----

## 📊 Example Output

Running the comparison with the example files above would produce an output similar to this:

```
## 📝 CSV File Comparison Results
---
### ❗️ ID `SN002` has discrepancies:
- `quantity`: `25` (File 1) vs `20` (File 2)
- `status`: `Inactive` (File 1) vs `Active` (File 2)

### ➕ ID `SN005` is new (found only in File 2)

### ➖ ID `SN004` deleted (found only in File 1)

✅ Comparison results successfully saved to: C:\your_path\comparison_results.txt
```

```
```
