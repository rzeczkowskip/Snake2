using System;

namespace Snake2
{
    public class DifficultyCalculator
    {
        public int Level { get; private set; } = 1;
        public int SleepTime { get; private set; } = 250;

        public void IncreaseLevel()
        {
            if (SleepTime == 1)
            {
                return;
            }
            
            Level += 1;
            SleepTime = Math.Max(SleepTime - (SleepTime / 2), 1);
        }
    }
}
