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
    public class RsSocket : RsActor
    {
        public new class Desc : RsActor.Desc
        {
            public int iSocket = 0;
        }
        public RsActor Tower=null;
        public int iSocket;
        public RsSocket(Desc desc)
            : base(desc)
        {
            iSocket = desc.iSocket;
        }
        public bool AttachTower(RsActor tower)
        {
            if (Tower == null)
            {
                Tower = tower;
                return true;
            }
            else return false;
        }
        public override void Update(GameTime gameTime)
        {
            Tower.Pos = Pos;
            base.Update(gameTime);
        }
    }
}
