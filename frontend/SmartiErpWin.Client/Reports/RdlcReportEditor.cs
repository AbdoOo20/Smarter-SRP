using System.Data;
using System.Xml.Linq;

namespace SmartiErpWin.Client.Reports;

public class RdlcReportEditor
{
    private XDocument _report;

    public RdlcReportEditor(string rdlcFilePath)
    {
        _report = XDocument.Load(rdlcFilePath);
    }

    public void AddDataTable(DataTable dataTable)
    {
        var dataSet = new XElement("DataSet",
            new XAttribute("Name", dataTable.TableName),
            new XElement("Query",
                new XElement("DataSourceName")
            ));

        // Add fields based on DataTable columns
        foreach (DataColumn column in dataTable.Columns)
        {
            dataSet.Add(new XElement("Field",
                new XAttribute("Name", column.ColumnName),
                new XElement("DataType", GetDataType(column.DataType))
            ));
        }

        // Add DataSet to the report
        var dataSetsElement = _report.Root.Element(XName.Get("DataSets", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition"));
        dataSetsElement.Add(dataSet);
    }

    public void AddColumn(string columnName, string dataType)
    {
        var tablix = _report.Root.Descendants(XName.Get("Tablix", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")).FirstOrDefault();
        if (tablix != null)
        {
            // Add a new column
            var column = new XElement("TablixColumn",
                new XElement("Width", "2.5cm"));
            var cells = tablix.Descendants(XName.Get("TablixCells", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")).FirstOrDefault();

            if (cells != null)
            {
                // Add a new cell for the new column
                var cell = new XElement("TablixCell",
                    new XElement("CellContents",
                        new XElement("Textbox",
                            new XAttribute("Name", columnName),
                            new XElement("CanGrow", "true"),
                            new XElement("KeepTogether", "true"),
                            new XElement("Paragraphs",
                                new XElement("Paragraph",
                                    new XElement("TextRuns",
                                        new XElement("TextRun",
                                            new XElement("Value"),
                                            new XElement("Style")
                                        )
                                    ),
                                    new XElement("Style")
                                )
                            ),
                            //new XElement("rd:DefaultName", columnName),
                            new XElement("Style",
                                new XElement("Border",
                                    new XElement("Color", "LightGrey"),
                                    new XElement("Style", "Solid")
                                ),
                                new XElement("PaddingLeft", "2pt"),
                                new XElement("PaddingRight", "2pt"),
                                new XElement("PaddingTop", "2pt"),
                                new XElement("PaddingBottom", "2pt")
                            )
                        )
                    )
                );

                cells.Add(cell);
                tablix.Element(XName.Get("TablixColumns", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")).Add(column);
            }
        }
    }

    public void Save(string outputPath)
    {
        _report.Save(outputPath);
    }

    private string GetDataType(Type type)
    {
        return type == typeof(string) ? "String" :
               type == typeof(int) ? "Integer" :
               type == typeof(decimal) ? "Decimal" :
               type == typeof(DateTime) ? "DateTime" :
               "String"; // Default to String
    }
}
