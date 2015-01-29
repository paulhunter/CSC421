using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search.Heuristics
{
    using Model;
    public class TileDistanceHeuristic : IHeuristic
    {
        public TileDistanceHeuristic() { }

        public double Calculate(MapTile A, MapTile B)
        {
            int x0 = A.X - (A.Y - (A.Y & 1)) / 2;
            int z0 = A.Y;
            int y0 = (-1 * x0) - z0;
            int x1 = A.X - (B.Y - (B.Y & 1)) / 2;
            int z1 = B.Y;
            int y1 = (-1 * x1) - z1;
            return Math.Max(Math.Max(
                Math.Abs(x0 - x1),
                Math.Abs(y0 - y1)),
                Math.Abs(z1 - z0));
        }
    }
}
