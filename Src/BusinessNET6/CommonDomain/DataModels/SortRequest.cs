using System;
using System.Collections.Generic;
using System.Text;

namespace CommonDomain.DataModels
{
    //
    // 摘要:
    //     Defines the sort descriptor.
    public class SortRequest
    {
        //     Gets the field name
        public string Name { get; set; }

        //     Gets the sort direction.
        public SortEnum Direction { get; set; }
    }

    public enum SortEnum
    {
        Ascending,
        Descending
    }
}
