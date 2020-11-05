using ItemClassLibrary.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemClassLibrary.Entity {
    public class ItemBase {
        public string[] allJobs = Utility.allJobs;
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Classification { get; set; }
        public string Description { get; set; }
        public int count { get; set; }

        public virtual ItemBase Clone() {
            return JsonConvert.DeserializeObject<ItemBase>(JsonConvert.SerializeObject(this));
        }

        public virtual void CreateItemData(Dictionary<string, string> itemData) {
        }

        public Dictionary<string, float> AbilityCalc(List<string> setAbility) {
            Dictionary<string, float> setEquip = new Dictionary<string, float>();
            for (int j = 0; j < setAbility.Count; j++) {
                string[] abilityList = setAbility[j].Split(new char[] { '|' });
                for (int k = 0; k < abilityList.Length; k++) {
                    if (abilityList[k].Contains("ガード")) {
                        string kind = abilityList[k].Substring(0, (abilityList[k].IndexOf("ガード") + 3) - (abilityList[k].IndexOf(":") + 1));
                        string grade = abilityList[k].Substring(abilityList[k].IndexOf("+") + 1, abilityList[k].Length - 1 - (abilityList[k].IndexOf("+") + 1)); // %を除外する
                        float nGrade = Calc.Analyze(grade.Replace("(", "").Replace(")", "")).Calc(null);
                        if (!setEquip.ContainsKey(kind)) {
                            setEquip[kind] = 0;
                        }
                        setEquip[kind] += nGrade;
                    }
                    else if (!abilityList[k].Contains("軽減") && abilityList[k].Contains("減")) {
                        string kind = abilityList[k].Substring(0, (abilityList[k].IndexOf("ダメージ") + 4) - (abilityList[k].IndexOf(":") + 1));
                        string grade = abilityList[k].Substring(abilityList[k].IndexOf("ダメージ") + 4, abilityList[k].Length - 2 - (abilityList[k].IndexOf("ダメージ") + 4)); // %減を除外する
                        float nGrade = Calc.Analyze(grade.Replace("(", "").Replace(")", "")).Calc(null);
                        if (!setEquip.ContainsKey(kind)) {
                            setEquip[kind] = 0;
                        }
                        setEquip[kind] += nGrade;
                    }
                }
            }

            return setEquip;
        }
    }
}
