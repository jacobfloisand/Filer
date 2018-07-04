using System;
using System.Dynamic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Microsoft.CSharp;

namespace FilerServiceTests2
{
    [TestClass]
    public class ServiceTests
    {
        private RestTestClient client = new RestTestClient("http://localhost:1993/FilerService.svc/");

        [TestMethod]
        public void TestMethod1()
        {
            dynamic d = new ExpandoObject();
            d.File = "This is the history of the united states beginning with its founding fathers...";
            d.Date = "7/3/2018";
            d.FileName = "HistoryDoc";
            d.Class = "US History";
            d.Unit = "The Beginning";
            d.Section = "Coming to Shore";
            d.Type = "Helpful Resources";
            d.isLink = false;
            d.Override = true;
            Response r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Accepted);

        }

        [TestMethod]
        public void TestMethod2()
        {
            dynamic d = new ExpandoObject();
            d.Address = "www.history.com";
            d.Date = "7/3/2018";
            d.LinkName = "HistoryDoc";
            d.Class = "US History";
            d.Unit = "The Beginning";
            d.Section = "Coming to Shore";
            d.Type = "Helpful Resources";
            d.isLink = true;
            d.Override = true;
            Response r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Accepted);

        }
    }
}
