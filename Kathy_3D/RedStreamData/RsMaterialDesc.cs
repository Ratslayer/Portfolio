using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RedStream
{
    public class RsMaterialDesc : RsData
    {
        public RsMaterialDesc()
        {
            DiffuseMapName = NormalMapName = SpecularMapName = "";
            Shininess = 100;
            DiffuseFactor = 1;
            FresnelBias = 1;
            FresnelPower = FresnelScale = 0;
            Color = new Vector4(1, 1, 1, 1);
            Specular = 1;
            Bumpiness = 1;
            Cull = Lit = CastsShadows = Colored = true;
            Refractivity = new Vector3(1, 1, 1);
        }
        public string DiffuseMapName, NormalMapName, SpecularMapName;
        public float Shininess, Specular;
        public float FresnelBias, FresnelScale, FresnelPower;
        public float DiffuseFactor, Bumpiness;
        public bool Cull, Lit, CastsShadows, Colored, Emissive;
        public Vector4 Color;
        /// <summary>
        /// The refractivity vector that defines the refraction index for Red, Green and Blue.
        /// Values should be less than 1.0. Different values for X, Y and Z will result in Chromatic Dispersion.
        /// Suggested values are [0.95, 0.99].
        /// </summary>
        public Vector3 Refractivity;
    }
}
