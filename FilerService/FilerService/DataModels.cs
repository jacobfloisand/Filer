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
    public class SearchData
    {
        //Number of links and files
        [DataMember(EmitDefaultValue = false)]
        public int NumFiles { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int NumLinks { get; set; }

        //File1 Data
        [DataMember(EmitDefaultValue = false)]
        public string F1Name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F1Date { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F1Class { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F1Unit { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F1Section { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F1Type { get; set; }

        //File2 Data
        [DataMember(EmitDefaultValue = false)]
        public string F2Name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F2Date { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F2Class { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F2Unit { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F2Section { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F2Type { get; set; }

        //File3 Data
        [DataMember(EmitDefaultValue = false)]
        public string F3Name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F3Date { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F3Class { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F3Unit { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F3Section { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F3Type { get; set; }

        //File4 Data
        [DataMember(EmitDefaultValue = false)]
        public string F4Name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F4Date { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F4Class { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F4Unit { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F4Section { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F4Type { get; set; }

        //File5 Data
        [DataMember(EmitDefaultValue = false)]
        public string F5Name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F5Date { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F5Class { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F5Unit { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F5Section { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string F5Type { get; set; }

        //Link1 Data
        [DataMember(EmitDefaultValue = false)]
        public string L1Name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L1Date { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L1Class { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L1Unit { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L1Section { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L1Type { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L1Address { get; set; }

        //Link2 Data
        [DataMember(EmitDefaultValue = false)]
        public string L2Name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L2Date { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L2Class { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L2Unit { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L2Section { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L2Type { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L2Address { get; set; }

        //Link3 Data
        [DataMember(EmitDefaultValue = false)]
        public string L3Name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L3Date { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L3Class { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L3Unit { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L3Section { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L3Type { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L3Address { get; set; }

        //Link4 Data
        [DataMember(EmitDefaultValue = false)]
        public string L4Name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L4Date { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L4Class { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L4Unit { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L4Section { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L4Type { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L4Address { get; set; }

        //Link5 Data
        [DataMember(EmitDefaultValue = false)]
        public string L5Name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L5Date { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L5Class { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L5Unit { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L5Section { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L5Type { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string L5Address { get; set; }
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