namespace WarehouseManagement.Services
{
    using ClosedXML.Excel;

    namespace WarehouseManagement.Services
    {
        public class ExcelExportService
        {
            public byte[] Export<T>(
                IEnumerable<T> data,
                string sheetName,
                string title,
                params (string Header, Func<T, object?> Value)[] columns)
            {
                using var workbook = new XLWorkbook();

                var worksheet = workbook.Worksheets.Add(
                    string.IsNullOrWhiteSpace(sheetName)
                        ? "Export"
                        : sheetName);

                int currentRow = 1;

                // ==========================================
                // TITLE
                // ==========================================

                worksheet.Cell(currentRow, 1).Value = title;

                worksheet.Range(
                    currentRow,
                    1,
                    currentRow,
                    columns.Length)
                    .Merge();

                worksheet.Cell(currentRow, 1)
                    .Style.Font.Bold = true;

                worksheet.Cell(currentRow, 1)
                    .Style.Font.FontSize = 16;

                currentRow += 2;


                // ==========================================
                // HEADER
                // ==========================================

                for (int columnIndex = 0;
                     columnIndex < columns.Length;
                     columnIndex++)
                {
                    var cell = worksheet.Cell(
                        currentRow,
                        columnIndex + 1);

                    cell.Value = columns[columnIndex].Header;

                    cell.Style.Font.Bold = true;

                    cell.Style.Fill.BackgroundColor =
                        XLColor.LightGray;

                    cell.Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Center;

                    cell.Style.Border.BottomBorder =
                        XLBorderStyleValues.Thin;
                }


                // ==========================================
                // DATA
                // ==========================================

                currentRow++;

                foreach (var item in data)
                {
                    for (int columnIndex = 0;
                         columnIndex < columns.Length;
                         columnIndex++)
                    {
                        var value =
                            columns[columnIndex].Value(item);

                        var cell = worksheet.Cell(
                            currentRow,
                            columnIndex + 1);

                        if (value == null)
                        {
                            cell.Value = string.Empty;
                        }
                        else if (value is DateTime dateTime)
                        {
                            cell.Value = dateTime;

                            cell.Style.DateFormat.Format =
                                "dd.MM.yyyy HH:mm";
                        }
                        else if (value is decimal decimalValue)
                        {
                            cell.Value = decimalValue;

                            cell.Style.NumberFormat.Format =
                                "#,##0.###";
                        }
                        else if (value is int intValue)
                        {
                            cell.Value = intValue;
                        }
                        else
                        {
                            cell.Value = value.ToString();
                        }
                    }

                    currentRow++;
                }


                // ==========================================
                // FILTER
                // ==========================================

                if (currentRow > 3)
                {
                    worksheet.Range(
                        3,
                        1,
                        currentRow - 1,
                        columns.Length)
                        .SetAutoFilter();
                }


                // ==========================================
                // FORMAT
                // ==========================================

                worksheet.SheetView.FreezeRows(2);

                worksheet.Columns()
                    .AdjustToContents();

                // Maximum width
                foreach (var column in worksheet.Columns())
                {
                    if (column.Width > 50)
                        column.Width = 50;
                }


                // ==========================================
                // EXPORT
                // ==========================================

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                return stream.ToArray();
            }
        }
    }
}
