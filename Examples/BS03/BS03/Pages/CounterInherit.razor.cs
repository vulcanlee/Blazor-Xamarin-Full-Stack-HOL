using Microsoft.AspNetCore.Components;

namespace BS03.Pages
{
    public class CounterInheritBase : ComponentBase
    {
        public int currentCount = 0;

        public void IncrementCount()
        {
            currentCount++;
        }
    }
}
