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
