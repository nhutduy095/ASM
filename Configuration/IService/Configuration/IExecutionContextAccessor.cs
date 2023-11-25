using System;
using System.Collections.Generic;
using System.Text;

namespace Application.IService.Configuration
{
    interface IExecutionContextAccessor
    {
        string UserId { get; }
        //string UserName { get; }

        //string DeptCd { get; }

        //string IsAdmin { get; }

        //string CorrelationId { get; }

        //bool IsAvailable { get; }
    }
}
