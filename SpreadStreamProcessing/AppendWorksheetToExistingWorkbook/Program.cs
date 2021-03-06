﻿using System;
using System.IO;
using System.Linq;
using Telerik.Documents.SpreadsheetStreaming;

namespace AppendWorksheetToExistingWorkbook
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter existing .xlsx file name: ");

            string fileName = Console.ReadLine();
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Such file does not exists. Press any key to exit.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Adding new worksheet to the existing workbook...");

            AddWorksheetToExistingDocument(fileName);

            Console.Write("Want to open the document? (Y/N)");
            var readKey = Console.ReadKey();
            if (readKey.KeyChar == 'Y' || readKey.KeyChar == 'y')
            {
                System.Diagnostics.Process.Start(fileName);
            }
        }

        private static void AddWorksheetToExistingDocument(string filePath)
        {
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                // Pass SpreadExportMode.Append parameter, and the created workbook exporter will preserve all of the existing worksheets.
                using (IWorkbookExporter workbook = SpreadExporter.CreateWorkbookExporter(SpreadDocumentFormat.Xlsx, stream, SpreadExportMode.Append))
                {
                    string sheetName = "Sheet name here";

                    var importedSheetsNames = workbook.GetSheetInfos().Select(sheetInfo => sheetInfo.Name);
                    if (importedSheetsNames.Contains(sheetName))
                    {
                        Console.WriteLine("Sheet with that name already exists in the workbook.");
                        return;
                    }

                    using (IWorksheetExporter worksheet = workbook.CreateWorksheetExporter(sheetName))
                    {
                        using (IRowExporter row = worksheet.CreateRowExporter())
                        {
                            using (ICellExporter cell = row.CreateCellExporter())
                            {
                                cell.SetValue("value 1");
                                cell.SetValue("value 2");
                                cell.SetValue("value 3");
                            }
                        }
                    }
                }
            }
        }
    }
}
