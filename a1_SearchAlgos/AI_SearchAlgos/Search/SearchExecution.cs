using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    /// <summary>
    /// SearchExecution is a class which contains each of the steps that occurred during a
    /// 
    /// </summary>
    public class SearchExecution
    {
        public uint TotalSteps;
        public List<TileState[]> TileStates;

        public enum TileState
        {
            UNTOUCHED,
            IN_FRONTIER,
            EXPLORED, 
            CURRENT,
        }
    }
}
