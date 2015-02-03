using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    using Heuristics;
    using Model;
    public class AStarSearch : ISearchAlgorithm
    {
        IHeuristic Heuristic;

        public AStarSearch(IHeuristic Heuristic)
        {
            this.Heuristic = Heuristic;
        }

        public override string ToString()
        {
            return "Best First Search - " + Heuristic.ToString();
        }
        public SearchResults Search(HexagonalTileSearchProblem Problem)
        {
            SearchResults r = new SearchResults();
            if (Problem == null)
                return r;

            Dictionary<MapTile, MapTile> Paths = new Dictionary<MapTile, MapTile>();

            Dictionary<MapTile, bool> Explored = new Dictionary<MapTile, bool>((int)Problem.SearchSpace.Size);
            foreach(MapTile mt in Problem.SearchSpace.XYTiles())
            {
                Explored.Add(mt, false);
            }

            hSortedList<double, MapTile> Available = new hSortedList<double, MapTile>();
            Available.Add(0, Problem.Start);

            MapTile current = null;
            int neighbour_cost;
            DateTime start_time = DateTime.Now;
            while(Available.Count != 0)
            {
                if(Available.Count > r.SpaceComplexity)
                {
                    r.SpaceComplexity = Available.Count;
                }

                current = Available.Pop();
                neighbour_cost = SearchHelper.GetPathLengthFromStart(current, Paths, Problem.Start) + 1;
                
                r.TimeComplexity++; 

                Explored[current] = true;

                //If we have recieved the destination, we have completed the search.
                if(current == Problem.Goal)
                {
                    r.Solved = true;
                    break;
                }

                //If we have not found the destination, catalog the available operations
                //from this mapTile.
                foreach(MapTile mt in current.GetNeighbours())
                {
                    if (Explored[mt] == false)
                    {
                        //We have not previously seen this location. 
                        if (!Paths.ContainsKey(mt))
                        {
                            Paths.Add(mt, current);
                            Available.Add(
                                neighbour_cost
                                + Heuristic.Calculate(mt, Problem.Goal),
                                mt);
                        }
                        //We have seen this location before, so check if the path we are currently on 
                        //is cheaper than the last route take to this location. If it is shorter, we 
                        //overwrite the Paths value to connect the Location to our current location.
                        else if (neighbour_cost < SearchHelper.GetPathLengthFromStart(mt, Paths, Problem.Start))
                        {
                            Paths[mt] = current;
                            Available.Add(
                                neighbour_cost
                                + Heuristic.Calculate(mt, Problem.Goal),
                                mt);
                        }
                    }

                }
            }
            DateTime end_time = DateTime.Now;
            r.TimeInMilliseconds = (int)(end_time - start_time).TotalMilliseconds;

            if(r.Solved)
            {
                r.Path = SearchHelper.GetPathFromStart(current, Paths, Problem.Start);
            }

            return r;
        }
    }
}
