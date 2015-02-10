using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AI_SearchAlgos.Model
{
    /// <summary>
    /// The MapTile class is an abstraction of a single Hexagonal tile within
    /// our search space. It provides methods to access and 
    /// </summary>
    public class MapTile
    {
        private List<MapTile> _Neighbours; //Neighbours of this tile
        private int _x; //X coordinate on the offset grid. 
        private int _y; //Y coordinate on the offset grid. 
        private int _id; //Unique id on the grid provided by creator. 

        /// <summary>
        /// Constructor. Create a new MapTile with coordiantes X,Y and
        /// an ID. 
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        /// <param name="ID">0-based Unique ID</param>
        public MapTile(int X, int Y, int ID)
        {
            this._x = X;
            this._y = Y;
            this._id = ID;
            _Neighbours = new List<MapTile>();
        }

        //Hide the default constructor.
        private MapTile() { }

        /// <summary>
        /// Reset the tile to its first constructed state, 
        /// connected to none of its neighbours. 
        /// </summary>
        public void ResetTile()
        {
            _Neighbours = new List<MapTile>();
        }

        /// <summary>
        /// The X coordinate of the tile within the grid. 
        /// </summary>
        public int X
        {
            get
            {
                return _x;
            }
        }

        /// <summary>
        /// The Y coordinate of the tile within the grid.
        /// </summary>
        public int Y
        {
            get
            {
                return _y;
            }
        }

        /// <summary>
        /// The unique ID of the tile within the search space.
        /// </summary>
        public int ID
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// The number of active neighbours to this tile.
        /// </summary>
        public int Connections
        {
            get
            {
                return _Neighbours.Count;
            }
        }

        /// <summary>
        /// Remove an enumerable list of the Neighbours available to 
        /// this tile. Neighbours are not providing in any specific ourder.
        /// </summary>
        /// <returns>
        /// An enumerator for the neighbour tiles available to this tile.
        /// </returns>
        public IEnumerable<MapTile> GetNeighbours()
        {
            foreach (MapTile mt in _Neighbours)
            {
                yield return mt;
            }
        }

        /// <summary>
        /// Add a neighbour to the tile, making it adjacent within the
        /// search space. 
        /// </summary>
        /// <remarks>
        /// Ensure you also call AddNeighbour on the Target unless
        /// you wish to create unidirectional connections between tiles.
        /// </remarks>
        /// <param name="Target">Neighbour to add to this tile.</param>
        public void AddNeighbour(MapTile Target)
        {
            //Don't allow the same neighbour to be added more than once.
            if(!this._Neighbours.Contains(Target))
            {
                this._Neighbours.Add(Target);
            }
        }

        /// <summary>
        /// Remove a neighbour from this time. This will remove the path
        /// between the provided Tile and this one if the path exists. 
        /// </summary>
        /// <remarks>
        /// Ensure you call the RemoveNeighbour on the Target as well
        /// unless you wish to create unidirectional connections.
        /// </remarks>
        /// <param name="Target">Neighbour to remove.</param>
        public void RemoveNeighbour(MapTile Target)
        {
            if(_Neighbours.Contains(Target))
            {
                _Neighbours.Remove(Target);
            }
        }
    }
}
