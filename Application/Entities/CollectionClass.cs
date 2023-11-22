using Application.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionClass: EntityBase
    {
        
        public string ClassId { get; set; }
        public string ClassName { get; set; }
        public string MajorID { get; set; }
        public string TeacherManage { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DeptId { get; set; }
        
    }
}
