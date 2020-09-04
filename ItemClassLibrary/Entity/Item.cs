using ItemClassLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemClassLibrary.Entity {
    public class Item : ItemBase {
        public Item(Dictionary<string, string> itemData) {
            CreateItemData(itemData);
        }

        public override void CreateItemData(Dictionary<string, string> itemData) {
            if (itemData != null) {
                this.Name = itemData[Utility.HEADER_DEFINE_NAME];
                this.Classification = itemData[Utility.HEADER_DEFINE_CLASSIFICATION];
            }
        }
    }
}
