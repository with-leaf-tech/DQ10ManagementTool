using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRClassLibrary.Entity {
    [Serializable]
    public class AzureOCRResultEntity {
        public string language;
        public float textAngle;
        public string orientation;
        // 複数のリージョン
        public AzureOCRRegionEntity[] regions;
    }

    // リージョン
    [Serializable]
    public class AzureOCRRegionEntity {
        public string boundingBox;
        // 複数行を持つ
        public AzureOCRLineEntity[] lines;
    }

    // 行
    [Serializable]
    public class AzureOCRLineEntity {
        public string boundingBox;
        public AzureOCRWordEntity[] words;
    }

    // 単語
    [Serializable]
    public class AzureOCRWordEntity {
        public string boundingBox;
        public string text;
    }
}
