using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using RapidSoft.GeoPoints.Entities;
using RapidSoft.Loaders.Geocoder;
using RapidSoft.Loaders.Geocoder.Entities;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace RapidSoft.Loaders.Geocoder.DataSources
{
    [DataSourceInfo(Name = "BankServicePointsExcel")]
    public class BankServicePointsExcelDataSource : IDataSource
    {
        Configuration config = new Configuration();

        public IEnumerable<ILocation> GetLocations()
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(config.SourceFile, false))
            {
                var workBook = document.WorkbookPart.Workbook;
                var workSheets = workBook.Descendants<Sheet>();
                var sharedStrings =
                  document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                var custID = workSheets.First().Id;
                var worksheet = (WorksheetPart)document.WorkbookPart.GetPartById(custID);

                var dataRows =
                      from row in worksheet.Worksheet.Descendants<Row>()
                      where (row.Descendants<Cell>().ElementAt(9).CellValue == null || String.IsNullOrEmpty(row.Descendants<Cell>().ElementAt(9).CellValue.InnerText))
                            || (row.Descendants<Cell>().ElementAt(10).CellValue == null || String.IsNullOrEmpty(row.Descendants<Cell>().ElementAt(10).CellValue.InnerText))
                            || (row.Descendants<Cell>().ElementAt(9).CellValue.InnerText == "0")
                            || (row.Descendants<Cell>().ElementAt(10).CellValue.InnerText == "0")
                      select row;

                List<ILocation> locations = new List<ILocation>();
                foreach (var row in dataRows)
                {
                    var cellValues = (from cell in row.Descendants<Cell>()
                                      select (cell.CellValue == null ? String.Empty : cell.DataType != null && cell.DataType.HasValue &&
                                              cell.DataType == CellValues.SharedString ? sharedStrings.ChildElements[int.Parse(cell.CellValue.InnerText)].InnerText : cell.CellValue.InnerText)).ToArray();

                    //var cellValues = from cell in row.Descendants<Cell>()
                    //                 select cell;

                    locations.Add(new BankServicePoint()
                    {
                        Id = row.RowIndex.Value,
                        City = cellValues[5],
                        SourceAddress = cellValues[6],
                        Address = String.Format("{0}, {1}", cellValues[5], cellValues[6])
                    });
                }

                return locations;
            }
        }

        private object GetValue(object value)
        {
            if (value is DBNull)
            {
                return null;
            }
            return value;
        }

        public void UpdateGeoInfo(object key, LocationGeoInfo geoInfo)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(config.SourceFile, true))
            {
                var workBook = document.WorkbookPart.Workbook;
                var workSheets = workBook.Descendants<Sheet>();
                var sharedStrings =
                  document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                var custID = workSheets.First().Id;
                var worksheet = (WorksheetPart)document.WorkbookPart.GetPartById(custID);

                var dataRow =
                      (from row in worksheet.Worksheet.Descendants<Row>()
                      where row.RowIndex.Value == (uint)key
                      select row).FirstOrDefault();

                var cellValues = from cell in dataRow.Descendants<Cell>()
                                 select cell;

                cellValues.ElementAt(9).CellValue = new CellValue(geoInfo.Lat.ToString(System.Globalization.CultureInfo.InvariantCulture));
                cellValues.ElementAt(10).CellValue = new CellValue(geoInfo.Lng.ToString(System.Globalization.CultureInfo.InvariantCulture));

                worksheet.Worksheet.Save();
            }
        }

        private string GetAddress(BankServicePoint location)
        {
            return String.Format("{0}, {1}", location.City, location.SourceAddress);
        }
    }
}
