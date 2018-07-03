using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FilerServiceTests2
{
    [TestClass]
    public class ServiceTests
    {
        private RestTestClient client = new RestTestClient("http://localhost:1993/FilerService.svc/");

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
