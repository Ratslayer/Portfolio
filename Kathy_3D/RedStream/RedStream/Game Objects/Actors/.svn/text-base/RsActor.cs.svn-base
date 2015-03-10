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
    public class RsActor : RsGameObject
    {
        public new class Desc : RsGameObject.Desc
        {
            public Desc()
                : base()
            {
                Name = "";
                MaterialDesc = new RsMaterialDesc();
                Scale = new Vector3(1, 1, 1);
                FaceVector = new Vector3(0, 0, 1);
                UpVector = new Vector3(0, 1, 0);
            }

            public Desc(string name)
                : base()
            {
                RsActorDesc desc;
                desc = (RsActorDesc)RedStream.Content.GetObjectAttributes("\\Descriptions\\" + name);
                ModelName = desc.ModelName;
                MaterialDesc = (RsMaterialDesc)RedStream.Content.GetObjectAttributes("\\Materials\\" + desc.MaterialName);
                Scale = desc.Scale;
                FaceVector = desc.FaceVector;
                Name = desc.Name;
            }
            public string ModelName;
            public RsMaterialDesc MaterialDesc;
            public Vector3 Scale, FaceVector, UpVector;
        }
        public RsActor(Desc desc)
            : base(desc)
        {
            if (desc.ModelName != "" && desc.ModelName != null)
            {
                Model = RedStream.Content.GetModel(desc.ModelName);
            }
            else
            {
                Visible = false;
            }
            Material = new RsMaterial(desc.MaterialDesc);
            Scale = desc.Scale;
            FaceVector = desc.FaceVector;
            FaceVector.Normalize();
            UpVector = desc.UpVector;
            UpVector.Normalize();
        }

        public Model Model;
        public RsMaterial Material;
        public Vector3 Scale 
        { 
            get 
            { 
                return _scale; 
            } 
            set 
            { 
                _scale = value; 
                RecomputeRadius(); 
            } 
        }
        private Vector3 _scale;
        public Vector3 FaceVector, UpVector;
        public Vector3 Up
        {
            get
            {
                return Vector3.Transform(Vector3.Up, Orientation);
            }
            private set { }
        }
        public Vector3 Forward
        {
            get
            {
                return Vector3.Transform(FaceVector, Orientation);
            }
            private set { }
        }
        public bool Visible = true;
        protected void RecomputeRadius()
        {
            if (Model != null)
            {
                base.SetRadius((GetMeshRadius() * Math.Max(Math.Max(Scale.X, Scale.Y), Scale.Z)));                
            }
        }
        protected override void SetRadius(float rad)
        {
            if (Model != null)
                _scale *= rad / Radius;
            base.SetRadius(rad);
        }
        private float GetMeshRadius()
        {
            BoundingSphere meshSphere;
            float radius=0;
            foreach (ModelMesh mesh in Model.Meshes)
            {
                meshSphere = mesh.BoundingSphere;
                radius = Math.Max(radius, meshSphere.Center.Length() + meshSphere.Radius);
            }
            return radius;
        }
        public void FaceDirection(Vector3 v)
        {
            Orientation = GetQuaternion(FaceVector, v);
        }
        public void FacePoint(Vector3 p)
        {
            FaceDirection(p - Pos);
        }
        public void SetUp(Vector3 v)
        {
            Orientation = GetQuaternion(UpVector, v);
        }
        public void Orient(Quaternion quat)
        {
            Matrix mat = Matrix.CreateLookAt(Vector3.Zero, FaceVector, UpVector);
            Orientation = quat * Quaternion.CreateFromRotationMatrix(mat);
        }
        public static Quaternion GetQuaternion(Vector3 v, Vector3 dir)
        {
            v.Normalize();
            Vector3 normal = Vector3.Cross(v, dir);
            float length = normal.Length();
            if (length > 0.0f)
            {
                normal.Normalize();
                float angle = (float)Math.Atan2(length, Vector3.Dot(v, dir));
                return Quaternion.CreateFromAxisAngle(normal, angle);
            }
            else return Quaternion.Identity;
        }
    }
}
