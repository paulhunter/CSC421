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
        private MapTile[][] _tiles;



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
        /// Adds each of the neighbours available to the MapTile's 
        /// </summary>
        private void AddAllNeighbours(MapTile Target)
        {

        }
    }
}
