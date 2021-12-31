using CommonDomain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonDomain.DataModels
{
    public struct VerifyRecordResult
    {
        public bool Success { get; set; }
        public ErrorMessageEnum MessageId { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
    }
}
