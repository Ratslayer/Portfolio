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
    public class Kathy : Actor
    {
        public Kathy(RsActor.Desc desc)
            : base(desc)
        {
            curDepth = 0;
        }
        public float StunnedCounter = 0;
        public override void Update(GameTime gameTime)
        {
            StunnedCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector3 bottom = Pos - new Vector3(0, Radius, 0);
            base.Update(gameTime);
        }
        public bool Owned = false;
        public override void ReactToCollision(RsGameObject obj)
        {
            if (!Owned && (obj is Projectile || obj is Dweller))
            {
                GameInfo.Level = 1;
                GameInfo.BeginLevel();
                RsContent.PlayCue("Scream");
                Owned = true;
            }
            base.ReactToCollision(obj);
        }
        public override void LandInPit(Pit pit)
        {
            StunnedCounter = 3;
            RsContent.PlayCue("Laugh");
            base.LandInPit(pit);
        }
    }
}
