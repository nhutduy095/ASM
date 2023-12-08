using Application.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal AveragePoints { get; set; }
        public string Season { get; set; }
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PointDiligence { get; set; }
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PointASM { get; set; }
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PointProtect { get; set; }
    }
}
