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
    public class RsGyro : RsActor
    {
        public List<RsSocket> Sockets;
        public RsGyroscope Owner;
        public RsGyro(RsActor.Desc desc, RsGyroscope owner)
            : base(desc)
        {
            Owner = owner;
            Sockets = new List<RsSocket>(Owner.MaxTowersPerGyro);
            for (int i = 0; i < Owner.MaxTowersPerGyro; i++)
            {
                RsSocket.Desc sdesc = new RsSocket.Desc();
                sdesc.iSocket = i;
                Sockets.Add(new RsSocket(sdesc));
            }
            RsMaterial.Desc mdesc = new RsMaterial.Desc();
            mdesc.Color = Material.Color;
            mdesc.DiffuseMapName = "BlueMarbleSlabs_Diffuse";
            mdesc.NormalMapName = "BlueMarbleSlabs_Normal";
            foreach (RsSocket socket in Sockets)
            {
                RsActor.Desc adesc = new RsActor.Desc();
                adesc.ModelName = "Sphere";
                adesc.MaterialDesc = mdesc;
                socket.Tower = new RsActor(adesc);
            }
        }
        public override void Update(GameTime gameTime)
        {
            int i = 0;
            float radius = BoundingSphere.Radius;
            foreach (RsActor socket in Sockets)
            {
                float angle = (float) i * (float)(Math.PI * 2) / (float) Owner.MaxTowersPerGyro;
                Vector3 pos = new Vector3((float)Math.Cos(angle), 0, (float)Math.Sin(angle));
                pos = Vector3.Transform(pos, Orientation);
                pos *= radius;
                socket.Pos = Pos + pos;
                i++;
            }
            base.Update(gameTime);
        }
    }
}
