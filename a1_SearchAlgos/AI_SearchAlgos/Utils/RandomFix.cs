using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Utils
{
    class RandomFix
    {
        static Random r = new Random();
        public static int GetSeed()
        {
            return r.Next();
        }
    }
}
