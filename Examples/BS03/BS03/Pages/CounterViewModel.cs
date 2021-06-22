using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BS03.Pages
{
    public class CounterViewModel
    {
        public int currentCount { get; set; }

        public void IncrementCount()
        {
            currentCount++;
        }
    }
}
