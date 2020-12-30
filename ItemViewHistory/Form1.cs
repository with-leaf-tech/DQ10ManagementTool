using ItemClassLibrary.Entity;
using ItemClassLibrary.Manage;
using ItemClassLibrary.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemViewHistory {
    public partial class Form1 : Form {

        ItemManager manager = ItemManager.GetInstance();
        List<ItemBase> itemList = null;
        List<ItemHistory> itemHist = null;

        object lockObject = new object();

        public Form1() {
            InitializeComponent();

            comboBox1.Items.Add("すべて");
            comboBox1.Items.AddRange(Utility.ITEM_MARKET_LIST);
            comboBox1.SelectedIndex = 0;
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";

            int columnNum = 0;

            DataGridViewLinkColumn link1 = new DataGridViewLinkColumn();
            DataGridViewLinkColumn link2= new DataGridViewLinkColumn();
            link1.Name = "column" + (columnNum++).ToString();
            link1.HeaderText = "リンク";
            link1.Width = 0;

            link1.Name = "column" + (columnNum++).ToString();
            link2.HeaderText = "名前";
            link2.LinkBehavior = LinkBehavior.HoverUnderline;
            link2.TrackVisitedState = true;
            link2.SortMode = DataGridViewColumnSortMode.Automatic;


            dataGridView1.Columns.Add(link1);
            dataGridView1.Columns.Add(link2);
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "カテゴリ");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "出品数");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "相場");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "変動値");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "★");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "★変動値");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "★★");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "★★変動値");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "★★★");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "★★★変動値");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "相場最大");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "相場最小");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "★最大");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "★最小");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "★★最大");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "★★最小");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "★★★最大");
            dataGridView1.Columns.Add("column" + (columnNum++).ToString(), "★★★最小");

            itemList = manager.GetItemData();
            itemHist = manager.GetItemHistoryData();

            label5.Text = "価格最終更新：" + itemHist.Where(x => x.NowPrice.Count > 0).Select(x => x.NowPrice.Keys.Min()).First().ToString("yyyy/MM/dd HH:mm:ss");

            manager.ProgressEvent += Manager_ProgressEvent;

            //itemHist = itemHist.Where(x => x.HistoryPrice.Count > 0 && x.HistoryPrice.Values.Min() > 0).ToList();
            itemHist = itemHist.Where(x => x.Classification != Utility.PARTS_SET).ToList();

        }

        private void Manager_ProgressEvent(ItemClassLibrary.Event.ItemEventArgs e) {
            this.Invoke((MethodInvoker)(() =>
            {
                this.label4.Text = e.TestStringValue + " ダウンロードしました";
            }));
        }

        private void button1_Click(object sender, EventArgs e) {
            Thread thread = new Thread(DownloadHistory);
            thread.Start();
        }

        private void DownloadHistory() {
            manager.DownloadItemDetail();
            lock(lockObject) {
                itemHist = manager.GetItemHistoryData();
                itemHist = itemHist.Where(x => x.Classification != Utility.PARTS_SET).ToList();
            }
            this.Invoke((MethodInvoker)(() => {
                this.label5.Text = "価格最終更新：" + itemHist.Where(x => x.NowPrice.Count > 0).Select(x => x.NowPrice.Keys.Min()).First().ToString("yyyy/MM/dd HH:mm:ss");
                this.label4.Text = "ダウンロード完了しました";
            }));
        }

        private void button2_Click(object sender, EventArgs e) {
            manager.DownloadItemData();
            itemList = manager.GetItemData();
        }

        private void button3_Click(object sender, EventArgs e) {
            Search("", 0);
        }

        private void Search(string freeword, int mode) {
            int days = int.Parse(numericUpDown1.Value.ToString()) + 1;
            decimal fluctuation = numericUpDown2.Value;
            decimal max = (1 + fluctuation / 100);
            decimal min = (1 - fluctuation / 100);

            string appendMsg = "[エラー発生]";
            dataGridView1.Rows.Clear();
            int resultCount = 0;
            try {
                List<ItemHistory> searchList = new List<ItemHistory>();
                lock (lockObject) {
                    searchList = new List<ItemHistory>(itemHist);
                }
                if (freeword.Length > 0 && mode == 0) {
                    searchList = searchList.Where(x => x.Name.Contains(freeword)).ToList();
                    if (checkBox1.Checked) {
                        searchList = searchList.Where(x => x.NowPrice.Values.First() > 0).ToList();
                    }
                }
                if (freeword.Length > 0 && mode == 1) {
                    searchList = searchList.Where(x => x.MaterialList.Where(y => y.materialName.Contains(freeword)).Count() > 0).ToList();
                    if (checkBox1.Checked) {
                        searchList = searchList.Where(x => x.NowPrice.Values.First() > 0).ToList();
                    }
                }
                if (mode == 2) {
                    if (comboBox1.SelectedItem.ToString() != "すべて") {
                        searchList = searchList.Where(x => x.Classification == comboBox1.SelectedItem.ToString()).ToList();
                    }
                    if (radioButton1.Checked) {
                        searchList = searchList.Where(x => x.MaterialCost > 0 && x.MaterialCost < x.NowPrice.Values.First()).ToList();
                    }
                    else if (radioButton2.Checked) {
                        searchList = searchList.Where(x => x.MaterialCost > 0 && x.MaterialCost < x.HistoryPriceStar1.Values.First()).ToList();
                    }
                    else if (radioButton3.Checked) {
                        searchList = searchList.Where(x => x.MaterialCost > 0 && x.MaterialCost < x.HistoryPriceStar2.Values.First()).ToList();
                    }
                    else if (radioButton4.Checked) {
                        searchList = searchList.Where(x => x.MaterialCost > 0 && x.MaterialCost < x.HistoryPriceStar3.Values.First()).ToList();
                    }
                    if (checkBox1.Checked) {
                        searchList = searchList.Where(x => x.NowPrice.Values.First() > 0).ToList();
                    }
                }
                else {
                    if (comboBox1.SelectedItem.ToString() != "すべて") {
                        searchList = searchList.Where(x => x.Classification == comboBox1.SelectedItem.ToString()).ToList();
                    }
                    if (radioButton1.Checked) {
                        searchList = searchList.Where(x => x.NowPrice.Count > 0 && x.HistoryPrice.Count > 0 && x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Count() > 0).ToList();
                        if (checkBox1.Checked) {
                            searchList = searchList.Where(x => x.NowPrice.Values.First() > 0).ToList();
                        }
                        searchList = searchList.Where(x =>
                        x.NowPrice.Values.First() * max < x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Select(y => y.Value).Max() ||
                        x.NowPrice.Values.First() * min > x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Select(y => y.Value).Min()
                        ).ToList();
                        searchList = searchList.OrderByDescending(x =>
                        Math.Abs(x.HistoryPrice.Where(y => y.Value != x.NowPrice.Values.First() && y.Value > 0).Count() == 0 ? 0 : x.NowPrice.Values.First() - (x.HistoryCount.Count == 0 ? 0 : x.HistoryPrice.Where(y => y.Value != x.NowPrice.Values.First() && y.Value > 0).Select(y => y.Value).First()))
                        ).ToList();
                    }
                    else if (radioButton2.Checked) {
                        searchList = searchList.Where(x => x.NowPriceStar1.Count > 0 && x.HistoryPriceStar1.Count > 0 && x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Count() > 0).ToList();
                        if (checkBox1.Checked) {
                            searchList = searchList.Where(x => x.NowPriceStar1.Values.First() > 0).ToList();
                        }
                        searchList = searchList.Where(x =>
                        x.NowPriceStar1.Values.First() * max < x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Select(y => y.Value).Max() ||
                        x.NowPriceStar1.Values.First() * min > x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Select(y => y.Value).Min()
                        ).ToList();
                        searchList = searchList.OrderByDescending(x =>
                        Math.Abs(x.NowPriceStar1.Count == 0 || x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar1.Values.First() && y.Value > 0).Count() == 0 ? 0 : x.NowPriceStar1.Values.First() - (x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar1.Values.First() && y.Value > 0).Count() == 0 ? 0 : x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar1.Values.First() && y.Value > 0).Select(y => y.Value).First()))
                        ).ToList();
                    }
                    else if (radioButton3.Checked) {
                        searchList = searchList.Where(x => x.NowPriceStar2.Count > 0 && x.HistoryPriceStar2.Count > 0 && x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Count() > 0).ToList();
                        if (checkBox1.Checked) {
                            searchList = searchList.Where(x => x.NowPriceStar2.Values.First() > 0).ToList();
                        }
                        searchList = searchList.Where(x =>
                        x.NowPriceStar2.Values.First() * max < x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Select(y => y.Value).Max() ||
                        x.NowPriceStar2.Values.First() * min > x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Select(y => y.Value).Min()
                        ).ToList();
                        searchList = searchList.OrderByDescending(x =>
                        Math.Abs(x.NowPriceStar2.Count == 0 || x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar2.Values.First() && y.Value > 0).Count() == 0 ? 0 : x.NowPriceStar2.Values.First() - (x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar2.Values.First() && y.Value > 0).Count() == 0 ? 0 : x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar2.Values.First() && y.Value > 0).Select(y => y.Value).First()))
                        ).ToList();
                    }
                    else if (radioButton4.Checked) {
                        searchList = searchList.Where(x => x.NowPriceStar3.Count > 0 && x.HistoryPriceStar3.Count > 0 && x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Count() > 0).ToList();
                        if (checkBox1.Checked) {
                            searchList = searchList.Where(x => x.NowPriceStar3.Select(y => y.Value).Min() > 0).ToList();
                        }
                        searchList = searchList.Where(x =>
                        x.NowPriceStar3.Values.First() * max < x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Select(y => y.Value).Max() ||
                        x.NowPriceStar3.Values.First() * min > x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Select(y => y.Value).Min()
                        ).ToList();
                        searchList = searchList.OrderByDescending(x =>
                        Math.Abs(x.NowPriceStar3.Count == 0 || x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar3.Values.First() && y.Value > 0).Count() == 0 ? 0 : x.NowPriceStar3.Values.First() - (x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar3.Values.First() && y.Value > 0).Count() == 0 ? 0 : x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar3.Values.First() && y.Value > 0).Select(y => y.Value).First()))
                        ).ToList();
                    }
                }
                resultCount = searchList.Count;
                List<string[]> dispList = searchList.Select(
                    x =>
                    x.Url + "\t" +
                    x.Name + "\t" +
                    x.Classification + "\t" +
                    str2decimal(x.NowCount.Values.First()) + "\t" +
                    str2decimal(x.NowPrice.Values.First()) + "\t" +
                    str2decimal(x.HistoryPrice.Where(y => y.Value != x.NowPrice.Values.First() && y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Count() == 0 ? 0 : x.NowPrice.Values.First() - (x.HistoryCount.Count == 0 ? 0 : x.HistoryPrice.Where(y => y.Value != x.NowPrice.Values.First() && y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Select(y => y.Value).First())) + "\t" +
                    str2decimal(x.NowPriceStar1.Count == 0 ? "0" : x.NowPriceStar1.Values.First().ToString()) + "\t" +
                    str2decimal(x.NowPriceStar1.Count == 0 || x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar1.Values.First() && y.Value > 0).Count() == 0 ? 0 : x.NowPriceStar1.Values.First() - (x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar1.Values.First() && y.Value > 0).Count() == 0 ? 0 : x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar1.Values.First() && y.Value > 0).Select(y => y.Value).First())) + "\t" +
                    str2decimal(x.NowPriceStar2.Count == 0 ? "0" : x.NowPriceStar2.Values.First().ToString()) + "\t" +
                    str2decimal(x.NowPriceStar2.Count == 0 || x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar2.Values.First() && y.Value > 0).Count() == 0 ? 0 : x.NowPriceStar2.Values.First() - (x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar2.Values.First() && y.Value > 0).Count() == 0 ? 0 : x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar2.Values.First() && y.Value > 0).Select(y => y.Value).First())) + "\t" +
                    str2decimal(x.NowPriceStar3.Count == 0 ? "0" : x.NowPriceStar3.Values.First().ToString()) + "\t" +
                    str2decimal(x.NowPriceStar3.Count == 0 || x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar3.Values.First() && y.Value > 0).Count() == 0 ? 0 : x.NowPriceStar3.Values.First() - (x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar3.Values.First() && y.Value > 0).Count() == 0 ? 0 : x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value != x.NowPriceStar3.Values.First() && y.Value > 0).Select(y => y.Value).First())) + "\t" +
                    str2decimal(x.HistoryPrice.Count == 0 ? 0 : x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max()) + "\t" +
                    str2decimal(x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Count() == 0 ? 0 : x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Select(y => y.Value).Min()) + "\t" +
                    str2decimal(x.HistoryPriceStar1.Count == 0 ? 0 : x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max()) + "\t" +
                    str2decimal(x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Count() == 0 ? 0 : x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Select(y => y.Value).Min()) + "\t" +
                    str2decimal(x.HistoryPriceStar2.Count == 0 ? 0 : x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max()) + "\t" +
                    str2decimal(x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Count() == 0 ? 0 : x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Select(y => y.Value).Min()) + "\t" +
                    str2decimal(x.HistoryPriceStar3.Count == 0 ? 0 : x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max()) + "\t" +
                    str2decimal(x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Count() == 0 ? 0 : x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1) && y.Value > 0).Select(y => y.Value).Min())
                    ).Distinct().Select(x => x.Split(new char[] { '\t' })).ToList();
                resultCount = dispList.Count;
                appendMsg = "";

                for (int i = 0; i < dispList.Count; i++) {
                    dataGridView1.Rows.Add(dispList[i]);
                }
                dataGridView1.AutoResizeColumns();
                dataGridView1.Columns[0].Width = 0;
            }
            catch (Exception ex) {

            }
            label3.Text = appendMsg + resultCount + "件検索されました";
        }

        private string str2decimal(object value) {
            try {
                decimal a = decimal.MinValue;
                if (decimal.TryParse(value.ToString(), out a)) {
                    return string.Format("{0:N0}", a);
                }
            }
            catch(Exception e) {
                int a = 0;
            }
            return value.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            DataGridView dgv = (DataGridView)sender;
            if (e.ColumnIndex == 1) {
                DataGridViewLinkCell cell = (DataGridViewLinkCell)dgv[e.ColumnIndex-1, e.RowIndex];
                string url = cell.Value.ToString();
                if(url.Length > 0) {
                    System.Diagnostics.Process.Start(url);
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e) {
            this.dataGridView1.Height = this.Height - 100;
        }

        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e) {
            decimal a = decimal.MinValue;
            decimal b = decimal.MinValue;
            bool ba = decimal.TryParse(e.CellValue1.ToString(), out a);
            bool bb = decimal.TryParse(e.CellValue2.ToString(), out b);

            if(ba == false || bb == false) {
                e.SortResult = string.Compare(e.CellValue2.ToString(), e.CellValue1.ToString());
            }
            else {
                if (a - b < 0) {
                    e.SortResult = -1;
                }
                else {
                    e.SortResult = 1;
                }
            }
            e.Handled = true;
        }

        private bool IsNumeric(object value) {
            bool ret = true;
            try {
                decimal d = decimal.Parse(value.ToString());
            }
            catch(Exception e) {
                ret = false;
            }
            return ret;
        }

        private void dataGridView1_Sorted(object sender, EventArgs e) {
            /*
            dataGridView1.Columns[3].ValueType = typeof(decimal);
            dataGridView1.Columns[4].ValueType = typeof(decimal);
            dataGridView1.Columns[3].DefaultCellStyle.Format = "N0";
            dataGridView1.Columns[4].DefaultCellStyle.Format = "N0";
            */

        }

        private void button4_Click(object sender, EventArgs e) {
            if(textBox1.Text.Length > 0) {
                Search(textBox1.Text, 0);
            }
        }

        private void button5_Click(object sender, EventArgs e) {
            Search(textBox1.Text, 1);
        }

        private void button6_Click(object sender, EventArgs e) {
            Search("", 2);
        }
    }
}
