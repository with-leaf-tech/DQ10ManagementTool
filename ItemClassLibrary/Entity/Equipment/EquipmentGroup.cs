using ItemClassLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemClassLibrary.Entity.Equipment {
    public class EquipmentGroup : EquipmentBase {
        public List<ItemBase> SetEquipList { get; set; }
        public List<string> GroupAbility { get; set; }

        public EquipmentGroup(Dictionary<string, string> itemData, List<ItemBase> equipList) {
            if(itemData != null) {
                this.Name = itemData[Utility.HEADER_DEFINE_SETNAME];
                this.Url = itemData[Utility.HEADER_DEFINE_URL];
                this.Classification = Utility.PARTS_SET;
                this.RequireLevel = (!itemData.ContainsKey(Utility.HEADER_DEFINE_LV) || itemData[Utility.HEADER_DEFINE_LV] == "-") ? 1 : int.Parse(itemData[Utility.HEADER_DEFINE_LV]);
                this.EquipableJobs = (!itemData.ContainsKey(Utility.HEADER_DEFINE_EQUIPABLE_JOBS) || itemData[Utility.HEADER_DEFINE_EQUIPABLE_JOBS] == Utility.ALL_JOBS) ? allJobs : itemData[Utility.HEADER_DEFINE_EQUIPABLE_JOBS].Replace(" ", "").Split(new char[] { ',' });

                this.SetEquipList = equipList;
                GroupAbility = itemData[Utility.HEADER_DEFINE_SET_SPECIAL_ABILITY].Replace("、", ",").Split(new char[] { ',' }).ToList();

                AbilityList = AbilityCalc(GroupAbility);
            }
        }

        public override void CreateItemData(Dictionary<string, string> itemData) {
           
        }

    }
}
