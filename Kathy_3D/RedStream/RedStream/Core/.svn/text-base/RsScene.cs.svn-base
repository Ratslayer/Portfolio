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
    class RsScene
    {
        public RsScene()
        {
        }
        public RsCamera Camera;
        public ParticleSystem ExplosionSystem, SmokeSystem;
        public static int gameRunning = 0;
        public void Init()
        {
            ExplosionSystem = new ExplosionParticleSystem(RedStream.Game, RedStream.Game.Content);
            SmokeSystem = new SmokePlumeParticleSystem(RedStream.Game, RedStream.Game.Content);
            //RsGameInfo.Init();
            Kathy.GameInfo.Init();
        }
        public static void CreateExplosion(int numParticles, Vector3 pos, float scale, Vector3 vel)
        {
            RedStream.Scene.ExplosionSystem.AddParticles(numParticles, pos, scale, vel);
        }
    }
}
