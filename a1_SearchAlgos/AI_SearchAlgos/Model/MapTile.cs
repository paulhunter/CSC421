using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Model
{
    public class MapTile
    {
        /// <summary>
        /// The Neighbours of the 
        /// </summary>
        public MapTile[] Neighbours;

        private int _x;
        private int _y;
        private int _id;

        public MapTile(int X, int Y, int ID)
        {
            this._x = X;
            this._y = Y;
            this._id = ID;
            Neighbours = new MapTile[6];
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
                return Neighbours.Count(p => p != null);
            }
        }

        public MapTile Right
        {
            get
            {
                return this.Neighbours[0];
            }
        }

        public MapTile TopRight
        {
            get
            {
                return this.Neighbours[1];
            }
        }

        public MapTile TopLeft
        {
            get
            {
                return this.Neighbours[2];
            }
        }

        public MapTile Left
        {
            get
            {
                return this.Neighbours[3];
            }
        }

        public MapTile BottomLeft
        {
            get
            {
                return this.Neighbours[4];
            }
        }

        public MapTile BottomRight
        {
            get
            {
                return this.Neighbours[5];
            }
        }

        public void AddNeighbour(MapTile Target, int direction)
        {
            this.Neighbours[direction] = Target;
        }

        public void RemoveNeighbour(MapTile Target)
        {
           for(int i = 0; i < 6; i++)
           {
               if(this.Neighbours[i] == Target)
               {
                   this.Neighbours[i] = null;
                   break;
               }
           }
        }
    }
}
