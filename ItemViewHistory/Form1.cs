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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemViewHistory {
    public partial class Form1 : Form {

        ItemManager manager = ItemManager.GetInstance();
        List<ItemBase> itemList = null;
        List<ItemHistory> itemHist = null;
        public Form1() {
            InitializeComponent();

            comboBox1.Items.Add("すべて");
            comboBox1.Items.AddRange(Utility.ITEM_MARKET_LIST);
            comboBox1.SelectedIndex = 0;
            label3.Text = "";

            DataGridViewLinkColumn link = new DataGridViewLinkColumn();
            link.HeaderText = "詳細";
            link.LinkBehavior = LinkBehavior.HoverUnderline;
            link.TrackVisitedState = true;

            dataGridView1.Columns.Add(link);
            dataGridView1.Columns.Add("column2", "名前");
            dataGridView1.Columns.Add("column3", "カテゴリ");
            dataGridView1.Columns.Add("column3", "出品数");
            dataGridView1.Columns.Add("column4", "相場");
            dataGridView1.Columns.Add("column5", "★");
            dataGridView1.Columns.Add("column6", "★★");
            dataGridView1.Columns.Add("column7", "★★★");
            dataGridView1.Columns.Add("column8", "相場最大");
            dataGridView1.Columns.Add("column9", "相場最小");
            dataGridView1.Columns.Add("column10", "★最大");
            dataGridView1.Columns.Add("column11", "★最小");
            dataGridView1.Columns.Add("column12", "★★最大");
            dataGridView1.Columns.Add("column13", "★★最小");
            dataGridView1.Columns.Add("column14", "★★★最大");
            dataGridView1.Columns.Add("column15", "★★★最小");



            itemList = manager.GetItemData();
            itemHist = manager.GetItemHistoryData();

            itemHist = itemHist.Where(x => x.HistoryPrice.Count > 0 && x.HistoryPrice.Values.Min() > 0).ToList();

        }

        private void button1_Click(object sender, EventArgs e) {
            manager.DownloadItemDetail();
        }

        private void button2_Click(object sender, EventArgs e) {
            manager.DownloadItemData();
        }

        private void button3_Click(object sender, EventArgs e) {
            int days = int.Parse(numericUpDown1.Value.ToString()) + 1;
            decimal fluctuation = numericUpDown2.Value;
            decimal max = (1 + fluctuation / 100);
            decimal min = (1 - fluctuation / 100);

            dataGridView1.Rows.Clear();
            int resultCount = 0;
            try {
                List<ItemHistory> searchList = new List<ItemHistory>(itemHist);
                if (comboBox1.SelectedItem.ToString() != "すべて") {
                    searchList = searchList.Where(x => x.Classification == comboBox1.SelectedItem.ToString()).ToList();
                }
                if (radioButton1.Checked) {
                    searchList = searchList.Where(x => x.NowPrice.Count > 0 && x.HistoryPrice.Count > 0 && x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min() > 0).ToList();
                    if (checkBox1.Checked) {
                        searchList = searchList.Where(x => x.NowPrice.Values.First() > 0).ToList();
                    }
                    searchList = searchList.Where(x => 
                    x.NowPrice.Values.First() * max < x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max() || 
                    x.NowPrice.Values.First() * min > x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()
                    ).ToList();
                    searchList = searchList.OrderByDescending(x =>
                    Math.Abs(x.NowPrice.Values.First() - x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()) >
                    Math.Abs(x.NowPrice.Values.First() - x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max()) ?
                    Math.Abs(x.NowPrice.Values.First() - x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()) :
                    Math.Abs(x.NowPrice.Values.First() - x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max())
                    ).ToList();
                }
                else if (radioButton2.Checked) {
                    searchList = searchList.Where(x => x.NowPriceStar1.Count > 0 && x.HistoryPriceStar1.Count > 0 && x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min() > 0).ToList();
                    if (checkBox1.Checked) {
                        searchList = searchList.Where(x => x.NowPriceStar1.Values.First() > 0).ToList();
                    }
                    searchList = searchList.Where(x => 
                    x.NowPriceStar1.Values.First() * max < x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max() || 
                    x.NowPriceStar1.Values.First() * min > x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()
                    ).ToList();
                    searchList = searchList.OrderByDescending(x =>
                    Math.Abs(x.NowPriceStar1.Values.First() - x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()) >
                    Math.Abs(x.NowPriceStar1.Values.First() - x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max()) ?
                    Math.Abs(x.NowPriceStar1.Values.First() - x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()) :
                    Math.Abs(x.NowPriceStar1.Values.First() - x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max())
                    ).ToList();
                }
                else if (radioButton3.Checked) {
                    searchList = searchList.Where(x => x.NowPriceStar2.Count > 0 && x.HistoryPriceStar2.Count > 0 && x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min() > 0).ToList();
                    if (checkBox1.Checked) {
                        searchList = searchList.Where(x => x.NowPriceStar2.Values.First() > 0).ToList();
                    }
                    searchList = searchList.Where(x => 
                    x.NowPriceStar2.Values.First() * max < x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max() || 
                    x.NowPriceStar2.Values.First() * min > x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()
                    ).ToList();
                    searchList = searchList.OrderByDescending(x =>
                    Math.Abs(x.NowPriceStar2.Values.First() - x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()) >
                    Math.Abs(x.NowPriceStar2.Values.First() - x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max()) ?
                    Math.Abs(x.NowPriceStar2.Values.First() - x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()) :
                    Math.Abs(x.NowPriceStar2.Values.First() - x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max())
                    ).ToList();
                }
                else if (radioButton4.Checked) {
                    searchList = searchList.Where(x => x.NowPriceStar3.Count > 0 && x.HistoryPriceStar3.Count > 0 && x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min() > 0).ToList();
                    if (checkBox1.Checked) {
                        searchList = searchList.Where(x => x.NowPriceStar3.Select(y => y.Value).Min() > 0).ToList();
                    }
                    searchList = searchList.Where(x => 
                    x.NowPriceStar3.Values.First() * max < x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max() || 
                    x.NowPriceStar3.Values.First() * min > x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()
                    ).ToList();
                    searchList = searchList.OrderByDescending(x =>
                    Math.Abs(x.NowPriceStar3.Values.First() - x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()) >
                    Math.Abs(x.NowPriceStar3.Values.First() - x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max()) ?
                    Math.Abs(x.NowPriceStar3.Values.First() - x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()) :
                    Math.Abs(x.NowPriceStar3.Values.First() - x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max())
                    ).ToList();
                }
                resultCount = searchList.Count;
                List<string[]> dispList = searchList.Select(
                    x => 
                    x.Url + "," +
                    x.Name + "," +
                    x.Classification + "," +
                    x.HistoryCount.Values.First() + "," + 
                    x.NowPrice.Values.First() + "(" + (x.NowPrice.Values.First() - x.HistoryPrice.Where(y => y.Value > 0).Select(y => y.Value).First()) + ")," +
                    (x.NowPriceStar1.Count == 0 ? "0" : x.NowPriceStar1.Values.First().ToString() + "(" + (x.NowPriceStar1.Values.First() - x.HistoryPriceStar1.Where(y => y.Value > 0).Select(y => y.Value).First()) + ")") + ", " +
                    (x.NowPriceStar2.Count == 0 ? "0" : x.NowPriceStar2.Values.First().ToString() + "(" + (x.NowPriceStar2.Values.First() - x.HistoryPriceStar2.Where(y => y.Value > 0).Select(y => y.Value).First()) + ")") + "," +
                    (x.NowPriceStar3.Count == 0 ? "0" : x.NowPriceStar3.Values.First().ToString() + "(" + (x.NowPriceStar3.Values.First() - x.HistoryPriceStar3.Where(y => y.Value > 0).Select(y => y.Value).First()) + ")") + "," +
                    (x.HistoryPrice.Count == 0 ? 0 : x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max()) + "," + 
                    (x.HistoryPrice.Count == 0 ? 0 : x.HistoryPrice.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()) + "," +
                    (x.HistoryPriceStar1.Count == 0 ? 0 : x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max()) + "," +
                    (x.HistoryPriceStar1.Count == 0 ? 0 : x.HistoryPriceStar1.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()) + "," +
                    (x.HistoryPriceStar2.Count == 0 ? 0 : x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max()) + "," +
                    (x.HistoryPriceStar2.Count == 0 ? 0 : x.HistoryPriceStar2.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min()) + "," +
                    (x.HistoryPriceStar3.Count == 0 ? 0 : x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Max()) + "," +
                    (x.HistoryPriceStar3.Count == 0 ? 0 : x.HistoryPriceStar3.Where(y => y.Key > DateTime.Now.AddDays(days * -1)).Select(y => y.Value).Min())
                    ).Select(x => x.Split(new char[] { ',' })).ToList();

                for (int i = 0; i < dispList.Count; i++) {
                    dataGridView1.Rows.Add(dispList[i]);
                }
            }
            catch(Exception ex) {

            }
            label3.Text = resultCount + "件検索されました";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            DataGridView dgv = (DataGridView)sender;
            if (e.ColumnIndex == 0) {
                DataGridViewLinkCell cell = (DataGridViewLinkCell)dgv[e.ColumnIndex, e.RowIndex];
                string url = cell.Value.ToString();
                if(url.Length > 0) {
                    System.Diagnostics.Process.Start(url);
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e) {
            this.dataGridView1.Height = this.Height - 100;
        }
    }
}
