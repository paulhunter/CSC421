using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AI_SearchAlgos.Model
{
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
        /// <returns></returns>
        public static Map BuildMap(uint Height, uint Width, double PercentFree)
        {
            Debug.Assert(PercentFree >= 0.0 && PercentFree <= 1.0, "PercentFree not in valid range!");
            return null;
        }

    }
}
