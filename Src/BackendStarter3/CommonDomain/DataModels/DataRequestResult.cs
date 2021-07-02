using System;
using System.Collections.Generic;
using System.Text;

namespace CommonDomain.DataModels
{
    //
    // 摘要:
    //     Defines the members of the data manager operation result.
    //
    // 類型參數:
    //   T:
    //     Type of the data source element.
    public class DataRequestResult<T>
    {
        //     Gets the result of the data operation.
        public IEnumerable<T> Result { get; set; }
        //     Gets the total count of the records in data source.
        public int Count { get; set; }
    }
}
