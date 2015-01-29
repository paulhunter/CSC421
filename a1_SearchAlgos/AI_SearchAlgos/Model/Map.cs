using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Model
{
    public class Map
    {
        //Tiles of the map stored by the x,y Coordinates. 
        private MapTile[,] _tiles;
        private uint _numberOfEdges;
        private uint _maxNumberOfEdges;

        /// <summary>
        /// Creates a map which has every possible connection established
        /// between each tile.
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public Map(uint Width, uint Height)
        {
            this._numberOfEdges = 0;
            this._tiles = new MapTile[Width,Height];
            this._width = Width;
            this._height = Height;
            
            //Instantiate every tile.
            int x, y;
            for(x = 0; x < Width; x++)
            {
                for(y = 0; y < Height; y++)
                {
                    _tiles[x, y] = new MapTile(x, y);
                }
            }

            //Connect each tile with its available neighbours
            for (x = 0; x < Width; x++)
            {
                for (y = 0; y < Height; y++)
                {
                    AddAllNeighbors(_tiles[x, y]);
                }
            }
            //Snag the max number of edges for statistics later.
            this._maxNumberOfEdges = _numberOfEdges;

        }

        private uint _height = 0;
        /// <summary>
        /// The height of the map in tiles.
        /// </summary>
        public uint Height
        {
            get
            {
                return _height;
            }   
        }

        private uint _width = 0;
        /// <summary>
        /// The width of the map in tiles.
        /// </summary>
        public uint Width
        {
            get
            {
                return _width;
            }
        }

        /// <summary>
        /// The size of the map in tiles. 
        /// </summary>
        public uint Size
        {
            get
            {
                return _height * _width;
            }
        }

        public uint EdgeCount
        {
            get
            {
                return _numberOfEdges;
            }
        }

        public double FreePathPercentage
        {
            get
            {
                return (_numberOfEdges) / (double)(_maxNumberOfEdges);
            }
        }

        private int[][] _neighbourSet;
        private int[][][] _neighbourCoordinates = new int [][][] {
            new int[][] { new int [] {+1, 0}, new int[] {0, -1}, new int[] {-1, -1}, 
                          new int [] {-1, 0}, new int [] {-1, +1}, new int [] {0 ,+1} }, //For those with an EVEN Y
            new int[][] { new int [] {+1, 0}, new int [] {+1, -1}, new int [] {0, -1}, 
                          new int [] {-1, 0}, new int [] {0, +1}, new int [] {+1 ,+1} }  //For those with an ODD Y
        };

        private void AddAllNeighbors(MapTile Target)
        {
            int xo, yo;
            //We find our neighbours based on the row (Y) being even or odd. 
            _neighbourSet = _neighbourCoordinates[Target.Y % 2];
            foreach(int[] offsetPair in _neighbourSet)
            {
                xo = Target.X + offsetPair[0];
                yo = Target.X + offsetPair[1];
                try
                {
                    MapTile n = _tiles[xo, yo];
                    Target.AddNeighbour(n);
                    _numberOfEdges += 1;
                }
                //this will occur when we are dealing with an edge tile, its a nominal exception.
                catch (IndexOutOfRangeException) { }
            }
        }

        //This method will arbitra
        public void RemoveRandomEdge()
        {

#if DEBUG
            DateTime now = DateTime.Now;
#endif
            Random r = new Random();
            int x, y, i;
            MapTile a, b;
            //attempt to remove a random edge until we are able to.
            while(true)
            {
                x = r.Next(0, (int)this._width);
                y = r.Next(0, (int)this._height);
                a = _tiles[x, y];
                if(a.Connections > 0)
                {
                    i = r.Next(0, a.Connections - 1);
                    b = a.Neighbours.ElementAt(i);
                    a.RemoveNeighbour(b);
                    b.RemoveNeighbour(a);
                    _numberOfEdges -= 1;
                    break;
                }
            }
#if DEBUG
            DateTime done = DateTime.Now;
            Utils.Log.Info(string.Format("Map: Took {0:0.000} milliseconds to remove random edge.", (done - now).TotalMilliseconds));
#endif
        }

        public void RemoveNeighbour(MapTile A, MapTile B)
        {


        }

    }
}
