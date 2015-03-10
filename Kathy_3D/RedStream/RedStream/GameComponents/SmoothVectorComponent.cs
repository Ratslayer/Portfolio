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
    public class SmoothVectorComponent : SmoothComponent
    {
        public SmoothVectorComponent(Vector3 v, Vector3 desv, float speed, float mod)
            : base(speed, mod)
        {
            curV = v;
            desiredV = desv;
        }
        public Vector3 V
        {
            get
            {
                return Vector3.Lerp(curV, desiredV, Amount);
            }
            set
            {
                ChangeTo(value);
            }
        }
        private Vector3 curV, desiredV;
        private void ChangeTo(Vector3 v)
        {
            curV = desiredV;
            desiredV = v;
            ModV();
            Amount = 0;
        }
        private void ModV()
        {
            if (Mod > 0)
            {
                Vector3 v = new Vector3();
                v.X = Mod * (float)Math.Max(Math.Floor(curV.X / Mod), Math.Floor(desiredV.X / Mod));
                v.Y = Mod * (float)Math.Max(Math.Floor(curV.Y / Mod), Math.Floor(desiredV.Y / Mod));
                v.Z = Mod * (float)Math.Max(Math.Floor(curV.Z / Mod), Math.Floor(desiredV.Z / Mod));
                curV -= v;
                desiredV -= v;
            }
        }
    }
}
