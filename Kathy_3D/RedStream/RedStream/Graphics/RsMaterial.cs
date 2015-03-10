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
    public class RsMaterial
    {
        public RsMaterial(RsMaterialDesc desc)
        {
            Shininess = desc.Shininess;
            Specular = desc.Specular;
            Color = desc.Color;
            FresnelBias = desc.FresnelBias;
            FresnelPower = desc.FresnelPower;
            FresnelScale = desc.FresnelScale;
            DiffuseFactor = desc.DiffuseFactor;
            Refractivity = desc.Refractivity;
            Bumpiness = desc.Bumpiness;
            Cull = desc.Cull;
            Lit = desc.Lit;
            CastsShadows = desc.CastsShadows;
            DiffuseMap = RedStream.Content.GetTexture(desc.DiffuseMapName);
            NormalMap = RedStream.Content.GetTexture(desc.NormalMapName);
            SpecularMap = RedStream.Content.GetTexture(desc.SpecularMapName);
            Colored = desc.Colored;
            Emissive = desc.Emissive;
        }
        public Texture2D DiffuseMap, NormalMap, SpecularMap;
        public float Shininess, Specular;
        public float FresnelBias, FresnelScale, FresnelPower;
        public float DiffuseFactor, Bumpiness;
        public bool Cull, Lit, CastsShadows, Colored, Emissive;
        public Vector3 Refractivity;
        public Vector4 Color;
    }
}
