using OCRClassLibrary.OCR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DQ10ManagementTool {
    public partial class DQ10ManagementForm : Form {

        GoogleVisionApiOCR googleOcr = null;
        WindowsOCR windowsOcr = null;
        TesseractOCR tesseractOcr = null;
        AzureComputerVisionApiOCR azureOcr = null;
        OcrBase ocr = null;

        public DQ10ManagementForm() {
            InitializeComponent();


            initialize();
        }

        private async void initialize() {
            googleOcr = new GoogleVisionApiOCR();
            googleOcr.initialize(@"C:\Users\tsutsumi\Downloads\try-apis-8b2095f28b0e.json", "");
            windowsOcr = new WindowsOCR();
            azureOcr = new AzureComputerVisionApiOCR();
            await Task.Run(() => azureOcr.initialize(@"C:\Users\tsutsumi\Downloads\azure.txt", ""));

            await Task.Run(() => windowsOcr.initialize("", "ja-JP"));
            tesseractOcr = new TesseractOCR();
            tesseractOcr.initialize(@"C:\Program Files\Tesseract-OCR\tessdata", "jpn");
            ocrRadioWindows.Checked = true;

            ocrRadioWindows.Checked = true;
        }

        private void ocrRadio_CheckedChanged(object sender, EventArgs e) {
            if (ocrRadioGoogle.Checked) {
                ocr = googleOcr;
            }
            else if (ocrRadioTesseract.Checked) {
                ocr = tesseractOcr;
            }
            else if (ocrRadioAzure.Checked) {
                ocr = azureOcr;
            }
            else {
                ocr = windowsOcr;
            }
        }
    }
}
