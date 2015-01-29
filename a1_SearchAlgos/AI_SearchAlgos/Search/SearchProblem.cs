using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    using Model;
    /// <summary>
    /// The search problem class describes an instance of the graph search problem for a 
    /// hexagonal grid. 
    /// </summary>
    public class HexagonalTileSearchProblem
    {
        MapTile Start;
        MapTile Goal;
        Map SearchSpace;
        private Random r;

        public HexagonalTileSearchProblem(uint Width, uint Height, double FreePathPercentage)
        {
#if DEBUG
            DateTime now = DateTime.Now;
#endif
            r = new Random();
            uint x, y;
            SearchSpace = MapFactory.BuildMap(Width, Height, 12);

            do
            {
                x = (uint)r.Next(0, (int)Width); y = (uint)r.Next(0, (int)Height);
                Start = SearchSpace.GetTile(x, y);
            } while (Start.Connections == 0);

            do
            {
                x = (uint)r.Next(0, (int)Width); y = (uint)r.Next(0, (int)Height);
                Goal = SearchSpace.GetTile(x, y);
            } while (Goal.Connections == 0 || Goal == Start);
#if DEBUG
            DateTime done = DateTime.Now;
            Utils.Log.Info(string.Format("HexagonalTileSearchProblem: {0:0} milliseconds total to create problem instance [w:{1}, h:{2}, p:{3}].", (done - now).TotalMilliseconds));
#endif
        }

    }
}
