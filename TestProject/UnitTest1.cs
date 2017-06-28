using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resources;

namespace TestProject {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void GetSetBit() {
            #region byte
            byte @byte = 0;
            for(int i = 0; i < 8; i++) {
                Assert.IsFalse(@byte.GetBit(i), i + " GetBit");
            }

            for(int i = 0; i < 8; i++) {
                Tools.SetBit(ref @byte,true, i);
                Assert.IsTrue(@byte.GetBit(i),i + " SetBit(true)");
                if(i > 0) {
                    Tools.SetBit(ref @byte, false, i - 1);
                    Assert.IsFalse(@byte.GetBit(i - 1),i + " SetBit(false)");
                }
            }
            #endregion
            #region int
            int @int = 0;
            for(int i = 0; i < 32; i++) {
                Assert.IsFalse(@int.GetBit(i), i + " GetBit");
            }

            for(int i = 0; i < 32; i++) {
                Tools.SetBit(ref @int,true, i);
                Assert.IsTrue(@int.GetBit(i),i + " SetBit(true)");
                if(i > 0) {
                    Tools.SetBit(ref @int, false, i - 1);
                    Assert.IsFalse(@int.GetBit(i - 1),i + " SetBit(false)");
                }
            }
            #endregion
            #region int
            long @long = 0;
            for(int i = 0; i < 64; i++) {
                Assert.IsFalse(@long.GetBit(i), i + " GetBit");
            }

            for(int i = 0; i < 64; i++) {
                Tools.SetBit(ref @long,true, i);
                Assert.IsTrue(@long.GetBit(i),i + " SetBit(true)");
                if(i > 0) {
                    Tools.SetBit(ref @long, false, i - 1);
                    Assert.IsFalse(@long.GetBit(i - 1),i + " SetBit(false)");
                }
            }
            #endregion
        }

        [TestMethod]
        public void TestHashh() {
            string text = "test1234";
            var hash = Hashing.Hash(text);
            Assert.IsTrue(Hashing.Verify(text, hash));
        }
    }
}
