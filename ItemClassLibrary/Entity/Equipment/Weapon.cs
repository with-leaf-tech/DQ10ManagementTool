﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemClassLibrary.Entity.Equipment {
    public class Weapon : EquipmentBase {
        public Weapon(Dictionary<string, string> itemData) {
            base.CreateItemData(itemData);
        }

    }
}
