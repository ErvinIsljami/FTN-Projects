using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESProject.Classes;
namespace ProjectTest
{
    [TestClass]
    public class CodeMapsTest
    {
        [TestMethod]
        
        public void TestMethod1()
        {
            
            string codeNames0 = "CODE_ANALOG";
            string codeNames1 = "CODE_CUSTOM";
            string codeNames2 = "CODE_SINGLENODE";
            string codeNames3 = "CODE_CONSUMER";
            string codeNames4 = "CODE_MOTION";
            string codeNames5 = "CODE_DIGITAL";
            string codeNames6 = "CODE_LIMITSET";
            string codeNames7 = "CODE_MULTIPLENODE";
            string codeNames8 = "CODE_SOURCE";
            string codeNames9 = "CODE_SENSOR";

            CodeMaps maps = new CodeMaps();

            Assert.AreEqual(codeNames0, maps.GetNameForCode(Codes.CODE_ANALOG));
            Assert.AreEqual(codeNames1, maps.GetNameForCode((Codes)1));
            Assert.AreEqual(codeNames2, maps.GetNameForCode((Codes)2));
            Assert.AreEqual(codeNames3, maps.GetNameForCode((Codes)3));
            Assert.AreEqual(codeNames4, maps.GetNameForCode((Codes)4));
            Assert.AreEqual(codeNames5, maps.GetNameForCode((Codes)5));
            Assert.AreEqual(codeNames6, maps.GetNameForCode((Codes)6));
            Assert.AreEqual(codeNames7, maps.GetNameForCode((Codes)7));
            Assert.AreEqual(codeNames8, maps.GetNameForCode((Codes)8));
            Assert.AreEqual(codeNames9, maps.GetNameForCode((Codes)9));
            Assert.AreEqual(null, maps.GetNameForCode((Codes)11));
        }
    }
}
