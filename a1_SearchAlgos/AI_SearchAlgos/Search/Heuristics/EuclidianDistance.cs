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
            double r = Math.Sqrt(
                    Math.Pow((A.X - B.X), 2) + 
                    Math.Pow((A.Y - B.Y), 2));
            return r;
        }

        public override string ToString()
        {
            return "Euclidian Distance";
        }
    }
}
