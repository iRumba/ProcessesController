using System;
using System.Diagnostics;

namespace ProcessesManager.GUI.Classes
{
    public static class RandomHelper
    {
        static Stopwatch _sw;

        public static Random Randomizer
        {
            get
            {
                // Так как числа генерируются почти одновременно, пришлось сделать такой хак. 
                return new Random((int)(_sw.ElapsedTicks % int.MaxValue));
            }
        }

        static RandomHelper()
        {
            _sw = Stopwatch.StartNew();
        }
    }
}
