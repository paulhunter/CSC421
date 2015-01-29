using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    using Model;
    using Utils;

    public class DepthFirstSearch
    {
        public static SearchResults Search(HexagonalTileSearchProblem Problem)
        {
            return Search(Problem, uint.MaxValue);
        }

        public static SearchResults Search(HexagonalTileSearchProblem Problem, uint DepthLimit)
        {
            SearchResults r = new SearchResults();

            Stack<Tuple<MapTile, uint>> Frontier = new Stack<Tuple<MapTile, uint>>();
            Frontier.Push(new Tuple<MapTile, uint>(Problem.Start, DepthLimit));

            //Initialize the Path 
            Dictionary<MapTile, MapTile> Paths = new Dictionary<MapTile, MapTile>();

            //Initialize our explored Table.
            Dictionary<MapTile, bool> Explored = new Dictionary<MapTile, bool>((int)Problem.SearchSpace.Size);
            foreach(MapTile mt in Problem.SearchSpace.XYTiles())
            {
                Explored.Add(mt, false);
            }

            MapTile current = null;
            uint currentDepth;
            DateTime start_time = DateTime.Now;
            while(Frontier.Count != 0)
            {
                if(Frontier.Count > r.SpaceComplexity)
                {
                    r.SpaceComplexity = Frontier.Count;
                }
                current = Frontier.Peek().Item1;
                currentDepth = Frontier.Pop().Item2;

                Explored[current] = true;
                r.TimeComplexity++;

                if(current == Problem.Goal)
                {
                    r.Solved = true;
                    break;
                }

                if(currentDepth == 0)
                {
                    //If we have hit our limit in depth, we ignore the addition of the
                    //children nodes to our frontier.
                    continue;
                }
                
                foreach(MapTile mt in current.GetNeighbours())
                {
                    if(Explored[mt] == false)
                    {
                        Frontier.Push(new Tuple<MapTile, uint>(mt, currentDepth - 1));
                        Paths[mt] = current; //Remember which way we came from to reach this tile.
                    }
                }

            }
            DateTime end_time = DateTime.Now;
            r.TimeInMilliseconds = (int)(end_time - start_time).TotalMilliseconds;

            if(r.Solved)
            {
                r.Path = new List<MapTile>();
                while (current != Problem.Start)
                {
                    r.Path.Insert(0, current);
                    current = Paths[current];
                }
                r.Path.Insert(0, current);
            }

            Log.Info(string.Format("DFS: Search Complete - Solution {0}", r.Solved ? "Found" : "Not Found"));
            
            return r;
        }
    }
}
