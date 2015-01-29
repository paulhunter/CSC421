using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    using Model;
    public class BreadthFirstSearch
    {
        public static SearchResults Search(HexagonalTileSearchProblem Problem)
        {
            SearchResults r = new SearchResults();

            Queue<MapTile> Frontier = new Queue<MapTile>();
            Frontier.Enqueue(Problem.Start);

            //Storage of the Search Tiles mapped to the Tile that Led to Their Discovery.
            Dictionary<MapTile, MapTile> Paths = new Dictionary<MapTile, MapTile>();

            Dictionary<MapTile, bool> Explored = new Dictionary<MapTile, bool>((int)Problem.SearchSpace.Size);
            foreach(MapTile mt in Problem.SearchSpace.XYTiles())
            {
                Explored.Add(mt, false);
            }

            MapTile current = null;
            DateTime start_time = DateTime.Now;
            while(true)
            {
                if(Frontier.Count == 0)
                {
                    break;
                }
                if(Frontier.Count > r.SpaceComplexity)
                {
                    //We have a new maxmim number of nodes.
                    r.SpaceComplexity = Frontier.Count;
                }

                current = Frontier.Dequeue();
                Explored[current] = true; //Set the current node as explore. 
                r.TimeComplexity++; //We have explored another node.
                
                //SUCCESS!
                if(current == Problem.Goal)
                {
                    r.Solved = true;
                    break;
                }

                foreach(MapTile mt in current.GetNeighbours())
                {
                    if(!Frontier.Contains(mt) && Explored[mt] == false)
                    {
                        Paths.Add(mt, current);
                        Frontier.Enqueue(mt);
                        
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


            return r;
        }
    }
}
