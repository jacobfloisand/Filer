using System;
using System.Dynamic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Microsoft.CSharp;

namespace FilerServiceTests
{
    [TestClass]
    public class ServiceTests
    {
        private RestTestClient client = new RestTestClient("http://localhost:1993/FilerService.svc/");

        [TestMethod] //Makes sure status is set to accepted when a file is added.
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
            d.isLink = "false";
            d.Override = "true";
            d.Link = "www.thisisnotused.com";
            d.LinkName = "placeholder";
            Response r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Accepted);

        }

        [TestMethod]
        public void TestMethod2() //Makes sure status is set to accepted when a link is added.
        {
            dynamic d = new ExpandoObject();
            d.Link = "www.history.com";
            d.Date = "7/3/2018";
            d.LinkName = "HistoryLink";
            d.Class = "US History";
            d.Unit = "The Beginning";
            d.Section = "Coming to Shore";
            d.Type = "Helpful Resources";
            d.isLink = "true";
            d.Override = "true";
            Response r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Accepted);

        }

        [TestMethod]
        public void TestMethod2()
        {
            //What happens if some values are null when adding a file
        }

        [TestMethod]
        public void TestMethod2()
        {
            //What happens if some values are null when adding a link
        }

        [TestMethod]
        public void TestMethod2()
        {
            //What happens if all values are null when adding a file
        }
        [TestMethod]
        public void TestMethod2()
        {
            //What happens if all values are null when adding a link.
        }
        [TestMethod]
        public void TestMethod2()
        {
            //Add file that is already in DB.
        }
        [TestMethod]
        public void TestMethod2()
        {
            //Add link that is already in DB.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Make sure previous link is overriden when true. And not overridden when false.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Make sure previous file is overridden when true. And not overriden when false.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Try adding File/Link with no section specified.
        }
        [TestMethod]
        public void TestMethod2()
        {
            //Try adding File/Link with no Unit specified.
        }
        [TestMethod]
        public void TestMethod2()
        {
            //Try adding File/Link with no Class specified.
        }
        [TestMethod]
        public void TestMethod2()
        {
            //Try adding File/Link with no Name specified.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Works with .pdf's
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Works with .docx's
        }
        [TestMethod]
        public void TestMethod2()
        {
            //Works with .pptx's
        }
        [TestMethod]
        public void TestMethod2()
        {
            //Works with .XL files.
        }
        //---------------------End of Add File/Link tests------------------------------------

        [TestMethod]
        public void TestMethod2()
        {
            //Make sure file is actually deleted.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Maks sure link is actually deleted.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //If file is not in DB we get 409
        }
        [TestMethod]
        public void TestMethod2()
        {
            //If link is not in DB we get 409
        }
        [TestMethod]
        public void TestMethod2()
        {
            //All match except for Section is different/Section is null. Should not delete file.
        }
        [TestMethod]
        public void TestMethod2()
        {
            //Delete works even when the match doesn't specify class. Type should not matter.
        }

        //--------------------End of Delete Tests-----------------------------

        [TestMethod]
        public void TestMethod2()
        {
            //Works with large files.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Works with small files.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Returns 409 if the file/link was not in DB.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Returns 403 if the file name field is empty or null.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Get full file works with pdf's.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Get full file works with docx files.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Get full file works with ppx files.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Get full file works with excel files.
        }

        //-------------------------end of get full file tests------------------------
        [TestMethod]
        public void TestMethod2()
        {
            //Do Search returns all of the results it's supposed. And all of the information.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Do search still works with no class specified.
        }
        [TestMethod]
        public void TestMethod2()
        {
            //Do search still works with no Section specified.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Do search returns at least 30 items.
        }
        [TestMethod]
        public void TestMethod2()
        {
            //Do search returns 204 if there is nothing to return.
        }

        //------------------------The end of the Do search tests------------------------------

        [TestMethod]
        public void TestMethod2()
        {
            //Make new tag name successfully adds the new tag name to the tag file
        }

        [TestMethod]
        public void TestMethod2()
        {
            //If tag is a duplicate responds with 409
        }
        [TestMethod]
        public void TestMethod2()
        {
            //If tag is null or empty responds with status code 403.
        }

        //-----------------------End of Make new tag tests-------------------------

        [TestMethod]
        public void TestMethod2()
        {
            //last five items are returned in the proper order. And are accurate.
        }

        [TestMethod]
        public void TestMethod2()
        {
         //Get recently files items when there's less than five.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Make sure file itself is not returned.
        }

        //---------------------End of Get recently files itemds tests-----------------------------

        [TestMethod]
        public void TestMethod2()
        {
            //Make sure XML comes back as a valid string for a constructor for an xml object.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //100 items.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Make sure XML is accurate.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //No tags to return
        }
        //---------------------End of get all tags tests----------------------------

        [TestMethod]
        public void TestMethod2()
        {
            //Returns the correct tag information.
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Make sure it works when there is no last tag data.
        }

        //------------------End of Get last used tags tests----------------------


    }
}
