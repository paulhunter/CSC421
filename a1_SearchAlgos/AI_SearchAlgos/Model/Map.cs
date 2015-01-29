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
        public MapTile[,] _tiles;
        public List<Tuple<MapTile, MapTile>> _edges;
        private uint _maxNumberOfEdges;

        /// <summary>
        /// Creates a map which has every possible connection established
        /// between each tile.
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public Map(uint Width, uint Height)
        {
#if DEBUG
            DateTime now = DateTime.Now;
#endif
            this._tiles = new MapTile[Width,Height];
            this._width = Width;
            this._height = Height;
            this._edges = new List<Tuple<MapTile, MapTile>>();
            //Instantiate every tile.
            int x, y;
            int i = 0;
            for(x = 0; x < Width; x++)
            {
                for(y = 0; y < Height; y++)
                {
                    _tiles[x, y] = new MapTile(x, y, i);
                    i++;
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
            this._maxNumberOfEdges = (uint)_edges.Count;
#if DEBUG
            DateTime done = DateTime.Now;
            Utils.Log.Info(string.Format("Map: Constructor took {0:0} milliseconds to create all connected map.", (done - now).TotalMilliseconds));
#endif
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
                return (uint)_edges.Count;
            }
        }

        public double FreePathPercentage
        {
            get
            {
                return (EdgeCount) / (double)(_maxNumberOfEdges);
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
                yo = Target.Y + offsetPair[1];
                try
                {
                    MapTile n = _tiles[xo, yo];
                    Target.AddNeighbour(n);
                    TrackEdge(Target, n);
                }
                //this will occur when we are dealing with an edge tile, its a nominal exception.
                catch (IndexOutOfRangeException) { }
            }
        }

        private void TrackEdge(MapTile A, MapTile B)
        {
            Tuple<MapTile, MapTile> t;
            if(A.X <= B.X || A.Y <= B.Y)
            {
                t = new Tuple<MapTile, MapTile>(A, B);
            }
            else
            {
                t = new Tuple<MapTile, MapTile>(B, A);
            }
            if(!this._edges.Contains(t))
            {
                this._edges.Add(t);
            }
        }

        //This method will arbitra
        public void RemoveRandomEdge()
        {

#if DEBUG
            DateTime now = DateTime.Now;
#endif
            Random r = new Random();
            int i = r.Next(0, _edges.Count);
            Tuple<MapTile, MapTile> mtp = _edges.ElementAt(i);
            mtp.Item1.Neighbours.Remove(mtp.Item2);
            mtp.Item2.Neighbours.Remove(mtp.Item1);
            _edges.RemoveAt(i);
            
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
