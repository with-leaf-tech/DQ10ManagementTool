using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCRClassLibrary.Camera {
    public class CameraManager {
        Capture cap = new Capture();

        public ArrayList GetDeviceList() {
            return cap.GetDeviceList();
        }

        public void CameraStart(int deviceNum) {
            cap.Initialize(deviceNum);
        }

        public void CameraEnd() {
            cap.CloseInterfaces();
        }

        public Bitmap CaptureImage() {
            return cap.CaptureImage();
        }
    }
}
