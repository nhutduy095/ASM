using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionScheduleDtl:EntityBase
    {
        public int DtlId { get; set; }
        public string UserId { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Day { get; set; }
        public string Shift { get; set; }
        public string RoomId { get; set; }
        public string ClassId { get; set; }
        public string SubjectId { get; set; }
        public string TeacherId { get; set; }
    }
}
