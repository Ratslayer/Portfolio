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
using RedStream.Core;
namespace RedStream
{
    public class RsGyro : RsActor
    {
        public List<RsSocket> Sockets;
        public RsGyroscope Owner;
        private Plane Plane=new Plane();
        private BoundingSphere InternalSphere = new BoundingSphere();
        private float BrokenCounter = 0;
        public bool Active
        {
            get { return BrokenCounter <= 0; }
            private set { }
        }
        public RsGyro(RsActor.Desc desc, RsGyroscope owner)
            : base(desc)
        {
            Owner = owner;
            Sockets = new List<RsSocket>(Owner.MaxTowersPerGyro);
            LoadSockets();
        }
        public void Break()
        {
            BrokenCounter = 3;
        }
        public override void Update(GameTime gameTime)
        {
            BrokenCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (RsSocket socket in Sockets)
                if (socket.Building != null)
                    socket.Building.Weapon.Active = Active;
            Plane.Normal = Vector3.Transform(Vector3.Up, Orientation);
            InternalSphere.Radius = BoundingSphereOnlyForAsenicsUse.Radius/RsGameInfo.Gyroscope.GyroScale;
            foreach (RsSocket socket in Sockets)
            {
                Vector3 pos = Vector3.Transform(socket.LocalPos, Orientation);
                socket.Pos = Pos + pos;
            }
            base.Update(gameTime);
        }
        private void LoadSockets()
        {
            RsMaterialDesc mdesc = new RsMaterialDesc();
            mdesc.Color = Material.Color;
            mdesc.DiffuseMapName = "BlueMarbleSlabs_Diffuse";
            mdesc.NormalMapName = "BlueMarbleSlabs_Normal";
            RsActor.Desc adesc = new RsActor.Desc();
            adesc.ModelName = "Sphere";
            adesc.MaterialDesc = mdesc;
            //create sockets
            float radius = BoundingSphereOnlyForAsenicsUse.Radius;
            for (int i = 0; i < Owner.MaxTowersPerGyro; i++)
            {
                RsSocket.Desc sdesc = new RsSocket.Desc();
                float angle = (float)i * (float)(Math.PI * 2) / (float)Owner.MaxTowersPerGyro;
                Vector3 pos = new Vector3((float)Math.Cos(angle), 0, (float)Math.Sin(angle));
                pos *= radius;
                sdesc.LocalPos = pos;
                sdesc.Owner = this;
                Sockets.Add(new RsSocket(sdesc));
            }
        }
        public bool HitsGyro(ref BoundingSphere sphere)
        {
            PlaneIntersectionType intersects;
            Plane.Intersects(ref sphere, out intersects);
            ContainmentType contains;
            BoundingSphereOnlyForAsenicsUse.Contains(ref sphere, out contains);
            return intersects == PlaneIntersectionType.Intersecting 
                && contains != ContainmentType.Contains;
        }
    }
}
