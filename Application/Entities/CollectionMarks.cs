using Application.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Application.Entities
{
    public class CollectionMarks:EntityBase
    {
        public int MarkId { get; set; }
        public string UserId { get; set; }
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal AveragePoints { get; set; }
        public int TotalCredit { get; set; }
        public int TotalPass { get; set; }
        public int TotalFail { get; set; }
        public int TotalStuding { get; set; }
        public int TotalNotYet { get; set; }
        public string ClassId { get; set; }
    }
}
