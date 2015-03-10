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
    public class RsPlanet : RsDamageableActor
    {
        public RsPlanet(RsActor.Desc desc, RsPlanetData data)
            : base(desc, data)
        {
            AngularVelocity = data.AngVel;
            DescDataPair<RsShieldData> pair = new DescDataPair<RsShieldData>("Planets\\Shield");
            Shield = new RsShield(pair.Desc, pair.Data, this);
            Sockets = new List<RsSocket>(MaxNumBuildings);
            LoadSockets();
        }
        public float BuildingRadius = 1;
        public int MaxNumBuildings=16;
        public List<RsSocket> Sockets;
        public RsShield Shield=null;
        public override void Update(GameTime gameTime)
        {
            float radius=BoundingSphereOnlyForAsenicsUse.Radius;
            foreach (RsSocket socket in Sockets)
            {
                socket.Pos = Vector3.Transform(socket.LocalPos, Orientation);
                socket.Pos.Normalize();
                socket.Pos *= radius;
                socket.Pos += Pos;
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
            float radius = Radius;
            for (int i = 0; i < MaxNumBuildings; i++)
            {
                RsSocket.Desc sdesc = new RsSocket.Desc();
                while (true)
                {
                    bool bExit=true;
                    sdesc.LocalPos = RsUtil.GetRandomVector(radius, radius);
                    BoundingSphere s1=new BoundingSphere(sdesc.LocalPos, BuildingRadius);
                    foreach (RsSocket socket in Sockets)
                    {
                        BoundingSphere s2 = new BoundingSphere(socket.LocalPos, BuildingRadius);
                        if (s1.Intersects(s2))
                        {
                            bExit = false;
                            break;
                        }
                    }
                    if (bExit)
                        break;
                }
                sdesc.Owner = this;
                Sockets.Add(new RsSocket(sdesc));
            }
        }
    }
}
