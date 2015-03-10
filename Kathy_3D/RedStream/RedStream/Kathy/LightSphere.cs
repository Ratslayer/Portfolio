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
    public class LightSphere : RsActor
    {
        public LightSphere(RsActor.Desc desc)
            : base(desc)
        {
            Visible = false;
            DefaultRadius = Scale.X;
            SmoothRadius = new SmoothFloatComponent(DefaultRadius, DefaultRadius, 1, 0);
        }
        public float DefaultRadius;
        public SmoothFloatComponent SmoothRadius;
        public override void Update(GameTime gameTime)
        {
            if (SmoothRadius.bActive)
                Radius = SmoothRadius.F;
            base.Update(gameTime);
        }
        public bool Deadly = false;
        public override void ReactToCollision(RsGameObject obj)
        {
            if (obj is Kathy)
            {
                GameInfo.lastKathyLitPos = obj.Pos;
            }
            if (Deadly && obj is Dweller)
            {
                obj.Die();
            }
            base.ReactToCollision(obj);
        }
    }
}
