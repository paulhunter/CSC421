using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AI_SearchAlgos.Model
{
    public class MapTile
    {
        /// <summary>
        /// The Neighbours of the 
        /// </summary>
        private List<MapTile> _Neighbours;

        private int _x;
        private int _y;
        private int _id;

        public MapTile(int X, int Y, int ID)
        {
            this._x = X;
            this._y = Y;
            this._id = ID;
            _Neighbours = new List<MapTile>();
        }

        public void ResetTile()
        {
            _Neighbours = new List<MapTile>();
        }

        public int X
        {
            get
            {
                return _x;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
        }

        public int ID
        {
            get
            {
                return _id;
            }
        }

        public int Connections
        {
            get
            {
                return _Neighbours.Count;
            }
        }

        public MapTile Right
        {
            get
            {

                return this._Neighbours[0];
            }
        }

        public MapTile TopRight
        {
            get
            {
                return this._Neighbours[1];
            }
        }

        public MapTile TopLeft
        {
            get
            {
                return this._Neighbours[2];
            }
        }

        public MapTile Left
        {
            get
            {
                return this._Neighbours[3];
            }
        }

        public MapTile BottomLeft
        {
            get
            {
                return this._Neighbours[4];
            }
        }

        public MapTile BottomRight
        {
            get
            {
                return this._Neighbours[5];
            }
        }

        public IEnumerable<MapTile> GetNeighbours()
        {
            foreach (MapTile mt in _Neighbours)
            {
                yield return mt;
            }
        }

        public void AddNeighbour(MapTile Target, int direction)
        {
            if(!this._Neighbours.Contains(Target))
            {
                this._Neighbours.Add(Target);
            }
            else
            {
                Debug.Assert(false, "Side case hit!");
            }
            
        }

        public void RemoveNeighbour(MapTile Target)
        {
            if(_Neighbours.Contains(Target))
            {
                _Neighbours.Remove(Target);
            }
            else
            {
                Debug.Assert(false, "Invalid Operations");
            }
        }
    }
}
