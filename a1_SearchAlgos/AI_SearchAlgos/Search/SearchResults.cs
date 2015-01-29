using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Search
{

    using Model;

    public class SearchResults
    {
        public int TimeInMilliseconds;
        public int TimeComplexity; //Nodes visited
        public int SpaceComplexity; //Nodes Examined?
        public bool Solved;
        public List<MapTile> Path;

        public SearchResults()
        {
            TimeInMilliseconds = 0;
            TimeComplexity = 0;
            SpaceComplexity = 0;
            Solved = false;
        }
        
    }
}
