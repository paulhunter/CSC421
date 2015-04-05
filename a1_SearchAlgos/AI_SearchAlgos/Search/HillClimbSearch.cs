using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    using Model;
    using Heuristics;
    public class HillClimbSearch : ISearchAlgorithm
    {
        IHeuristic Heuristic;
        public HillClimbSearch(IHeuristic Heuristic)
        {
            this.Heuristic = Heuristic;
        }
        private HillClimbSearch() { }

        public override string ToString()
        {
            return "Hill Climb Search - " + Heuristic.ToString();
        }

        public SearchResults Search(HexagonalTileSearchProblem Problem)
        {
            /* ----- SETUP ----- */
            SearchResults r = new SearchResults();

            Dictionary<MapTile, MapTile> Paths = new Dictionary<MapTile, MapTile>();
            DateTime start_time = DateTime.Now;
            /* ----- SEARCH ----- */
            MapTile current = Problem.Start;
            double current_cost = Heuristic.Calculate(current, Problem.Goal);
            do
            {
                r.TimeComplexity++;
                /* Find neighour with the lowest h cost */
                MapTile best = null;
                double cost;
                double best_cost = 0;
                foreach(MapTile neighbour in current.GetNeighbours())
                {
                    cost = Heuristic.Calculate(neighbour, Problem.Goal);
                    if(cost < current_cost)
                    {
                        best = neighbour;
                        best_cost = cost;
                    }
                }

                /* No neighbour is better than the current tile, so it */
                if(best == null)
                {
                    break;
                }
                current_cost = best_cost;
                Paths.Add(best, current);
                current = best;
            } while (true);
            DateTime end_time = DateTime.Now;
            r.TimeInMilliseconds = (int)(end_time - start_time).TotalMilliseconds;
            //Regardless of whether we found the goal, we want to path to the
            //local maxima we did find. 
            r.Path = SearchHelper.GetPathFromStart(current, Paths, Problem.Start);

            if(current == Problem.Goal)
            {
                r.Solved = true;
            }
            return r;
        }
    }
}
