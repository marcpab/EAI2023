using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.IDOC.Json
{
    public class JSegment
    {
        public string Segment_Name {  get; set; }
        public string Segment_Type { get; set; }
        public string Segment_RefType { get; set; }
        public string Segment_Number { get; set; }
        public string Segment_ParentNumber { get; set; }
        public string Segment_HierarchyLevel { get; set; }
    }
}
