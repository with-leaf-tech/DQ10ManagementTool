using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRClassLibrary.Image {
    public class Screen {
        public Bitmap screenCapture(int width, int height, int left, int top, string saveFile) {
            // スクリーンショット
            Bitmap captureImage = new System.Drawing.Bitmap(width, height);
            //Graphicsの作成
            Graphics g = Graphics.FromImage(captureImage);
            //画面全体をコピーする
            g.CopyFromScreen(new Point(left, top), new Point(0, 0), captureImage.Size);
            //解放
            g.Dispose();

            if(saveFile.Length > 0) {
                captureImage.Save(saveFile);
            }
            return captureImage;
        }
    }
}
