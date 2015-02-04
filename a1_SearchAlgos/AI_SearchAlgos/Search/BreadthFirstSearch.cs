using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    using Model;
    using Utils;
    public class BreadthFirstSearch : ISearchAlgorithm
    {

        public override string ToString()
        {
            return "Breadth First Search";
        }
        public SearchResults Search(HexagonalTileSearchProblem Problem)
        {
            return Search(Problem, uint.MaxValue);
        }
        public SearchResults Search(HexagonalTileSearchProblem Problem, uint DepthLimit)
        {
            Queue<Tuple<MapTile, uint>> Frontier;
            Dictionary<MapTile, MapTile> Paths;
            Dictionary<MapTile, bool> Explored;

            SearchResults r = new SearchResults();
            if (Problem == null)
                return r;

            Frontier = new Queue<Tuple<MapTile, uint>>();
            Frontier.Enqueue(new Tuple<MapTile, uint>(Problem.Start, DepthLimit));

            //Storage of the Search Tiles mapped to the Tile that Led to Their Discovery.
            Paths = new Dictionary<MapTile, MapTile>();

            Explored = new Dictionary<MapTile, bool>((int)Problem.SearchSpace.Size);
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
                        try
                        {
                            Paths.Add(mt, current);
                        }
                        catch (ArgumentException)
                        {
                            Paths[mt] = current;
                        }
                        Frontier.Enqueue(new Tuple<MapTile, uint>(mt, currentDepth - 1));
                    }
                }
            }
            DateTime end_time = DateTime.Now;
            r.TimeInMilliseconds = (int)(end_time - start_time).TotalMilliseconds;

            //If we found a solution, find the path we actually discovered.
            if(r.Solved)
            {
                r.Path = SearchHelper.GetPathFromStart(current, Paths, Problem.Start);
            }

            Log.Info(string.Format("BFS: Search Complete - Solution {0}", r.Solved ? "Found" : "Not Found"));
            return r;
        }

        
    }
}
