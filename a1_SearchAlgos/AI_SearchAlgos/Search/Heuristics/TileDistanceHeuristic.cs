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
            /* The tile distance heuristic relies on a 3 axis
             * represntation of the horizontal map. Below we convert
             * our two axis system to the three axis system. 
             * 
             * Each axis of the three axis system, x y and z, can be through of as
             * one of the three directions one can travel from a tile. 
             * You can draw the three axis through three lines forming all six
             * paths. they would appear as follows. -- / \
             * 
             */
            int x0 = A.X - (A.Y - (A.Y & 1)) / 2;
            int z0 = A.Y;
            int y0 = (-1 * x0) - z0;
            int x1 = B.X - (B.Y - (B.Y & 1)) / 2;
            int z1 = B.Y;
            int y1 = (-1 * x1) - z1;
            /* With the conversin complete, we can find the distance as the maximum difference on any axis. */
            int m = Math.Max(Math.Max(
                Math.Abs(x0 - x1),
                Math.Abs(y0 - y1)),
                Math.Abs(z0 - z1));
            return m;
        }

        public override string ToString()
        {
            return "Tile Distance";
        }
    }
}
