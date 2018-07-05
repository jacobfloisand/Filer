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

        [WebInvoke(Method = "DELETE", UriTemplate = "/")]
        void Delete(ResourceData Nickname);

        [WebGet(UriTemplate = "/File")]
        ResourceData GetFullFile(ResourceData data);

        [WebGet(UriTemplate = "/Search")]
        SearchData DoSearch(ResourceData data);

        [WebInvoke(Method = "POST", UriTemplate = "/Tag")]
        void MakeNewTag(ResourceData data);

        [WebGet(UriTemplate = "Files/Recent")]
        SearchData GetRecentlyFiled();

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
