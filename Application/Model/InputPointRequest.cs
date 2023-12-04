using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Model
{
    public class InputPointRequest
    {
        public string _Id { get; set; }
        public int MarkId { get; set; }
        public int MarkDtlId { get; set; }
        public string StudentId { get; set; }
        public string SubjectId { get; set; }
        public string MajorId { get; set; }
        public string DeptId { get; set; }
        public decimal PointDiligence { get; set; }
        public decimal PointASM { get; set; }
        public decimal PointProtect { get; set; }
        public decimal AveragePoints { get; set; }
        public string Season { get; set; }
    }
}
