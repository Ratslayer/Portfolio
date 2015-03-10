using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace RedStream.Kathy
{
    public class Pit
    {
        public BoundingBox box;
        public Dweller Dweller;
        public Vector3 Center
        {
            get
            {
                return (box.Min + box.Max) / 2;
            }
            set { }
        }

    }
}
