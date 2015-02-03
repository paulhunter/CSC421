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
            return "A* Search - " + Heuristic.ToString();
        }
        public SearchResults Search(HexagonalTileSearchProblem Problem)
        {
            /* ----- SETUP ----- */
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

            /* ----- SEARCH ----- */
            MapTile current = null;
            int current_cost;
            DateTime start_time = DateTime.Now;
            while(Available.Count != 0)
            {
                if(Available.Count > r.SpaceComplexity)
                {
                    r.SpaceComplexity = Available.Count;
                }

                current = Available.Pop();
                current_cost = SearchHelper.GetPathLengthFromStart(current, Paths, Problem.Start);

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
                                current_cost + 1 //All nodes have a distance of one from their neighbour
                                + Heuristic.Calculate(mt, Problem.Goal),
                                mt);
                        }
                        else
                        {
                            int old_cost = SearchHelper.GetPathLengthFromStart(mt, Paths, Problem.Start);
                            MapTile oldParent = Paths[mt];
                            Paths[mt] = current;
                            int new_cost = SearchHelper.GetPathLengthFromStart(mt, Paths, Problem.Start);

                            //If the new cost to the tile is more than our previous
                            //path to this tile, we want to keep our previous parent assignment
                            if (new_cost > old_cost)
                            {
                                Paths[mt] = oldParent;
                            }
                            else
                            {
                                Available.Add(
                                    current_cost + 1 + 
                                    Heuristic.Calculate(mt, Problem.Goal),
                                    mt);
                            }
                        }
                    }
                }
            }
            DateTime end_time = DateTime.Now;
            r.TimeInMilliseconds = (int)(end_time - start_time).TotalMilliseconds;

            /* ----- BACKTRACK PATH GENERATION ----- */
            if(r.Solved)
            {
                r.Path = SearchHelper.GetPathFromStart(current, Paths, Problem.Start);
            }

            return r;
        }
    }
}
