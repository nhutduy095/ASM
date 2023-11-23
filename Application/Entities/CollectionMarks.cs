using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionMarks:EntityBase
    {
        public int MarkId { get; set; }
        public string UserId { get; set; }
        public float AveragePoints { get; set; }
        public string ClassId { get; set; }
    }
}
