using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionUserInfo : EntityBase
    {
        public string UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool Sex { get; set; }
        public string UserType { get; set; }
        public string Birthday { get; set; }
        public string IdClass { get; set; }
        public string PhoneNumber { get; set; }
        public string MailAddress { get; set; }
        public string Address { get; set; }
        public string ParentsPhoneNumber { get; set; }
    }
}
