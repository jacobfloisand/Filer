using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace FilerService
{
    [ServiceContract]
    public interface IFilerService
    {

        [WebInvoke(Method = "POST", UriTemplate = "/save")]
        void AddFile(ResourceData data);

        [WebInvoke(Method = "POST", UriTemplate = "/delete")]
        void Delete(ResourceData Nickname);

        [WebGet(UriTemplate = "/File/{Class}/{Unit}/{Section}/{Name}")]
        ResourceData GetFullFile(string Class, String Unit, String Section, String Name);

        [WebGet(UriTemplate = "/Search/{Class}/{Unit}/{Section}/{Name}/{Date}/{Type}")]
        ResourceData[] DoSearch(string Class, string Unit, string Section, string Name, string Date, string Type);

        [WebInvoke(Method = "POST", UriTemplate = "/Tag")]
        void MakeNewTag(ResourceData data);

        [WebGet(UriTemplate = "Files/Recent")]
        ResourceData[] GetRecentlyFiled();

        [WebGet(UriTemplate = "/tags")]
        TagData GetAllTags();

        [WebGet(UriTemplate = "/tags/last")]
        ResourceData GetLastTags();

        [WebInvoke(Method = "POST", UriTemplate = "toDoList")]
        void AddNewTask(OneTask task);

        [WebInvoke(Method = "POST", UriTemplate = "toDoList/remove")]
        void RemoveTask(OneTask task);

        [WebGet(UriTemplate = "/toDoList")]
        TaskData GetToDoList();
    }
}
