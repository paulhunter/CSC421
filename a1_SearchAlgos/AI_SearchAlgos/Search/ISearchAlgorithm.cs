using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{
    public interface ISearchAlgorithm
    {
        SearchResults Search(HexagonalTileSearchProblem Problem);
    }
}
