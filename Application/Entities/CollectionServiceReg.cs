using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionServiceReg:EntityBase
    {
        public int DtlID { get; set; }
        public string ServiceId { get; set; }
        public string Requester { get; set; }
        public string RequestDate { get; set; }
        public string ConfirmBy { get; set; }
        public string ConfirmDate { get; set; }
        public string FinishConfirmDate { get; set; }
        public string RejectType { get; set; } 
        public string Remark { get; set;}
        public string Remark1 { get; set; }
        public string SubjectId { get; set; }
        public string MajorFrom { get; set; }
        public string MajorTo { get; set; }
        public string ReciveDate { get; set; }
        public string Status { get; set; }//N:New,A:Approve,W:Wait,R:reject,C:Cancel
    }
}
