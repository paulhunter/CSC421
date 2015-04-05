using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    using Heuristics;
    using Model;
    public class BestFirstSearch : ISearchAlgorithm
    {
        IHeuristic Heuristic;

        public BestFirstSearch(IHeuristic Heuristic)
        {
            this.Heuristic = Heuristic;
        }

        private BestFirstSearch() { }

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

            /* ***** SEARCH ************ */
            /* In the best first search approach, each node is evaluated based
             * on an estimation of its distance from the goal. In a fashion similar
             * to the A* search, if a node has been view before, or is part of a path,
             * costs are compared and the cheaper used. */
            MapTile current = null;
            DateTime start_time = DateTime.Now;
            while (Available.Count != 0)
            {
                if (Available.Count > r.SpaceComplexity)
                {
                    r.SpaceComplexity = Available.Count;
                }

                current = Available.Pop();
                r.TimeComplexity++;

                Explored[current] = true;

                //If we have recieved the destination, we have completed the search.
                if (current == Problem.Goal)
                {
                    r.Solved = true;
                    break;
                }

                //If we have not found the destination, catalog the available operations
                //from this mapTile.
                foreach (MapTile mt in current.GetNeighbours())
                {
                    if (Explored[mt] == false)
                    {
                        if (!Paths.ContainsKey(mt))
                        {
                            Paths[mt] = current;
                            Available.Add(
                                Heuristic.Calculate(mt, Problem.Goal),
                                mt);
                        }
                        else
                        {
                            int old_cost = SearchHelper.GetPathLengthFromStart(mt, Paths, Problem.Start);
                            MapTile oldParent = Paths[mt];
                            Paths[mt] = current;
                            int new_cost = SearchHelper.GetPathLengthFromStart(mt, Paths, Problem.Start);
                            //If the new cost to the tile is more than our previous
                            //path to this tile, we want to keep our previous parent.
                            if (new_cost > old_cost)
                            {
                                Paths[mt] = oldParent;
                            }
                            else
                            {
                                Available.Add(
                                Heuristic.Calculate(mt, Problem.Goal),
                                mt);
                            }
                        }
                    }               
                }
            }
             
            DateTime end_time = DateTime.Now;
            r.TimeInMilliseconds = (int)(end_time - start_time).TotalMilliseconds;

            /* Backtrack for find path */
            if(r.Solved)
            {
                r.Path = SearchHelper.GetPathFromStart(current, Paths, Problem.Start);
            }

            return r;
        }
    }
}
