using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemClassLibrary.Event {
    public class ItemEventArgs : EventArgs {
        private readonly int _TestNumValue;
        private readonly string _TestStringValue;

        public ItemEventArgs(int TestNumValue, string TestStringValue) {
            _TestNumValue = TestNumValue;
            _TestStringValue = TestStringValue;
        }
        public int TestNumValue { get { return _TestNumValue; } }
        public string TestStringValue { get { return _TestStringValue; } }
    }
}