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
    public class RsLight : RsGameObject
    {
        public new class Desc : RsGameObject.Desc
        {
            public Desc()
            {
                Color = new Vector4(1,1,1,1);
                Attenuation = new Vector3(1,0,0);
                _at = Vector3.Zero;
                Type = RsLightDesc.LightType.Spot;
                Radius = 1000.0f;
                Focus = Vector3.Zero;
                OrbitVel = Vector3.Zero;
                KeepFocus = true;
            }
            public Desc(string name)
                : base()
            {
                RsLightDesc desc;
                desc = (RsLightDesc)RedStream.Content.GetObjectAttributes("\\Descriptions\\" + name);
                Color = desc.Color;
                At = desc.At;
                Attenuation = desc.Attenuation;
                Direction = desc.Direction;
                Type = desc.Type;
                Radius = desc.Radius;
                Focus = desc.Focus;
                OrbitVel = desc.OrbitVel;
                KeepFocus = desc.KeepFocus;
            }

            public Vector4 Color;
            public Vector3 Attenuation;
            public Vector3 Direction { get { return _at - Pos; } set { _at = Pos + value; } }
            public Vector3 At { get { return _at; } set { _at = value; } }
            public Vector3 Focus, OrbitVel;
            public bool KeepFocus;
            public RsLightDesc.LightType Type;
            public float Radius;
            private Vector3 _at;
        }
        public Vector4 Color;
        public Vector3 Direction, Attenuation;
        public Vector3 At { get { return Pos + Direction; } set { Direction = value - Pos; } }
        public Vector3 Focus, OrbitVel;
        public RsLightDesc.LightType LightType;
        public bool KeepFocus;
        public RsLight(Desc desc) 
            : base(desc)
        {
            Color = desc.Color;
            Attenuation = desc.Attenuation;
            Direction = desc.Direction;
            LightType = desc.Type;
            Radius = desc.Radius;
            Focus = desc.Focus;
            OrbitVel = desc.OrbitVel;
            KeepFocus = desc.KeepFocus;
        }
        public RsCamera GetCamera(int cubeFace)
        {
            RsCamera.Desc desc = new RsCamera.Desc();
            desc.Pos = Pos;
            desc.At = At;
            desc.FieldOfView = 90;
            desc.Up = At == new Vector3(0, 1, 0) ? new Vector3(0, 0, 1) : new Vector3(0, 1, 0);
            return new RsCamera(desc);
        }
        public override void Update(GameTime gameTime)
        {
            if (OrbitVel != Vector3.Zero)
            {
                Vector3 dir = Focus - Pos;
                Vector3 norm = Vector3.Cross(dir, OrbitVel);
                norm.Normalize();
                Pos += norm * OrbitVel.Length();
                if (KeepFocus)
                    At = Focus;
            }
            base.Update(gameTime);
        }
        protected override void SetRadius(float rad)
        {
            base.SetRadius(rad);
        }
    }
}
