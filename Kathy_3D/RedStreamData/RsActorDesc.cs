using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RedStream
{
    public class RsActorDesc : RsData
    {
        public RsActorDesc()
            : base()
        {
            Name = "";
            MaterialName = "DefaultMaterial";
            Scale = new Vector3(1, 1, 1);
            FaceVector = new Vector3(0, 0, 1);
            UpVector = new Vector3(0, 1, 0);
        }
        public string Name;
        public string ModelName;
        public string MaterialName;
        public Vector3 Scale, FaceVector, UpVector;
    }
}
