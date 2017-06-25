using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resources;

namespace TestProject {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void GetSetBit() {
            int t = 0;
            for(int i = 0; i < 8; i++) {
                Assert.IsFalse(t.GetBit(i));
            }
            for(int i = 0; i < 8; i++) {
                t.SetBit(true, i);
                Assert.IsTrue(t.GetBit(i));
            }

        }
    }
}
