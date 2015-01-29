using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AI_SearchAlgos.Model
{
    using Utils;
    /// <summary>
    /// A class which can be used to create parameterized instances of the Map class
    /// </summary>
    public class MapFactory
    {
        /// <summary>
        /// Build an instance of a Map which has a specified height and width, with each 
        /// of the tiles connected to each of its available neightbours. The PercentFreePaths
        /// parameter is then used to remove enough edges between tiles such that there are 
        /// floor(PercentFree * TotalEdges). 
        /// </summary>
        /// <param name="Height">Desired Height</param>
        /// <param name="Width">Desired Width</param>
        /// <param name="PercentFreePaths">A percent of free paths given as 0.0 to 1.0</param>
        /// <returns>A generated map.</returns>
        public static Map BuildMap(uint Width, uint Height, uint Obstacles)
        {
#if DEBUG
            DateTime now = DateTime.Now;
#endif
            Map result = new Map(Width, Height);
            if(Obstacles > result.EdgeCount)
            {
                Log.Critical("MapFactory: Obstacle count greater than total edges!");
                Obstacles = result.EdgeCount;
            }
            
            for(int a = 0; a < Obstacles; a++)
            {
                result.RemoveRandomEdge();
            }
#if DEBUG
            DateTime done = DateTime.Now;
            Utils.Log.Info(string.Format("MapFactory: Map took {0:0} milliseconds total to create.", (done-now).TotalMilliseconds));
#endif
            return result;
        }

    }
}
