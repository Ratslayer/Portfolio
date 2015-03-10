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
    public class RsCamera : GameComponent
    {
        public class Desc
        {
            public Desc()
            {
                At = new Vector3();
                Up = new Vector3(0,1,0);
                FarViewPlane = 10000.0f;
                NearViewPlane = 1.0f;
                AspectRatio = 1.0f;
                FieldOfView = 45.0f;
                MinZoom = 50;
                MaxZoom = 500;
            }
            public Vector3 Pos, At, Up;
            public float FarViewPlane, NearViewPlane, AspectRatio, FieldOfView;
            public float MaxZoom, MinZoom;
        }
        public Matrix Proj
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(
                    FieldOfView * (float)Math.PI / 180.0f,
                    AspectRatio,
                    NearViewPlane,
                    FarViewPlane);
            }
            private set { }
        }
        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(Pos, At, Up);
            }
            private set { }
        }
        public BoundingFrustum Frustum
        {
            get
            {
                return _frustum; 
            }
            private set { }
        }
        private BoundingFrustum _frustum;
        public RsCamera(Desc desc) 
            : base(RedStream.Game)
        {
            Pos = desc.Pos;
            At = desc.At;
            Up = desc.Up;
            FarViewPlane = desc.FarViewPlane;
            NearViewPlane = desc.NearViewPlane;
            AspectRatio = desc.AspectRatio;
            FieldOfView = desc.FieldOfView;
            MinZoom = desc.MinZoom;
            MaxZoom = desc.MaxZoom;
            _frustum = new BoundingFrustum(View * Proj);
        }
        public Vector3 Pos, At, Up;
        public float MaxZoom, MinZoom;
        public float FarViewPlane, NearViewPlane, AspectRatio, FieldOfView;
        public float Zoom
        {
            get { return (Pos-At).Length(); }
            set { SetZoom(value); }
        }
        public void UpdateFrustum()
        {
           _frustum = new BoundingFrustum(View * Proj);
        }
        public void Orbit(Vector3 rot)
        {
            Orbit(rot.X, rot.Y, rot.Z);
            UpdateFrustum();
        }
        public void Orbit(float yaw, float pitch, float roll)
        {
            Vector3 dir = Pos - At;
            float deg2Pi = (float) Math.PI / 180.0f;
            Quaternion quat = Quaternion.CreateFromYawPitchRoll(yaw * deg2Pi, pitch * deg2Pi, roll * deg2Pi);
            dir = Vector3.Transform(dir, quat);
            Pos = At + dir;
            //Up = Vector3.Transform(Vector3.Up, quat);
            UpdateFrustum();
        }
        public void Orbit(float yaw, float pitch)
        {
            Vector3 dir = Pos - At;
            float deg2Pi = (float)Math.PI / 180.0f;
            Vector3 axis = Vector3.Cross(dir, Up);
            axis.Normalize();
            Quaternion quat = Quaternion.CreateFromAxisAngle(axis, pitch * deg2Pi);
            Up = Vector3.Transform(Up, quat);
            quat *= Quaternion.CreateFromAxisAngle(Up, yaw * deg2Pi);
            dir = Vector3.Transform(dir, quat);
            Pos = At + dir;
        }
        private void SetZoom(float distance)
        {
            distance = MathHelper.Clamp(distance, MinZoom, MaxZoom);
            Vector3 dir = Pos - At;
            dir.Normalize();
            Pos = dir * distance + At;
            UpdateFrustum();
        }
        public bool CanSee(RsActor actor)
        {
            bool bCanSee;
            _frustum.Intersects(ref actor.BoundingSphereOnlyForAsenicsUse, out bCanSee);
            return bCanSee;
        }
    }
}
