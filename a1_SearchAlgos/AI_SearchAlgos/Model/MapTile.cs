using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SearchAlgos.Model
{
    public class MapTile
    {
        public List<MapTile> Neighbours;

        private int _x;
        private int _y;
        private int _id;

        public MapTile(int X, int Y, int ID)
        {
            this._x = X;
            this._y = Y;
            Neighbours = new List<MapTile>();
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
                return Neighbours.Count;
            }
        }

        public void AddNeighbour(MapTile Target)
        {
            this.Neighbours.Add(Target);
        }

        public void RemoveNeighbour(MapTile Target)
        {
            this.Neighbours.Remove(Target);
        }
    }
}
