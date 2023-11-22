using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities
{
    public class CollectionDepartment : EntityBase
    {
        public string CommonCd { get; set; }
        public string CommonNameEn { get; set; }
        public string CommonNameVi { get; set; }
        public string GroupCommon { get; set; }
    }
}
