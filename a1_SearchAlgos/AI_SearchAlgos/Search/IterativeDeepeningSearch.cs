using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    /// <summary>
    /// This class 
    /// </summary>
    class IterativeDeepeningSearch
    {
        public static SearchResults Search(HexagonalTileSearchProblem Problem)
        {
            //The result we will return from the search.
            SearchResults r = new SearchResults();
            //The results of the last iteration.
            SearchResults lastResult = null;

            DateTime start_time = DateTime.Now;
            uint currentDepthLimit;
            for(currentDepthLimit = 0; currentDepthLimit < Problem.SearchSpace.Size; currentDepthLimit++)
            {
                lastResult = BreadthFirstSearch.Search(Problem, currentDepthLimit);

                r.TimeComplexity += lastResult.TimeComplexity;
                if(lastResult.SpaceComplexity > r.SpaceComplexity)
                {
                    r.SpaceComplexity = lastResult.SpaceComplexity;
                }
                

                if(lastResult.Solved == true)
                {
                    r.Solved = true;
                    break;
                }
            }

            DateTime end_time = DateTime.Now;
            r.TimeInMilliseconds = (int)(end_time - start_time).TotalMilliseconds;

            if(r.Solved)
            {
                r.Path = new List<Model.MapTile>(lastResult.Path);
            }

            return r;
        }
    }
}
