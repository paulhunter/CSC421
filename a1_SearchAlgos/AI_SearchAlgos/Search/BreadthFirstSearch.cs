using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    using Model;
    using Utils;
    public class BreadthFirstSearch
    {
        public static SearchResults Search(HexagonalTileSearchProblem Problem)
        {
            return Search(Problem, uint.MaxValue);
        }
        public static SearchResults Search(HexagonalTileSearchProblem Problem, uint DepthLimit)
        {
            SearchResults r = new SearchResults();

            Queue<Tuple<MapTile, uint>> Frontier = new Queue<Tuple<MapTile, uint>>();
            Frontier.Enqueue(new Tuple<MapTile, uint>(Problem.Start, DepthLimit));

            //Storage of the Search Tiles mapped to the Tile that Led to Their Discovery.
            Dictionary<MapTile, MapTile> Paths = new Dictionary<MapTile, MapTile>();

            Dictionary<MapTile, bool> Explored = new Dictionary<MapTile, bool>((int)Problem.SearchSpace.Size);
            foreach(MapTile mt in Problem.SearchSpace.XYTiles())
            {
                Explored.Add(mt, false);
            }

            MapTile current = null;
            uint currentDepth;
            DateTime start_time = DateTime.Now;
            while (Frontier.Count != 0)
            {
                if(Frontier.Count > r.SpaceComplexity)
                {
                    //We have a new maxmim number of nodes.
                    r.SpaceComplexity = Frontier.Count;
                }

                current = Frontier.Peek().Item1;
                currentDepth = Frontier.Dequeue().Item2;
                Explored[current] = true; //Set the current node as explore. 
                r.TimeComplexity++; //We have explored another node.
                
                //SUCCESS!
                if(current == Problem.Goal)
                {
                    r.Solved = true;
                    break;
                }

                if(currentDepth == 0)
                {
                    //We have reached the limit we are allowed to search on this branch.
                    break;
                }

                foreach(MapTile mt in current.GetNeighbours())
                {
                    if(Frontier.FirstOrDefault(p => p.Item1 == mt) == null && Explored[mt] == false)
                    {
                        Paths.Add(mt, current);
                        Frontier.Enqueue(new Tuple<MapTile, uint>(mt, currentDepth - 1));
                        
                    }
                }
            }
            DateTime end_time = DateTime.Now;
            r.TimeInMilliseconds = (int)(end_time - start_time).TotalMilliseconds;

            //If we found a solution, find the path we actually discovered.
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

            Log.Info(string.Format("BFS: Search Complete - Solution {0}", r.Solved ? "Found" : "Not Found"));
            return r;
        }

        
    }
}
