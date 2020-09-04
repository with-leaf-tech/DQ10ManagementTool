using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemClassLibrary.Entity.Equipment {
    public class Leg : EquipmentBase {
        public Leg(Dictionary<string, string> itemData) {
            base.CreateItemData(itemData);
        }

    }
}
