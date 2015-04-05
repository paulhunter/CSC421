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
    /// The Map class is an abstraction of the tiles within a search space. 
    /// 
    /// </summary>
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
            Log.Info("Map: Instantiating Tiles...");
            this._tiles = new MapTile[Width,Height];
            this._width = Width;
            this._height = Height;
            this._edges = new List<Tuple<MapTile, MapTile>>();
            //Instantiate every tile.
            int x, y;
            int i = 0;

            for(y = 0; y < Height; y++)
            {
                for (x = 0; x < Width; x++)
                {
                    _tiles[x, y] = new MapTile(x, y, i);
                    i++;
                }
            }
            Log.Info("Map: Adding all neighbours...");
            //Connect each tile with its available neighbours
            AddNeighbours();
            //Snag the max number of edges for statistics later.
            this._maxNumberOfEdges = (uint)_edges.Count;
            Log.Info("Map: Done");
#if DEBUG
            DateTime done = DateTime.Now;
            Utils.Log.Info(string.Format("Map: Constructor took {0:0} milliseconds to create all connected map.", (done - now).TotalMilliseconds));
#endif

        }
        /// <summary>
        /// Add each of the neighbours of all the tiles in the map.
        /// </summary>
        private void AddNeighbours()
        {
            int x, y;
            for (y = 0; y < Height; y++)
            {
                for (x = 0; x < Width; x++)
                {

                    AddAllNeighbors(_tiles[x, y]);
                }
            }
        }

        public void Reset()
        {
            Log.Info("Map.Reset: Resetting Nodes...");
            foreach(MapTile mt in _tiles)
            {
                mt.ResetTile();
            }
            Log.Info("Map.Reset: Resetting Edges...");
            _edges = new List<Tuple<MapTile, MapTile>>();
            Log.Info("Map.Reset: Re-adding All Neighbours...");
            AddNeighbours();
            Log.Info("Map.Reset: Complete!");

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

        /// <summary>
        /// The number of edges present in the graph. Duplicates, 
        /// that is those formed by pairs like X,Y Y,X are not present. 
        /// The first 
        /// </summary>
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

        /// <summary>
        /// Retrieve a tile at the provided coordinates.
        /// </summary>
        public MapTile GetTile(uint X, uint Y)
        {
            Debug.Assert(X < this._width && Y < this._height, "Map.GetTile: Invalid Coordinates!");
            return this._tiles[X, Y];
        }

        /// <summary>
        /// Enumerate the Tiles of the Map by row. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MapTile> XYTiles()
        {
            int x, y;

            for(y = 0; y < this._height; y++)
            {
                for (x = 0; x < this._width; x++)
                {
                    yield return this._tiles[x,y];
                }
            }
        }

        private int[][] _neighbourSet;
        //Neighbour coordinate offsets are listed in the order, Right, TopRight, TopLeft, Left, BottomLeft, BottomRight
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
            int d = 0; //Indicator of direction.
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
                d++;
            }
        }

        /// <summary>
        /// Add an edge to the record of edges. This method will ensure only edges between 
        /// nodes are added once, with no duplicates formed by pairs like X,Y, and Y,X. 
        /// If the edge is already it will not be added a second time. 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        private void TrackEdge(MapTile A, MapTile B)
        {
            Tuple<MapTile, MapTile> t;
            Debug.Assert(A.ID != B.ID, "CYCLIC EDGES");
            if(A.ID < B.ID)
            {
                t = new Tuple<MapTile, MapTile>(A, B);
            }
            else
            {
                t = new Tuple<MapTile, MapTile>(B, A);
            }
            if(this._edges.FirstOrDefault(p => ( p.Item1 == t.Item1 && p.Item2 == t.Item2)) == null)
            {
                this._edges.Add(t);
            }
        }

        /// <summary>
        /// This method will arbitrarily remove an edge from the graph.
        /// </summary>
        public void RemoveRandomEdge()
        {

#if DEBUG
            DateTime now = DateTime.Now;
#endif
            Random r = new Random(Utils.RandomFix.GetSeed());
            int i = r.Next(0, _edges.Count);
            Tuple<MapTile, MapTile> mtp = _edges.ElementAt(i);
            mtp.Item1.RemoveNeighbour(mtp.Item2);
            mtp.Item2.RemoveNeighbour(mtp.Item1);
            _edges.RemoveAt(i);
            
#if DEBUG
            DateTime done = DateTime.Now;
            Utils.Log.Info(string.Format("Map: Took {0:0.000} milliseconds to remove random edge.", (done - now).TotalMilliseconds));
#endif
        }

    }
}
