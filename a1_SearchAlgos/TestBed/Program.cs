using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBed
{
    using AI_SearchAlgos;
    using AI_SearchAlgos.Search;
    using AI_SearchAlgos.Utils;
    class Program
    {
        static void Main(string[] args)
        {
            Log.Start();
            SearchManager.RunTestSuite();
        }
    }
}
