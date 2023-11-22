using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionCheckIO: EntityBase
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string RoomId { get; set; }
        public string ClassId { get; set; }
        public string SubjectId { get; set; }
    }
}
