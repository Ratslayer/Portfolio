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
            public RsActor Owner = null;
            public Vector3 LocalPos = new Vector3();
        }
        public RsTower Building { get; private set; }
        public RsActor Owner;
        public Vector3 LocalPos;
        public RsSocket(Desc desc)
            : base(desc)
        {
            Owner = desc.Owner;
            LocalPos = desc.LocalPos;
            Pos = LocalPos + Owner.Pos;
            Building = null;
        }
        public void AttachBuilding(RsTower building)
        {
             Building = building;
        }
        public override void Update(GameTime gameTime)
        {
            if (Building != null)
            {
                Building.Scale = new Vector3(1, 1, 1);
                Building.Pos = Pos;
                Building.FaceDirection(Pos);
            }
            base.Update(gameTime);
        }
    }
}
