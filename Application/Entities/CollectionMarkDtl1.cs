using Application.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionMarkDtl1:EntityBase
    {
        //public int Id { get; set; } 
        public int SubId { get; set; }
        public int MarkDtlId { get; set; }
        public int MarkId { get; set; }
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public float AveragePoints { get; set; }
        public string Type { get; set; }
    }
}
