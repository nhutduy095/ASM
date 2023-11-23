using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionSchedule:EntityBase
    {
        public int ScheduleId { get; set; }
        public string UserId { get; set; }
    }
}
