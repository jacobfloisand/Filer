using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FilerClient
{
    public class FilerController
    {
        FilerView GUI;
        public FilerController(FilerView _GUI)
        {
            GUI = _GUI;
            GUI.SaveEvent += SaveAsync;
            GUI.SearchEvent += SearchAsync;
        }

        private HttpClient MakeClient()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("http://floisandcompany.dynu.net:1993/FilerService.svc/")
            };
            HttpResponseMessage r = client.GetAsync("Search").Result;
            if ((int)r.StatusCode == 200 || (int)r.StatusCode == 204) //If the ping worked we don't need to bother connecting to LAN client.
            {
                return client;
            }
            HttpClient LANClient = new HttpClient //Since ping failed we will try to use LAN client.
            {
                BaseAddress = new Uri("http://10.0.0.27:1993/FilerService.svc/")
            };
            HttpResponseMessage s = LANClient.GetAsync("Search").Result;
            if ((int)s.StatusCode == 200 || (int)s.StatusCode == 204) //If the ping worked we are good to use the LAN client.
            {
                return LANClient;
            }
            //TODO: Here we need to make a popup that says something went wrong in making the httpClient.
            return null;
   //         throw new Exception("Failed to make normal client and LANClient. Please call or Text Floisand Co. at 801-381-1721");
        }


        public async void SaveAsync(string File, string Address, string Date, string FileName, string LinkName, string Class, string Unit, string Section, string Type, string isLink, string Override)
        {
            using (HttpClient client = MakeClient()) //TODO - We need an if statement that executes if user is at Floisand Company Capital.(Or that tries both uri's)
            {
                dynamic data = new ExpandoObject();
                data.File = File;
                data.Link = Address;
                data.Date = Date;
                data.FileName = FileName;
                data.LinkName = LinkName;
                data.Class = Class;
                data.Unit = Unit;
                data.Section = Section;
                data.Type = Type;
                data.isLink = isLink;
                data.Override = Override;
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("save", content);
                string stringResponse = response.Content.ReadAsStringAsync().Result;
                dynamic deserializedResponse = JsonConvert.DeserializeObject(stringResponse);
                int statusCode = (int)response.StatusCode;
                if (statusCode == 202 || statusCode == 409) //If it's 409 we need a popup that lets the user override.
                {
                    return;
                }
                //TODO: If it comes back with 403 we need to let the user know that something might be missing.
                throw new Exception("We got a bad status code from SaveAsync");
                
            }
        }

        public async void SearchAsync(string Class, string Unit, string Section, string Name, string Date, string Type)
        {
            string serviceString = "Search?";
            bool ranAtLeastOne = false;
            if(!Class.Equals(""))
            {
                serviceString += "Class=" + Class + "&";
                ranAtLeastOne = true;
            }
            if (!Unit.Equals(""))
            {
                serviceString += "Unit=" + Unit + "&";
                ranAtLeastOne = true;
            }
            if (!Section.Equals(""))
            {
                serviceString += "Section=" + Section + "&";
                ranAtLeastOne = true;
            }
            if (!Name.Equals(""))
            {
                serviceString += "Name=" + Name + "&";
                ranAtLeastOne = true;
            }
            if (!Date.Equals(""))
            {
                serviceString += "Date=" + Date + "&";
                ranAtLeastOne = true;
            }
            if (!Type.Equals(""))
            {
                serviceString += "Type=" + Type + "&";
            }
            char[] chars = serviceString.ToCharArray();
            serviceString = "";
            for(int i = 0; i < chars.Length - 1; i++)
            {
                serviceString += chars[i];
            }
            

            using (HttpClient client = MakeClient())
            {
                HttpResponseMessage response = await client.GetAsync(serviceString);
                string stringResponse = response.Content.ReadAsStringAsync().Result;
                dynamic deserializedResponse = JsonConvert.DeserializeObject(stringResponse);
                int statusCode = (int)response.StatusCode; 
                if(statusCode == 204)
                {
                    return;
                }
                GUI.foundInSearch = new FilerView.SearchData[deserializedResponse.Count];
                for(int i = 0; i < deserializedResponse.Count; i++)
                {
                    if (((string)deserializedResponse[i].isLink).Equals("false"))
                    {
                        GUI.ResultListBox.Items.Add(deserializedResponse[i].FileName);
                    }
                    else
                    {
                        GUI.ResultListBox.Items.Add(deserializedResponse[i].LinkName);
                    }

                    GUI.foundInSearch[i] = new FilerView.SearchData()
                    {
                        FileName = deserializedResponse[i].FileName,
                        LinkName = deserializedResponse[i].LinkName,
                        Date = deserializedResponse[i].Date,
                        Class = deserializedResponse[i].Class,
                        Unit = deserializedResponse[i].Unit,
                        Section = deserializedResponse[i].Section,
                        Type = deserializedResponse[i].Section,
                        Link = deserializedResponse[i].Link,
                        isLink = deserializedResponse[i].isLink
                    };
                }
                //TODO: delete all of the extra entries that may have been left over from last time.

            }
        }
    }
}
