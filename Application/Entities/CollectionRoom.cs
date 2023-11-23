using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionRoom:EntityBase
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }
        public bool IsLocked { get; set; }
        public string MajorType { get; set; }
    }
}
