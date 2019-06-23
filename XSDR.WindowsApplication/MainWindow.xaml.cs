using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;
using System.IO;
using XSDR;

namespace XSDR.WindowsApplication
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void browseButton1_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == true)
            {
                textBox1.Text = fileDialog.FileName;
            }
        }

        private void browseButton2_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == true)
            {
                textBox2.Text = fileDialog.FileName;
            }
        }

        private void compileButton_Click(object sender, RoutedEventArgs e)
        {
            var xmlImporter = new XMLImporter();
            var wordExporter = new WordExporter();

            var filePath1 = textBox1.Text;
            var filePath2 = textBox2.Text;

            var directoryPath = Path.GetDirectoryName(filePath1);
            var fileName = Path.GetFileNameWithoutExtension(filePath1);
            var filePath3 = Path.Combine(directoryPath, fileName + ".docx");

            var document = xmlImporter.ImportDocument(filePath1, filePath2);

            wordExporter.ExportXSDRDocument(document, filePath3);
        }
    }
}
