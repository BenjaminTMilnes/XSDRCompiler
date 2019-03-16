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
            var wordExporter = new WordExporter();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../Examples/example0.docx");

            wordExporter.ExportXSDRDocument(new XSDRDocument(), filePath);
        }
    }
}
