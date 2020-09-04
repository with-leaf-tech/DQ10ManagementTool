using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRClassLibrary.OCR {
    abstract public class OcrBase {

        public string option { set; get; }
        protected string _baseLang = "";

        public virtual void initialize(string accessKey, string baseLang) {
            option = "";
        }

        public virtual string GetTextFromImage(System.Drawing.Bitmap bitmap) {
            return "";
        }

        protected byte[] ImageToByte(System.Drawing.Image img) {
            System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }


    }
}
