using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskDispatchManager.Common;
using TaskDispatchManager.DBModels.Base;
using TaskDispatchManager.IService;
using TaskDispatchManager.Service;

namespace TaskDispatchManager.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLoadExpressCompany()
        {
            LogHelper.WriteInfoLog("aaa");


            IExpressCompanyService expressCompanyService = new ExpressCompanyService();
            var result = expressCompanyService.LoadEntities(r => !string.IsNullOrEmpty(r.Code)).ToList();
           
            Assert.AreEqual(true,result.Any());
        }

        [TestMethod]
        public void TestUpdateProxy()
        {
            IProxyService service = new ProxyService();
            var result =  service.Update(new Proxy() {Guid = new Guid("C79CC4B7-37E0-4886-B21D-F7ECB1ED6ADB") ,IsDelete = true},r=>r.IsDelete);
            
            Assert.AreEqual(result, true);
        }
    }
}
