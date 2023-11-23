using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionScheduleDtl:EntityBase
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string RoomId { get; set; }
        public string ClassId { get; set; }
        public string SubjectId { get; set; }
    }
}
