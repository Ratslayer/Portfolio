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
namespace RedStream.Kathy
{
    public class LightBall : Actor
    {
        public LightSphere LightSphere;
        public RsLight Light;
        public LightBall(RsActor.Desc desc)
            : base(desc)
        {
            SmoothColor = new SmoothVectorComponent(new Vector3(1), new Vector3(1), 1, 0);
            RsActor.Desc adesc = new RsActor.Desc("LightSphere");
            LightSphere = new LightSphere(adesc);
            RsLight.Desc ldesc = new RsLight.Desc("PointLight");
            Light = new RsLight(ldesc);
            DefaultAttenuation = Light.Attenuation;
            SmoothAttenuation = new SmoothVectorComponent(DefaultAttenuation, DefaultAttenuation, 2, 0);
            ExplosionAttenuation = new Vector3(0, 0, 0);
        }
        public SmoothVectorComponent SmoothColor, SmoothAttenuation;
        public float DimCounter = 0, ExplosionRadius = 40;
        public Vector3 DefaultAttenuation, ExplosionAttenuation;
        public void Explode()
        {
            if (DimCounter > 0)
                return;
            LightSphere.Deadly = true;
            LightSphere.SmoothRadius.F = ExplosionRadius;
            //RedStream.Scene.ExplosionSystem.AddParticles(200, Pos, 1, new Vector3());
            DimCounter = 5;
            SmoothAttenuation.V = ExplosionAttenuation;
        }
        public override void Update(GameTime gameTime)
        {
            DimCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (DimCounter > 0)
            {
                if (!SmoothAttenuation.bActive)
                {
                    SmoothAttenuation.V = DefaultAttenuation;
                    SmoothColor.V = new Vector3(0.3f);
                }
                if (!LightSphere.SmoothRadius.bActive && LightSphere.Deadly)
                {
                    LightSphere.Deadly = false;
                    LightSphere.SmoothRadius.F = LightSphere.DefaultRadius;
                }
            }
            else
            {
                if (!SmoothColor.bActive && SmoothColor.V != new Vector3(1))
                    SmoothColor.V = new Vector3(1);
            }
            Material.Color = new Vector4(SmoothColor.V, 1);
            Light.Color = new Vector4(SmoothColor.V, 1);
            Light.Attenuation = SmoothAttenuation.V;
            LightSphere.Velocity = Velocity;
            LightSphere.Acceleration = Acceleration;
            LightSphere.Pos = Pos;
            Light.Pos = Pos;
            base.Update(gameTime);
        }
    }
}
