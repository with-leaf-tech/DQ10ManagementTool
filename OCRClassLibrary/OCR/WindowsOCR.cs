using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;

namespace OCRClassLibrary.OCR {
    public class WindowsOCR : OcrBase {

        public WindowsOCR() {

        }

        public override void initialize(string accessKey, string baseLang) {
            base.initialize(accessKey, baseLang);
            _baseLang = baseLang;
        }

        public override string GetTextFromImage(System.Drawing.Bitmap bitmap) {
            string returnText = OcrFromBitmap(bitmap, _baseLang);
            return returnText;
        }

        private string OcrFromBitmap(Bitmap bitmap, string baseLang) {
            StringBuilder sb = new StringBuilder();
            var result = Recognize(bitmap, baseLang);
            foreach (var l in result.Lines) {
                sb.Append(l.Text + Environment.NewLine);
            }
            return sb.ToString();
        }

        private OcrResult Recognize(Bitmap bitmap, string baseLang) {
            Task<OcrResult> result = OcrMain(bitmap, baseLang);
            result.Wait();
            return result.Result;
        }

        private async Task<OcrResult> OcrMain(Bitmap bitmap, string baseLang) {
            OcrEngine ocrEngine = OcrEngine.TryCreateFromLanguage(new Language(baseLang));

            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var stream = await ConvertToRandomAccessStream(ms);
            var bb = await LoadImage(stream);

            var ocrResult = await ocrEngine.RecognizeAsync(bb);
            return ocrResult;
        }

        private async Task<IRandomAccessStream> ConvertToRandomAccessStream(MemoryStream memoryStream) {
            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            var dw = new DataWriter(outputStream);
            var task = new Task(() => dw.WriteBytes(memoryStream.ToArray()));
            task.Start();
            await task;
            await dw.StoreAsync();
            await outputStream.FlushAsync();
            return randomAccessStream;
        }

        private async Task<SoftwareBitmap> LoadImage(IRandomAccessStream stream) {
            var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
            var bitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            return bitmap;
        }
    }
}
