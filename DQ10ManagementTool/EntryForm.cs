using ItemClassLibrary.Entity;
using ItemClassLibrary.Entity.Equipment;
using ItemClassLibrary.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DQ10ManagementTool {
    public partial class EntryForm : Form {
        protected List<ItemBase> entryItems = null;
        protected List<ItemBase> allitemList = null;

        List<ItemBase> defineList = new List<ItemBase>();


        private List<Label> nameLabel = new List<Label>();
        private List<TextBox> nameTextBox = new List<TextBox>();
        private List<ComboBox> nameComboBox = new List<ComboBox>();
        private List<NumericUpDown> itemCountNum = new List<NumericUpDown>();
        private Button entryButton = new Button();
        private Button cancelButton = new Button();
        private Button updateButton = new Button();
        private Button deleteButton = new Button();
        private PictureBox pictureArea = new PictureBox();

        private int userId = -1;
        private string saveItemFile = System.Configuration.ConfigurationManager.AppSettings["saveItemFile"];
        private string saveEquipFile = System.Configuration.ConfigurationManager.AppSettings["saveEquipFile"];
        private string imageFileName = "";
        private Bitmap screenShot = null;

        private bool updateMode = false;

        public EntryForm() {
            InitializeComponent();
        }

        public DialogResult ShowEntry() {
            updateMode = false;
            return ShowDialog();
        }
        public DialogResult ShowUpdate() {
            updateMode = true;
            return ShowDialog();
        }

        public void SetItems(int id, List<ItemBase> items, List<ItemBase> itemList, string imageFile) {
            userId = id;
            entryItems = items;
            allitemList = itemList;
            imageFileName = imageFile;

            if (Utility.EQUIP_CATEGORY_LIST.Contains(entryItems[0].Classification)) {
                defineList = allitemList.Where(x => Utility.EQUIP_CATEGORY_LIST.Contains(x.Classification)).ToList();
                if(imageFile == null) {
                    EquipmentBase equip = (EquipmentBase)items[0];
                    screenShot = Utility.DeSerializeBitmap(equip.Screenshot);
                }
            }
            else {
                defineList = allitemList.Where(x => Utility.ITEM_CATEGORY_LIST.Contains(x.Classification)).ToList();

            }
        }

        private void EntryForm_Load(object sender, EventArgs e) {
            Controls.Clear();
            nameLabel.Clear();
            nameTextBox.Clear();
            nameComboBox.Clear();
            itemCountNum.Clear();

            if (Utility.EQUIP_CATEGORY_LIST.Contains(entryItems[0].Classification)) {
                int index = 0;
                EquipmentBase equip = (EquipmentBase)entryItems[0];

                nameLabel.Add(new Label());
                nameLabel[nameLabel.Count -1].Text = "名前：";
                nameLabel[nameLabel.Count - 1].Size = new Size(80, 12);
                nameLabel[nameLabel.Count - 1].Location = new Point(10, 10 + index * 30);

                nameTextBox.Add(new TextBox());
                nameTextBox[nameTextBox.Count - 1].Name = "nameTextBox" + index;
                nameTextBox[nameTextBox.Count - 1].Text = equip.Name;
                nameTextBox[nameTextBox.Count - 1].Size = new Size(150, 12);
                nameTextBox[nameTextBox.Count - 1].Location = new Point(100, 8 + index * 30);
                nameTextBox[nameTextBox.Count - 1].TextChanged += nameTextBoxChange;

                nameComboBox.Add(new ComboBox());
                nameComboBox[nameComboBox.Count - 1].Name = "nameComboBox" + index;
                nameComboBox[nameComboBox.Count - 1].Items.AddRange(Utility.GetNearlyString(equip.Name, defineList.Select(x => x.Name).ToList()).ToArray());
                nameComboBox[nameComboBox.Count - 1].SelectedIndex = 0;
                nameComboBox[nameComboBox.Count - 1].Size = new Size(150, 12);
                nameComboBox[nameComboBox.Count - 1].Location = new Point(260, 8 + index * 30);
                index++;

                nameLabel.Add(new Label());
                nameLabel[nameLabel.Count -1].Text = "分類：";
                nameLabel[nameLabel.Count - 1].Size = new Size(80, 12);
                nameLabel[nameLabel.Count - 1].Location = new Point(10, 10 + index * 30);

                nameComboBox.Add(new ComboBox());
                nameComboBox[nameComboBox.Count - 1].Name = "nameComboBox" + index;
                nameComboBox[nameComboBox.Count - 1].Items.AddRange(Utility.EQUIP_CATEGORY_LIST.ToArray());
                nameComboBox[nameComboBox.Count - 1].SelectedItem = equip.Classification;
                nameComboBox[nameComboBox.Count - 1].Size = new Size(120, 12);
                nameComboBox[nameComboBox.Count - 1].Location = new Point(100, 6 + index * 30);
                index++;

                nameLabel.Add(new Label());
                nameLabel[nameLabel.Count - 1].Text = "説明：";
                nameLabel[nameLabel.Count - 1].Size = new Size(80, 12);
                nameLabel[nameLabel.Count - 1].Location = new Point(10, 10 + index * 30);

                nameTextBox.Add(new TextBox());
                nameTextBox[nameTextBox.Count - 1].Name = "nameTextBox" + index;
                nameTextBox[nameTextBox.Count - 1].Multiline = true;
                nameTextBox[nameTextBox.Count - 1].Text = equip.Description;
                nameTextBox[nameTextBox.Count - 1].Size = new Size(400, 80);
                nameTextBox[nameTextBox.Count - 1].Location = new Point(100, 8 + index * 30);
                index += 3;

                nameLabel.Add(new Label());
                nameLabel[nameLabel.Count - 1].Text = "製錬値：";
                nameLabel[nameLabel.Count - 1].Size = new Size(80, 12);
                nameLabel[nameLabel.Count - 1].Location = new Point(10, 10 + index * 30);

                itemCountNum.Add(new NumericUpDown());
                itemCountNum[itemCountNum.Count - 1].Minimum = 0;
                itemCountNum[itemCountNum.Count - 1].Maximum = 5;
                itemCountNum[itemCountNum.Count - 1].Name = "itemCountNum" + index;
                itemCountNum[itemCountNum.Count - 1].Value = equip.Refine;
                itemCountNum[itemCountNum.Count - 1].Size = new Size(50, 13);
                itemCountNum[itemCountNum.Count - 1].Location = new Point(100, 8 + index * 30);
                index++;

                nameLabel.Add(new Label());
                nameLabel[nameLabel.Count - 1].Text = "要求レベル：";
                nameLabel[nameLabel.Count - 1].Size = new Size(80, 12);
                nameLabel[nameLabel.Count - 1].Location = new Point(10, 10 + index * 30);

                itemCountNum.Add(new NumericUpDown());
                itemCountNum[itemCountNum.Count - 1].Minimum = 1;
                itemCountNum[itemCountNum.Count - 1].Maximum = 1000;
                itemCountNum[itemCountNum.Count - 1].Name = "itemCountNum" + index;
                itemCountNum[itemCountNum.Count - 1].Value = equip.RequireLevel;
                itemCountNum[itemCountNum.Count - 1].Size = new Size(50, 13);
                itemCountNum[itemCountNum.Count - 1].Location = new Point(100, 8 + index * 30);
                index++;

                nameLabel.Add(new Label());
                nameLabel[nameLabel.Count - 1].Text = "装備可能職：";
                nameLabel[nameLabel.Count - 1].Size = new Size(80, 12);
                nameLabel[nameLabel.Count - 1].Location = new Point(10, 10 + index * 30);

                nameLabel.Add(new Label());
                nameLabel[nameLabel.Count - 1].Text = string.Join(",", equip.EquipableJobs);
                nameLabel[nameLabel.Count - 1].Size = new Size(400, 12);
                nameLabel[nameLabel.Count - 1].Location = new Point(100, 10 + index * 30);
                index++;

                for (int i = 0; i < equip.BasicAbility.Count; i++) {
                    nameLabel.Add(new Label());
                    nameLabel[nameLabel.Count - 1].Text = "基礎効果：";
                    nameLabel[nameLabel.Count - 1].Size = new Size(80, 12);
                    nameLabel[nameLabel.Count - 1].Location = new Point(10, 10 + index * 30);

                    nameTextBox.Add(new TextBox());
                    nameTextBox[nameTextBox.Count - 1].Name = "nameTextBox" + index;
                    nameTextBox[nameTextBox.Count - 1].Text = equip.BasicAbility[i];
                    nameTextBox[nameTextBox.Count - 1].Size = new Size(150, 12);
                    nameTextBox[nameTextBox.Count - 1].Location = new Point(100, 8 + index * 30);
                    nameTextBox[nameTextBox.Count - 1].TextChanged += nameAbilityTextBoxChange;

                    nameComboBox.Add(new ComboBox());
                    nameComboBox[nameComboBox.Count - 1].Name = "nameComboBox" + index;
                    nameComboBox[nameComboBox.Count - 1].Items.AddRange(Utility.GetNearlyString(equip.BasicAbility[i], equip.AbilityPattern.Select(x => x).ToList()).ToArray());
                    nameComboBox[nameComboBox.Count - 1].SelectedItem = equip.BasicAbility[i];
                    nameComboBox[nameComboBox.Count - 1].Size = new Size(150, 12);
                    nameComboBox[nameComboBox.Count - 1].Location = new Point(260, 8 + index * 30);
                    index++;
                }

                for (int i = 0; i < equip.RefineAbility.Count; i++) {
                    string refineName = Utility.GetRefineAbility(equip.Classification, equip.Name);

                    nameLabel.Add(new Label());
                    nameLabel[nameLabel.Count - 1].Text = refineName + "効果：";
                    nameLabel[nameLabel.Count - 1].Size = new Size(80, 12);
                    nameLabel[nameLabel.Count - 1].Location = new Point(10, 10 + index * 30);

                    nameTextBox.Add(new TextBox());
                    nameTextBox[nameTextBox.Count - 1].Name = "nameTextBox" + index;
                    nameTextBox[nameTextBox.Count - 1].Text = equip.RefineAbility[i];
                    nameTextBox[nameTextBox.Count - 1].Size = new Size(150, 12);
                    nameTextBox[nameTextBox.Count - 1].Location = new Point(100, 8 + index * 30);
                    nameTextBox[nameTextBox.Count - 1].TextChanged += nameAbilityTextBoxChange;

                    nameComboBox.Add(new ComboBox());
                    nameComboBox[nameComboBox.Count - 1].Name = "nameComboBox" + index;
                    nameComboBox[nameComboBox.Count - 1].Items.AddRange(Utility.GetNearlyString(equip.RefineAbility[i], equip.AbilityPattern.Select(x => x).ToList()).ToArray());
                    nameComboBox[nameComboBox.Count - 1].SelectedItem = equip.RefineAbility[i];
                    nameComboBox[nameComboBox.Count - 1].Size = new Size(150, 12);
                    nameComboBox[nameComboBox.Count - 1].Location = new Point(260, 8 + index * 30);
                    index++;
                }

                for (int i = 0; i < equip.SpecialAbility.Count; i++) {
                    string refineName = Utility.GetSpecialAbility(equip.Classification, equip.Name);

                    nameLabel.Add(new Label());
                    nameLabel[nameLabel.Count - 1].Text = refineName + "効果：";
                    nameLabel[nameLabel.Count - 1].Size = new Size(80, 12);
                    nameLabel[nameLabel.Count - 1].Location = new Point(10, 10 + index * 30);

                    nameTextBox.Add(new TextBox());
                    nameTextBox[nameTextBox.Count - 1].Name = "nameTextBox" + index;
                    nameTextBox[nameTextBox.Count - 1].Text = equip.SpecialAbility[i];
                    nameTextBox[nameTextBox.Count - 1].Size = new Size(150, 12);
                    nameTextBox[nameTextBox.Count - 1].Location = new Point(100, 8 + index * 30);
                    nameTextBox[nameTextBox.Count - 1].TextChanged += nameAbilityTextBoxChange;

                    nameComboBox.Add(new ComboBox());
                    nameComboBox[nameComboBox.Count - 1].Name = "nameComboBox" + index;
                    nameComboBox[nameComboBox.Count - 1].Items.AddRange(Utility.GetNearlyString(equip.SpecialAbility[i], equip.AbilityPattern.Select(x => x).ToList()).ToArray());
                    nameComboBox[nameComboBox.Count - 1].SelectedItem = equip.SpecialAbility[i];
                    nameComboBox[nameComboBox.Count - 1].Size = new Size(150, 12);
                    nameComboBox[nameComboBox.Count - 1].Location = new Point(260, 8 + index * 30);
                    index++;
                }

                Controls.AddRange(nameLabel.ToArray());
                Controls.AddRange(nameTextBox.ToArray());
                Controls.AddRange(nameComboBox.ToArray());
                Controls.AddRange(itemCountNum.ToArray());

                if(imageFileName != null) {
                    pictureArea.Image = new Bitmap(imageFileName);
                }
                else {
                    if(screenShot != null) {
                        pictureArea.Image = screenShot;
                    }
                }
                pictureArea.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureArea.Size = new Size(320, 280);
                pictureArea.Location = new Point(550, 10);

                entryButton.Text = "登録";
                entryButton.Size = new Size(80, 20);
                entryButton.Location = new Point(690, 10 + index * 30);
                entryButton.Click += EntryButton_Click;

                cancelButton.Text = "キャンセル";
                cancelButton.Size = new Size(80, 20);
                cancelButton.Location = new Point(780, 10 + index * 30);
                cancelButton.Click += CancelButton_Click;

                Controls.Add(entryButton);
                Controls.Add(cancelButton);
                Controls.Add(pictureArea);

                if (updateMode) {
                    deleteButton.Text = "削除";
                    deleteButton.Size = new Size(80, 20);
                    deleteButton.Location = new Point(600, 10 + index * 30);
                    deleteButton.Click += DeleteButton_Click;

                    entryButton.Text = "更新";
                    Controls.Add(deleteButton);
                }


                this.Size = new Size(900, 80 + index * 30);
                this.ActiveControl = entryButton;
            }
            else {
                for (int i = 0; i < entryItems.Count; i++) {
                    nameLabel.Add(new Label());
                    nameLabel[i].Text = "名前：";
                    nameLabel[i].Size = new Size(35, 12);
                    nameLabel[i].Location = new Point(10, 10 + i * 30);

                    nameTextBox.Add(new TextBox());
                    nameTextBox[i].Name = "nameTextBox" + i;
                    nameTextBox[i].Text = entryItems[i].Name;
                    nameTextBox[i].Size = new Size(150, 12);
                    nameTextBox[i].Location = new Point(50, 8 + i * 30);
                    nameTextBox[i].TextChanged += nameTextBoxChange;

                    nameComboBox.Add(new ComboBox());
                    nameComboBox[i].Name = "nameComboBox" + i;
                    nameComboBox[i].Items.AddRange(Utility.GetNearlyString(entryItems[i].Name, defineList.Select(x => x.Name).ToList()).ToArray());
                    nameComboBox[i].SelectedIndex = 0;
                    nameComboBox[i].Size = new Size(150, 12);
                    nameComboBox[i].Location = new Point(210, 8 + i * 30);

                    itemCountNum.Add(new NumericUpDown());
                    itemCountNum[i].Minimum = 1;
                    itemCountNum[i].Maximum = 99;
                    itemCountNum[i].Name = "itemCountNum" + i;
                    itemCountNum[i].Value = entryItems[i].count;
                    itemCountNum[i].Size = new Size(50, 13);
                    itemCountNum[i].Location = new Point(370, 8 + i * 30);
                }
                Controls.AddRange(nameLabel.ToArray());
                Controls.AddRange(nameTextBox.ToArray());
                Controls.AddRange(nameComboBox.ToArray());
                Controls.AddRange(itemCountNum.ToArray());

                if (imageFileName != null) {
                    pictureArea.Image = new Bitmap(imageFileName);
                }
                pictureArea.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureArea.Size = new Size(320, 280);
                pictureArea.Location = new Point(550, 10);

                entryButton.Text = "登録";
                entryButton.Size = new Size(80, 20);
                entryButton.Location = new Point(590, 10 + entryItems.Count * 30);
                entryButton.Click += EntryButton_Click;

                cancelButton.Text = "キャンセル";
                cancelButton.Size = new Size(80, 20);
                cancelButton.Location = new Point(680, 10 + entryItems.Count * 30);
                cancelButton.Click += CancelButton_Click;

                Controls.Add(entryButton);
                Controls.Add(cancelButton);

                if (updateMode) {
                    deleteButton.Text = "削除";
                    deleteButton.Size = new Size(80, 20);
                    deleteButton.Location = new Point(500, 10 + entryItems.Count * 30);
                    deleteButton.Click += DeleteButton_Click;

                    entryButton.Text = "更新";
                    Controls.Add(deleteButton);
                }


                this.Size = new Size(800, 80 + entryItems.Count * 30);
                this.ActiveControl = entryButton;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e) {
            if (Utility.EQUIP_CATEGORY_LIST.Contains(entryItems[0].Classification)) {
                EquipmentBase equip = (EquipmentBase)entryItems[0];
                EquipmentBase prevEquip = equip.Clone();

                // 削除する
                List<EquipmentBase> equipList = new List<EquipmentBase>();
                if (File.Exists(userId + "_" + saveEquipFile)) {
                    equipList = JsonConvert.DeserializeObject<List<EquipmentBase>>(File.ReadAllText(userId + "_" + saveEquipFile));
                }
                if (updateMode) {
                    string itemName = prevEquip.Name;
                    string ability = string.Join(" ", prevEquip.RefineAbility) + " " + string.Join(" ", prevEquip.SpecialAbility);
                    equipList.Remove(equipList.Where(x => x.Name == itemName && string.Join(" ", x.RefineAbility) + " " + string.Join(" ", x.SpecialAbility) == ability).First());
                }
                File.WriteAllText(userId + "_" + saveEquipFile, JsonConvert.SerializeObject(equipList));
            }
            else {
                if(MessageBox.Show("以下全て削除されます！", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK) {
                    string updateItemName = entryItems[0].Name;

                    // 削除する
                    List<Item> itemList = new List<Item>();
                    if (File.Exists(userId + "_" + saveItemFile)) {
                        itemList = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(userId + "_" + saveItemFile));
                    }
                    if (updateMode) {
                        itemList.RemoveAll(x => x.Name == updateItemName);
                    }
                    File.WriteAllText(userId + "_" + saveItemFile, JsonConvert.SerializeObject(itemList));
                }
                else {
                    return;
                }
            }
            this.Hide();
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            this.Hide();
        }

        private void EntryButton_Click(object sender, EventArgs e) {
            int index = 0;
            if (Utility.EQUIP_CATEGORY_LIST.Contains(entryItems[0].Classification)) {
                EquipmentBase equip = (EquipmentBase)entryItems[0];
                EquipmentBase prevEquip = equip.Clone();

                Control[] control = this.Controls.Find("nameComboBox" + index, true);
                equip.Name = ((ComboBox)control[0]).SelectedItem.ToString();
                index++;

                control = this.Controls.Find("nameComboBox" + index, true);
                equip.Classification = ((ComboBox)control[0]).SelectedItem.ToString();
                index++;

                control = this.Controls.Find("nameTextBox" + index, true);
                equip.Description = ((TextBox)control[0]).Text;
                index += 3;

                control = this.Controls.Find("itemCountNum" + index, true);
                equip.Refine = (int)((NumericUpDown)control[0]).Value;
                index++;

                control = this.Controls.Find("itemCountNum" + index, true);
                equip.RequireLevel = (int)((NumericUpDown)control[0]).Value;
                index += 2;

                for (int i = 0; i < equip.BasicAbility.Count; i++) {
                    control = this.Controls.Find("nameComboBox" + index, true);
                    equip.BasicAbility[i] = ((ComboBox)control[0]).SelectedItem.ToString();
                    index++;
                }

                for (int i = 0; i < equip.RefineAbility.Count; i++) {
                    control = this.Controls.Find("nameComboBox" + index, true);
                    equip.RefineAbility[i] = ((ComboBox)control[0]).SelectedItem.ToString();
                    index++;
                }

                for (int i = 0; i < equip.SpecialAbility.Count; i++) {
                    control = this.Controls.Find("nameComboBox" + index, true);
                    equip.SpecialAbility[i] = ((ComboBox)control[0]).SelectedItem.ToString();
                    index++;
                }
                equip.AbilityList = equip.AbilityCalc(equip.BasicAbility.Concat(equip.RefineAbility).Concat(equip.SpecialAbility).ToList());
                equip.Screenshot = Utility.SerializeBitmap(new Bitmap(imageFileName));

                // 登録する
                List<EquipmentBase> equipList = new List<EquipmentBase>();
                if (File.Exists(userId + "_" + saveEquipFile)) {
                    equipList = JsonConvert.DeserializeObject<List<EquipmentBase>>(File.ReadAllText(userId + "_" + saveEquipFile));
                }
                if(updateMode) {
                    string itemName = prevEquip.Name;
                    string ability = string.Join(" ", prevEquip.RefineAbility) + " " + string.Join(" ", prevEquip.SpecialAbility);
                    equipList.Remove(equipList.Where(x => x.Name == itemName && string.Join(" ", x.RefineAbility) + " " + string.Join(" ", x.SpecialAbility) == ability).First());
                }
                equipList.Add(equip);
                File.WriteAllText(userId + "_" + saveEquipFile, JsonConvert.SerializeObject(equipList));
            }
            else {
                List<Item> entryItemData = new List<Item>();
                string updateItemName = entryItems[0].Name;
                for (int i = 0; i < entryItems.Count; i++) {
                    Control[] control = this.Controls.Find("nameComboBox" + index, true);
                    entryItems[i].Name = ((ComboBox)control[0]).SelectedItem.ToString();

                    control = this.Controls.Find("itemCountNum" + index, true);
                    entryItems[i].count = (int)((NumericUpDown)control[0]).Value;
                    entryItemData.Add((Item)entryItems[i]);
                    index++;
                }

                // 登録する
                List<Item> itemList = new List<Item>();
                if (File.Exists(userId + "_" + saveItemFile)) {
                    itemList = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(userId + "_" + saveItemFile));
                }
                if (updateMode) {
                    itemList.RemoveAll(x => x.Name == updateItemName);
                }
                itemList.AddRange(entryItemData);
                File.WriteAllText(userId + "_" + saveItemFile, JsonConvert.SerializeObject(itemList));
            }
            this.Hide();
        }

        private void nameAbilityTextBoxChange(object sender, EventArgs e) {
            string name = ((TextBox)sender).Name;
            string text = ((TextBox)sender).Text;
            Control[] combo = this.Controls.Find(name.Replace("nameTextBox", "nameComboBox"), true);
            ((ComboBox)combo[0]).Items.Clear();
            ((ComboBox)combo[0]).Items.AddRange(Utility.GetNearlyString(text, ((EquipmentBase)entryItems[0]).AbilityPattern).ToArray());
            ((ComboBox)combo[0]).SelectedIndex = 0;
        }

        private void nameTextBoxChange(object sender, EventArgs e) {
            string name = ((TextBox)sender).Name;
            string text = ((TextBox)sender).Text;
            Control[] combo = this.Controls.Find(name.Replace("nameTextBox", "nameComboBox"), true);
            ((ComboBox)combo[0]).Items.Clear();
            ((ComboBox)combo[0]).Items.AddRange(Utility.GetNearlyString(text, defineList.Select(x => x.Name).ToList()).ToArray());
            ((ComboBox)combo[0]).SelectedIndex = 0;
        }
    }
}
