using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskDispatchManager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDispatchManager.Common.Tests
{
    [TestClass()]
    public class WebUtilsTestsCommonUtils
    {
        [TestMethod()]
        public void PingProxyTestPingProxy()
        {
            var result = WebUtils.PingProxy("121.20.13.109", "6666"); 
            Assert.AreEqual(true,result);
            var result2 = WebUtils.PingProxy("221.7.178.77", "8123");
            Assert.AreEqual(false, result2);
        }
    }
}