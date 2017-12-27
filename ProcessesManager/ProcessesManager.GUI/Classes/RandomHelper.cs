using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessesManager.GUI.Classes
{
    public static class RandomHelper
    {
        static Stopwatch _sw;

        public static Random Randomizer
        {
            get
            {
                return new Random((int)(_sw.ElapsedTicks % int.MaxValue));
            }
        }

        static RandomHelper()
        {
            _sw = Stopwatch.StartNew();
        }
    }
}
