﻿using System;
using System.Dynamic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Microsoft.CSharp;
using System.IO;
using System.Net.Http;

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
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);

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
 //           Assert.AreEqual(r.Status, HttpStatusCode.Accepted);
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
        }

        [TestMethod]
        public void TestMethod3()  //What happens if some values are null when adding a file

        {
            dynamic d = new ExpandoObject();
            d.File = "This is the history of the united states beginning with its founding fathers...";
            d.Date = "7/3/2018";
            d.FileName = "HistoryDoc";
            d.Class = "US History";
            d.Unit = "The Beginning";
            d.Type = "Helpful Resources";
            d.isLink = "false";
            d.Override = "true";
            d.Link = "www.thisisnotused.com";
            d.LinkName = "placeholder";
            Response r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Accepted);
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
        }

        [TestMethod]
        public void TestMethod4()  //What happens if some values are null when adding a link

        {
            dynamic d = new ExpandoObject();
            d.Link = "www.history.com";
            d.Date = "7/3/2018";
            d.LinkName = "HistoryLink";
            d.Type = "Helpful Resources";
            d.isLink = "true";
            d.Override = "true";
            Response r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Accepted);
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
        }

        [TestMethod]
        public void TestMethod5()
        {
            //What happens if all values are null when adding a file
            dynamic d = new ExpandoObject();
            Response r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Forbidden);
        }
        [TestMethod]
        public void TestMethod6()
        {
            //What happens if all values are null when adding a link.
            dynamic d = new ExpandoObject();
            d.isLink = "true";
            Response r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Forbidden);
        }
        [TestMethod]
        public void TestMethod7()
        {
            //Add file that is already in DB.
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

            Response s = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.Conflict);

            Response t = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(t.Status, HttpStatusCode.OK);
        }
        [TestMethod]
        public void TestMethod8()
        {
            //Add link that is already in DB.
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

            Response s = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.Conflict);

            Response t = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(t.Status, HttpStatusCode.OK);
        }

        [TestMethod]
        public void TestMethod9() //This won't work until we implement do Search.
        {
            //Make sure previous link is overriden when true. And not overridden when false.
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

            d.Link = "www.historycnn.com";
            Response s = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.Accepted);

            dynamic a = client.DoGetAsync("Search/US History/The Beginning/Coming to Shore/HistoryLink/7/3/2018/Helpful Resources").Result;
            Assert.AreEqual(a.L1Address, "www.historycnn.com");

            //Deletes the link from DB.
            Response t = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(t.Status, HttpStatusCode.OK);
        }

        [TestMethod]
        public void TestMethod10()//This won't work until we implement do Search.
        {
            //Make sure previous file is overridden when true. And not overriden when false.
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

            d.File = "This is a nice story about the gunshot that was heard all around the world.";
            Response s = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.Accepted);

            dynamic a = client.DoGetAsync("File/US History/The Beginning/Coming to Shore/HistoryDoc").Result;
            Assert.AreEqual(a.File, "This is a nice story about the gunshot that was heard all around the world.");

            Response t = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(t.Status, HttpStatusCode.OK);
        }

        [TestMethod]
        public void TestMethod11()
        {
            //Try adding File/Link with no section specified.
            dynamic d = new ExpandoObject();
            d.File = "This is the history of the united states beginning with its founding fathers...";
            d.Date = "7/3/2018";
            d.FileName = "HistoryDoc";
            d.Class = "US History";
            d.Unit = "The Beginning";
            d.Type = "Helpful Resources";
            d.isLink = "false";
            d.Override = "true";
            d.Link = "www.thisisnotused.com";
            d.LinkName = "placeholder";
            Response r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Accepted);
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);

            d = new ExpandoObject();
            d.Link = "www.history.com";
            d.Date = "7/3/2018";
            d.LinkName = "HistoryLink";
            d.Class = "US History";
            d.Unit = "The Beginning";
            d.Type = "Helpful Resources";
            d.isLink = "true";
            d.Override = "true";
            r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Accepted);
            s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);


        }
        [TestMethod]
        public void TestMethod12() //This is all we need as far as testing specific files goes. It's all the same.
        {
            //Try adding File/Link with no Unit specified.
            dynamic d = new ExpandoObject();
            d.File = "This is the history of the united states beginning with its founding fathers...";
            d.Date = "7/3/2018";
            d.FileName = "HistoryDoc";
            d.Class = "US History";
            d.Section = "Coming to Shore";
            d.Type = "Helpful Resources";
            d.isLink = "false";
            d.Override = "true";
            d.Link = "www.thisisnotused.com";
            d.LinkName = "placeholder";
            Response r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Accepted);
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);

            d = new ExpandoObject();
            d.Link = "www.history.com";
            d.Date = "7/3/2018";
            d.LinkName = "HistoryLink";
            d.Class = "US History";
            d.Section = "Coming to Shore";
            d.Type = "Helpful Resources";
            d.isLink = "true";
            d.Override = "true";
            r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Accepted);
            s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
        }
        [TestMethod]
        public void TestMethod13()
        {
            //Try adding File/Link with no Class specified.
            dynamic d = new ExpandoObject();
            d.File = "This is the history of the united states beginning with its founding fathers...";
            d.Date = "7/3/2018";
            d.FileName = "HistoryDoc";
            d.Unit = "The Beginning";
            d.Section = "Coming to Shore";
            d.Type = "Helpful Resources";
            d.isLink = "false";
            d.Override = "true";
            d.Link = "www.thisisnotused.com";
            d.LinkName = "placeholder";
            Response r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Accepted);
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);

            d = new ExpandoObject();
            d.Link = "www.history.com";
            d.Date = "7/3/2018";
            d.LinkName = "HistoryLink";
            d.Unit = "The Beginning";
            d.Section = "Coming to Shore";
            d.Type = "Helpful Resources";
            d.isLink = "true";
            d.Override = "true";
            r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Accepted);
            s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
        }
        [TestMethod]
        public void TestMethod14()
        {
            //Try adding File/Link with no Name specified.
            dynamic d = new ExpandoObject();
            d.File = "This is the history of the united states beginning with its founding fathers...";
            d.Date = "7/3/2018";
            d.Class = "US History";
            d.Unit = "The Beginning";
            d.Section = "Coming to Shore";
            d.Type = "Helpful Resources";
            d.isLink = "false";
            d.Override = "true";
            d.Link = "www.thisisnotused.com";
            d.LinkName = "placeholder";
            Response r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Forbidden);

            d = new ExpandoObject();
            d.Link = "www.history.com";
            d.Date = "7/3/2018";
            d.Class = "US History";
            d.Unit = "The Beginning";
            d.Section = "Coming to Shore";
            d.Type = "Helpful Resources";
            d.isLink = "true";
            d.Override = "true";
            r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Forbidden);
        }

        [TestMethod]
        public void TestMethod15() //Won't work until we make the GetFullFile method.
        {
            //Works with actual files.
            
            //Read file as string.
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = System.IO.Path.Combine(path, "New members 7_29_18.pdf");
            byte[] bytes = File.ReadAllBytes(path);
            StringWriter writer = new StringWriter();
            foreach (byte b in bytes)
            {
                writer.Write((char)b);
            }
            string myPdf = writer.ToString();

            //Store everything in the DB.
            dynamic d = new ExpandoObject();
            d.File = myPdf;
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
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);

            //Now we try to get the file out of the DB.
            dynamic a = client.DoGetAsync("File/US History/The Beginning/Coming to Shore/HistoryDoc").Result;

            //Delete the file we put in the DB to reduce clutter.
            Response t = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(t.Status, HttpStatusCode.OK);


            //This turns the string we got from the DB into a Byte array.
            char[] charFile = a.File.ToCharArray();
            byte[] newBytes = new byte[charFile.Length];
            for(int i = 0; i < charFile.Length; i++)
            {
                newBytes[i] = (byte)charFile[i];
            }

            //This checks to make sure the bytes we got from the DB match the ones we sent up.
            for (int i = 0; i < charFile.Length; i++)
            {
                Assert.AreEqual(newBytes[i], bytes[i]);
            }

            //Use this commented out section if you want to write the file somewhere.
            //       string newPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //        newPath = System.IO.Path.Combine(newPath, "New Members Copy.pdf");
            //        File.WriteAllBytes(newPath, newBytes);
            
        }


        //---------------------End of Add File/Link tests------------------------------------

        [TestMethod]
        public void TestMethod16()
        {
            //Make sure file is actually deleted. Requires DoSearch
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
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
            dynamic a = client.DoGetAsync("Search/US History/The Beginning/Coming to Shore/HistoryDoc/7/3/2018/Helpful Resources").Result;
            Assert.AreEqual(a.NumFiles, 0);
            Assert.AreEqual(a.NumLinks, 0);
        }

        [TestMethod]
        public void TestMethod17()
        {
            //Maks sure link is actually deleted.
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
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
            dynamic a = client.DoGetAsync("Search/US History/The Beginning/Coming to Shore/HistoryLink/7/3/2018/Helpful Resources").Result;
            Assert.AreEqual(a.NumFiles, 0);
            Assert.AreEqual(a.NumLinks, 0);
        }

        [TestMethod]
        public void TestMethod18()
        {
            //If file is not in DB we get 409
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
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.Conflict);
        }
        [TestMethod]
        public void TestMethod19()
        {
            //If link is not in DB we get 409
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
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.Conflict);
        }
        [TestMethod]
        public void TestMethod20()
        {
            //All match except for Section is different/Section is null. Should not delete file.
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
            d.Section = "1900's";
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.Conflict);
        }
        [TestMethod]
        public void TestMethod21()
        {
            //Delete works even when the match doesn't specify class(neither may specify class). Type should not matter.
            dynamic d = new ExpandoObject();
            d.File = "This is the history of the united states beginning with its founding fathers...";
            d.Date = "7/3/2018";
            d.FileName = "HistoryDoc";
            d.Type = "Helpful Resources";
            d.isLink = "false";
            d.Override = "true";
            d.Link = "www.thisisnotused.com";
            d.LinkName = "placeholder";
            Response r = client.DoPostAsync("save", d).Result;
            Assert.AreEqual(r.Status, HttpStatusCode.Accepted);
            Response s = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
        }

        //--------------------End of Delete Tests-----------------------------
/*
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
*/
        //-------------------------end of get full file tests------------------------
        [TestMethod]
        public void TestMethod30()
        {
            //Do Search returns all of the results it's supposed to. And all of the information.
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
  //          Assert.AreEqual(r.Status, HttpStatusCode.Accepted);

            dynamic e = new ExpandoObject();
            e.Link = "www.history.com";
            e.Date = "7/3/2018";
            e.LinkName = "HistoryLink";
            e.Class = "US History";
            e.Unit = "The Beginning";
            e.Section = "Coming to Shore";
            e.Type = "Helpful Resources";
            e.isLink = "true";
            e.Override = "true";
            Response s = client.DoPostAsync("save", e).Result;
  //          Assert.AreEqual(s.Status, HttpStatusCode.Accepted);

            Response response = client.DoGetAsync("Search/US History/The Beginning/Coming to Shore//7/3/2018/Helpful Resources").Result;
            Assert.IsTrue(response.Data[0] != null);
            Assert.AreEqual(response.Data.Length, 1);
            Assert.AreEqual(response.Data[0].FileName, "HistoryDoc");
            Assert.AreEqual(response.Data[0].LinkName, "HistoryLink");

            Response t = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
            Response u = client.DoPostAsync("delete", e).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
        }

        [TestMethod]
        public void TestMethod31()
        {
            //Do search still works with no class specified.
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

            dynamic e = new ExpandoObject();
            e.Link = "www.history.com";
            e.Date = "7/3/2018";
            e.LinkName = "HistoryLink";
            e.Class = "US History";
            e.Unit = "The Beginning";
            e.Section = "Coming to Shore";
            e.Type = "Helpful Resources";
            e.isLink = "true";
            e.Override = "true";
            Response s = client.DoPostAsync("save", e).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.Accepted);

            dynamic a = client.DoGetAsync("Search/US History/The Beginning///7/3/2018/Helpful Resources").Result;
            Assert.AreEqual(a.NumFiles, 1);
            Assert.AreEqual(a.NumLinks, 1);
            Assert.AreEqual(a.F1Name, "HistoryDoc");
            Assert.AreEqual(a.L1Name, "HistoryLink");

            Response t = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
            Response u = client.DoPostAsync("delete", e).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
        }
        [TestMethod]
        public void TestMethod32()
        {
            //Do search still works with no Section specified.
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

            dynamic e = new ExpandoObject();
            e.Link = "www.history.com";
            e.Date = "7/3/2018";
            e.LinkName = "HistoryLink";
            e.Class = "US History";
            e.Unit = "The Beginning";
            e.Section = "Coming to Shore";
            e.Type = "Helpful Resources";
            e.isLink = "true";
            e.Override = "true";
            Response s = client.DoPostAsync("save", e).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.Accepted);

            dynamic a = client.DoGetAsync("Search/US History/The Beginning///7/3/2018/Helpful Resources").Result;
            Assert.AreEqual(a.NumFiles, 1);
            Assert.AreEqual(a.NumLinks, 1);
            Assert.AreEqual(a.F1Name, "HistoryDoc");
            Assert.AreEqual(a.L1Name, "HistoryLink");

            Response t = client.DoPostAsync("delete", d).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
            Response u = client.DoPostAsync("delete", e).Result;
            Assert.AreEqual(s.Status, HttpStatusCode.OK);
        }

        [TestMethod]
        public void TestMethod33()
        {
            //Do search returns at least 30 items.
            dynamic d = new ExpandoObject();
            dynamic e = null;
            for (int i = 0; i < 30; i++)
            {
                d.File = "This is the history of the united states beginning with its founding fathers...";
                d.Date = "7/3/2018";
                d.FileName = i.ToString();
                d.Class = "US History";
                d.Unit = "The Beginning";
                d.Section = "Coming to Shore";
                d.Type = "Helpful Resources";
                d.isLink = "false";
                d.Override = "true";
                d.Link = "www.thisisnotused.com";
                d.LinkName = "placeholder";
                e = client.DoPostAsync("save", d).Result;
                Assert.AreEqual(e.Status, HttpStatusCode.Accepted);
            }

            Assert.AreEqual(e.NumFiles, 30);

            for (int i = 0; i < 30; i++)
            {
                d.FileName = i.ToString();
                Response s = client.DoPostAsync("delete", d).Result;
                Assert.AreEqual(s.Status, HttpStatusCode.OK);
            }
        }
        [TestMethod]
        public void TestMethod34()
        {
            //Do search returns 204 if there is nothing to return.
            Response r = client.DoGetAsync("Search/US History/The Beginning/Coming to Shore//7/3/2018/Helpful Resources").Result;
            Assert.AreEqual(r.Status, HttpStatusCode.NoContent);
        }

        //------------------------The end of the Do search tests------------------------------
/*
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
        
    */
    }
}
