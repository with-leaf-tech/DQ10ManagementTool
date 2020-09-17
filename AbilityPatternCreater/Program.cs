using ItemClassLibrary.Entity;
using ItemClassLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AbilityPatternCreater {
    class Program {
        static void Main(string[] args) {
            string settingFile = @"C:\Tools\ability.json";


            List<AbilityPattern> abilityList = new List<AbilityPattern>();

            // 武器
            string[] parts = new string[] { Utility.PARTS_WEAPON_STICK, Utility.PARTS_WEAPON_WAND };
            for(int i = 0; i < Utility.WEAPON_LIST.Length; i++) {
                if(parts.Contains(Utility.WEAPON_LIST[i])) {
                    abilityList.Add(new AbilityPattern(Utility.WEAPON_LIST[i], "",
                        new List<string[]> {
                        new string[]{ "こうげき力+(num)", "2,4,6", "0,1,3,-1|0,1,2,3,-2|0,1,2,3,-3" },
                        new string[]{ "MP吸収ガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                        new string[]{ "おしゃれさ+(num)", "5,10,15", "0,2,3,-4|0,2,3,-9|0,1,2,3,-14" },
                        new string[]{ "MP吸収率+(num)%", "1.0,1.2,1.4", "0,0.2,-0.8|0,0.2,-0.8|0,0.2,0.3,-0.8" },
                        new string[]{ "MP消費しない率+(num)%", "2,4,5", "0,0.5,1,2,-1.8|0,0.5,1,2,-3.6|0,0.5,1,2,-4.4" },
                        new string[]{ "呪文発動速度+(num)%", "2,4,6", "0,1,-2|0,1,-3|0,1,-5" },
                        new string[]{ "呪文ぼうそう率+(num)%", "1,1.2,1.4", "0,0.1,0.2,-0.5|0,0.1,0.2,-0.7|0,0.2,0.3,-0.9" },
                        new string[]{ "かいしん率+(num)%", "1.0,1.2,1.4", "0,0.2,-0.8|0,0.2,-0.8|0,0.2,0.3,-0.8" },
                        new string[]{ "攻撃時(num)%でどく", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%で眠り", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%でマヒ", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%で混乱", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%で幻惑", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%で魅了", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%でマホトーン", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%でヘナトス", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%でルカニ", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%でリホ系解除", "10,20,30", "0,4|0,4|0,4" },
                        new string[]{ "攻撃時(num)%でマホ系解除", "10,20,30", "0,4|0,4|0,4" },
                        new string[]{ "攻撃時(num)%で会心率上昇", "3,6,10", "0,2|0,2|0,2" },
                        }));
                }
                else {
                    abilityList.Add(new AbilityPattern(Utility.WEAPON_LIST[i], "",
                        new List<string[]> {
                        new string[]{ "こうげき力+(num)", "2,4,6", "0,1,3,-1|0,1,2,3,-2|0,1,2,3,-3" },
                        new string[]{ "かいしん率+(num)%", "1.0,1.2,1.4", "0,0.2,-0.8|0,0.2,-0.8|0,0.2,0.3,-0.8" },
                        new string[]{ "おしゃれさ+(num)", "5,10,15", "0,2,3,-4|0,2,3,-9|0,1,2,3,-14" },
                        new string[]{ "攻撃時(num)%でヘナトス", "2,3,4", "0,1,-1|0,1,-2|0,1,-3" },
                        new string[]{ "攻撃時(num)%でルカニ", "2,3,4", "0,1,-1|0,1,-2|0,1,-3" },
                        new string[]{ "呪文発動速度+(num)%", "2,4,6", "0,1|0,1|0,1" },
                        new string[]{ "MP消費しない率+(num)%", "2,4,5", "0,2|0,2|0,2" },
                        new string[]{ "武器ガード率+(num)%", "1,1.2,1.4", "0,0.2|0,0.2|0,0.3"},
                        new string[]{ "呪文ぼうそう率+(num)%", "1,1.2,1.4", "0,0.2|0,0.2|0,0.3" },
                        new string[]{ "攻撃時(num)%でどく", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%で眠り", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%でマヒ", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%で混乱", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%で幻惑", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%で魅了", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%でマホトーン", "2,3,4", "0,1|0,1|0,1" },
                        new string[]{ "攻撃時(num)%でリホ系解除", "10,20,30", "0,4|0,4|0,4" },
                        new string[]{ "攻撃時(num)%でマホ系解除", "10,20,30", "0,4|0,4|0,4" },
                        new string[]{ "攻撃時(num)%で会心率上昇", "3,6,10", "0,2|0,2|0,2" },
                        }));
                }
            }

            // 盾
            abilityList.Add(new AbilityPattern(Utility.PARTS_SHIELD, "",
                new List<string[]> {
                    new string[]{ "盾ガード率+(num)%","1,1.2,1.4", "0,0.1,0.2,-0.5|0,0.1,0.2,-0.7|0,0.2,0.3,-0.8" },
                    new string[]{ "ブレス系ダメージ(num)%減", "6,10", "0,2,-5|0,2,3,4,-9" },
                    new string[]{ "攻撃呪文ダメージ(num)%減", "3,5,7", "0,1,-2|0,1,-4|0,1,-6" },
                    new string[]{ "呪いガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "しばりガード+(num)%", "30,60,90", "0,2,4,6,8,10,-27|0,2,4,6,8,10,-54|0,2,4,6,8,10,-81" },
                    new string[]{ "おびえガード+(num)%", "30,60,90", "0,10|0,10|0,10" },
                    new string[]{ "踊らされガード+(num)%", "30,60,90", "0,10|0,10|0,10" },
                    new string[]{ "転びガード+(num)%", "30,60,90", "0,10|0,10|0,10" },
                    new string[]{ "炎ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
                    new string[]{ "氷ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
                    new string[]{ "風ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
                    new string[]{ "雷ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
                    new string[]{ "土ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
                    new string[]{ "闇ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
                    new string[]{ "光ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
            }));

            // アタマ
            abilityList.Add(new AbilityPattern(Utility.PARTS_HEAD, "",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)", "3,5,7", "0,2,4,-2|0,2,3,4,-3|0,1,2,3,-4" },
                    new string[]{ "さいだいMP+(num)", "5,10,15", "0,2,3,-4|0,2,3,-8|0,1,2,3,-12" },
                    new string[]{ "眠りガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "マヒガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "混乱ガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "封印ガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "幻惑ガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "即死ガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "おびえガード+(num)%", "30,60,90", "0,2,4,6,8,10,-27|0,2,4,6,8,10,-54|0,2,4,6,8,10,-81" },
                    new string[]{ "どくガード+(num)%", "20,40,60", "0,10|0,10|0,10" },
                    new string[]{ "開戦時(num)%でピオラ", "3,6,10", "0,2|0,2|0,2" },
            }));

            // からだ上
            abilityList.Add(new AbilityPattern(Utility.PARTS_UPPERBODY, "",
                new List<string[]> {
                    new string[]{ "しゅび力+(num)", "2,4,6", "0,1,3,-1|0,1,2,3,-2|0,1,2,3,-3" },
                    new string[]{ "ブレス系ダメージ(num)%減", "6,10", "0,2,-5|0,2,3,4,-9" },
                    new string[]{ "攻撃呪文ダメージ(num)%減", "3,5,7", "0,1,-2|0,1,-4|0,1,-6" },
                    new string[]{ "こうげき魔力+(num)", "5,10,15", "0,2,3,-4|0,2,3,-8|0,1,2,3,-12" },
                    new string[]{ "かいふく魔力+(num)", "5,10,15", "0,2,3,-4|0,2,3,-8|0,1,2,3,-12" },
                    new string[]{ "呪いガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "即死ガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "おもさ+(num)", "5,10,15", "0,3|0,3|0,3" },
                    new string[]{ "炎ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
                    new string[]{ "氷ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
                    new string[]{ "風ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
                    new string[]{ "雷ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
                    new string[]{ "土ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
                    new string[]{ "闇ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
                    new string[]{ "光ダメージ(num)%減", "6,10,14", "0,2|0,4|0,4" },
                    new string[]{ "開戦時(num)%でスカラ", "3,6,10", "0,2|0,2|0,2" },
            }));

            // からだ下
            abilityList.Add(new AbilityPattern(Utility.PARTS_LOWERBODY, "",
                new List<string[]> {
                    new string[]{ "しゅび力+(num)", "2,4,6", "0,1,3,-1|0,1,2,3,-2|0,1,2,3,-3" },
                    new string[]{ "MP吸収ガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "こうげき魔力+(num)", "5,10,15", "0,2,3,-4|0,2,3,-8|0,1,2,3,-12" },
                    new string[]{ "かいふく魔力+(num)", "5,10,15", "0,2,3,-4|0,2,3,-8|0,1,2,3,-12" },
                    new string[]{ "眠りガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "マヒガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "混乱ガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "封印ガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "幻惑ガード+(num)%", "20,40,60", "0,2,4,6,8,10,-15|0,2,4,6,8,10,-30|0,2,4,6,8,10,-45" },
                    new string[]{ "どくガード+(num)%", "20,40,60", "0,10|0,10|0,10" },
                    new string[]{ "開戦時(num)%でマホキテ", "3,6,10", "0,2|0,2|0,2" },
            }));

            // ウデ
            abilityList.Add(new AbilityPattern(Utility.PARTS_ARM, "",
                new List<string[]> {
                    new string[]{ "きようさ+(num)",  "5,10,15", "0,2,3,-4|0,2,3,-8|0,1,2,3,-12" },
                    new string[]{ "かいしん率+(num)%", "1.0,1.2,1.4", "0,0.2,-0.8|0,0.2,-0.8|0,0.2,0.3,-0.8" },
                    new string[]{ "MP消費しない率+(num)%", "2,4,5", "0,0.5,1,2,-1.8|0,0.5,1,2,-3.6|0,0.5,1,2,-4.4" },
                    new string[]{ "呪文発動速度+(num)%", "2,4,6", "0,1,-2|0,1,-3|0,1,-5" },
                    new string[]{ "呪文ぼうそう率+(num)%", "1,1.2,1.4", "0,0.1,0.2,-0.5|0,0.1,0.2,-0.7|0,0.2,0.3,-0.9" },
                    new string[]{ "攻撃時(num)%で眠り", "2,3,4", "0,1,-1|0,1,-2|0,1,-3" },
                    new string[]{ "攻撃時(num)%でヘナトス", "2,3,4", "0,1,-1|0,1,-2|0,1,-3" },
                    new string[]{ "攻撃時(num)%でルカニ", "2,3,4", "0,1,-1|0,1,-2|0,1,-3" },
                    new string[]{ "通常ドロップ率(num)倍", "1.2,1.4,1.5", "0,0.4|0,0.4|0,0.4" },
                    new string[]{ "レアドロップ率(num)倍", "1.2,1.4,1.5", "0,0.4|0,0.4|0,0.4" },
                    new string[]{ "攻撃時(num)%でどく", "2,3,4", "0,1|0,1|0,1" },
                    new string[]{ "攻撃時(num)%でマヒ", "2,3,4", "0,1|0,1|0,1" },
                    new string[]{ "攻撃時(num)%で混乱", "2,3,4", "0,1|0,1|0,1" },
                    new string[]{ "攻撃時(num)%で幻惑", "2,3,4", "0,1|0,1|0,1" },
                    new string[]{ "攻撃時(num)%で魅了", "2,3,4", "0,1|0,1|0,1" },
                    new string[]{ "攻撃時(num)%でマホトーン", "2,3,4", "0,1|0,1|0,1" },
            }));

            // 足
            abilityList.Add(new AbilityPattern(Utility.PARTS_LEG, "",
                new List<string[]> {
                    new string[]{ "おもさ+(num)", "5,10,15", "0,2,3,-4|0,2,3,-7|0,1,2,3,-10" },
                    new string[]{ "すばやさ+(num)",  "5,10,15", "0,2,3,-4|0,2,3,-8|0,1,2,3,-12" },
                    new string[]{ "身かわし率+(num)%", "1.0,1.2,1.4", "0,0.2,-0.8|0,0.2,-0.8|0,0.2,0.3,-0.8" },
                    new string[]{ "おしゃれさ+(num)", "5,10,15", "0,2,3,-4|0,2,3,-9|0,1,2,3,-14" },
                    new string[]{ "踊らされガード+(num)%", "30,60,90", "0,2,4,6,8,10,-27|0,2,4,6,8,10,-54|0,2,4,6,8,10,-81" },
                    new string[]{ "転びガード+(num)%", "30,60,90", "0,2,4,6,8,10,-27|0,2,4,6,8,10,-54|0,2,4,6,8,10,-81" },
                    new string[]{ "不意をつく確率+(num)%", "2,3,4", "0,1|0,1|0,1" },
                    new string[]{ "移動速度+(num)%", "1.0,2.0,3.0", "0,0.2|0,0.2|0,0.2" },
            }));

            // アクセサリー（顔）
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "魔犬の仮面",
                new List<string[]> {
                    new string[]{ "必殺チャージ時バイキルト", "0", "0" },
                    new string[]{ "必殺チャージ時魔力かくせい", "0", "0" },
                    new string[]{ "必殺チャージ時聖なる祈り", "0", "0" },
                    new string[]{ "必殺チャージ時早詠みの杖", "0", "0" },
                    new string[]{ "必殺チャージ時スカラ2段階", "0", "0" },
                    new string[]{ "必殺チャージ時魔結界2段階", "0", "0" },
                    new string[]{ "必殺チャージ時ピオラ2段階", "0", "0" },
                    new string[]{ "必殺チャージ時心頭滅却", "0", "0" },
                    new string[]{ "必殺チャージ時弓聖の守り星", "0", "0" },
                    new string[]{ "必殺チャージ時ホップスティック", "0", "0" },
                    new string[]{ "必殺チャージ時10%で聖騎士の堅陣", "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "死神のピアス",
                new List<string[]> {
                    new string[]{ "特技のダメージ+(num)", "5", "0" },
                    new string[]{ "特技の回復量+(num)", "5", "0" },
                    new string[]{ "呪文のダメージ+(num)", "5", "0" },
                    new string[]{ "呪文の回復量+(num)", "5", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "機神の眼甲",
                new List<string[]> {
                    new string[]{ "味方死亡時(num)%でためる", "1,2", "0|0" },
                    new string[]{ "味方死亡時(num)%で聖女の守り",  "1,2", "0|0" },
                    new string[]{ "さいだいHP+2", "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "悪霊の仮面",
                new List<string[]> {
                    new string[]{ "さいだいHP+2", "0", "0" },
                    new string[]{ "さいだいMP+2 ",  "0", "0" },
                    new string[]{ "こうげき魔力+2", "0", "0" },
                    new string[]{ "かいふく魔力+2", "0", "0" },
                    new string[]{ "すばやさ+2", "0", "0" },
                    new string[]{ "きようさ+2", "0", "0" },
                    new string[]{ "おしゃれさ+2", "0", "0" },
                    new string[]{ "おもさ+2", "0", "0" },
                    new string[]{ "戦闘開始時2%で必殺チャージ", "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "アクセルギア",
                new List<string[]> {
                    new string[]{ "(num)%で行動ターンを消費しない", "1.0,1.5", "0|0" },
                    new string[]{ "さいだいHP+(num)",  "1,2", "0|0" },
                    new string[]{ "すばやさ+(num)", "3,4", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "海魔の眼甲",
                new List<string[]> {
                    new string[]{ "魔物を倒すと(num)%でためる", "2,3,5", "0|0|0" },
                    new string[]{ "さいだいHP+2",  "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "モノクル",
                new List<string[]> {
                    new string[]{ "こうげき魔力+(num)", "1,2", "0|0" },
                    new string[]{ "かいふく魔力+(num)", "1,2", "0|0" },
                    new string[]{ "呪文ぼうそう率+(num)%", "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "ダークアイ",
                new List<string[]> {
                    new string[]{ "さいだいHP+2",  "0", "0" },
                    new string[]{ "こうげき魔力+(num)", "2,3", "0|0" },
                    new string[]{ "魅了ガード+(num)%", "5,10", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "ほしくずのピアス",
                new List<string[]> {
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "すばやさ+(num)", "1,2,3,4,5", "0|0|0|0|0" },
                    new string[]{ "おしゃれさ+(num)", "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "かいとうの仮面",
                new List<string[]> {
                    new string[]{ "しゅび力+1",  "0", "0" },
                    new string[]{ "すばやさ+1",  "0", "0" },
                    new string[]{ "きようさ+1",  "0", "0" },
                    new string[]{ "おしゃれさ+(num)", "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "ネレウスマスク",
                new List<string[]> {
                    new string[]{ "さいだいHP+2",  "0", "0" },
                    new string[]{ "どくガード+(num)%", "5,10", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "ダンディサングラス",
                new List<string[]> {
                    new string[]{ "おしゃれさ+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
                    new string[]{ "幻惑ガード+(num)%", "5,10", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "ダンディサングラス",
                new List<string[]> {
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)",  "1,2,3,4", "0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "地中ゴーグル",
                new List<string[]> {
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)",  "1,2,3,4", "0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "黒ぶちメガネ角",
                new List<string[]> {
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)",  "1,2,3,4", "0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "黒ぶちメガネ丸",
                new List<string[]> {
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)",  "1,2,3,4", "0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "教授のメガネ",
                new List<string[]> {
                    new string[]{ "さいだいMP+(num)",  "1,2", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "こうげき魔力+(num)", "1,2,3", "0|0|0" },
                    new string[]{ "かいふく魔力+(num)", "1,2,3", "0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "ダークグラス",
                new List<string[]> {
                    new string[]{ "こうげき力+2",  "0", "0" },
                    new string[]{ "さいだいHP+2",  "0", "0" },
                    new string[]{ "幻惑ガード+(num)%", "5,10", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "マスカレイドマスク",
                new List<string[]> {
                    new string[]{ "さいだいHP+1",  "0", "0" },
                    new string[]{ "しゅび力+1",  "0", "0" },
                    new string[]{ "こうげき魔力+1",  "0", "0" },
                    new string[]{ "かいふく魔力+1",  "0", "0" },
                    new string[]{ "きようさ+1",  "0", "0" },
                    new string[]{ "おしゃれさ+(num)", "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "ファントムマスク",
                new List<string[]> {
                    new string[]{ "すばやさ+(num)",  "1,2", "0|0" },
                    new string[]{ "きようさ+(num)",  "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)", "1,2,3,4", "0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "ぐるぐるメガネ",
                new List<string[]> {
                    new string[]{ "おしゃれさ+(num)",  "1,2", "0|0" },
                    new string[]{ "魅了ガード+(num)%",  "5,10", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "白アイパッチ",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "1,2", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "戦闘開始時に(num)%でリホイミ",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "エラい人のヒゲ",
                new List<string[]> {
                    new string[]{ "さいだいHP+1",  "0", "0" },
                    new string[]{ "しゅび力+1",  "0", "0" },
                    new string[]{ "こうげき魔力+1",  "0", "0" },
                    new string[]{ "かいふく魔力+1",  "0", "0" },
                    new string[]{ "きようさ+1",  "0", "0" },
                    new string[]{ "おしゃれさ+(num)", "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "ねこひげ",
                new List<string[]> {
                    new string[]{ "すばやさ+(num)",  "1,2,3,4", "0|0|0|0" },
                    new string[]{ "不意をつく確率(num)%",  "1,1.5", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "フサフサのヒゲ",
                new List<string[]> {
                    new string[]{ "さいだいHP+1",  "0", "0" },
                    new string[]{ "しゅび力+1",  "0", "0" },
                    new string[]{ "こうげき魔力+1",  "0", "0" },
                    new string[]{ "かいふく魔力+1",  "0", "0" },
                    new string[]{ "きようさ+1",  "0", "0" },
                    new string[]{ "おしゃれさ+(num)", "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "ふちなしメガネ",
                new List<string[]> {
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "こうげき魔力+(num)",  "1,2,3", "0|0|0" },
                    new string[]{ "かいふく魔力+(num)",  "1,2,3", "0|0|0" },
                    new string[]{ "おしゃれさ+(num)", "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "いやしのメガネ",
                new List<string[]> {
                    new string[]{ "さいだいMP+1",  "0", "0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "かいふく魔力+(num)",  "1,2,3", "0|0|0" },
                    new string[]{ "おしゃれさ+2", "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "マジカルメガネ",
                new List<string[]> {
                    new string[]{ "さいだいMP+1",  "0", "0" },
                    new string[]{ "こうげき魔力+(num)",  "1,2,3", "0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "パーティーメガネ",
                new List<string[]> {
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)", "1,2", "0|0" },
                    new string[]{ "戦闘開始時(num)%でためる",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "スライムピアス",
                new List<string[]> {
                    new string[]{ "さいだいHP+1",  "0", "0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "きようさ+(num)", "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)", "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "黒アイパッチ",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "1,2", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "戦闘開始時に(num)%でバイシオン",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "かいけつマスク",
                new List<string[]> {
                    new string[]{ "きようさ+(num)",  "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)", "1,2", "0|0" },
                    new string[]{ "戦闘開始時(num)%でためる",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "フサフサの白ヒゲ",
                new List<string[]> {
                    new string[]{ "さいだいHP+1",  "0", "0" },
                    new string[]{ "しゅび力+1",  "0", "0" },
                    new string[]{ "こうげき魔力+1",  "0", "0" },
                    new string[]{ "かいふく魔力+1",  "0", "0" },
                    new string[]{ "きようさ+1",  "0", "0" },
                    new string[]{ "おしゃれさ+(num)", "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "スライムベスピアス",
                new List<string[]> {
                    new string[]{ "さいだいHP+1",  "0", "0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "きようさ+(num)", "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)", "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FACE, "オーシャングラス",
                new List<string[]> {
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)", "1,2,3,4", "0|0|0|0" },
            }));

            // アクセサリー（首）
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "魔王のネックレス",
                new List<string[]> {
                    new string[]{ "こうげき魔力+(num)",  "3,5", "0|0" },
                    new string[]{ "さいだいMP+(num)", "2,3", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "竜のうろこ",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "2,3", "0|0" },
                    new string[]{ "こうげき力+(num)",  "3,5", "0|0" },
                    new string[]{ "しゅび力+5", "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "おうじょのあい",
                new List<string[]> {
                    new string[]{ "レアドロップ率1.1倍", "0", "0" },
                    new string[]{ "転生モンスター出現率1.1倍", "0", "0" },
                    new string[]{ "メタル系モンスター出現率1.1倍", "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "智謀の首かざり",
                new List<string[]> {
                    new string[]{ "炎の攻撃ダメージ+(num)%",  "0.5,1", "0|0" },
                    new string[]{ "氷の攻撃ダメージ+(num)%",  "0.5,1", "0|0" },
                    new string[]{ "風の攻撃ダメージ+(num)%",  "0.5,1", "0|0" },
                    new string[]{ "雷の攻撃ダメージ+(num)%",  "0.5,1", "0|0" },
                    new string[]{ "土の攻撃ダメージ+(num)%",  "0.5,1", "0|0" },
                    new string[]{ "闇の攻撃ダメージ+(num)%",  "0.5,1", "0|0" },
                    new string[]{ "光の攻撃ダメージ+(num)%",  "0.5,1", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "金のロザリオ",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "2,3", "0|0" },
                    new string[]{ "こうげき力+(num)",  "3,5", "0|0" },
                    new string[]{ "しゅび力+5", "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "銀のロザリオ",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "2,3", "0|0" },
                    new string[]{ "しゅび力+(num)",  "3,4", "0|0" },
                    new string[]{ "致死ダメージ時生存確率+5%", "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "忠誠のチョーカー",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "1,2", "0|0" },
                    new string[]{ "こうげき力+(num)",  "1,2,3,5", "0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "バトルチョーカー",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "1,2", "0|0" },
                    new string[]{ "こうげき力+(num)",  "1,2,3,5", "0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "ちからのペンダント",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "1,2", "0|0" },
                    new string[]{ "こうげき力+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "まもりのペンダント",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "1,2", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2,3", "0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "きんのネックレス",
                new List<string[]> {
                    new string[]{ "さいだいHP+1",  "0", "0" },
                    new string[]{ "しゅび力+1",  "0", "0" },
                    new string[]{ "おもさ+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "ようせいの首かざり",
                new List<string[]> {
                    new string[]{ "さいだいHP+1",  "0", "0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "MP吸収ガード+(num)%",  "5,10", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "ハートのペンダント",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "1,2,3", "0|0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "木彫りのロザリオ",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "1,2,3", "0|0|0" },
                    new string[]{ "しゅび力+(num)",  "3,4", "0|0" },
                    new string[]{ "致死ダメージ時生存確率+5%", "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "命のネックレス",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "1,2,3", "0|0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "戦闘勝利時にHP回復+2", "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "ちょうネクタイ",
                new List<string[]> {
                    new string[]{ "しゅび力+1",  "0", "0" },
                    new string[]{ "すばやさ+1",  "0", "0" },
                    new string[]{ "きようさ+1",  "0", "0" },
                    new string[]{ "おしゃれさ+(num)", "1,2,3,4,5", "0|0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NECK, "ラッキーペンダント",
                new List<string[]> {
                    new string[]{ "さいだいHP+1",  "0", "0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "能力弱体系状態異常耐性+(num)%",  "3,5", "0|0" },
            }));

            // アクセサリー（指）
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "武刃将軍のゆびわ",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "1,2", "0|0" },
                    new string[]{ "行動時(num)%でバイシオン",  "1.0,2.0,3.0", "0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "聖守護者のゆびわ",
                new List<string[]> {
                    new string[]{ "眠りガード+(num)%",  "20,30", "0|0" },
                    new string[]{ "マヒガード+(num)%",  "20,30", "0|0" },
                    new string[]{ "混乱ガード+(num)%",  "20,30", "0|0" },
                    new string[]{ "封印ガード+(num)%",  "20,30", "0|0" },
                    new string[]{ "幻惑ガード+(num)%",  "20,30", "0|0" },
                    new string[]{ "呪いガード+(num)%",  "20,30", "0|0" },
                    new string[]{ "即死ガード+(num)%",  "20,30", "0|0" },
                    new string[]{ "どくガード+(num)%",  "20,30", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "幻界闘士のゆびわ",
                new List<string[]> {
                    new string[]{ "攻撃力アップの効果が(num)秒増加",  "3,4,5", "0|0|0" },
                    new string[]{ "こうげき力+2",  "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "はやてのリング",
                new List<string[]> {
                    new string[]{ "すばやさ+(num)",  "1,2,3", "0|0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "魔導将軍のゆびわ",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "1,2", "0|0" },
                    new string[]{ "行動時(num)%で早詠みの杖",  "1.0,2.0,3.0", "0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "ひらめきのゆびわ",
                new List<string[]> {
                    new string[]{ "戦闘勝利時にMP回復+1",  "0", "0" },
                    new string[]{ "さいだいMP+(num)",  "1,2,3,4", "0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "幻界闘士のゆびわ",
                new List<string[]> {
                    new string[]{ "呪文威力アップの効果が(num)秒増加",  "3,4,5", "0|0|0" },
                    new string[]{ "こうげき魔力+2",  "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "ちからのゆびわ",
                new List<string[]> {
                    new string[]{ "こうげき力+(num)",  "1,2,3", "0|0|0" },
                    new string[]{ "さいだいHP+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "きんのゆびわ",
                new List<string[]> {
                    new string[]{ "おもさ+1",  "0", "0" },
                    new string[]{ "しゅび力+1",  "0", "0" },
                    new string[]{ "すばやさ+1",  "0", "0" },
                    new string[]{ "きようさ+1",  "0", "0" },
                    new string[]{ "おしゃれさ+1",  "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "聖印のゆびわ",
                new List<string[]> {
                    new string[]{ "即死ガード+(num)%",  "5,10", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "破呪のリング",
                new List<string[]> {
                    new string[]{ "呪いガード+(num)%",  "5,10", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "まんげつリング",
                new List<string[]> {
                    new string[]{ "マヒガード+(num)%",  "5,10", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "ソーサリーリング",
                new List<string[]> {
                    new string[]{ "戦闘勝利時にMP回復+1",  "0", "0" },
                    new string[]{ "さいだいMP+(num)",  "1,2,3,4", "0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "ピンクパールリング",
                new List<string[]> {
                    new string[]{ "さいだいMP+(num)",  "1,2", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)",  "3,5", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "理性のリング",
                new List<string[]> {
                    new string[]{ "混乱ガード+(num)%",  "5,10", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "はくあいのゆびわ",
                new List<string[]> {
                    new string[]{ "封印ガード+(num)%",  "5,10", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "めざましリング",
                new List<string[]> {
                    new string[]{ "眠りガード+(num)%",  "5,10", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "破幻のリング",
                new List<string[]> {
                    new string[]{ "幻惑ガード+(num)%",  "5,10", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_FINGER, "破毒のリング",
                new List<string[]> {
                    new string[]{ "どくガード+(num)%",  "5,10", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
            }));

            // アクセサリー（胸）
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CHEST, "アヌビスのアンク",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "3,4", "0|0" },
                    new string[]{ "さいだいMP+(num)",  "3,4", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CHEST, "アヌビスのブローチ",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "3,4", "0|0" },
                    new string[]{ "さいだいMP+(num)",  "3,4", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CHEST, "セトのアンク",
                new List<string[]> {
                    new string[]{ "こうげき力+(num)",  "2,3", "0|0" },
                    new string[]{ "さいだいMP+(num)",  "3,4", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CHEST, "セトのブローチ",
                new List<string[]> {
                    new string[]{ "こうげき力+(num)",  "2,3", "0|0" },
                    new string[]{ "さいだいMP+(num)",  "3,4", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CHEST, "セルケトのアンク",
                new List<string[]> {
                    new string[]{ "こうげき魔力+(num)",  "4,5", "0|0" },
                    new string[]{ "さいだいHP+(num)",  "2,3", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CHEST, "セルケトのブローチ",
                new List<string[]> {
                    new string[]{ "こうげき魔力+(num)",  "4,5", "0|0" },
                    new string[]{ "さいだいHP+(num)",  "2,3", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CHEST, "オシリスのアンク",
                new List<string[]> {
                    new string[]{ "おもさ+2",  "0", "0" },
                    new string[]{ "しゅび力+3",  "0", "0" },
                    new string[]{ "さいだいMP+(num)",  "2,3", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CHEST, "オシリスのブローチ",
                new List<string[]> {
                    new string[]{ "おもさ+2",  "0", "0" },
                    new string[]{ "しゅび力+3",  "0", "0" },
                    new string[]{ "さいだいMP+(num)",  "2,3", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CHEST, "バステトのアンク",
                new List<string[]> {
                    new string[]{ "さいだいHP+2",  "0", "0" },
                    new string[]{ "さいだいMP+2",  "0", "0" },
                    new string[]{ "おしゃれさ+(num)",  "3,4", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CHEST, "バステトのブローチ",
                new List<string[]> {
                    new string[]{ "さいだいHP+2",  "0", "0" },
                    new string[]{ "さいだいMP+2",  "0", "0" },
                    new string[]{ "おしゃれさ+(num)",  "3,4", "0|0" },
            }));

            // アクセサリー（腰）
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_WAIST, "剛勇のベルト",
                new List<string[]> {
                    new string[]{ "こうげき力+(num)",  "2,3,4", "0|0|0" },
                    new string[]{ "おもさ+(num)",  "1,2,3", "0|0|0" },
                    new string[]{ "さいだいHP+(num)",  "1,2,3", "0|0|0" },
                    new string[]{ "開戦時(num)%でヘヴィチャージ",  "20,25", "0|0" },
                    new string[]{ "開戦時(num)%で天使の守り",  "5,7", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_WAIST, "ハイドラベルト",
                new List<string[]> {
                    new string[]{ "おもさ+(num)",  "2,3,4", "0|0|0" },
                    new string[]{ "さいだいHP+(num)",  "1,2,3", "0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_WAIST, "隠者のベルト",
                new List<string[]> {
                    new string[]{ "不意をつく確率+1%",  "0", "0" },
                    new string[]{ "さいだいHP+(num)",  "1,2", "0|0" },
                    new string[]{ "さいだいMP+(num)",  "1,2", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_WAIST, "おしゃれなベルト",
                new List<string[]> {
                    new string[]{ "しゅび力+1",  "0", "0" },
                    new string[]{ "すばやさ+1",  "0", "0" },
                    new string[]{ "きようさ+1",  "0", "0" },
                    new string[]{ "おしゃれさ+(num)",  "1,2", "0|0" },
            }));

            // 戦神のベルト
            string[] oneHandWeapon = new string[] {
                Utility.PARTS_WEAPON_ONEHANDSWORD,
                Utility.PARTS_WEAPON_KNIFE,
                Utility.PARTS_WEAPON_STICK,
                Utility.PARTS_WEAPON_FAN,
                Utility.PARTS_WEAPON_HAMMER,
                Utility.PARTS_WEAPON_BOOMERANG,
            };

            List<string[]> patternList = new List<string[]>();
            patternList.Add(new string[] { "こうげき力+(num)", "10,12,14", "0|0|0" });
            patternList.Add(new string[] { "こうげき魔力とかいふく魔力+(num)", "16,20,24", "0|0|0" });
            patternList.Add(new string[] { "かいしん率と呪文ぼうそう率+(num)%", "1.5,2.0", "0|0" });
            for(int i = 0; i < Utility.WEAPON_LIST.Length; i++) {
                string appendStr = "";
                if(oneHandWeapon.Contains(Utility.WEAPON_LIST[i])) {
                    appendStr = "右手に";
                }
                patternList.Add(new string[] { appendStr + Utility.WEAPON_LIST[i] + "装備時炎攻撃ダメージ+(num)%", "10,11,12,13", "0|0|0|0" });
                patternList.Add(new string[] { appendStr + Utility.WEAPON_LIST[i] + "装備時氷攻撃ダメージ+(num)%", "10,11,12,13", "0|0|0|0" });
                patternList.Add(new string[] { appendStr + Utility.WEAPON_LIST[i] + "装備時光攻撃ダメージ+(num)%", "10,11,12,13", "0|0|0|0" });
                patternList.Add(new string[] { appendStr + Utility.WEAPON_LIST[i] + "装備時闇攻撃ダメージ+(num)%", "10,11,12,13", "0|0|0|0" });
                patternList.Add(new string[] { appendStr + Utility.WEAPON_LIST[i] + "装備時風攻撃ダメージ+(num)%", "10,11,12,13", "0|0|0|0" });
                patternList.Add(new string[] { appendStr + Utility.WEAPON_LIST[i] + "装備時雷攻撃ダメージ+(num)%", "10,11,12,13", "0|0|0|0" });
                patternList.Add(new string[] { appendStr + Utility.WEAPON_LIST[i] + "装備時土攻撃ダメージ+(num)%", "10,11,12,13", "0|0|0|0" });
            }
            patternList.Add(new string[] { "スライム系にダメージ+(num)%", "6,7,8,9", "0|0|0|0" });
            patternList.Add(new string[] { "けもの系にダメージ+(num)%", "6,7,8,9", "0|0|0|0" });
            patternList.Add(new string[] { "ドラゴン系にダメージ+(num)%", "6,7,8,9", "0|0|0|0" });
            patternList.Add(new string[] { "虫系にダメージ+(num)%", "6,7,8,9", "0|0|0|0" });
            patternList.Add(new string[] { "鳥系にダメージ+(num)%", "6,7,8,9", "0|0|0|0" });
            patternList.Add(new string[] { "植物系にダメージ+(num)%", "6,7,8,9", "0|0|0|0" });
            patternList.Add(new string[] { "物質系にダメージ+(num)%", "6,7,8,9", "0|0|0|0" });
            patternList.Add(new string[] { "マシン系にダメージ+(num)%", "6,7,8,9", "0|0|0|0" });
            patternList.Add(new string[] { "ゾンビ系にダメージ+(num)%", "6,7,8,9", "0|0|0|0" });
            patternList.Add(new string[] { "悪魔系にダメージ+(num)%", "6,7,8,9", "0|0|0|0" });
            patternList.Add(new string[] { "エレ系にダメージ+(num)%", "6,7,8,9", "0|0|0|0" });
            patternList.Add(new string[] { "怪人系にダメージ+(num)%", "6,7,8,9", "0|0|0|0" });
            patternList.Add(new string[] { "水系にダメージ+(num)%", "6,7,8,9", "0|0|0|0" });

            patternList.Add(new string[] { "盾装備で開戦時(num)%でピオラ", "14,16,18,20", "0|0|0|0" });
            patternList.Add(new string[] { "盾装備で開戦時(num)%で聖女の守り", "7,8,9,10", "0|0|0|0" });
            patternList.Add(new string[] { "盾装備で開戦時(num)%でキラキラポーン", "7,8,9,10", "0|0|0|0" });
            patternList.Add(new string[] { "盾装備で開戦時(num)%で天使の守り", "7,8,9,10", "0|0|0|0" });
            patternList.Add(new string[] { "盾装備で開戦時(num)%でマホトラのころも", "14,16,18,20", "0|0|0|0" });
            patternList.Add(new string[] { "盾装備で開戦時(num)%でマホターン", "7,8,9,10", "0|0|0|0" });
            patternList.Add(new string[] { "盾装備で開戦時(num)%で盾ガード率上昇", "7,8,9,10", "0|0|0|0" });

            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_WAIST, "戦神のベルト",
                patternList
                ));

            // 輝石のベルト
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NOTE, "輝石のベルト",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)", "6,8,10", "0|0|0" },
                    new string[]{ "さいだいMP+(num)", "6,8,10", "0|0|0" },
                    new string[]{ "こうげき力+(num)", "6,8,10", "0|0|0" },
                    new string[]{ "しゅび力+(num)",  "10,12,14,16,18", "0|0|0|0|0" },
                    new string[]{ "こうげき魔力+(num)",  "10,12,14,16,18", "0|0|0|0|0" },
                    new string[]{ "かいふく魔力+(num)",  "10,12,14,16,18", "0|0|0|0|0" },
                    new string[]{ "すばやさ+(num)",  "10,12,14,16,18", "0|0|0|0|0" },
                    new string[]{ "きようさ+(num)",  "10,12,14,16,18", "0|0|0|0|0" },
                    new string[]{ "おしゃれさ+(num)", "8,10,12", "0|0|0" },
                    new string[]{ "炎呪文ダメージ+(num)%", "4,5,6,8,10", "0|0|0|0|0" },
                    new string[]{ "氷呪文ダメージ+(num)%", "4,5,6,8,10", "0|0|0|0|0" },
                    new string[]{ "光呪文ダメージ+(num)%", "4,5,6,8,10", "0|0|0|0|0" },
                    new string[]{ "闇呪文ダメージ+(num)%", "4,5,6,8,10", "0|0|0|0|0" },
                    new string[]{ "風呪文ダメージ+(num)%", "4,5,6,8,10", "0|0|0|0|0" },
                    new string[]{ "炎特技ダメージ+(num)%", "4,5,6,8,10", "0|0|0|0|0" },
                    new string[]{ "氷特技ダメージ+(num)%", "4,5,6,8,10", "0|0|0|0|0" },
                    new string[]{ "光特技ダメージ+(num)%", "4,5,6,8,10", "0|0|0|0|0" },
                    new string[]{ "闇特技ダメージ+(num)%", "4,5,6,8,10", "0|0|0|0|0" },
                    new string[]{ "風特技ダメージ+(num)%", "4,5,6,8,10", "0|0|0|0|0" },
                    new string[]{ "雷特技ダメージ+(num)%", "4,5,6,8,10", "0|0|0|0|0" },
                    new string[]{ "土特技ダメージ+(num)%", "4,5,6,8,10", "0|0|0|0|0" },
                    new string[]{ "スライム系にダメージ+(num)%", "4,5,6,7,8", "0|0|0|0|0" },
                    new string[]{ "けもの系にダメージ+(num)%", "4,5,6,7,8", "0|0|0|0|0" },
                    new string[]{ "ドラゴン系にダメージ+(num)%", "4,5,6,7,8", "0|0|0|0|0" },
                    new string[]{ "虫系にダメージ+(num)%", "4,5,6,7,8", "0|0|0|0|0" },
                    new string[]{ "鳥系にダメージ+(num)%", "4,5,6,7,8", "0|0|0|0|0" },
                    new string[]{ "植物系にダメージ+(num)%", "4,5,6,7,8", "0|0|0|0|0" },
                    new string[]{ "物質系にダメージ+(num)%", "4,5,6,7,8", "0|0|0|0|0" },
                    new string[]{ "マシン系にダメージ+(num)%", "4,5,6,7,8", "0|0|0|0|0" },
                    new string[]{ "ゾンビ系にダメージ+(num)%", "4,5,6,7,8", "0|0|0|0|0" },
                    new string[]{ "悪魔系にダメージ+(num)%", "4,5,6,7,8", "0|0|0|0|0" },
                    new string[]{ "エレ系にダメージ+(num)%", "4,5,6,7,8", "0|0|0|0|0" },
                    new string[]{ "怪人系にダメージ+(num)%", "4,5,6,7,8", "0|0|0|0|0" },
                    new string[]{ "水系にダメージ+(num)%", "4,5,6,7,8", "0|0|0|0|0" },
                    new string[]{ "眠りガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "混乱ガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "マヒガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "幻惑ガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "封印ガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "どくガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "呪いガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "即死ガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "魅了ガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "しばりガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "おびえガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "転びガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "踊らされガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "ふっとびガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "MP吸収ガード+(num)%", "2,4,6,8,10", "0|0|0|0|0" },
                    new string[]{ "各弱体系耐性+(num)%", "6,8,10", "0|0|0" },
                    new string[]{ "通常ドロップ率1.1倍", "0", "0" },
                    new string[]{ "レアドロップ率1.1倍", "0", "0" },
                    new string[]{ "戦闘勝利時HP回復10～20", "0", "0" },
                    new string[]{ "戦闘勝利時MP回復1～3", "0", "0" },
                    new string[]{ "呪文発動速度+(num)%", "0.5,1,1.5,2", "0|0|0|0" },
                    new string[]{ "MP消費しない率+(num)%", "1,2,3", "0|0|0" },
                    new string[]{ "ダメージ10％反射+(num)%", "6,7,8,9,10", "0|0|0|0|0" },
                    new string[]{ "かいしん率+(num)%", "0.5,1", "0|0" },
                    new string[]{ "呪文ぼうそう率+(num)%", "1,1.5,2,3", "0|0|0|0" },
                    new string[]{ "攻撃時(num)%でルカニ", "2.0,2.5,3.0,3.5,4.0", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%でヘナトス", "2.0,2.5,3.0,3.5,4.0", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%で幻惑", "2.0,2.5,3.0,3.5,4.0", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%で眠り", "2.0,2.5,3.0,3.5,4.0", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%で混乱", "2.0,2.5,3.0,3.5,4.0", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%でマヒ", "2.0,2.5,3.0,3.5,4.0", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%でどく", "2.0,2.5,3.0,3.5,4.0", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%で猛毒", "2.0,2.5,3.0,3.5,4.0", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%でマホトーン", "2.0,2.5,3.0,3.5,4.0", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%で魅了", "2.0,2.5,3.0,3.5,4.0", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%でぶきみなひかり", "2.0,2.5,3.0,3.5,4.0", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%で魔導の書", "2.0,2.5,3.0,3.5,4.0", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%でテンション下げ", "12,14,16,18,20", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%でマホ系解除", "15,16,17,18,20", "0|0|0|0|0" },
                    new string[]{ "攻撃時(num)%でリホ系解除", "30,32,34,36,40", "0|0|0|0|0" },
                    new string[]{ "開戦時(num)%でピオラ", "12,14,16,18,20", "0|0|0|0|0" },
                    new string[]{ "開戦時(num)%でバイシオン", "12,14,16,18,20", "0|0|0|0|0" },
                    new string[]{ "開戦時(num)%でリホイミ", "12,14,16,18,20", "0|0|0|0|0" },
                    new string[]{ "開戦時(num)%でマホキテ", "12,14,16,18,20", "0|0|0|0|0" },
                    new string[]{ "開戦時(num)%でマホトラのころも", "12,14,16,18,20", "0|0|0|0|0" },
                    new string[]{ "開戦時(num)%でメイクアップ", "12,14,16,18,20", "0|0|0|0|0" },
                    new string[]{ "開戦時(num)%でキラキラポーン", "6.0,7.0,8.0,9.0,10.0", "0|0|0|0|0" },
                    new string[]{ "開戦時(num)%で聖女の守り", "6.0,7.0,8.0,9.0,10.0", "0|0|0|0|0" },
                    new string[]{ "開戦時(num)%で天使の守り", "6.0,7.0,8.0,9.0,10.0", "0|0|0|0|0" },
                    new string[]{ "開戦時(num)%でかいしん率上昇", "6.0,7.0,8.0,9.0,10.0", "0|0|0|0|0" },
                    new string[]{ "開戦時(num)%で盾ガード率上昇", "6.0,7.0,8.0,9.0,10.0", "0|0|0|0|0" },
                    new string[]{ "開戦時(num)%でためる", "6.0,7.0,8.0,9.0,10.0", "0|0|0|0|0" },
                    new string[]{ "夜開戦時(num)%でためる", "6.0,7.0,8.0,9.0,10.0", "0|0|0|0|0" },
                    new string[]{ "開戦時(num)%で必殺チャージ", "1.0,2.0", "0|0" },
            }));



            // アクセサリー（札）
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NOTE, "不思議のカード",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "3,6,9,12,15", "0|0|0|0|0" },
                    new string[]{ "さいだいMP+(num)",  "3,6,9,12,15", "0|0|0|0|0" },
                    new string[]{ "しゅび力+(num)",  "3,6,9,12,15", "0|0|0|0|0" },
                    new string[]{ "すばやさ+(num)",  "3,6,9,12,15", "0|0|0|0|0" },
                    new string[]{ "きようさ+(num)",  "3,6,9,12,15", "0|0|0|0|0" },
                    new string[]{ "おしゃれさ+(num)",  "3,6,9,12,15", "0|0|0|0|0" },
                    new string[]{ "こうげき魔力+(num)",  "3,6,9,12,15", "0|0|0|0|0" },
                    new string[]{ "かいふく魔力+(num)",  "3,6,9,12,15", "0|0|0|0|0" },
                    new string[]{ "こうげき力+(num)",  "1,2,4,6,8", "0|0|0|0|0" },
                    new string[]{ "おもさ+(num)",  "1,2,4,6,8", "0|0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_NOTE, "しんぴのカード",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "2,4", "0|0" },
                    new string[]{ "さいだいMP+(num)",  "2,4", "0|0" },
                    new string[]{ "しゅび力+(num)",  "2,4", "0|0" },
                    new string[]{ "すばやさ+(num)",  "2,4", "0|0" },
                    new string[]{ "きようさ+(num)",  "2,4", "0|0" },
                    new string[]{ "おしゃれさ+(num)",  "2,4", "0|0" },
                    new string[]{ "こうげき魔力+(num)",  "2,4", "0|0" },
                    new string[]{ "かいふく魔力+(num)",  "2,4", "0|0" },
                    new string[]{ "こうげき力+(num)",  "1,2", "0|0" },
                    new string[]{ "おもさ+(num)",  "1,2", "0|0" },
            }));

            // アクセサリー（他）
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_OTHER, "大地の大竜玉",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "3,4,5", "0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_OTHER, "大地の竜玉",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "3,4,5", "0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_OTHER, "炎光の勾玉",
                new List<string[]> {
                    new string[]{ "さいだいHPとすばやさ+(num)",  "3,4", "0|0" },
                    new string[]{ "さいだいHPときようさ+(num)",  "3,4", "0|0" },
                    new string[]{ "さいだいHPとおもさ+(num)",  "2,3", "0|0" },
                    new string[]{ "こうげき力+(num)",  "2,3", "0|0" },
                    new string[]{ "すばやさときようさ+(num)",  "4,5", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_OTHER, "ビーナスのなみだ",
                new List<string[]> {
                    new string[]{ "風ダメージ3%減",  "0", "0" },
                    new string[]{ "雷ダメージ3%減",  "0", "0" },
                    new string[]{ "さいだいHP+2",  "0", "0" },
                    new string[]{ "おしゃれさ+4",  "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_OTHER, "風雷のいんろう",
                new List<string[]> {
                    new string[]{ "さいだいHPとすばやさ+(num)",  "3,4", "0|0" },
                    new string[]{ "さいだいHPときようさ+(num)",  "3,4", "0|0" },
                    new string[]{ "さいだいHPとおもさ+(num)",  "2,3", "0|0" },
                    new string[]{ "こうげき力+(num)",  "2,3", "0|0" },
                    new string[]{ "すばやさときようさ+(num)",  "4,5", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_OTHER, "氷闇の月飾り",
                new List<string[]> {
                    new string[]{ "さいだいHPとすばやさ+(num)",  "3,4", "0|0" },
                    new string[]{ "さいだいHPときようさ+(num)",  "3,4", "0|0" },
                    new string[]{ "さいだいHPとおもさ+(num)",  "2,3", "0|0" },
                    new string[]{ "こうげき力+(num)",  "2,3", "0|0" },
                    new string[]{ "すばやさときようさ+(num)",  "4,5", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_OTHER, "ロイヤルチャーム",
                new List<string[]> {
                    new string[]{ "氷ダメージ3%減",  "0", "0" },
                    new string[]{ "闇ダメージ3%減",  "0", "0" },
                    new string[]{ "さいだいHP+2",  "0", "0" },
                    new string[]{ "おしゃれさ+4",  "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_OTHER, "まよけのすず",
                new List<string[]> {
                    new string[]{ "さいだいMP+(num)",  "1,2", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)",  "1,2,3,4", "0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_OTHER, "パワーチャーム",
                new List<string[]> {
                    new string[]{ "さいだいMP+(num)",  "1,2", "0|0" },
                    new string[]{ "こうげき力+(num)",  "1,2,3", "0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_OTHER, "幸運のおまもり",
                new List<string[]> {
                    new string[]{ "レアドロップ率1.1倍",  "0", "0" },
                    new string[]{ "きようさ+1",  "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_OTHER, "ひきよせのすず",
                new List<string[]> {
                    new string[]{ "さいだいMP+(num)",  "1,2", "0|0" },
                    new string[]{ "こうげき力+(num)",  "1,2", "0|0" },
                    new string[]{ "おしゃれさ+(num)",  "1,2,3,4", "0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_OTHER, "竜のおまもり",
                new List<string[]> {
                    new string[]{ "炎ダメージ3%減",  "0", "0" },
                    new string[]{ "光ダメージ3%減",  "0", "0" },
                    new string[]{ "さいだいHP+2",  "0", "0" },
                    new string[]{ "おしゃれさ+4",  "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_OTHER, "うさぎのおまもり",
                new List<string[]> {
                    new string[]{ "さいだいHP+(num)",  "1,2", "0|0" },
                    new string[]{ "さいだいMP+(num)",  "1,2", "0|0" },
                    new string[]{ "しゅび力+(num)",  "1,2", "0|0" },
                    new string[]{ "すばやさ+(num)",  "1,2,3,4", "0|0|0|0" },
                    new string[]{ "おしゃれさ+(num)",  "1,2", "0|0" },
            }));

            // アクセサリー（証）
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_PROOF, "ガナン帝国の勲章",
                new List<string[]> {
                    new string[]{ "会心と暴走ダメージ+(num)",  "1,3,5", "0|0|0" },
                    new string[]{ "さいだいHP+(num)",  "1,2", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_PROOF, "忠義の勲章",
                new List<string[]> {
                    new string[]{ "会心率と暴走率+(num)%",  "0.2,0.5", "0|0" },
                    new string[]{ "さいだいHP+(num)",  "2,3", "0|0" },
                    new string[]{ "きようさ+(num)",  "4,5", "0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_PROOF, "魔人の勲章",
                new List<string[]> {
                    new string[]{ "攻撃時 (num)%でためる",  "0.5,1.0", "0|0" },
                    new string[]{ "おもさ+2",  "0", "0" },
                    new string[]{ "きようさ+4",  "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_PROOF, "絆のエンブレム",
                new List<string[]> {
                    new string[]{ "絆時30%で主人にバイシオン",  "0", "0" },
                    new string[]{ "絆時30%で主人にスカラ",  "0", "0" },
                    new string[]{ "絆時30%で主人に聖女の守り",  "0", "0" },
                    new string[]{ "絆時30%で主人に弓聖の守り星",  "0", "0" },
                    new string[]{ "絆時30%で主人にマホステ",  "0", "0" },
                    new string[]{ "絆時30%で主人にまもりのきり",  "0", "0" },
                    new string[]{ "絆時30%で主人のHP100回復",  "0", "0" },
                    new string[]{ "絆時30%で主人のMP20回復",  "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_PROOF, "絆のエンブレム",
                new List<string[]> {
                    new string[]{ "絆時50%で主人にバイキルト",  "0", "0" },
                    new string[]{ "絆時50%で主人に2回スカラ",  "0", "0" },
                    new string[]{ "絆時50%で主人に聖女の守り",  "0", "0" },
                    new string[]{ "絆時50%で主人に聖なる祈り",  "0", "0" },
                    new string[]{ "絆時50%で主人に弓聖の守り星",  "0", "0" },
                    new string[]{ "絆時50%で主人に早詠みのつえ",  "0", "0" },
                    new string[]{ "絆時50%で主人に魔力かくせい",  "0", "0" },
                    new string[]{ "絆時50%で主人にマホステ",  "0", "0" },
                    new string[]{ "絆時50%で主人にまもりのきり",  "0", "0" },
                    new string[]{ "絆時30%で主人のHP100回復",  "0", "0" },
                    new string[]{ "絆時30%で主人のMP20回復",  "0", "0" },
            }));

            // アクセサリー（紋章）
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CREST, "ハルファスの紋章",
                new List<string[]> {
                    new string[]{ "こうげき力とすばやさ+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
                    new string[]{ "こうげき力ときようさ+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
                    new string[]{ "こうげき力としゅび力+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CREST, "はじまりの紋章",
                new List<string[]> {
                    new string[]{ "さいだいHP+1",  "0", "0" },
                    new string[]{ "さいだいMP+1",  "0", "0" },
                    new string[]{ "おしゃれさ+1",  "0", "0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CREST, "アモンの紋章",
                new List<string[]> {
                    new string[]{ "おもさとすばやさ+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
                    new string[]{ "おもさときようさ+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
                    new string[]{ "おもさとしゅび力+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CREST, "アガレスの紋章",
                new List<string[]> {
                    new string[]{ "こうげき魔力とすばやさ+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
                    new string[]{ "こうげき魔力ときようさ+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
                    new string[]{ "こうげき魔力としゅび力+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CREST, "ブエルの紋章",
                new List<string[]> {
                    new string[]{ "かいふく魔力とすばやさ+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
                    new string[]{ "かいふく魔力ときようさ+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
                    new string[]{ "かいふく魔力としゅび力+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
            }));
            abilityList.Add(new AbilityPattern(Utility.PARTS_ACCESSORY_CREST, "グレモリーの紋章",
                new List<string[]> {
                    new string[]{ "おしゃれさとすばやさ+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
                    new string[]{ "おしゃれさときようさ+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
                    new string[]{ "おしゃれさとしゅび力+(num)",  "1,2,3,4,5", "0|0|0|0|0" },
            }));




            Utility.WriteAbilityPattern(settingFile, abilityList);




            List<AbilityPattern> DeAbilityList = Utility.ReadAbilityPattern(settingFile);

            var aaa = DeAbilityList.Where(x => x.Name == "輝石のベルト").ToList();
            var bbb = DeAbilityList.Where(x => x.Name == "戦神のベルト").ToList();


        }
    }
}
