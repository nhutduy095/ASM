using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionMajorDtl:EntityBase
    {
        public string MajorID { get; set; }
        public string SubjectID { get; set; }
        public bool IsLocked { get; set; }
        public string MajorType { get; set; }
    }
}
