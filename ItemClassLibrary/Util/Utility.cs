using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using ItemClassLibrary.Entity;
using ItemClassLibrary.Entity.Equipment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ItemClassLibrary.Util {
    static public class Utility {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string PARTS_SET = "セット装備";
        public static string PARTS_HEAD = "アタマ";
        public static string PARTS_UPPERBODY = "からだ上";
        public static string PARTS_LOWERBODY = "からだ下";
        public static string PARTS_ARM = "ウデ";
        public static string PARTS_LEG = "足";
        public static string PARTS_WEAPON = "武器";
        public static string PARTS_SHIELD = "盾";
        public static string PARTS_WEAPON_ONEHANDSWORD = "片手剣";
        public static string PARTS_WEAPON_TWOHANDSWORD = "両手剣";
        public static string PARTS_WEAPON_KNIFE = "短剣";
        public static string PARTS_WEAPON_SPEAR = "ヤリ";
        public static string PARTS_WEAPON_AXE = "オノ";
        public static string PARTS_WEAPON_NAIL = "ツメ";
        public static string PARTS_WEAPON_WHIP = "ムチ";
        public static string PARTS_WEAPON_STICK = "スティック";
        public static string PARTS_WEAPON_WAND = "杖";
        public static string PARTS_WEAPON_CLUB = "棍";
        public static string PARTS_WEAPON_FAN = "扇";
        public static string PARTS_WEAPON_HAMMER = "ハンマー";
        public static string PARTS_WEAPON_BOW = "弓";
        public static string PARTS_WEAPON_BOOMERANG = "ブーメラン";
        public static string PARTS_WEAPON_SICKLE = "鎌";

        public static string PARTS_ACCESSORY_FACE = "アクセサリー（顔）";
        public static string PARTS_ACCESSORY_NECK = "アクセサリー（首）";
        public static string PARTS_ACCESSORY_FINGER = "アクセサリー（指）";
        public static string PARTS_ACCESSORY_CHEST = "アクセサリー（胸）";
        public static string PARTS_ACCESSORY_WAIST = "アクセサリー（腰）";
        public static string PARTS_ACCESSORY_NOTE = "アクセサリー（札）";
        public static string PARTS_ACCESSORY_OTHER = "アクセサリー（他）";
        public static string PARTS_ACCESSORY_PROOF = "アクセサリー（証）";
        public static string PARTS_ACCESSORY_CREST = "アクセサリー（紋章）";

        public static string PARTS_CRAFT_HAMMER = "鍛冶ハンマー";
        public static string PARTS_CRAFT_KNIFE = "木工刀";
        public static string PARTS_CRAFT_NEEDLE = "さいほう針";
        public static string PARTS_CRAFT_POT = "錬金ツボ";
        public static string PARTS_CRAFT_LAMP = "錬金ランプ";
        public static string PARTS_CRAFT_FLYPAN = "フライパン";
        public static string PARTS_CRAFT_FISHING = "釣りどうぐ";

        public static string ITEM_MATERIAL = "素材";
        public static string ITEM_SUPPLY = "つかうもの";
        public static string ITEM_FOOD = "料理";
        public static string ITEM_COIN = "コイン";
        public static string ITEM_FLOWER = "花";
        public static string ITEM_SEED = "タネ";
        public static string ITEM_RECIPE = "レシピ";
        public static string ITEM_SCOUT = "スカウトの書";
        public static string ITEM_GESTURE = "しぐさ書";
        public static string ITEM_HOUSE = "家キット";
        public static string ITEM_FURNITURE = "家具";
        public static string ITEM_GARDEN = "庭具";

        public static string REGIST_PALSY = "マヒ";
        public static string REGIST_CONFUSE = "混乱";
        public static string REGIST_SEALED = "封印";
        public static string REGIST_ILLUSION = "幻惑";
        public static string REGIST_POISON = "どく";
        public static string REGIST_SLEEP = "眠り";
        public static string REGIST_DEATH = "即死";
        public static string REGIST_CURSE = "呪い";
        public static string REGIST_MPDRAIN = "MP吸収";
        public static string REGIST_FALL = "転び";
        public static string REGIST_DANCE = "踊り";
        public static string REGIST_FIER = "おびえ";
        public static string REGIST_BIND = "しばり";
        public static string REGIST_CHARM = "魅了";


        public static string[] REGIST_LIST = new string[] {
            REGIST_PALSY,
            REGIST_CONFUSE,
            REGIST_SEALED,
            REGIST_ILLUSION,
            REGIST_POISON,
            REGIST_SLEEP,
            REGIST_DEATH,
            REGIST_CURSE,
            REGIST_MPDRAIN,
            REGIST_FALL,
            REGIST_DANCE,
            REGIST_FIER,
            REGIST_BIND,
            REGIST_CHARM,
        };


        public static string[] EQUIP_CATEGORY_LIST = new string[] {
            PARTS_HEAD,
            PARTS_UPPERBODY,
            PARTS_LOWERBODY,
            PARTS_ARM,
            PARTS_LEG,
            PARTS_SHIELD,
            PARTS_WEAPON_ONEHANDSWORD,
            PARTS_WEAPON_TWOHANDSWORD,
            PARTS_WEAPON_KNIFE,
            PARTS_WEAPON_SPEAR,
            PARTS_WEAPON_AXE,
            PARTS_WEAPON_NAIL,
            PARTS_WEAPON_WHIP,
            PARTS_WEAPON_STICK,
            PARTS_WEAPON_WAND,
            PARTS_WEAPON_CLUB,
            PARTS_WEAPON_FAN,
            PARTS_WEAPON_HAMMER,
            PARTS_WEAPON_BOW,
            PARTS_WEAPON_BOOMERANG,
            PARTS_WEAPON_SICKLE,

            PARTS_ACCESSORY_FACE,
            PARTS_ACCESSORY_NECK,
            PARTS_ACCESSORY_FINGER,
            PARTS_ACCESSORY_CHEST,
            PARTS_ACCESSORY_WAIST,
            PARTS_ACCESSORY_NOTE,
            PARTS_ACCESSORY_OTHER,
            PARTS_ACCESSORY_PROOF,
            PARTS_ACCESSORY_CREST,

            PARTS_CRAFT_HAMMER,
            PARTS_CRAFT_KNIFE,
            PARTS_CRAFT_NEEDLE,
            PARTS_CRAFT_POT,
            PARTS_CRAFT_LAMP,
            PARTS_CRAFT_FLYPAN,
            PARTS_CRAFT_FISHING,
        };

        public static string[] EQUIP_ACCESSORY_LIST = new string[] {
            PARTS_ACCESSORY_FACE,
            PARTS_ACCESSORY_NECK,
            PARTS_ACCESSORY_FINGER,
            PARTS_ACCESSORY_CHEST,
            PARTS_ACCESSORY_WAIST,
            PARTS_ACCESSORY_NOTE,
            PARTS_ACCESSORY_OTHER,
            PARTS_ACCESSORY_PROOF,
            PARTS_ACCESSORY_CREST,
        };

        public static string[] ITEM_CATEGORY_LIST = new string[] {
            ITEM_MATERIAL,
            ITEM_SUPPLY,
            ITEM_FOOD,
            ITEM_COIN,
            ITEM_FLOWER,
            ITEM_SEED,
            ITEM_RECIPE,
            ITEM_SCOUT,
            ITEM_GESTURE,
            ITEM_HOUSE,
            ITEM_FURNITURE,
            ITEM_GARDEN,
        };


        public static string HEADER_DEFINE_LV = "LV";
        public static string HEADER_DEFINE_SETNAME = "セット名";
        public static string HEADER_DEFINE_REQUIRE_EQUIP = "必要装備";
        public static string HEADER_DEFINE_DEFENCE = "守備";
        public static string HEADER_DEFINE_WEIGHT = "重さ";
        public static string HEADER_DEFINE_ATTACK_MAGIC = "攻魔";
        public static string HEADER_DEFINE_RECOVERY_MAGIC = "回魔";
        public static string HEADER_DEFINE_DEXTERITY = "器用";
        public static string HEADER_DEFINE_AGILITY = "早さ";
        public static string HEADER_DEFINE_FASHIONABLE = "おしゃれ";
        public static string HEADER_DEFINE_SET_SPECIAL_ABILITY = "セット特殊効果";
        public static string HEADER_DEFINE_SET_ABILITY = "セット効果";
        public static string HEADER_DEFINE_EQUIPABLE_JOBS = "装備職";
        public static string HEADER_DEFINE_NAME = "アイテム名";
        public static string HEADER_DEFINE_CLASSIFICATION = "分類";
        public static string HEADER_DEFINE_EXHIBYTS = "出品数";
        public static string HEADER_DEFINE_MIN_PRICE = "最安値";
        public static string HEADER_DEFINE_RANK1 = "★";
        public static string HEADER_DEFINE_RANK2 = "★★";
        public static string HEADER_DEFINE_RANK3 = "★★★";
        public static string HEADER_DEFINE_OFFICIALPAGE = "広場";
        public static string HEADER_DEFINE_ABILITY = "効果";
        public static string HEADER_DEFINE_BLANK = "空白";
        public static string HEADER_DEFINE_BUY_PRICE = "店買価格";
        public static string HEADER_DEFINE_SELL_PRICE = "店売価格";

        public static string[] allJobs = new string[] { "戦士", "僧侶", "魔使", "武闘", "盗賊", "旅芸", "バト", "パラ", "魔戦", "レン", "賢者", "スパ", "まも", "どう", "踊り", "占い", "天地", "遊び", "デス" };
        public static string ALL_JOBS = "全職業";

        public static string[] setHeader = new string[] { HEADER_DEFINE_LV, HEADER_DEFINE_SETNAME, HEADER_DEFINE_REQUIRE_EQUIP, HEADER_DEFINE_DEFENCE, HEADER_DEFINE_WEIGHT, HEADER_DEFINE_ATTACK_MAGIC, 
            HEADER_DEFINE_RECOVERY_MAGIC, HEADER_DEFINE_DEXTERITY, HEADER_DEFINE_AGILITY, HEADER_DEFINE_FASHIONABLE, HEADER_DEFINE_SET_SPECIAL_ABILITY, HEADER_DEFINE_SET_ABILITY, HEADER_DEFINE_EQUIPABLE_JOBS };
        public static string[] equipHeader = new string[] { HEADER_DEFINE_NAME, HEADER_DEFINE_LV, HEADER_DEFINE_CLASSIFICATION, HEADER_DEFINE_EXHIBYTS, HEADER_DEFINE_MIN_PRICE,
            HEADER_DEFINE_RANK1, HEADER_DEFINE_RANK2, HEADER_DEFINE_RANK3, HEADER_DEFINE_OFFICIALPAGE, HEADER_DEFINE_ABILITY, HEADER_DEFINE_EQUIPABLE_JOBS, HEADER_DEFINE_BLANK };
        public static string[] accessoryHeader = new string[] { HEADER_DEFINE_NAME, HEADER_DEFINE_CLASSIFICATION, HEADER_DEFINE_EXHIBYTS, HEADER_DEFINE_MIN_PRICE,
            HEADER_DEFINE_BUY_PRICE, HEADER_DEFINE_SELL_PRICE, HEADER_DEFINE_OFFICIALPAGE, HEADER_DEFINE_ABILITY, HEADER_DEFINE_BLANK };

        public static string[] craftHeader = new string[] { HEADER_DEFINE_NAME, HEADER_DEFINE_LV, HEADER_DEFINE_CLASSIFICATION, HEADER_DEFINE_EXHIBYTS, HEADER_DEFINE_MIN_PRICE,
            HEADER_DEFINE_RANK1, HEADER_DEFINE_RANK2, HEADER_DEFINE_RANK3, HEADER_DEFINE_OFFICIALPAGE, HEADER_DEFINE_ABILITY, HEADER_DEFINE_BLANK };

        public static string[] itemHeader = new string[] { HEADER_DEFINE_NAME, HEADER_DEFINE_CLASSIFICATION, HEADER_DEFINE_EXHIBYTS, HEADER_DEFINE_MIN_PRICE,
            HEADER_DEFINE_BUY_PRICE, HEADER_DEFINE_SELL_PRICE, HEADER_DEFINE_OFFICIALPAGE };
        public static string[] itemHeaderDetail = new string[] { HEADER_DEFINE_NAME, HEADER_DEFINE_CLASSIFICATION, HEADER_DEFINE_EXHIBYTS, HEADER_DEFINE_MIN_PRICE,
            HEADER_DEFINE_BUY_PRICE, HEADER_DEFINE_SELL_PRICE, HEADER_DEFINE_OFFICIALPAGE, HEADER_DEFINE_ABILITY };
        public static string[] itemHeaderSimple = new string[] { HEADER_DEFINE_NAME };

        public static string GetRefineAbility(string classification, string itemName) {
            string refineName = "合成";
            if (!Utility.EQUIP_ACCESSORY_LIST.Contains(classification)) {
                refineName = "錬金";
            }
            if (itemName == "輝石のベルト") {
                refineName = "輝石";
            }
            else if (itemName == "戦神のベルト") {
                refineName = "戦神";
            }
            return refineName;
        }

        public static string GetSpecialAbility(string classification, string itemName) {
            string refineName = "伝承";
            if (itemName == "輝石のベルト") {
                refineName = "秘石";
            }
            else if (itemName == "戦神のベルト") {
                refineName = "鬼石";
            }
            return refineName;
        }

        public static void WriteAbilityPattern(string path, List<AbilityPattern> abilityList) {
            string jsonString = JsonConvert.SerializeObject(abilityList);
            File.WriteAllText(path, jsonString);
        }

        public static List<AbilityPattern> ReadAbilityPattern(string path) {
            string jsonString = File.ReadAllText(path);
            List<AbilityPattern> newItems = JsonConvert.DeserializeObject<List<AbilityPattern>>(jsonString);
            return newItems;
        }

        public static List<string> GetNearlyString(string source, List<string> candidacyData) {
            List<string> topList = new List<string>();
            if (candidacyData.Where(x => x == source).Count() > 0) {
                topList.Add(source);
                return topList;
            }

            List<int> hitList = new List<int>(candidacyData.Select(x => 0).ToArray());
            for (int i = 0; i < source.Length - 1; i++) {
                string part = source.Substring(i, 1);
                int[] tempHitList = candidacyData.Select(x => x.Contains(part) ? 1 : 0).ToArray();
                hitList = hitList.Select((x, j) => x + tempHitList[j]).ToList();
            }
            if(hitList.Count > 0) {
                int topScore = hitList.Max();
                var hitIndexList = hitList.Select((x, i) => new int[] { x, i }).OrderByDescending(y => y[0]).ToList();

                // 候補を5件まで取る
                for (int i = 0; i < (hitIndexList.Count >= 5 ? 5 : hitIndexList.Count); i++) {
                    topList.Add(candidacyData[hitIndexList[i][1]]);
                }
            }
            else {
                topList.Add(source);
            }

            //var topIndex = hitList.Select((x, i) => new int[] { x, i }).Where(x => x[0] == topScore).Select(x => x[1]).ToArray();
            //topList = topIndex.Select(x => candidacyData[x]).ToList();

            return topList;
        }

        public static List<ItemBase> AnalyzeItem(int userId, string text, List<ItemBase> itemList) {
            try {
                string name = "";
                string equip = "";
                List<string> detailList = new List<string>();
                List<ItemBase> returnList = new List<ItemBase>();
                int remainBorder = 11;
                string tempName = "";

                MatchCollection mc1 = Regex.Matches(text, @"\dこ");
                MatchCollection mc2 = Regex.Matches(text, @"\dに");
                if (mc1.Count > 0 || mc2.Count > 0) {
                    List<ItemBase> defineList = itemList.Where(x => ITEM_CATEGORY_LIST.Contains(x.Classification)).ToList();
                    equip = "";
                    text = text
                        .Replace("1に", "1こ")
                        .Replace("2に", "2こ")
                        .Replace("3に", "3こ")
                        .Replace("4に", "4こ")
                        .Replace("5に", "5こ")
                        .Replace("6に", "6こ")
                        .Replace("7に", "7こ")
                        .Replace("8に", "8こ")
                        .Replace("9に", "9こ")
                        .Replace("0に", "0こ")
                        .Replace(" 2", "こ");
                    int itemIndex = 0;
                    string[] parts = text.Replace("”", "").Replace("\"", "").Replace("\r\n", "\n").Split(new char[] { '\n' });
                    for (int i = 0; i < parts.Length; i++) {
                        mc1 = Regex.Matches(parts[i], @"\dこ");
                        if (parts[i] == "る") {
                            continue;
                        }
                        if (parts[i].Length == 0) {
                            continue;
                        }
                        if (mc1.Count > 0
                            ) {
                            if (parts[i].Contains(" ")) {
                                string[] div = parts[i].Split(new char[] { ' ' });
                                string itemName = GetNearlyString(div[0], defineList.Select(x => x.Name).ToList()).FirstOrDefault();
                                detailList.Add(itemName + "," + div[1].Replace("こ", ""));
                            }
                            else {
                                while (detailList[itemIndex].Contains(",")) {
                                    itemIndex++;
                                }
                                detailList[itemIndex++] += "," + parts[i].Replace("こ", "");
                            }
                        }
                        else {
                            string itemName = GetNearlyString(parts[i], defineList.Select(x => x.Name).ToList()).FirstOrDefault();
                            detailList.Add(itemName);
                        }
                    }
                    for (int i = 0; i < detailList.Count; i++) {
                        string[] itemParts = detailList[i].Split(new char[] { ',' });
                        Item item = ((Item)(itemList.Where(x => x.Name == itemParts[0]).FirstOrDefault())).Clone();
                        item.OwnerId = userId;
                        item.count = int.Parse(itemParts[1]);
                        returnList.Add(item);
                    }
                }
                else {
                    string[] longLineText = new string[] { "輝石", "戦神" };
                    string[] specialAbility = new string[] { "伝承", "秘石", "鬼石" };
                    string[] namehead = new string[] { "EO", "ED", "EE", "NB", "の", "○", "N回", "E回", "回", "囚", "图", "四", "E图", "NO", "NG", "NE", "v3", "①", "②", "D", "E", "S", "N", "O", "B", "@", "3", "2" };
                    string[] parts = text.Replace(" ", "").Replace("\r\n", "\n").Split(new char[] { '\n' });
                    List<ItemBase> defineList = itemList.Where(x => EQUIP_CATEGORY_LIST.Contains(x.Classification)).ToList();
                    List<string> remainList = new List<string>();
                    bool remainStart = false;
                    for (int i = 0; i < parts.Length; i++) {
                        if (parts[i].Length == 0) {
                            continue;
                        }
                        if (tempName.Length == 0) {
                            tempName = parts[i];
                        }
                        if (remainStart == false && name.Length == 0 && parts[i].Contains("+")) {
                            name = parts[i].Replace(" ", "");
                            if (longLineText.Where(x => name.Contains(x)).Count() > 0) {
                                remainBorder = 13;
                            }
                        }

                        if (equip.Length == 0 && EQUIP_CATEGORY_LIST.Where(x => parts[i].Contains(x.Replace("アクセサリー（", "").Replace("）", ""))).Count() > 0) {
                            equip = EQUIP_CATEGORY_LIST.Where(x => parts[i].Contains(x.Replace("アクセサリー（", "").Replace("）", ""))).FirstOrDefault();
                        }

                        if (parts[i].Contains("追加効果")) {
                            remainStart = true;
                            continue;
                        }
                        if (parts[i].Contains("錬金石")) {
                            continue;
                        }
                        parts[i] = parts[i].Replace("効果企", "効果:").Replace("効果金", "効果:").Replace("効果:企", "効果:").Replace("効果:金", "効果:").Replace("効果:", "効果");
                        if (parts[i].Contains("錬金効") || parts[i].Contains("基礎効") || parts[i].Contains("合成効") || parts[i].Contains("伝承効")
                            || parts[i].Contains("輝石効") || parts[i].Contains("秘石効") || parts[i].Contains("戦神効") || parts[i].Contains("鬼石効")) {
                            string detail = parts[i]
                                .Replace("錬金効果", "錬金:")
                                .Replace("輝石効果", "輝石:")
                                .Replace("秘石効果", "秘石:")
                                .Replace("戦神効果", "戦神:")
                                .Replace("鬼石効果", "鬼石:")
                                .Replace("合成効果", "合成:")
                                .Replace("伝承効果", "伝承:")
                                .Replace("基礎効果", "基礎:");

                            detailList.Add(detail);
                        }
                        else if (!parts[i].Contains("できのよさ")) {
                            for (int j = 0; j < detailList.Count; j++) {
                                if (detailList[j].Length < remainBorder && remainList.Count > 0) {
                                    detailList[j] += remainList[remainList.Count - 1];
                                    remainList.RemoveAt(remainList.Count - 1);
                                    break;
                                }
                            }
                            if (remainStart) {
                                remainList.Add(parts[i].Replace(" ", ""));
                            }
                        }

                        if (parts[i].Contains("戦士") || parts[i].Contains("僧侶")) {
                            break;
                        }
                    }

                    if (name.Length == 0) {
                        name = tempName.Replace(" ", "");
                    }
                    if (name.Length > 0) {
                        for (int i = 0; i < namehead.Length; i++) {
                            if (name.Substring(0, namehead[i].Length) == namehead[i]) {
                                name = name.Substring(namehead[i].Length);
                                break;
                            }
                        }
                    }
                    if (name.IndexOf("+") > 0 && name.IndexOf("+") + 2 != name.Length) {
                        name = name.Substring(0, name.IndexOf("+") + 2);
                    }
                    for (int i = 0; i < detailList.Count; i++) {
                        detailList[i] = detailList[i].Replace(" ", "").Replace("_", "");
                    }
                    int refine = 0;
                    string itemName = GetNearlyString(name, defineList.Select(x => x.Name).ToList()).FirstOrDefault();
                    string refineString = name.Substring(name.IndexOf("+"));
                    if (refineString.Length > 0) {
                        refine = int.Parse(refineString);
                    }

                    EquipmentBase item = ((EquipmentBase)(defineList.Where(x => x.Name == itemName).FirstOrDefault())).Clone();
                    item.OwnerId = userId;
                    item.Refine = refine;
                    for (int i = 0; i < detailList.Count; i++) {
                        string[] ability = detailList[i].Split(new char[] { ':' });
                        if (ability.Length > 1) {
                            if (ability[0].Contains("基礎")) {
                                item.BasicAbility.Add(ability[1]);
                            }
                            else if (specialAbility.Where(x => ability[0].Contains(x)).Count() > 0) {
                                item.SpecialAbility.Add(ability[1]);
                            }
                            else {
                                item.RefineAbility.Add(ability[1]);
                            }
                        }
                    }
                    returnList.Add(item);
                }
                return returnList;
            }
            catch(Exception e) {
                logger.Error("アナライズエラー Error=" + e.Message + " StackTrace=" + e.StackTrace);
                return null;
            }

        }

        public static string SerializeBitmap(Bitmap bitmap) {
            ImageConverter converter = new ImageConverter();
            byte[] imageBytes = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
            return JsonConvert.SerializeObject(imageBytes);
        }

        public static Bitmap DeSerializeBitmap(string text) {
            try {
                byte[] imageBytes = JsonConvert.DeserializeObject<byte[]>(text);
                Bitmap bitmap = new Bitmap(new MemoryStream(imageBytes));
                return bitmap;
            }
            catch(Exception e) {
                return null;
            }
        }

    }
}
