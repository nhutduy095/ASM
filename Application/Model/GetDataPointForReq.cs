using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Model
{
    public class GetDataPointForReq:RequestPaging
    {
        public string userId { get; set; }
        public string subjectName { get; set; }
    }
}
