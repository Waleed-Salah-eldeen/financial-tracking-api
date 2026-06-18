using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Common
{
    public enum ErrorType
    {
        None,
        Validation,
        NotFound,
        Unauthorized,
        Failure
    }
}
