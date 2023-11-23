using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionMajor:EntityBase
    {
        public string MajorID { get; set; }
        public string MajorName { get; set;}
        public bool IsLocked { get; set; }
        public int NumberOfCredits { get; set; }
        public string MajorType { get; set; }
        public string DeptId { get; set; }
    }
}
