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
    class RsLaser : RsActor
    {
        TimeSpan timer = TimeSpan.Zero;
        TimeSpan Life = TimeSpan.Zero;
        
        RsActor Origin;
        RsActor Target;
        Vector3 scale = new Vector3(0.1f, 0.1f, 1);
        public RsLaser(RsActor.Desc desc, RsActor origin, RsActor target, int lifeTime)
            : base(desc)
        {
            Origin = origin;
            Target = target;

            Life = timer = TimeSpan.FromMilliseconds(lifeTime);
            //RsContent.PlayCue("Laser");
        }
        public override void Update(GameTime gameTime)
        {

            Pos = (Origin.Pos + Target.Pos) / 2;

            FacePoint(Target.Pos);
            scale.Z = ((Origin.Pos - Target.Pos) /*/ 2*/).Length();

            Scale = scale;


            timer -= gameTime.ElapsedGameTime;

            if (timer <= TimeSpan.Zero)
            {
                Die();
            }

            base.Update(gameTime);
        }
    }
}
