using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ItemClassLibrary.Util {
    static public class Utility {

        public static string PARTS_SET = "セット装備";
        public static string PARTS_HEAD = "アタマ";
        public static string PARTS_UPPERBODY = "からだ上";
        public static string PARTS_LOWERBODY = "からだ下";
        public static string PARTS_ARM = "ウデ";
        public static string PARTS_LEG = "足";
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




    }
}
