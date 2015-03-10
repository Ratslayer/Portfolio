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
    public class Actor : RsActor
    {
        public Actor(RsActor.Desc desc)
            : base(desc)
        {
        }
        public float curDepth = 0;
        public Pit hitPit = null;
        public bool bLanded = false;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public void Jump()
        {
            if (Pos.Y > Radius-curDepth)
                return;
            if (curDepth == 0)
                Velocity = new Vector3(Velocity.X, 150, Velocity.Z);
            if(curDepth == GameInfo.pitDepth)
                Velocity = new Vector3(Velocity.X, 250, Velocity.Z);
            bLanded = false;
            RsContent.PlayCue("Step");
        }
        public virtual void HitPit(Pit pit)
        {
            curDepth = GameInfo.pitDepth;
            hitPit = pit;
        }
        public virtual void LandInPit(Pit pit)
        {
            bLanded = true;
        }
    }
}
