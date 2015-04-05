using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search.Heuristics
{
    using Model;
    public class EuclidianDistance : IHeuristic
    {
        public EuclidianDistance() { }


        public double Calculate(MapTile A, MapTile B)
        {
            /* The adjusted Euclidian distance heuristic requires an 
             * offset to be admissable. It can be defined as 
             * sqrt(2) - 1, but we round to 0.5. */
            double r = Math.Sqrt(
                    Math.Pow((A.X - B.X), 2) + 
                    Math.Pow((A.Y - B.Y), 2)) - 0.5;
            return r;
        }

        public override string ToString()
        {
            return "Euclidian Distance";
        }
    }
}
