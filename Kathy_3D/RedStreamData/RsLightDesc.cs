using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RedStream
{
    public class RsLightDesc : RsData
    {
        public enum LightType
        {
            Directional,
            Spot,
            Point
        };
        public Vector4 Color;
        public Vector3 Attenuation;
        public Vector3 Direction;
        public Vector3 At;
        public Vector3 Focus, OrbitVel;
        public bool KeepFocus;
        public RsLightDesc.LightType Type;
        public float Radius;
    }
}
