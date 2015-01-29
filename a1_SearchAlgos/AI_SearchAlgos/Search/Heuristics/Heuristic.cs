using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search.Heuristics
{
    using Model;
    using Utils;
    public interface IHeuristic
    {
        /// <summary>
        /// Calculate the value of the admissable heuristic
        /// </summary>
        /// <param name="A">Start</param>
        /// <param name="B">Goal</param>
        /// <returns></returns>
        double Calculate(MapTile A, MapTile B);
    }
}
