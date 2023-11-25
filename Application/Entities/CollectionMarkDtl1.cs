using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionMarkDtl1:EntityBase
    {
        //public int Id { get; set; } 
        public int MarkDtlId { get; set; }
        public int MarkId { get; set; }
        public float AveragePoints { get; set; }
        public string Type { get; set; }
    }
}
