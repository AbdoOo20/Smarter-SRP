using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Financial.Dtos;
using System.Text;

namespace ConsoleApp1.Runs;

public static class RDLCDesigner
{
    /// <summary>
    /// RDLC
    /// </summary>
    /// <param name="args"></param>
    public static void Start(string[] args)
    {

        var types = new[] {

            typeof(DtoBase),
            typeof(DtoStatment),

        };
        var xri = new System.Xml.Serialization.XmlReflectionImporter();
        var xss = new System.Xml.Serialization.XmlSchemas();
        var xse = new System.Xml.Serialization.XmlSchemaExporter(xss);
        foreach (var type in types)
        {
            var xtm = xri.ImportTypeMapping(type);
            xse.ExportTypeMapping(xtm);
        }
        var dir = Path.Combine(Directory.GetCurrentDirectory(), "../../../", "DataSources");
        Directory.CreateDirectory(dir);
        var file = Path.Combine(dir, "ReportItemSchemas.xsd");
        using var sw = new System.IO.StreamWriter(file, false, Encoding.UTF8);
        for (int i = 0; i < xss.Count; i++)
        {
            var xs = xss[i];
            xs.Id = "ReportItemSchemas";
            xs.Write(sw);
        }
    }

}