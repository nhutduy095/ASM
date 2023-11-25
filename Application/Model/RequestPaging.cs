using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Model
{
    public class RequestPaging
    {
        public int? Page { get; set; }

        /// <summary>
        /// Count records of a page.
        /// </summary>
        public int? PerPage { get; set; }
    }
}
