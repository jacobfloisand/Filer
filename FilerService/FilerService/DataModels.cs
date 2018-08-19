using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FilerService
{
   
    [DataContract]
    public class ResourceData
    {
        [DataMember(EmitDefaultValue = false)]
        public string File { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Link { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FileName { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string LinkName { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Date { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Class { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Unit { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Section { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Type { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string isLink { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Override { get; set; }
    }

    [DataContract]
    public class TagData
    {
        [DataMember(EmitDefaultValue = false)]
        public string XML { get; set; }
    }

    [DataContract]
    public class TaskData
    {
        [DataMember(EmitDefaultValue = false)]
        public int NumItems { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string[] Tasks { get; set; }
    }

    [DataContract]
    public class OneTask
    {
        [DataMember(EmitDefaultValue = false)]
        public string Task { get; set; }
        
    }
}