using OCRClassLibrary.Camera;
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

        private string saveItemFile = @"ItemData.json";
        private string saveEquipFile = @"EquipData.json";

        List<(int top, int left, int right, int bottom, bool check)> settingList = new List<(int top, int left, int right, int bottom, bool check)>();
        List<(int id, string name, bool check)> userList = new List<(int id, string name, bool check)>();

        ItemManager itemManager = ItemManager.GetInstance();
        List<ItemBase> defineItemData = new List<ItemBase>();

        List<ItemBase> entryItems = new List<ItemBase>();

        CameraManager camera = new CameraManager();

        //EntryEquipForm entryEquipWindow = new EntryEquipForm();
        //ItemEntryForm entryItemWindow = new ItemEntryForm();
        EntryForm entryWindow = new EntryForm();

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

            jobListBox.Items.AddRange(Utility.allJobs);

            for(int i = 0; i < Utility.REGIST_LIST.Length; i++) {
                Control[] comboBox = this.Controls.Find("registComboBox" + string.Format("{0:00}", (i + 1)), true);
                ((ComboBox)comboBox[0]).Items.Add(Utility.REGIST_LIST[i] + "ガード 指定なし");
                for (int j = 0; j < 10; j++) {
                    ((ComboBox)comboBox[0]).Items.Add(Utility.REGIST_LIST[i] + "ガード " + ((j+1) * 10) + "%以上");
                }
                ((ComboBox)comboBox[0]).SelectedIndex = 0;
            }

            searchEquipRadioButton.Checked = true;
            selectUserRadioButton.Checked = true;

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
            if(captureRadioCamera.Checked) {
                CameraInitilize();
            }
            else {
                CameraRelease();
            }

            screenCapture();
        }

        private void screenCapture() {
            try {
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
                        if (InvokeRequired) {
                            Invoke((MethodInvoker)delegate {
                                //camera.CameraStart(comboBoxDevice.SelectedIndex);

                                Bitmap captureImage = camera.CaptureImage(imageFileName);
                                //Graphicsの作成
                                Graphics g = Graphics.FromImage(captureImage);
                                //画面全体をコピーする
                                g.CopyFromScreen(new Point((int)numericUpDownLeft.Value, (int)numericUpDownTop.Value), new Point(0, 0), new Size((int)numericUpDownRight.Value, (int)numericUpDownBottom.Value));
                                //解放
                                g.Dispose();

                                //表示
                                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox1.Image = captureImage;
                                //camera.CameraEnd();
                            });
                        }
                        else {
                            //camera.CameraStart(comboBoxDevice.SelectedIndex);

                            Bitmap captureImage = camera.CaptureImage(imageFileName);
                            //Graphicsの作成
                            Graphics g = Graphics.FromImage(captureImage);
                            //画面全体をコピーする
                            g.CopyFromScreen(new Point((int)numericUpDownLeft.Value, (int)numericUpDownTop.Value), new Point(0, 0), new Size((int)numericUpDownRight.Value, (int)numericUpDownBottom.Value));
                            //解放
                            g.Dispose();

                            captureImage.Save(imageFileName);

                            // 画像を切り抜く
                            Bitmap bmpBase = new Bitmap(imageFileName);
                            Rectangle rect = new Rectangle((int)numericUpDownLeft.Value, (int)numericUpDownTop.Value, (int)numericUpDownRight.Value, (int)numericUpDownBottom.Value);
                            Bitmap bmpNew = bmpBase.Clone(rect, bmpBase.PixelFormat);

                            //表示
                            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBox1.Image = bmpNew;
                            //camera.CameraEnd();
                            bmpBase.Dispose();
                            //bmpNew.Dispose();
                        }

                    }
                }
            }
            catch(Exception ex) {
                logger.Error("画像キャプチャエラー Error=" + ex.Message + " StackTrace=" + ex.StackTrace);
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
            text = itemManager.replaceText(text);
            img.Dispose();

            if (InvokeRequired) {
                Invoke((MethodInvoker)delegate {
                    OcrMain(text);
                });
            }
            else {
                OcrMain(text);
            }
        }

        private void OcrMain(string text) {
            int userId = userList.Where(x => x.name == userListBox.SelectedItem.ToString()).First().id;
            List<ItemBase> analyzeData = Utility.AnalyzeItem(userId, text, itemManager.GetItemData());
            string analyzeText = "";
            if (Utility.EQUIP_CATEGORY_LIST.Contains(analyzeData[0].Classification)) {
                analyzeText = DisplayEquip((EquipmentBase)analyzeData[0]);
            }
            else {
                analyzeText = DisplayItem(analyzeData);
            }
            entryItems = analyzeData;

            captureTextBox.Text = text;
            analyzeTextBox.Text = analyzeText;

            logger.Info("読み取り結果：" + Environment.NewLine + captureTextBox.Text);
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

        private string DisplayItem(List<ItemBase> items) {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < items.Count; i++) {
                sb.Append("名前:" + items[i].Name + Environment.NewLine);
                sb.Append("分類:" + items[i].Classification + Environment.NewLine);
                sb.Append("説明:" + items[i].Description + Environment.NewLine);
                sb.Append("個数:" + items[i].count + Environment.NewLine + Environment.NewLine);
            }
            return sb.ToString();
        }


        private void dataDownloadButton_Click(object sender, EventArgs e) {
            itemManager.DownloadItemData();
        }

        private void debug_button_Click(object sender, EventArgs e) {
            string text = @"
カテドラルロープ+2
からE上レア度 B
使い込み度100
結晶の個数 39
Lv 99 以上装備可
錬金石D赤の錬金石
神に身を捧げた
高徳の聖職者を
守護する法衣
追加効果
錬金効果金即死ガー ド+60(+10)%
錬金効果:即死ガー ド+40%
できのよさ: しゅびカ +2
戦士 僧侶 魔使 武闘 盗賊 旅芸 バト パラ 魔戦 レン 賢者 スパ
まも どう 踊り 占い 天地 遊び デス
0錬金強化を見る
O装備できる仲間モンスターを見る
";
            text = itemManager.replaceText(text);
            int userId = userList.Where(x => x.name == userListBox.SelectedItem.ToString()).First().id;
            List<ItemBase> analyzeData = Utility.AnalyzeItem(userId, text, itemManager.GetItemData());
            string analyzeText = "";
            if (Utility.EQUIP_CATEGORY_LIST.Contains(analyzeData[0].Classification)) {
                analyzeText = DisplayEquip((EquipmentBase)analyzeData[0]);
            }
            else {
                analyzeText = DisplayItem(analyzeData);
            }
            entryItems = analyzeData;

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

        private void entryButton_Click(object sender, EventArgs e) {
            if(entryItems.Count == 0) {
                return;
            }
            int userId = userList.Where(x => x.name == userListBox.SelectedItem.ToString()).First().id;
            entryWindow.SetItems(userId, entryItems, defineItemData, imageFileName);
            entryWindow.ShowEntry();
        }

        private void displaySearchCategory() {
            searchPartsListBox.Items.Clear();
            searchAbilityListBox.Items.Clear();
            searchResultListBox.Items.Clear();
            if (searchEquipRadioButton.Checked) {
                searchPartsListBox.Items.AddRange(Utility.EQUIP_CATEGORY_LIST);
            }
            else {
                searchPartsListBox.Items.AddRange(Utility.ITEM_CATEGORY_LIST);
            }
        }

        private void displaySearchList() {
            searchAbilityListBox.Items.Clear();
            searchResultListBox.Items.Clear();
            List<int> userIdList = new List<int>();
            if (selectUserRadioButton.Checked) {
                userIdList.Add(userList.Where(x => x.name == userListBox.SelectedItem.ToString()).First().id);
            }
            else {
                userIdList.AddRange(userList.Select(x => x.id).ToArray());
            }

            if (searchPartsListBox.SelectedIndex >= 0) {
                if (searchEquipRadioButton.Checked) {
                    List<EquipmentBase> equipList = new List<EquipmentBase>();
                    for (int i = 0; i < userIdList.Count; i++) {
                        if (File.Exists(userIdList[i] + "_" + saveEquipFile)) {
                            equipList.AddRange(JsonConvert.DeserializeObject<List<EquipmentBase>>(File.ReadAllText(userIdList[i] + "_" + saveEquipFile)));
                        }
                    }
                    searchAbilityListBox.Items.AddRange(equipList.Where(x => x.Classification == searchPartsListBox.SelectedItem.ToString()).Select(x => x.Name).Distinct().ToArray());
                }
                else {
                    List<Item> itemList = new List<Item>();
                    for (int i = 0; i < userIdList.Count; i++) {
                        if (File.Exists(userIdList[i] + "_" + saveItemFile)) {
                            itemList.AddRange(JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(userIdList[i] + "_" + saveItemFile)));
                        }
                    }
                    searchAbilityListBox.Items.AddRange(itemList.Where(x => x.Classification == searchPartsListBox.SelectedItem.ToString()).Select(x => x.Name).Distinct().ToArray());
                }
            }
        }

        private void DispaySearchResult() {
            searchResultListBox.Items.Clear();
            Dictionary<int, string> nameDic = new Dictionary<int, string>();
            for(int i = 0; i < userList.Count; i++) {
                nameDic[userList[i].id] = userList[i].name;
            }
            List<int> userIdList = new List<int>();
            if (selectUserRadioButton.Checked) {
                userIdList.Add(userList.Where(x => x.name == userListBox.SelectedItem.ToString()).First().id);
            }
            else {
                userIdList.AddRange(userList.Select(x => x.id).ToArray());
            }
            if (searchPartsListBox.SelectedIndex >= 0 && searchAbilityListBox.SelectedIndex >= 0) {
                if (searchEquipRadioButton.Checked) {
                    List<EquipmentBase> equipList = new List<EquipmentBase>();
                    for (int i = 0; i < userIdList.Count; i++) {
                        if (File.Exists(userIdList[i] + "_" + saveEquipFile)) {
                            equipList.AddRange(JsonConvert.DeserializeObject<List<EquipmentBase>>(File.ReadAllText(userIdList[i] + "_" + saveEquipFile)));
                        }
                    }
                    List<EquipmentBase> displayList = equipList.Where(x => x.Classification == searchPartsListBox.SelectedItem.ToString() && x.Name == searchAbilityListBox.SelectedItem.ToString()).ToList();
                    searchResultListBox.Items.AddRange(displayList.Select(x => nameDic[x.OwnerId] + "\t" + x.Name + "\t" + string.Join(" ", x.RefineAbility) + " " + string.Join(" ", x.SpecialAbility)).ToArray());
                }
                else {
                    List<Item> itemList = new List<Item>();
                    for (int i = 0; i < userIdList.Count; i++) {
                        if (File.Exists(userIdList[i] + "_" + saveItemFile)) {
                            itemList.AddRange(JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(userIdList[i] + "_" + saveItemFile)));
                        }
                    }
                    List<Item> displayList = itemList.Where(x => x.Classification == searchPartsListBox.SelectedItem.ToString() && x.Name == searchAbilityListBox.SelectedItem.ToString()).ToList();
                    searchResultListBox.Items.AddRange(displayList.Select(x => nameDic[x.OwnerId] + "\t" + x.Name + " " + x.count + "個").ToArray());
                    searchResultListBox.Items.Add("　合計" + displayList.Select(x => x.count).Sum() + "個");
                }
            }

        }






        private void searchButton_CheckedChanged(object sender, EventArgs e) {
            displaySearchCategory();
        }

        private void userRadioButton_CheckedChanged(object sender, EventArgs e) {
            displaySearchList();
        }

        private void searchAbilityListBox_SelectedIndexChanged(object sender, EventArgs e) {
            DispaySearchResult();
        }

        private void searchPartsListBox_SelectedIndexChanged(object sender, EventArgs e) {
            displaySearchList();
        }

        private void searchResultListBox_DoubleClick(object sender, EventArgs e) {
            if(searchResultListBox.SelectedIndex < 0) {
                return;
            }

            string[] data = searchResultListBox.SelectedItem.ToString().Split(new char[] { '\t' });
            if(data.Length < 2) {
                return;
            }
            string userName = data[0];
            string itemName = data[1];
            if(itemName.Contains(" ")) {
                itemName = itemName.Split(new char[] { ' ' })[0];
            }
            int userId = userList.Where(x => x.name == userName).First().id;

            List<ItemBase> updateList = new List<ItemBase>();
            if(Utility.EQUIP_CATEGORY_LIST.Contains(defineItemData.Where(x => x.Name == itemName).Select(x => x.Classification).First())) {
                string ability = data[2];
                List<EquipmentBase> itemdata = JsonConvert.DeserializeObject<List<EquipmentBase>>(File.ReadAllText(userId + "_" + saveEquipFile));
                updateList = itemdata.Where(x => x.Name == itemName && string.Join(" ", x.RefineAbility) + " " + string.Join(" ", x.SpecialAbility) == ability).ToList<ItemBase>();
            }
            else {
                List<Item> itemdata = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(userId + "_" + saveItemFile));
                updateList = itemdata.Where(x => x.Name == itemName).ToList<ItemBase>();
            }

            entryWindow.SetItems(userId, updateList, defineItemData, null);
            entryWindow.ShowUpdate();
        }

        private void searchRegistButton_Click(object sender, EventArgs e) {
            searchResultListBox.Items.Clear();
            string user = userListBox.SelectedItem.ToString(); // 未使用
            string job = jobListBox.SelectedItem.ToString();
            bool onlySetEquip = setOnlyCheckBox.Checked;
            List<string> targetParts = (new string[] { Utility.PARTS_HEAD, Utility.PARTS_UPPERBODY, Utility.PARTS_LOWERBODY, Utility.PARTS_LEG }).ToList();
            List<string> appendParts = new List<string>();
            if(includeSheildCheckBox.Checked) {
                appendParts.Add(Utility.PARTS_SHIELD);
            }
            if(includeFaceCheckBox.Checked) {
                appendParts.Add(Utility.PARTS_ACCESSORY_FACE);
            }
            if (includeFingerCheckBox.Checked) {
                appendParts.Add(Utility.PARTS_ACCESSORY_FINGER);
            }
            if (includeWaistCheckBox.Checked) {
                appendParts.Add(Utility.PARTS_ACCESSORY_WAIST);
            }
            if (includeOtherCheckBox.Checked) {
                appendParts.Add(Utility.PARTS_ACCESSORY_OTHER);
            }

            int userId = userList.Where(x => x.name == user).First().id;
            List<EquipmentBase> haveEquipList = JsonConvert.DeserializeObject<List<EquipmentBase>>(File.ReadAllText(userId + "_" + saveEquipFile));

            Dictionary<string, float> needRegist = new Dictionary<string, float>();
            Dictionary<string, float> orbRegistList = new Dictionary<string, float>();
            for (int i = 0; i < Utility.REGIST_LIST.Length; i++) {
                Control[] comboBox = this.Controls.Find("registComboBox" + string.Format("{0:00}", (i + 1)), true);
                Control[] numericBox = this.Controls.Find("registOrbNum" + string.Format("{0:00}", (i + 1)), true);
                string[] regist = ((ComboBox)comboBox[0]).SelectedItem.ToString().Split(new char[] { ' ' });
                if (regist.Length > 1 && regist[1].Contains("%以上")) {
                    needRegist.Add(regist[0], float.Parse(regist[1].Replace("%以上", "")));
                    orbRegistList.Add(regist[0], float.Parse(((NumericUpDown)numericBox[0]).Value.ToString()));
                }
            }

            (List<List<EquipmentBase>> returnList, List<string> abilityList) list = itemManager.GetEquipList(user, job, onlySetEquip, targetParts, appendParts, haveEquipList, needRegist, orbRegistList);

            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < list.returnList.Count; i++) {
                List<EquipmentBase> setEquip = list.returnList[i];
                string ability = list.abilityList[i];
                sb.Append("　" + (i+1) + "件目" + Environment.NewLine);
                for (int j = 0; j < setEquip.Count; j++) {
                    sb.Append(user + "\t" + setEquip[j].Name + "\t" + string.Join(" ", setEquip[j].RefineAbility) + " " + string.Join(" ", setEquip[j].SpecialAbility) + Environment.NewLine);
                }
                sb.Append("　全体の耐性 " + ability + Environment.NewLine + Environment.NewLine);
            }

            searchResultListBox.Items.AddRange(sb.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None));

        }

        private void replaceAddButton_Click(object sender, EventArgs e) {
            itemManager.addReplaceString(replaceSourceTextBox.Text, replaceDistTextBox.Text);
        }

        private void autoCaptureTextStartButton_Click(object sender, EventArgs e) {
            timer1.Interval = int.Parse(autoCaptureInterval.Value.ToString()) * 1000;
            timer1.Enabled = true;
        }

        private void autoCaptureTextStopButton_Click(object sender, EventArgs e) {
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e) {
            timer1.Enabled = false;
            captureTextButton_Click(null, null);
            entryButton_Click(null, null);
            timer1.Enabled = true;
        }

        private void comboBoxDevice_SelectedIndexChanged(object sender, EventArgs e) {
            if (captureRadioCamera.Checked) {
                CameraInitilize();
            }
            else {
                CameraRelease();
            }
        }

        private void CameraInitilize() {
            CameraRelease();
            camera = new CameraManager();
            camera.CameraStart(comboBoxDevice.SelectedIndex);
        }

        private void CameraRelease() {
            if(camera != null) {
                camera.CameraEnd();
                camera = null;
            }
        }
    }
}
