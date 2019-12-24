using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using XSDR;

namespace XSDR.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var xmlImporter = new XMLImporter();
            var wordExporter = new WordExporter();

            for (var i = 1; i <= 3; i++)
            {
                var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "../../Examples/example" + i + ".xml");
                var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "../../Examples/example" + i + ".dss");
                var filePath3 = Path.Combine(Directory.GetCurrentDirectory(), "../../Examples/example" + i + ".docx");

                var document = xmlImporter.ImportDocument(filePath1, filePath2);

                wordExporter.ExportXSDRDocument(document, filePath3);
            }
        }
    }
}
