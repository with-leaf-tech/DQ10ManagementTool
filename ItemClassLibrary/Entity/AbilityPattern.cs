using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemClassLibrary.Entity {
    public class AbilityPattern {
        private string replaceString = "(num)";
        public string Classification { get; set; }
        public string Name { get; set; }
        public List<string> PatternList { get; set; }
        public AbilityPattern(string classification, string name, List<string[]> param, string option) {
            this.Classification = classification;
            this.Name = name;
            PatternList = new List<string>();
            for (int i = 0; i < param.Count; i++) {
                string baseString = param[i][0];
                string[] baseParams = param[i][1].Replace(" ", "").Split(new char[] { ',' });
                for(int j = 0; j < baseParams.Length; j++) {
                    if(param[i].Length > 2) {
                        string[] paramList = param[i][2].Replace(" ", "").Split(new char[] { '|' });
                        string[] status = paramList[j].Replace(" ", "").Split(new char[] { ',' });
                        for (int k = 0; k < status.Length; k++) {
                            string appendNum = status[k] == "0" ? "" : float.Parse(status[k]) > 0 ? "(+" + status[k] + ")" : "(" + status[k] + ")";
                            PatternList.Add(baseString.Replace(replaceString, baseParams[j].ToString() + appendNum + option));

                        }
                    }
                }
            }

        }
    }
}
