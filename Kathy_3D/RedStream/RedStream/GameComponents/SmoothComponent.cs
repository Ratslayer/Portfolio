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
    public class SmoothComponent : GameComponent
    {
        public SmoothComponent(float speed, float mod)
            : base(RedStream.Game)
        {
            RedStream.Game.LoadComponent(this);
            Speed = speed;
            Mod = mod;
        }
        public float Speed, Mod;
        public bool bActive
        {
            get
            {
                return Amount != 1.0f;
            }
        }
        protected float Amount = 0;
        public override void Update(GameTime time)
        {
            Amount += Speed * (float)time.ElapsedGameTime.TotalSeconds;
            Amount = Math.Min(Amount, 1);
        }
        public void Delete()
        {
            RedStream.Game.DestroyComponent(this);
        }
    }
}
