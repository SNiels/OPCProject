using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OPCLib.Service;
using SharedLib.Models;
using System.Collections;
using System.Collections.Generic;
using OPCLib.Models;
using System.Linq;
using OpcLabs.EasyOpc.DataAccess;

namespace TestProject
{
    [TestClass]
    public class UnitTestWCF
    {

        string json = "[{\"ItemId\":\"Data Type Examples.16 Bit Device.R Registers.WordArray\"},{\"ItemId\":\"Data Type Examples.16 Bit Device.R Registers.Boolean1\"}]";
        
        [TestMethod]
        public void TestParseJson()
        {
            IEnumerable<WCFNode> nodes = WCFNode.ParseJSONToNodes(json);
            Assert.IsNotNull(nodes);
            Assert.IsTrue(nodes.Count() == 2);
        }
    }
}
