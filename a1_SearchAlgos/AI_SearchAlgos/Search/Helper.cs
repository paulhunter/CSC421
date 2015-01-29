using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AI_SearchAlgos.Search
{
    using Model;
    using Utils;
    /// <summary>
    /// This class contains definitions of static functions which are commonly used by 
    /// each of the search algoriths. 
    /// </summary>
    public class SearchHelper
    {     
        /// <summary>
        /// Find the length of the path from the Current Tile to Start 
        /// using the parents relations provided in Paths. 
        /// </summary>
        /// <returns></returns>
        public static int GetPathLengthFromStart(MapTile Current, Dictionary<MapTile, MapTile> Paths, MapTile Start)
        {
            try
            {
                int d = 0;
                MapTile c = Current;
                while (c != Start)
                {
                    d++;
                    c = Paths[c];
                }
                return d;
            }
            catch
            {
                Debug.Assert(false, "SearchHelper.GetPathLengthFromStart: Invalid Parameters caused failure!");
            }
            return 0;
        }

        /// <summary>
        /// Finds and returns to path from the Start Tile to Target Tile.
        /// The first element will be the start tile, and can be iterated to find
        /// the path to the Target Tile.
        /// </summary>
        public static List<MapTile> GetPathFromStart(MapTile Target, Dictionary<MapTile, MapTile> Paths, MapTile Start)
        {
            List<MapTile> r = new List<MapTile>();
            MapTile current = Target;
            while (current != Start)
            {
                r.Insert(0, current);
                current = Paths[current];
            }
            r.Insert(0, current);
            return r;

        }
    }
}
