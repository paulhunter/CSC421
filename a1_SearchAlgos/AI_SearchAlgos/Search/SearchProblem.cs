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
        private int obstacles;
        private double _intentededObstacles;
        private double _actualObstacles;
        private uint _iWidth;
        private uint _iHeight;

        public HexagonalTileSearchProblem(uint Width, uint Height, double ObstaclePercentage)
        {
            Log.Info(string.Format("SearchProblem: Creating new Instance of  [w:{0}, h:{1}, p:{2:0.000}]", Width, Height, ObstaclePercentage));
            _intentededObstacles = ObstaclePercentage;
            _iWidth = Width;
            _iHeight = Height;
        }

        public HexagonalTileSearchProblem Clone()
        {
            return new HexagonalTileSearchProblem(_iWidth, _iHeight, this._intentededObstacles);
        }

        public double IntendedObstaclePercentage
        {
            get
            {
                return _intentededObstacles * 100;
            }
          
        }

        public double ActualObstaclePercentage
        {
            get
            {
                return _actualObstacles * 100;
            }
           
        }

        public void SelectRandomStartAndGoal()
        {
            Random r = new Random(Utils.RandomFix.GetSeed());
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
            obstacles = (int)Math.Floor(_intentededObstacles*SearchSpace.EdgeCount);
            _actualObstacles = ( obstacles / (SearchSpace.EdgeCount * 1.0));
            for (int a = 0; a < obstacles; a++)
            {
                SearchSpace.RemoveRandomEdge();
            }
        }

        public void Reset()
        {
            if(this.SearchSpace == null)
            {
                this.SearchSpace = new Map(_iWidth, _iHeight);
            }
            this.SearchSpace.Reset();
            this.CreateObstacles();
            this.SelectRandomStartAndGoal();
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", SearchSpace.Size, _intentededObstacles);
        }

    }
}
