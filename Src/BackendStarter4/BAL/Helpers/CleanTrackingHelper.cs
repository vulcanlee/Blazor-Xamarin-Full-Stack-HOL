using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Helpers
{
    using Domains.Models;
    using Microsoft.EntityFrameworkCore;
    public class CleanTrackingHelper
    {
        public static void Clean<T>(BackendDBContext context) where T : class
        {
            foreach (var fooXItem in context.Set<T>().Local)
            {
                context.Entry(fooXItem).State = EntityState.Detached;
            }
        }
    }
}
