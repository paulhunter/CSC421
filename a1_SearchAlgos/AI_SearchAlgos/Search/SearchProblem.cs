using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    using Model;
    using Utils;
    /// <summary>
    /// The search problem class describes an instance of the graph search problem for a 
    /// hexagonal grid. 
    /// </summary>
    public class HexagonalTileSearchProblem
    {
        public MapTile Start;
        public MapTile Goal;
        public Map SearchSpace;
        private Random r;
        private int obstacles;

        public HexagonalTileSearchProblem(uint Width, uint Height, double FreePathPercentage)
        {
#if DEBUG
            DateTime now = DateTime.Now;
#endif
            Log.Info(string.Format("SearchProblem: Creating new Instance of  [w:{0}, h:{1}, p:{2:0.000}]", Width, Height, FreePathPercentage));
            r = new Random();
            uint x, y;
            Log.Info("SearchProblem: Generating Map...");
            SearchSpace = new Map(Width, Height);
            obstacles = (int)SearchSpace.EdgeCount - (int)Math.Floor(FreePathPercentage*SearchSpace.EdgeCount);
            Log.Info("SearchProblem: Creating Obstacles...");
            CreateObstacles();
            Log.Info("SearchProblem: Selecting Random Start/Goal");
            SelectStartAndGoal();

#if DEBUG
            DateTime done = DateTime.Now;
            Utils.Log.Info(string.Format("HexagonalTileSearchProblem: {0:0} milliseconds total to create problem instance [w:{1}, h:{2}, p:{3:0.000}].", (done - now).TotalMilliseconds, SearchSpace.Width, SearchSpace.Height, SearchSpace.FreePathPercentage));
#endif
        }

        private void SelectStartAndGoal()
        {
            uint x, y;
            do
            {
                x = (uint)r.Next(0, (int)SearchSpace.Width); y = (uint)r.Next(0, (int)SearchSpace.Height);
                Start = SearchSpace.GetTile(x, y);
            } while (Start == null); //Start.Connections == 0);

            do
            {
                x = (uint)r.Next(0, (int)SearchSpace.Width); y = (uint)r.Next(0, (int)SearchSpace.Height);
                Goal = SearchSpace.GetTile(x, y);
            } while (Goal == Start); // || Goal.Connections == 0);
        }

        private void CreateObstacles()
        {
            for (int a = 0; a < obstacles; a++)
            {
                SearchSpace.RemoveRandomEdge();
            }
        }

        public void Reset()
        {
            this.SearchSpace.Reset();
            this.CreateObstacles();
            this.SelectStartAndGoal();
        }

    }
}
