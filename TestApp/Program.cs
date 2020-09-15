using ItemClassLibrary;
using ItemClassLibrary.Manage;
using ItemClassLibrary.Util;
using Newtonsoft.Json;
using OCRClassLibrary.Camera;
using OCRClassLibrary.Image;
using OCRClassLibrary.OCR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            Bitmap bitmap = new Bitmap(@"C:\temp\image.png");

            System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
            byte[] a = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));

            string bbb = JsonConvert.SerializeObject(a);

            byte[] aaa = JsonConvert.DeserializeObject<byte[]>(bbb);

            Bitmap aa = new Bitmap(new MemoryStream(aaa));

            /*
            CameraManager camera = new CameraManager();
            var list = camera.GetDeviceList();

            camera.CameraStart(0);
            Bitmap bitmap = camera.CaptureImage(@"C:\temp\aaa.png");
            camera.CameraEnd();
            */
            //Screen sc = new Screen();
            //Bitmap bitmap = sc.screenCapture(300, 500, 20, 20, @"C:\Tools\e89239da.jpg");
            /*
            string text = "";
            Bitmap bitmap1 = new Bitmap(@"C:\Tools\e89239da.jpg");
            Bitmap bitmap2 = new Bitmap(@"C:\Tools\e89239da_1637348035721804313.jpg");
            Bitmap bitmap3 = new Bitmap(@"C:\Tools\e89239da_2637348035724694277.jpg");
            */
            /*
            AzureComputerVisionApiOCR ocr = new AzureComputerVisionApiOCR();
            ocr.initialize(@"C:\Users\tsutsumi\Downloads\azure.txt", "");
            text = ocr.GetTextFromImage(bitmap1);
            text = ocr.GetTextFromImage(bitmap2);
            text = ocr.GetTextFromImage(bitmap3);
            */
            /*
            GoogleVisionApiOCR ocr = new GoogleVisionApiOCR();
            ocr.initialize(@"C:\Users\tsutsumi\Downloads\try-apis-8b2095f28b0e.json", "");
            text = ocr.GetTextFromImage(bitmap1);
            text = ocr.GetTextFromImage(bitmap2);
            text = ocr.GetTextFromImage(bitmap3);
            */
            /*
            TesseractOCR ocr = new TesseractOCR();
            ocr.initialize(@"C:\Program Files\Tesseract-OCR\tessdata", "jpn");
            text = ocr.GetTextFromImage(bitmap1);
            text = ocr.GetTextFromImage(bitmap2);
            text = ocr.GetTextFromImage(bitmap3);
            */
            /*
            WindowsOCR ocr = new WindowsOCR();
            ocr.initialize("", "ja-JP");
            text = ocr.GetTextFromImage(bitmap1);
            text = ocr.GetTextFromImage(bitmap2);
            text = ocr.GetTextFromImage(bitmap3);
            */
            /*
            List<OneHandSword> items = new List<OneHandSword>();
            OneHandSword item = new OneHandSword();
            item.Name = "1";
            item.Description = "2";
            item.Refine = 1;
            item.Classification = 3;
            item.BasicAbility = new List<string>();
            item.BasicAbility.Add("4");
            item.BasicAbility.Add("5");
            item.BasicAbility.Add("6");
            items.Add(item);
            items.Add(item);

            string jsonString = JsonConvert.SerializeObject(items);
            List<OneHandSword> newItems = JsonConvert.DeserializeObject<List<OneHandSword>>(jsonString);
            */

            ItemManager manager = ItemManager.GetInstance();

            string text = @"
EE輝石のベルト+4
腰アクセサリーレア度_A
とりどりの宝石が
さまざまな効果を
生み出すベルト
取り引き不可
Lv 1以上装備可
追加効果
輝石効果:かいふく魔力 +1%
輝石効果:エレ系に ダメージ+6.0%
輝石効果:金
秘石効果:烏系に ダメージ +6.0%
>魅了 ガード+10.0%
戦士 僧侶 魔使 武闘 盗賊 旅芸 バト パラ 魔戦 レン 賢者 スパ
まも どう 踊り 占い 天地 遊び デス
O装備できる仲間モンスターを見る
";
            Utility.AnalyzeItem(1, text, manager.GetItemData());













            manager.DownloadItemData();
        }
    }
}
