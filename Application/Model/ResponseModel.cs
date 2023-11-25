using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Model
{
    public class ResponseModel
    {
        public bool Status { get; set; } = true;
        public string ErrMessageCode { get; set; }
        public string ErrMessage { get; set; }
        public int TotalPage { get; set; } = 0;
        public dynamic Data { get; set; }
        public ResponseModel()
        {
            Status = true;
        }
        public ResponseModel(string errMessageCode, string errMessage)
        {
            Status = false;
            ErrMessageCode = errMessageCode;
            ErrMessage = errMessage;
        }
    }
}
