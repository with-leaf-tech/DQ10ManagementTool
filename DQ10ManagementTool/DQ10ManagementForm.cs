﻿using OCRClassLibrary.Camera;
using OCRClassLibrary.OCR;
using System;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using DShowNET.Device;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using ItemClassLibrary.Util;
using ItemClassLibrary.Manage;
using ItemClassLibrary.Entity;
using System.Text;
using ItemClassLibrary.Entity.Equipment;

namespace DQ10ManagementTool {
    public partial class DQ10ManagementForm : Form {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        GoogleVisionApiOCR googleOcr = null;
        WindowsOCR windowsOcr = null;
        TesseractOCR tesseractOcr = null;
        AzureComputerVisionApiOCR azureOcr = null;
        OcrBase ocr = null;
        string imageFileName = "image.png";
        string settingFileName = "setting.txt";
        string userFileName = "user.txt";
        int settingCount = 6;
        bool noChange = false;

        List<(int top, int left, int right, int bottom, bool check)> settingList = new List<(int top, int left, int right, int bottom, bool check)>();
        List<(int id, string name, bool check)> userList = new List<(int id, string name, bool check)>();

        ItemManager itemManager = ItemManager.GetInstance();
        List<ItemBase> defineItemData = new List<ItemBase>();

        CameraManager camera = new CameraManager();

        public DQ10ManagementForm() {
            InitializeComponent();


            initialize();
        }

        private async void initialize() {
            noCaptureCheckBox.Checked = true;

            googleOcr = new GoogleVisionApiOCR();
            googleOcr.initialize(@"C:\Users\tsutsumi\Downloads\try-apis-8b2095f28b0e.json", "");
            windowsOcr = new WindowsOCR();
            azureOcr = new AzureComputerVisionApiOCR();
            await Task.Run(() => azureOcr.initialize(@"C:\Users\tsutsumi\Downloads\azure.txt", ""));

            await Task.Run(() => windowsOcr.initialize("", "ja-JP"));
            tesseractOcr = new TesseractOCR();
            tesseractOcr.initialize(@"C:\Program Files\Tesseract-OCR\tessdata", "jpn");
            ocrRadioWindows.Checked = true;

            ArrayList deviceList = camera.GetDeviceList();
            for(int i = 0; i < deviceList.Count; i++) {
                comboBoxDevice.Items.Add(((DsDevice)deviceList[i]).Name);
            }
            if(comboBoxDevice.Items.Count > 0) {
                comboBoxDevice.SelectedIndex = 0;
            }
            captureRadioDisplay.Checked = true;

            defineItemData = itemManager.GetItemData();
            if(defineItemData.Count == 0) {
                MessageBox.Show("アイテム定義データがありません。ダウンロードしてください。");
            }

            loadSetting();
            loadUserData();
        }

        private void loadUserData() {
            userListBox.Items.Clear();
            if(File.Exists(userFileName)) {
                string jsonString = File.ReadAllText(userFileName);
                userList = JsonConvert.DeserializeObject<List<(int id, string name, bool check)>>(jsonString);

                int selectIndex = -1;
                for(int i = 0; i < userList.Count; i++) {
                    userListBox.Items.Add(userList[i].name);
                    if(userList[i].check == true) {
                        selectIndex = i;
                    }
                }
                userListBox.SelectedIndex = selectIndex;
            }
        }

        private void addUserData(string name) {
            int nextId = userList.Count != 0 ? userList.Select(x => x.id).Max() + 1 : 0;
            userList.Add((nextId, name, false));
            userListBox.Items.Add(name);
        }

        private void deleteUserData(string name) {
            for (int i = 0; i < userList.Count; i++) {
                if (name == userList[i].name) {
                    userList.RemoveAt(i);
                    i--;
                }
            }
            saveUserData();
        }

        private void saveUserData() {
            string jsonString = JsonConvert.SerializeObject(userList);
            File.WriteAllText(userFileName, jsonString);
        }


        private void saveSetting() {
            screenCapture();

            string jsonString = JsonConvert.SerializeObject(settingList);
            File.WriteAllText(settingFileName, jsonString);
        }
        private void loadSetting() {
            settingList.Clear();
            if (File.Exists(settingFileName)) {
                string settingString = File.ReadAllText(settingFileName);
                settingList = JsonConvert.DeserializeObject<List<(int top, int left, int right, int bottom, bool check)>>(settingString);
            }
            else {
                settingList.Add((0, 0, 1, 1, true));
                for(int i = 1; i < settingCount; i++) {
                    settingList.Add((0, 0, 1, 1, false));
                }
            }

            for (int i = 0; i < settingCount; i++) {
                if (settingList[i].check == true) {
                    numericUpDownTop.Value = settingList[i].top;
                    numericUpDownLeft.Value = settingList[i].left;
                    numericUpDownRight.Value = settingList[i].right;
                    numericUpDownBottom.Value = settingList[i].bottom;

                    Control[] checkBox = this.Controls.Find("settingRadio" + (i + 1), true);
                    ((RadioButton)checkBox[0]).Checked = true;
                }
            }
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

        private void captureRadio_CheckedChanged(object sender, EventArgs e) {
            screenCapture();
        }

        private void screenCapture() {
            if (noCaptureCheckBox.Checked == false) {
                if (captureRadioDisplay.Checked) {
                    this.SendToBack();

                    // スクリーンショット
                    Bitmap captureImage = new System.Drawing.Bitmap((int)numericUpDownRight.Value, (int)numericUpDownBottom.Value);
                    //Graphicsの作成
                    Graphics g = Graphics.FromImage(captureImage);
                    //画面全体をコピーする
                    g.CopyFromScreen(new Point((int)numericUpDownLeft.Value, (int)numericUpDownTop.Value), new Point(0, 0), captureImage.Size);
                    //解放
                    g.Dispose();

                    captureImage.Save(imageFileName);

                    //表示
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = captureImage;

                    this.TopMost = true;
                    this.TopMost = false;
                }
                else {
                    camera.CameraStart(comboBoxDevice.SelectedIndex);

                    Bitmap captureImage = camera.CaptureImage();
                    //Graphicsの作成
                    Graphics g = Graphics.FromImage(captureImage);
                    //画面全体をコピーする
                    g.CopyFromScreen(new Point((int)numericUpDownLeft.Value, (int)numericUpDownTop.Value), new Point(0, 0), new Size((int)numericUpDownRight.Value, (int)numericUpDownBottom.Value));
                    //解放
                    g.Dispose();

                    camera.CameraEnd();
                }
            }
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e) {
            if(!noChange) {
                for (int i = 0; i < settingCount; i++) {
                    if (settingList[i].check == true) {
                        settingList[i] = ((int)numericUpDownTop.Value, (int)numericUpDownLeft.Value, (int)numericUpDownRight.Value, (int)numericUpDownBottom.Value, true);
                    }
                }
                saveSetting();
            }
        }

        private void settingRadio_CheckedChanged(object sender, EventArgs e) {
            noChange = true;
            for (int i = 0; i < settingCount; i++) {
                Control[] checkBox = this.Controls.Find("settingRadio" + (i + 1), true);
                if(((RadioButton)checkBox[0]).Checked) {
                    numericUpDownTop.Value = settingList[i].top;
                    numericUpDownLeft.Value = settingList[i].left;
                    numericUpDownRight.Value = settingList[i].right;
                    numericUpDownBottom.Value = settingList[i].bottom;
                    
                    settingList[i] = (settingList[i].top, settingList[i].left, settingList[i].right, settingList[i].bottom, true);
                }
                else {
                    settingList[i] = (settingList[i].top, settingList[i].left, settingList[i].right, settingList[i].bottom, false);
                }
            }
            saveSetting();
            noChange = false;
        }

        private void userAddButton_Click(object sender, EventArgs e) {
            string str = Microsoft.VisualBasic.Interaction.InputBox("入力してください", "ユーザー登録", default, 300, 400);
            if (str.Length > 0) {
                addUserData(str);
            }
            saveUserData();
        }

        private void userListBox_SelectedIndexChanged(object sender, EventArgs e) {
            for(int i = 0; i < userList.Count; i++) {
                if(i == userListBox.SelectedIndex) {
                    userList[i] = (userList[i].id, userList[i].name, true);
                }
                else {
                    userList[i] = (userList[i].id, userList[i].name, false);
                }
            }
            saveUserData();
        }

        private void userDeleteButton_Click(object sender, EventArgs e) {
            if (userListBox.SelectedIndex != -1) {
                if (MessageBox.Show("削除していいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                    deleteUserData(userListBox.SelectedItem.ToString());
                    userListBox.Items.RemoveAt(userListBox.SelectedIndex);
                }
            }
        }

        private async void captureTextButton_Click(object sender, EventArgs e) {
            screenCapture();
            if (ocrRadioWindows.Checked || ocrRadioAzure.Checked) {
                await Task.Run(() => GetOCRTest());
            }
            else {
                GetOCRTest();
            }
        }

        private void GetOCRTest() {
            Bitmap img = new Bitmap(imageFileName);
            string text = ocr.GetTextFromImage(img).Replace("\n", "\r\n");
            img.Dispose();

            List<(ItemBase, int)> analyzeData = Utility.AnalyzeItem(text, itemManager.GetItemData());
            string analyzeText = "";
            if (Utility.EQUIP_CATEGORY_LIST.Contains(analyzeData[0].Item1.Classification)) {
                analyzeText = DisplayEquip((EquipmentBase)analyzeData[0].Item1);
            }
            else {
                analyzeText = DisplayItem(analyzeData);
            }

            if (InvokeRequired) {
                Invoke((MethodInvoker)delegate {
                    captureTextBox.Text = text;
                    analyzeTextBox.Text = analyzeText;

                    logger.Info("読み取り結果：" + Environment.NewLine + captureTextBox.Text);
                });
            }
            else {
                captureTextBox.Text = text;
                analyzeTextBox.Text = analyzeText;

                logger.Info("読み取り結果：" + Environment.NewLine + captureTextBox.Text);
            }

        }

        private string DisplayEquip(EquipmentBase equip) {
            StringBuilder sb = new StringBuilder();
            sb.Append("名前:" + equip.Name + Environment.NewLine);
            sb.Append("分類:" + equip.Classification + Environment.NewLine);
            sb.Append("説明:" + equip.Description + Environment.NewLine);
            sb.Append("製錬値:" + equip.Refine + Environment.NewLine);
            sb.Append("必要レベル:" + equip.RequireLevel + Environment.NewLine);
            sb.Append("装備可能職:" + string.Join(",", equip.EquipableJobs) + Environment.NewLine);
            sb.Append("基礎効果:" + Environment.NewLine + "　" + string.Join(Environment.NewLine + "　", equip.BasicAbility) + Environment.NewLine + Environment.NewLine);
            sb.Append("合成効果:" + Environment.NewLine + "　" + string.Join(Environment.NewLine + "　", equip.RefineAbility) + Environment.NewLine + Environment.NewLine);
            sb.Append("特殊効果:" + Environment.NewLine + "　" + string.Join(Environment.NewLine + "　", equip.SpecialAbility) + Environment.NewLine);
            return sb.ToString();
        }

        private string DisplayItem(List<(ItemBase, int)> items) {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < items.Count; i++) {
                sb.Append("名前:" + items[i].Item1.Name + Environment.NewLine);
                sb.Append("分類:" + items[i].Item1.Classification + Environment.NewLine);
                sb.Append("説明:" + items[i].Item1.Description + Environment.NewLine);
                sb.Append("個数:" + items[i].Item2 + Environment.NewLine + Environment.NewLine);
            }
            return sb.ToString();
        }


        private void dataDownloadButton_Click(object sender, EventArgs e) {
            itemManager.DownloadItemData();
        }

        private void debug_button_Click(object sender, EventArgs e) {
            string text = @"
せかいじゅの葉
せかいじゅの葉
せかいじゅのしずく
まほうのせいすい
まほうのせいすい
まほうのせいすい
まほうのせいすい
まほうのせいすい
まほうのせいすい
まほうのせいすい
99こ
33こ
4こ
99こ
99こ
99こ
99こ
99こ
99こ
75こ
";

            List<(ItemBase, int)> analyzeData = Utility.AnalyzeItem(text, itemManager.GetItemData());
            string analyzeText = "";
            if (Utility.EQUIP_CATEGORY_LIST.Contains(analyzeData[0].Item1.Classification)) {
                analyzeText = DisplayEquip((EquipmentBase)analyzeData[0].Item1);
            }
            else {
                analyzeText = DisplayItem(analyzeData);
            }

            if (InvokeRequired) {
                Invoke((MethodInvoker)delegate {
                    captureTextBox.Text = text;
                    analyzeTextBox.Text = analyzeText;

                    logger.Info("読み取り結果：" + Environment.NewLine + captureTextBox.Text);
                });
            }
            else {
                captureTextBox.Text = text;
                analyzeTextBox.Text = analyzeText;

                logger.Info("読み取り結果：" + Environment.NewLine + captureTextBox.Text);
            }



        }
    }
}
