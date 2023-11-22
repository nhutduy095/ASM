using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    class CollectionUser : EntityBase
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string UserGroup { get; set; }
        public string UserType { get; set; }
        public bool IsLocked { get; set; }
    }
}
