using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionMarkDtl:EntityBase
    {
        public int MarkDtlId { get; set; }
        public int MarkId { get; set; }
        public string SubjectId { get; set; }
        public string Teacher {  get; set; }
        public decimal AveragePoints { get; set; }
        public string Season { get; set; }
    }
}
