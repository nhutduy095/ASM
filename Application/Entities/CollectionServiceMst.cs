using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionServiceMst:EntityBase
    {
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
    }
}
