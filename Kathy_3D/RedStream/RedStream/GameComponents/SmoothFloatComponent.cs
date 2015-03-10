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
    public class SmoothFloatComponent : SmoothComponent
    {
        public SmoothFloatComponent(float curf, float desf, float speed, float mod)
            : base(speed, mod)
        {
            curF = curf;
            desiredF = desf;
            Speed = speed;
            Mod = mod;
        }
        public float F
        {
            get
            {
                return MathHelper.Lerp(curF, desiredF, Amount);
            }
            set
            {
                ChangeTo(value);
            }
        }
        private float curF, desiredF;
        private void ChangeTo(float f)
        {
            if (!bActive)
            {
                curF = desiredF;
                desiredF = f;
                if (Mod > 0)
                    ModF();
                Amount = 0;
            }
            else desiredF = f;
        }
        
        private void ModF()
        {
            float f = Mod * (float)Math.Max(Math.Floor(curF / Mod), Math.Floor(desiredF / Mod));
            curF -= f;
            desiredF -= f;
        }
        public void SafeSet(float f)
        {
            if (!bActive && f != F)
                F = f;
        }
    }
}
