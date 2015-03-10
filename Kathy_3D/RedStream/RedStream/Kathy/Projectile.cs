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
    public class Projectile : Actor
    {
        public Projectile(RsActor.Desc desc)
            : base(desc)
        {
        }
        public override void ReactToCollision(RsGameObject obj)
        {
            if (obj == GameInfo.Kathy)
            {
                Die();
            }
            base.ReactToCollision(obj);
        }
    }
}
