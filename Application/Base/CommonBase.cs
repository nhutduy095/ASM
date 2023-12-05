using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Base
{
    public class CommonBase
    {
        public static string fnGertDateTimeNow()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
