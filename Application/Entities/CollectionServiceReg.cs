using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionServiceReg:EntityBase
    {
        public int Id { get; set; }
        public string ServiceId { get; set; }
        public string Requester { get; set; }
        public string RequestDate { get; set; }
        public string Confirmby { get; set; }
        public string ConfirmDate { get; set; }
        public string RejectType { get; set; } 
        public string Remark { get; set;}
        public string Remark1 { get; set; }
    }
}
