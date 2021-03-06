﻿using System;
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
        public int SpaceComplexity; 
        public bool Solved;
        public List<MapTile> Path;

        public SearchExecution Execution;

        public SearchResults()
        {
            TimeInMilliseconds = 0;
            TimeComplexity = 0;
            SpaceComplexity = 0;
            Solved = false;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", SpaceComplexity, TimeComplexity, TimeInMilliseconds);
        }

    }
}
