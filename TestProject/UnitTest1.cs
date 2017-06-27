using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resources;

namespace TestProject {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void GetSetBit() {
            int t = 0;
            for(int i = 0; i < 32; i++) {
                Assert.IsFalse(t.GetBit(i), i + " GetBit");
            }

            for(int i = 0; i < 32; i++) {
                Tools.SetBit(ref t,true, i);
                Assert.IsTrue(t.GetBit(i),i + " SetBit + GetBit");
            }
        }
    }
}
