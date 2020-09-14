using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using ItemClassLibrary.Entity;
using ItemClassLibrary.Entity.Equipment;
using ItemClassLibrary.Util;
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
using System.Threading.Tasks;

namespace ItemClassLibrary.Manage {
    public class ItemManager {
        private static ItemManager m_itemManager = null;

        string saveDirectory = @"C:\Text\";
        string saveItemFile = @"itemData.json";
        string saveAbilityFile = @"ability.json";
        string replaceFile = @"Replace.json";

        List<AbilityPattern> abilityList = new List<AbilityPattern>();

        string[] setEquipUrls = new string[] {
                "http://bazaar.d-quest-10.com/list/sp_set/lv_1.html"
            };

        string[] equipUrls = new string[] {
                "http://bazaar.d-quest-10.com/list/d_head/lv_1.html",
                "http://bazaar.d-quest-10.com/list/d_upper/lv_1.html",
                "http://bazaar.d-quest-10.com/list/d_lower/lv_1.html",
                "http://bazaar.d-quest-10.com/list/d_arm/lv_1.html",
                "http://bazaar.d-quest-10.com/list/d_leg/lv_1.html",
                "http://bazaar.d-quest-10.com/list/d_shield/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_hand/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_both/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_short/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_spear/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_axe/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_claw/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_whip/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_stick/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_cane/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_club/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_fan/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_hammer/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_bow/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_boomerang/lv_1.html",
                "http://bazaar.d-quest-10.com/list/w_falx/lv_1.html"
            };

        string[] equipAccessoryUrls = new string[] {
                "http://bazaar.d-quest-10.com/list/d_accessory/pop_2.html"
            };

        string[] equipCraftUrls = new string[] {
                "http://bazaar.d-quest-10.com/list/t_hammer/lv_1.html",
                "http://bazaar.d-quest-10.com/list/t_knife/lv_1.html",
                "http://bazaar.d-quest-10.com/list/t_needle/lv_1.html",
                "http://bazaar.d-quest-10.com/list/t_pot/lv_1.html",
                "http://bazaar.d-quest-10.com/list/t_lamp/lv_1.html",
                "http://bazaar.d-quest-10.com/list/t_flypan/lv_1.html",
                "http://bazaar.d-quest-10.com/list/o_fishing/popular_1.html",
                "http://bazaar.d-quest-10.com/list/o_food/popular_1.html"
            };


        string[] itemUrls = new string[] {
                "http://bazaar.d-quest-10.com/list/o_material/popular_1.html",
                "http://bazaar.d-quest-10.com/list/o_orb/popular_1.html",
                "http://bazaar.d-quest-10.com/list/o_coin/popular_1.html",
                "http://bazaar.d-quest-10.com/list/o_flower/popular_1.html",
                "http://bazaar.d-quest-10.com/list/h_seed/popular_1.html",
                "http://bazaar.d-quest-10.com/list/o_recipe/popular_1.html",
                "http://bazaar.d-quest-10.com/list/o_scout/popular_1.html",
                "http://bazaar.d-quest-10.com/list/o_gesture/popular_1.html"
            };

        string[] itemDetailUrls = new string[] {
                "http://bazaar.d-quest-10.com/list/o_supply/popular_1.html",
                "http://bazaar.d-quest-10.com/list/h_house/popular_1.html",
            };

        string[] itemSimpleUrls = new string[] {
                "http://bazaar.d-quest-10.com/list/h_furniture/popular_1.html",
                "http://bazaar.d-quest-10.com/list/h_garden/popular_1.html"
            };

        string[] setHeader = Utility.setHeader;
        string[] equipHeader = Utility.equipHeader;
        string[] accessoryHeader = Utility.accessoryHeader;
        string[] craftHeader = Utility.craftHeader;
        string[] itemHeader = Utility.itemHeader;
        string[] itemHeaderDetail = Utility.itemHeaderDetail;
        string[] itemHeaderSimple = Utility.itemHeaderSimple;

        List<ItemBase> m_itemList = new List<ItemBase>();
        List<(string source, string dist)> replaceList = new List<(string source, string dist)>();


        public List<ItemBase> GetItemData() {
            return m_itemList;
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
            // ローカル(ネットワーク)から定義ファイルを読み込む
            // 定義ファイルを解析しアイテムリストを保持する
            loadReplaceData();

            if (File.Exists(saveDirectory + saveAbilityFile)) {
                abilityList = Utility.ReadAbilityPattern(saveDirectory + saveAbilityFile);
            }

            if (File.Exists(saveDirectory + saveItemFile)) {
                string jsonString = File.ReadAllText(saveDirectory + saveItemFile);

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
                        if (abilityList.Where(x => x.Classification == Utility.PARTS_WEAPON).Count() > 0) {
                            AbilityPattern ability = abilityList.Where(x => x.Classification == Utility.PARTS_WEAPON).First();
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

            File.WriteAllText(saveDirectory + saveItemFile, JsonConvert.SerializeObject(m_itemList));
        }

        private List<ItemBase> DownloadItemData(string[] header, string url) {
            List<ItemBase> itemList = new List<ItemBase>();
            List<Dictionary<string, string>> equipSetList = new List<Dictionary<string, string>>();


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
