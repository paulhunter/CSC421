﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    using Heuristics;
    using Model;
    public class BestFirstSearch
    {
        public static SearchResults Search(HexagonalTileSearchProblem Problem, IHeuristic Heuristic)
        {
            SearchResults r = new SearchResults();
            Dictionary<MapTile, MapTile> Paths = new Dictionary<MapTile, MapTile>();

            Dictionary<MapTile, bool> Explored = new Dictionary<MapTile, bool>((int)Problem.SearchSpace.Size);
            foreach(MapTile mt in Problem.SearchSpace.XYTiles())
            {
                Explored.Add(mt, false);
            }

            SortedList<double, MapTile> Available = new SortedList<double, MapTile>();
            Available.Add(0, Problem.Start);

            MapTile current = null;
            DateTime start_time = DateTime.Now;
            while(Available.Count != 0)
            {
                if(Available.Count > r.SpaceComplexity)
                {
                    r.SpaceComplexity = Available.Count;
                }

                current = Available.Values[0];
                Available.RemoveAt(0);
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
                    if(Explored[mt] == false)
                    {
                        Paths.Add(mt, current);
                        Available.Add(
                            SearchHelper.GetPathLengthFromStart(mt, Paths, Problem.Start)
                            + Heuristic.Calculate(mt, Problem.Goal),
                            mt);
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
