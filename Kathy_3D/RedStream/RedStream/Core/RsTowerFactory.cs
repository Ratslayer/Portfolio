using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedStream.Core
{
    public static class RsTowerFactory
    {

        public static RsTower CreateTower(string towerName, RsSocket socket)
        {
            /* Read the graphic attributes and the tower attributes */
            DescDataPair<RsTowerData> pair = new DescDataPair<RsTowerData>("Buildings\\" + towerName);

            RsTower tower = new RsTower(pair.Desc, socket, pair.Data);
            return tower;
        }
    }
}
