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
namespace RedStream
{
    public class RsShip : RsEnemy
    {
        public RsShip(RsActor.Desc desc, RsShipData shipData) 
            : base(desc, shipData)
        {
           
        }

        public override void Die()
        {
            RsScene.CreateExplosion(5, Pos, 0.4f, Velocity);
            RsGameInfo.ShipsLeft--;
            base.Die();
        }
    }
}
