using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using ItemClassLibrary.Entity;
using ItemClassLibrary.Entity.Equipment;
using ItemClassLibrary.Event;
using ItemClassLibrary.Util;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;

namespace ItemClassLibrary.Manage {
    public class ItemManager {
        public event ItemEventHandler ProgressEvent;
        public delegate void ItemEventHandler(ItemEventArgs e);

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ItemManager m_itemManager = null;

        string allItemFile = System.Configuration.ConfigurationManager.AppSettings["allItemFile"];
        string allAbilityFile = System.Configuration.ConfigurationManager.AppSettings["allAbilityFile"];
        string replaceFile = System.Configuration.ConfigurationManager.AppSettings["replaceFile"];

        List<AbilityPattern> abilityList = new List<AbilityPattern>();

        string[] setEquipUrls = System.Configuration.ConfigurationManager.AppSettings["setEquipUrls"].Replace(" ", Environment.NewLine).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        string[] equipUrls = System.Configuration.ConfigurationManager.AppSettings["equipUrls"].Replace(" ", Environment.NewLine).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        string[] equipAccessoryUrls = System.Configuration.ConfigurationManager.AppSettings["equipAccessoryUrls"].Replace(" ", Environment.NewLine).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        string[] equipCraftUrls = System.Configuration.ConfigurationManager.AppSettings["equipCraftUrls"].Replace(" ", Environment.NewLine).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        string[] itemUrls = System.Configuration.ConfigurationManager.AppSettings["itemUrls"].Replace(" ", Environment.NewLine).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        string[] itemDetailUrls = System.Configuration.ConfigurationManager.AppSettings["itemDetailUrls"].Replace(" ", Environment.NewLine).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        string[] itemSimpleUrls = System.Configuration.ConfigurationManager.AppSettings["itemSimpleUrls"].Replace(" ", Environment.NewLine).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        string[] setHeader = Utility.setHeader;
        string[] equipHeader = Utility.equipHeader;
        string[] accessoryHeader = Utility.accessoryHeader;
        string[] craftHeader = Utility.craftHeader;
        string[] itemHeader = Utility.itemHeader;
        string[] itemHeaderDetail = Utility.itemHeaderDetail;
        string[] itemHeaderSimple = Utility.itemHeaderSimple;

        List<ItemBase> m_itemList = new List<ItemBase>();
        List<ItemHistory> m_historyList = new List<ItemHistory>();
        List<(string source, string dist)> replaceList = new List<(string source, string dist)>();

        private void UpdateProgress(int TestNumValue, string TestStringValue) {
            ProgressEvent(new ItemEventArgs(TestNumValue, TestStringValue));
        }


        public List<ItemBase> GetItemData() {
            return m_itemList;
        }

        public List<ItemHistory> GetItemHistoryData() {
            return m_historyList;
        }

        private void loadReplaceData() {
            if (File.Exists(replaceFile)) {
                replaceList = JsonConvert.DeserializeObject<List<(string source, string dist)>>(File.ReadAllText(replaceFile));
            }
        }

        private void saveReplaceData() {
            File.WriteAllText(replaceFile, JsonConvert.SerializeObject(replaceList));
        }

        public void addReplaceString(string source, string dist) {
            replaceList.Add((source, dist));
            saveReplaceData();
        }

        public string replaceText(string source) {
            string retString = source;
            for (int i = 0; i < replaceList.Count; i++) {
                retString = retString.Replace(replaceList[i].source, replaceList[i].dist);
            }
            return retString;
        }

        private ItemManager() {
            if(Directory.Exists(allItemFile.Split(new char[] { '/' })[0])) {
                Directory.CreateDirectory(allItemFile.Split(new char[] { '/' })[0]);
            }

            // ローカル(ネットワーク)から定義ファイルを読み込む
            // 定義ファイルを解析しアイテムリストを保持する
            loadReplaceData();

            if (File.Exists(allAbilityFile)) {
                abilityList = Utility.ReadAbilityPattern(allAbilityFile);
            }

            if (File.Exists(allItemFile.Replace(".", "_history."))) {
                string jsonString = File.ReadAllText(allItemFile.Replace(".", "_history."));
                m_historyList = JsonConvert.DeserializeObject<List<ItemHistory>>(jsonString);
            }


            if (File.Exists(allItemFile)) {
                string jsonString = File.ReadAllText(allItemFile);

                List<JObject> obj = JsonConvert.DeserializeObject<List<JObject>>(jsonString);
                for(int i = 0; i < obj.Count; i++) {
                    string classification = obj[i]["Classification"].ToString();
                    if (classification == Utility.PARTS_SET) {
                        EquipmentGroup data = JsonConvert.DeserializeObject<EquipmentGroup>(obj[i].ToString());
                        m_itemList.Add(data);
                    }
                    else if (classification == Utility.PARTS_HEAD) {
                        Head data = JsonConvert.DeserializeObject<Head>(obj[i].ToString());
                        if(abilityList.Where(x => x.Classification == classification).Count() > 0) {
                            AbilityPattern ability = abilityList.Where(x => x.Classification == classification).First();
                            data.AbilityPattern = ability.PatternList;
                        }
                        m_itemList.Add(data);
                    }
                    else if (classification == Utility.PARTS_UPPERBODY) {
                        UpperBody data = JsonConvert.DeserializeObject<UpperBody>(obj[i].ToString());
                        if (abilityList.Where(x => x.Classification == classification).Count() > 0) {
                            AbilityPattern ability = abilityList.Where(x => x.Classification == classification).First();
                            data.AbilityPattern = ability.PatternList;
                        }
                        m_itemList.Add(data);
                    }
                    else if (classification == Utility.PARTS_LOWERBODY) {
                        LowerBody data = JsonConvert.DeserializeObject<LowerBody>(obj[i].ToString());
                        if (abilityList.Where(x => x.Classification == classification).Count() > 0) {
                            AbilityPattern ability = abilityList.Where(x => x.Classification == classification).First();
                            data.AbilityPattern = ability.PatternList;
                        }
                        m_itemList.Add(data);
                    }
                    else if (classification == Utility.PARTS_ARM) {
                        Arm data = JsonConvert.DeserializeObject<Arm>(obj[i].ToString());
                        if (abilityList.Where(x => x.Classification == classification).Count() > 0) {
                            AbilityPattern ability = abilityList.Where(x => x.Classification == classification).First();
                            data.AbilityPattern = ability.PatternList;
                        }
                        m_itemList.Add(data);
                    }
                    else if (classification == Utility.PARTS_LEG) {
                        Leg data = JsonConvert.DeserializeObject<Leg>(obj[i].ToString());
                        if (abilityList.Where(x => x.Classification == classification).Count() > 0) {
                            AbilityPattern ability = abilityList.Where(x => x.Classification == classification).First();
                            data.AbilityPattern = ability.PatternList;
                        }
                        m_itemList.Add(data);
                    }
                    else if (classification == Utility.PARTS_SHIELD) {
                        Shield data = JsonConvert.DeserializeObject<Shield>(obj[i].ToString());
                        if (abilityList.Where(x => x.Classification == classification).Count() > 0) {
                            AbilityPattern ability = abilityList.Where(x => x.Classification == classification).First();
                            data.AbilityPattern = ability.PatternList;
                        }
                        m_itemList.Add(data);
                    }
                    else if (
                        classification == Utility.PARTS_ACCESSORY_FACE ||
                        classification == Utility.PARTS_ACCESSORY_NECK ||
                        classification == Utility.PARTS_ACCESSORY_FINGER ||
                        classification == Utility.PARTS_ACCESSORY_CHEST ||
                        classification == Utility.PARTS_ACCESSORY_WAIST ||
                        classification == Utility.PARTS_ACCESSORY_NOTE ||
                        classification == Utility.PARTS_ACCESSORY_OTHER ||
                        classification == Utility.PARTS_ACCESSORY_PROOF ||
                        classification == Utility.PARTS_ACCESSORY_CREST
                        ) {
                        Accessory data = JsonConvert.DeserializeObject<Accessory>(obj[i].ToString());
                        if (abilityList.Where(x => x.Classification == classification && data.Name == x.Name).Count() > 0) {
                            AbilityPattern ability = abilityList.Where(x => x.Classification == classification && data.Name == x.Name).First();
                            data.AbilityPattern = ability.PatternList;
                        }
                        m_itemList.Add(data);
                    }
                    else if (
                        classification == Utility.PARTS_CRAFT_HAMMER ||
                        classification == Utility.PARTS_CRAFT_KNIFE ||
                        classification == Utility.PARTS_CRAFT_NEEDLE ||
                        classification == Utility.PARTS_CRAFT_POT ||
                        classification == Utility.PARTS_CRAFT_LAMP ||
                        classification == Utility.PARTS_CRAFT_FLYPAN
                        ) {
                        Craft data = JsonConvert.DeserializeObject<Craft>(obj[i].ToString());
                        m_itemList.Add(data);
                    }
                    else if (
                        classification == Utility.PARTS_CRAFT_FISHING
                        ) {
                        Fishing data = JsonConvert.DeserializeObject<Fishing>(obj[i].ToString());
                        m_itemList.Add(data);
                    }
                    else if (
                        classification == Utility.ITEM_FOOD
                        ) {
                        Food data = JsonConvert.DeserializeObject<Food>(obj[i].ToString());
                        m_itemList.Add(data);
                    }
                    else if (
                        classification == Utility.ITEM_MATERIAL ||
                        classification == Utility.ITEM_SUPPLY ||
                        classification == Utility.ITEM_COIN ||
                        classification == Utility.ITEM_FLOWER ||
                        classification == Utility.ITEM_SEED ||
                        classification == Utility.ITEM_RECIPE ||
                        classification == Utility.ITEM_SCOUT ||
                        classification == Utility.ITEM_GESTURE ||
                        classification == Utility.ITEM_HOUSE ||
                        classification == Utility.ITEM_FURNITURE ||
                        classification == Utility.ITEM_GARDEN
                        ) {
                        Item data = JsonConvert.DeserializeObject<Item>(obj[i].ToString());
                        m_itemList.Add(data);
                    }
                    else if (
                        classification == Utility.PARTS_WEAPON_ONEHANDSWORD ||
                        classification == Utility.PARTS_WEAPON_TWOHANDSWORD ||
                        classification == Utility.PARTS_WEAPON_KNIFE ||
                        classification == Utility.PARTS_WEAPON_SPEAR ||
                        classification == Utility.PARTS_WEAPON_AXE ||
                        classification == Utility.PARTS_WEAPON_NAIL ||
                        classification == Utility.PARTS_WEAPON_WHIP ||
                        classification == Utility.PARTS_WEAPON_STICK ||
                        classification == Utility.PARTS_WEAPON_WAND ||
                        classification == Utility.PARTS_WEAPON_CLUB ||
                        classification == Utility.PARTS_WEAPON_FAN ||
                        classification == Utility.PARTS_WEAPON_HAMMER ||
                        classification == Utility.PARTS_WEAPON_BOW ||
                        classification == Utility.PARTS_WEAPON_BOOMERANG ||
                        classification == Utility.PARTS_WEAPON_SICKLE
                        ) {
                        Weapon data = JsonConvert.DeserializeObject<Weapon>(obj[i].ToString());
                        if (abilityList.Where(x => x.Classification == classification).Count() > 0) {
                            AbilityPattern ability = abilityList.Where(x => x.Classification == classification).First();
                            data.AbilityPattern = ability.PatternList;
                        }
                        m_itemList.Add(data);
                    }
                    else {
                        int a = 0;
                    }
                }
            }


        }

        public static ItemManager GetInstance() {
            if (m_itemManager == null) {
                m_itemManager = new ItemManager();
            }
            return m_itemManager;
        }

        public void DownloadItemData() {
            m_itemList = new List<ItemBase>();
            for (int i = 0; i < equipUrls.Length; i++) {
                m_itemList.AddRange(DownloadItemData(equipHeader, equipUrls[i]));
            }
            for (int i = 0; i < equipAccessoryUrls.Length; i++) {
                m_itemList.AddRange(DownloadItemData(accessoryHeader, equipAccessoryUrls[i]));
            }
            for (int i = 0; i < setEquipUrls.Length; i++) {
                m_itemList.AddRange(DownloadItemData(setHeader, setEquipUrls[i]));
            }
            for (int i = 0; i < equipCraftUrls.Length; i++) {
                m_itemList.AddRange(DownloadItemData(craftHeader, equipCraftUrls[i]));
            }
            for (int i = 0; i < itemSimpleUrls.Length; i++) {
                m_itemList.AddRange(DownloadItemData(itemHeaderSimple, itemSimpleUrls[i]));
            }
            for (int i = 0; i < itemUrls.Length; i++) {
                m_itemList.AddRange(DownloadItemData(itemHeader, itemUrls[i]));
            }
            for (int i = 0; i < itemDetailUrls.Length; i++) {
                m_itemList.AddRange(DownloadItemData(itemHeaderDetail, itemDetailUrls[i]));
            }

            string[] fileParts = allItemFile.Split(new char[] { '/' });
            string itemFileDir = allItemFile.Replace(fileParts[fileParts.Length - 1], "");

            if (!Directory.Exists(itemFileDir)) {
                Directory.CreateDirectory(itemFileDir);
            }

            File.WriteAllText(allItemFile, JsonConvert.SerializeObject(m_itemList));
        }

        public void DownloadItemDetail() {
            DateTime nowTime = DateTime.Now;
            List<ItemHistory> historyList = new List<ItemHistory>();
            for(int i = 0; i < m_itemList.Count; i++) {
                UpdateProgress(1, i.ToString() + "/" + m_itemList.Count.ToString());
                string url = m_itemList[i].Url;
                string Classification = m_itemList[i].Classification;
                string name = m_itemList[i].Name;
                if (url.Length > 0) {
                    // 指定したサイトのHTMLをストリームで取得する
                    IHtmlDocument doc = null;
                    while(doc == null) {
                        try {
                            WebRequest req = WebRequest.Create(url);
                            doc = default(IHtmlDocument);
                            using (WebResponse res = req.GetResponse()) {
                                using (Stream stream = res.GetResponseStream()) {
                                    var parser = new HtmlParser();
                                    doc = parser.ParseDocument(stream);
                                }
                            }
                        }
                        catch (Exception e) {
                            logger.Debug("ダウンロードエラー url=" + url + " Error=" + e.Message);
                            Thread.Sleep(500);
                        }
                    }
                    ItemHistory history = new ItemHistory();
                    history.Name = name;
                    history.Classification = Classification;
                    history.Url = url;
                    AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> urlElements = doc.QuerySelectorAll("table");
                    for(int j = 0; j < urlElements.Length; j++) {
                        AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> lines = urlElements[j].QuerySelectorAll("tr");
                        if (history.NowPrice.Count == 0 && urlElements[j].InnerHtml.Contains(name) && urlElements[j].InnerHtml.Contains(Utility.HEADER_DEFINE_NAME)) {
                            AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> columns = lines[1].QuerySelectorAll("td");
                            if (urlElements[j].InnerHtml.Contains("★")) {
                                string count = parseText(columns[3].InnerHtml).Replace("以上", "").Replace("不可","0");
                                string lowPrice = parseText(columns[4].InnerHtml).Replace("G", "");
                                string star1Price = parseText(columns[5].InnerHtml).Replace("G", "");
                                string star2Price = parseText(columns[6].InnerHtml).Replace("G", "");
                                string star3Price = parseText(columns[7].InnerHtml).Replace("G", "");
                                history.NowCount.Add(nowTime, Str2Decimal(count));
                                history.NowPrice.Add(nowTime, Str2Decimal(lowPrice));
                                history.NowPriceStar1.Add(nowTime, Str2Decimal(star1Price));
                                history.NowPriceStar2.Add(nowTime, Str2Decimal(star2Price));
                                history.NowPriceStar3.Add(nowTime, Str2Decimal(star3Price));

                            }
                            else {
                                string count = parseText(columns[2].InnerHtml).Replace("以上", "").Replace("不可", "0");
                                string lowPrice = parseText(columns[3].InnerHtml).Replace("G", "");
                                string buyPrice = parseText(columns[4].InnerHtml).Replace("G", "");
                                string sellPrice = parseText(columns[5].InnerHtml).Replace("G", "");
                                history.NowCount.Add(nowTime, Str2Decimal(count));
                                history.NowPrice.Add(nowTime, Str2Decimal(lowPrice));
                                history.BuyPrice = Str2Decimal(buyPrice);
                                history.SellPrice = Str2Decimal(sellPrice);
                            }
                        }
                        if (urlElements[j].InnerHtml.Contains("日付")) {
                            int lineCount = lines.Count();
                            for (int k = 1; k < lineCount; k++) {
                                AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> columns = lines[k].QuerySelectorAll("td");
                                DateTime date = DateTime.Parse(columns[0].InnerHtml.Split(new string[] { "（" },StringSplitOptions.RemoveEmptyEntries)[0]);
                                string count = parseText(columns[1].InnerHtml);
                                string price = parseText(columns[2].InnerHtml).Replace("G", "");
                                string star1Price = parseText(columns[3].InnerHtml).Replace("G", "");
                                string star2Price = parseText(columns[4].InnerHtml).Replace("G", "");
                                string star3Price = parseText(columns[5].InnerHtml).Replace("G", "");
                                history.HistoryCount.Add(date, Str2Decimal(count));
                                history.HistoryPrice.Add(date, Str2Decimal(price));
                                history.HistoryPriceStar1.Add(date, Str2Decimal(star1Price));
                                history.HistoryPriceStar2.Add(date, Str2Decimal(star2Price));
                                history.HistoryPriceStar3.Add(date, Str2Decimal(star3Price));
                            }
                        }
                        if (urlElements[j].InnerHtml.Contains("必要数×単価")) {
                            int lineCount = lines.Count();
                            for (int k = 2; k < lineCount -1; k++) {
                                AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> columns = lines[k].QuerySelectorAll("td");

                                string itemName = parseText(columns[0].InnerHtml);
                                string count = parseText(columns[1].InnerHtml);
                                if(itemName == "合計原価") {
                                    string cost = parseText(columns[3].InnerHtml).Replace("G", "");
                                    history.MaterialCost = decimal.Parse(cost);
                                    break;
                                }
                                history.MaterialList.Add((itemName, int.Parse(count)));
                            }
                        }
                    }
                    historyList.Add(history);
                }
            }

            m_historyList = historyList;
            string[] fileParts = allItemFile.Split(new char[] { '/' });
            string itemFileDir = allItemFile.Replace(fileParts[fileParts.Length - 1], "");

            if (!Directory.Exists(itemFileDir)) {
                Directory.CreateDirectory(itemFileDir);
            }

            File.WriteAllText(allItemFile.Replace(".", "_history."), JsonConvert.SerializeObject(m_historyList));
        }

        private string parseText(string text) {
            text = text.Replace("\r\n", " ");
            text = text.Replace("\n", " ");
            text = text.Replace("<br>", ",");
            text = text.Replace("</div><div ", "</div>,<div ");
            text = Regex.Replace(text, @"<(([^>]|\n)*)>", "");
            return text;
        }

        private decimal Str2Decimal(string value) {
            decimal returnValue = 0;
            decimal.TryParse(value, out returnValue);
            return returnValue;
        }

        private List<ItemBase> DownloadItemData(string[] header, string url) {
            List<ItemBase> itemList = new List<ItemBase>();
            List<Dictionary<string, string>> equipSetList = new List<Dictionary<string, string>>();

            string[] urlParts = url.Split(new string[] { "//" },StringSplitOptions.RemoveEmptyEntries)[1].Split(new char[] { '/' });
            string baseUrl = url.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[0] + "//" + urlParts[0];

            // 指定したサイトのHTMLをストリームで取得する
            WebRequest req = WebRequest.Create(url);
            var doc = default(IHtmlDocument);
            using (WebResponse res = req.GetResponse()) {
                using (Stream stream = res.GetResponseStream()) {
                    var parser = new HtmlParser();
                    doc = parser.ParseDocument(stream);
                }
            }

            int headerIndex = 0;

            AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> urlElements = doc.QuerySelectorAll("table");
            string correctionClass = "";
            for (int i = 0; i < urlElements.Count(); i++) {
                if(correctionClass.Length == 0) {
                    if (urlElements[i].InnerHtml.Contains(Utility.ITEM_FURNITURE + "のバザー相場一覧")) {
                        correctionClass = Utility.ITEM_FURNITURE;
                    }
                    else if (urlElements[i].InnerHtml.Contains(Utility.ITEM_GARDEN + "のバザー相場一覧")) {
                        correctionClass = Utility.ITEM_GARDEN;
                    }
                }
                if (urlElements[i].InnerHtml.Contains(header[0])) {
                    AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> lines = urlElements[i].QuerySelectorAll("tr");
                    Dictionary<string, string> equipSet = new Dictionary<string, string>();
                    for (int j = 0; j < lines.Count(); j++) {
                        if (lines[j].InnerHtml.Contains(header[0])) {
                            continue;
                        }
                        AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> columns = lines[j].QuerySelectorAll("td");
                        for (int k = 0; k < columns.Count(); k++) {
                            string text = "";

                            text = columns[k].InnerHtml;
                            text = text.Replace("\r\n", " ");
                            text = text.Replace("\n", " ");
                            text = text.Replace("<br>", ",");
                            text = text.Replace("</div><div ", "</div>,<div ");
                            text = Regex.Replace(text, @"<(([^>]|\n)*)>", "");

                            if(header[headerIndex] == Utility.HEADER_DEFINE_NAME || header[headerIndex] == Utility.HEADER_DEFINE_SETNAME) {
                                var element = columns[k].FirstElementChild;
                                string data = text;
                                if(data.Contains(",")) {
                                    data = data.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[0];
                                }
                                
                                while (element != null) {
                                    if(element.Text() == data) {
                                        if(element.LastElementChild != null) {
                                            equipSet[Utility.HEADER_DEFINE_URL] = baseUrl + ((AngleSharp.Html.Dom.IHtmlAnchorElement)element.LastElementChild).PathName;
                                        }
                                        else {
                                            equipSet[Utility.HEADER_DEFINE_URL] = baseUrl + ((AngleSharp.Html.Dom.IHtmlAnchorElement)element).PathName;
                                        }
                                    }
                                    element = element.NextElementSibling;
                                }

                                int a = 1;
                            }

                            if (header[headerIndex] != Utility.HEADER_DEFINE_BLANK && header[headerIndex] != Utility.HEADER_DEFINE_EQUIPABLE_JOBS && header[headerIndex] != Utility.HEADER_DEFINE_SET_ABILITY && text.Length == 0) {
                                continue;
                            }
                            if (text.Contains("adsbygoogle")) {
                                continue;
                            }
                            //}
                            equipSet[header[headerIndex++]] = text;
                            if (headerIndex == header.Length) {
                                if(!equipSet.ContainsKey(Utility.HEADER_DEFINE_CLASSIFICATION) && correctionClass.Length > 0) {
                                    equipSet[Utility.HEADER_DEFINE_CLASSIFICATION] = correctionClass;
                                }
                                itemList.Add(CreateItemData(equipSet));
                                //equipSetList.Add(equipSet);
                                equipSet = new Dictionary<string, string>();
                                headerIndex = 0;
                            }
                        }
                    }
                }
            }



            return itemList;
        }

        private ItemBase CreateItemData(Dictionary<string, string> itemData) {
            ItemBase returnData;
            if (!itemData.ContainsKey(Utility.HEADER_DEFINE_CLASSIFICATION)) {
                string[] equipList = itemData[Utility.HEADER_DEFINE_REQUIRE_EQUIP].Replace(" ", "").Split(new char[] { ',' });
                List<ItemBase> list = m_itemList.Where(x => equipList.Contains(x.Name)).ToList();
                returnData = new EquipmentGroup(itemData, list);
            }
            else if (itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_HEAD) {
                returnData = new Head(itemData);
            }
            else if (itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_UPPERBODY) {
                returnData = new UpperBody(itemData);
            }
            else if (itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_LOWERBODY) {
                returnData = new LowerBody(itemData);
            }
            else if (itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_ARM) {
                returnData = new Arm(itemData);
            }
            else if (itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_LEG) {
                returnData = new Leg(itemData);
            }
            else if (itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_SHIELD) {
                returnData = new Shield(itemData);
            }
            else if (
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_ACCESSORY_FACE ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_ACCESSORY_NECK ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_ACCESSORY_FINGER ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_ACCESSORY_CHEST ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_ACCESSORY_WAIST ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_ACCESSORY_NOTE ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_ACCESSORY_OTHER ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_ACCESSORY_PROOF ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_ACCESSORY_CREST
                ) {
                returnData = new Accessory(itemData);
            }
            else if (
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_CRAFT_HAMMER ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_CRAFT_KNIFE ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_CRAFT_NEEDLE ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_CRAFT_POT ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_CRAFT_LAMP ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_CRAFT_FLYPAN
                ) {
                returnData = new Craft(itemData);
            }
            else if (
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.PARTS_CRAFT_FISHING
                ) {
                returnData = new Fishing(itemData);
            }
            else if (
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.ITEM_FOOD
                ) {
                returnData = new Food(itemData);
            }
            else if (
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.ITEM_MATERIAL ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.ITEM_SUPPLY ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.ITEM_COIN ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.ITEM_FLOWER ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.ITEM_SEED ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.ITEM_RECIPE ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.ITEM_SCOUT ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.ITEM_GESTURE ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.ITEM_HOUSE
                ) {
                returnData = new Item(itemData);
            }
            else if (
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.ITEM_FURNITURE ||
                itemData[Utility.HEADER_DEFINE_CLASSIFICATION] == Utility.ITEM_GARDEN
                ) {
                string data = itemData[Utility.HEADER_DEFINE_NAME].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[0];
                itemData[Utility.HEADER_DEFINE_NAME] = data;
                returnData = new Item(itemData);
            }
            else {
                returnData = new Weapon(itemData);
            }

            return returnData;
        }

        public (List<List<EquipmentBase>>, List<string>) GetEquipList(string user, string job, bool onlySetEquip, List<string> targetParts, List<string> appendParts, List<EquipmentBase> haveEquipList, Dictionary<string, float> needRegist, Dictionary<string, float> orbRegistList) {
            (List<List<EquipmentBase>> equipList, List<string> abilityList) resultList = (new List<List<EquipmentBase>>(), new List<string>());

            List<EquipmentBase> setEquips = m_itemList.Where(x => x.Classification == Utility.PARTS_SET && ((EquipmentBase)x).EquipableJobs.Contains(job)).Select(x => (EquipmentBase)x).OrderByDescending(x => x.RequireLevel).ToList();
            // 選択された職のすべての装備
            List<EquipmentBase> allEquips = m_itemList.Where(x => targetParts.Contains(x.Classification) && ((EquipmentBase)x).EquipableJobs.Contains(job)).Select(x => (EquipmentBase)x).OrderByDescending(x => x.RequireLevel).ToList();

            if(onlySetEquip) {
                for(int i = 0; i < setEquips.Count; i++) {
                    EquipmentGroup setEquip = (EquipmentGroup)setEquips[i];

                    Dictionary<string, int> partsCheckList = new Dictionary<string, int>();
                    for (int j = 0; j < targetParts.Count; j++) {
                        partsCheckList[targetParts[j]] = 0;
                    }

                    bool haveSet = true;
                    List<EquipmentBase> haveSetList = new List<EquipmentBase>();
                    for(int j = 0;j < setEquip.SetEquipList.Count; j++) {
                        partsCheckList[setEquip.SetEquipList[j].Classification] = 1;

                        if (haveEquipList.Where(x => x.Name == setEquip.SetEquipList[j].Name).Count() == 0) { // 持ってない
                            haveSet = false;
                        }
                        else {
                            haveSetList.AddRange(haveEquipList.Where(x => x.Name == setEquip.SetEquipList[j].Name));
                        }
                    }

                    if (haveSet) {
                        (List<List<EquipmentBase>> equipList, List<string> abilityList) result = displayEquipSet(user, targetParts, appendParts, partsCheckList, allEquips, haveEquipList, haveSetList, needRegist, setEquip, orbRegistList);
                        resultList.equipList.AddRange(result.equipList);
                        resultList.abilityList.AddRange(result.abilityList);
                    }
                }
            }
            else {
                Dictionary<string, int> partsCheckList = new Dictionary<string, int>();
                for (int j = 0; j < targetParts.Count; j++) {
                    partsCheckList[targetParts[j]] = 0;
                }

                List<EquipmentBase> haveSetList = new List<EquipmentBase>();
                (List<List<EquipmentBase>> equipList, List<string> abilityList) result = displayEquipSet(user, targetParts, appendParts, partsCheckList, allEquips, haveEquipList, haveSetList, needRegist, null, orbRegistList);
                resultList.equipList.AddRange(result.equipList);
                resultList.abilityList.AddRange(result.abilityList);
            }
            return resultList;
        }

        private (List<List<EquipmentBase>>, List<string>) displayEquipSet(string user, List<string> bodyParts, List<string> appendParts, Dictionary<string, int> partsCheckList, List<EquipmentBase> allEquips, List<EquipmentBase> haveEquipList, List<EquipmentBase> haveSetList, Dictionary<string, float> targetRegistList, EquipmentGroup setEquip, Dictionary<string, float> orbRegistList) {
            // セットに含まれていない装備を検索
            List<EquipmentBase> nonSetList = new List<EquipmentBase>();
            List<string> nonParts = partsCheckList.Where(x => x.Value == 0).Select(x => x.Key).ToList();
            for (int j = 0; j < nonParts.Count; j++) {
                nonSetList.AddRange(allEquips.Where(x => x.Classification == nonParts[j]).ToList());
            }
            nonSetList = nonSetList.OrderByDescending(x => x.RequireLevel).ToList();
            for (int j = 0; j < nonSetList.Count; j++) {
                if (appendParts.Contains(nonSetList[j].Classification)) {
                    haveSetList.AddRange(haveEquipList.Where(x => x.Name == nonSetList[j].Name && x.AbilityList.Count > 0).ToList());
                }
                else {
                    haveSetList.AddRange(haveEquipList.Where(x => x.Name == nonSetList[j].Name).ToList());
                }
            }

            List<List<EquipmentBase>> allCheckEquipList = new List<List<EquipmentBase>>();
            // 手持ちの装備がそろった

            // すべての組み合わせを取得する
            checkEquipList(haveSetList, bodyParts.ToList(), null, 0, ref allCheckEquipList);


            //List<List<Dictionary<string, string>>> checkOkEquipList = new List<List<Dictionary<string, string>>>();
            List<Dictionary<string, float>> registList = new List<Dictionary<string, float>>();

            List<bool> checkList = new List<bool>();
            for (int j = 0; j < allCheckEquipList.Count; j++) {
                registList.Add(new Dictionary<string, float>());
                List<EquipmentBase> equipSet = allCheckEquipList[j];
                Dictionary<string, float> prevTargetRegistList = new Dictionary<string, float>(targetRegistList);
                for (int k = 0; k < equipSet.Count; k++) {
                    EquipmentBase equip = equipSet[k];
                    foreach (string key in targetRegistList.Keys) {
                        if (equip.AbilityList.ContainsKey(key)) {
                            prevTargetRegistList[key] -= equip.AbilityList[key];
                        }
                    }
                    foreach (string key in equip.AbilityList.Keys) {
                        if (!registList[j].ContainsKey(key)) {
                            registList[j].Add(key, 0);
                        }
                        registList[j][key] += equip.AbilityList[key];
                    }

                }
                if (setEquip != null) {
                    foreach (string key in setEquip.AbilityList.Keys) {
                        if (!registList[j].ContainsKey(key)) {
                            registList[j].Add(key, 0);
                        }
                        registList[j][key] += setEquip.AbilityList[key];
                    }
                }

                checkList.Add(true);
                foreach (string key in prevTargetRegistList.Keys) {
                    if (prevTargetRegistList[key] - orbRegistList[key] > 0) {
                        checkList[j] = false;
                    }
                }
            }

            // 仮決め
            List<List<EquipmentBase>> returnList = new List<List<EquipmentBase>>();
            List<string> abilityList = new List<string>();
            for (int j = 0; j < checkList.Count; j++) {
                if (checkList[j] == true) {
                    List<EquipmentBase> itemList = new List<EquipmentBase>();
                    for (int k = 0; k < allCheckEquipList[j].Count; k++) {
                        itemList.Add(allCheckEquipList[j][k]);
                    }
                    abilityList.Add("");
                    foreach (string key in registList[j].Keys) {
                        string orbString = "";
                        float orbNum = 0;
                        if (orbRegistList.ContainsKey(key)) {
                            orbNum = orbRegistList[key];
                            if (orbNum > 0) {
                                orbString = "(宝珠" + orbRegistList[key] + ")";
                            }
                        }
                        abilityList[abilityList.Count -1] += key + (registList[j][key] + orbNum) + orbString + "% ";
                    }
                    returnList.Add(itemList);
                }
            }
            return (returnList, abilityList);
        }


        private void checkEquipList(List<EquipmentBase> haveSetList, List<string> bodyParts, List<EquipmentBase> setEquipList, int index, ref List<List<EquipmentBase>> allCheckEquipList) {
            if (index >= bodyParts.Count) {
                allCheckEquipList.Add(setEquipList);
                return;
            }

            if (setEquipList == null) {
                setEquipList = new List<EquipmentBase>();
            }
            int prevIndex = index;
            List<EquipmentBase> prevList = new List<EquipmentBase>(setEquipList);
            string nowParts = bodyParts[index];
            List<EquipmentBase> partsList = haveSetList.Where(x => x.Classification == nowParts).ToList();
            for (int i = 0; i < partsList.Count; i++) {
                index = prevIndex;
                setEquipList = new List<EquipmentBase>(prevList);
                setEquipList.Add(partsList[i]);
                checkEquipList(haveSetList, bodyParts, setEquipList, ++index, ref allCheckEquipList);
            }
            return;
        }

    }
}
