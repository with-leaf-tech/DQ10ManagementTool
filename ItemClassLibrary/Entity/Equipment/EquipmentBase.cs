using ItemClassLibrary.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemClassLibrary.Entity.Equipment {
    public class EquipmentBase : ItemBase {
        public int Refine { get; set; }
        public int RequireLevel { get; set; }
        public string[] EquipableJobs { get; set; }
        public List<string> BasicAbility { get; set; } // 基礎効果
        public List<string> RefineAbility { get; set; } // 錬金、合成効果
        public List<string> SpecialAbility { get; set; } // 伝承、秘石、鬼石効果
        public List<string> AbilityPattern { get; set; } // この装備で有効な錬金、合成効果の一覧
        public Dictionary<string, float> AbilityList { get; set; }

        public virtual EquipmentBase Clone() {
            return JsonConvert.DeserializeObject<EquipmentBase>(JsonConvert.SerializeObject(this));
        }

        public override void CreateItemData(Dictionary<string, string> itemData) {
            if(itemData != null) {
                this.BasicAbility = new List<string>();
                this.RefineAbility = new List<string>();
                this.SpecialAbility = new List<string>();
                this.AbilityPattern = new List<string>();

                this.Name = itemData[Utility.HEADER_DEFINE_NAME];
                this.Description = itemData[Utility.HEADER_DEFINE_ABILITY];
                this.RequireLevel = (!itemData.ContainsKey(Utility.HEADER_DEFINE_LV) || itemData[Utility.HEADER_DEFINE_LV] == "-") ? 1 : int.Parse(itemData[Utility.HEADER_DEFINE_LV]);
                this.Classification = itemData[Utility.HEADER_DEFINE_CLASSIFICATION];
                this.EquipableJobs = (!itemData.ContainsKey(Utility.HEADER_DEFINE_EQUIPABLE_JOBS) || itemData[Utility.HEADER_DEFINE_EQUIPABLE_JOBS] == Utility.ALL_JOBS) ? allJobs : itemData[Utility.HEADER_DEFINE_EQUIPABLE_JOBS].Replace(" ", "").Split(new char[] { ',' });
                AbilityList = AbilityCalc(BasicAbility.Concat(RefineAbility).Concat(SpecialAbility).ToList());
            }
        }

    }
}
