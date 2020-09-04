using ItemClassLibrary.Entity;
using ItemClassLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilityPatternCreater {
    class Program {
        static void Main(string[] args) {
            AbilityPattern app = new AbilityPattern(Utility.PARTS_HEAD, "",
                new List<string[]> {
                    new string[]{ "テスト1+(num)", "3,5,7", "0,1,2,3,-2|0,2,3,4,-3|0,3,5,-4" },
                    new string[]{ "テスト2+(num)", "5,7,10", "4,-3|2,5,-4|4,7,-5" },
                }, "%");
        }
    }
}
