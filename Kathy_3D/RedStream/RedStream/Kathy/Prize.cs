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
    public class Prize : Actor
    {
        public SmoothFloatComponent SmoothColor;
        public SmoothFloatComponent SmoothTime;
        public Prize(RsActor.Desc desc)
            : base(desc)
        {
            SmoothColor = new SmoothFloatComponent(1, 0, (1/60.0f), 0);
            Material.Color = new Vector4(1, 0, 0, 1);
        }
        public override void ReactToCollision(RsGameObject obj)
        {
            if (obj is Kathy)
            {
                GameInfo.Level++;
                GameInfo.BeginLevel();
                Die();
            }
            base.ReactToCollision(obj);
        }
    }
}
