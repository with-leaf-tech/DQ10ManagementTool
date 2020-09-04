using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace OCRClassLibrary.OCR {
    public class TesseractOCR : OcrBase {
        TesseractEngine tesseract = null;

        public TesseractOCR() {

        }

        [STAThread]
        public override void initialize(string accessKey, string baseLang) {
            base.initialize(accessKey, baseLang);
            tesseract = new TesseractEngine(accessKey, baseLang);
        }

        [STAThread]
        public override string GetTextFromImage(System.Drawing.Bitmap bitmap) {
            Page page = tesseract.Process(PixConverter.ToPix(bitmap));
            string retText = page.GetText();
            page.Dispose();

            return retText;
        }
    }
}
