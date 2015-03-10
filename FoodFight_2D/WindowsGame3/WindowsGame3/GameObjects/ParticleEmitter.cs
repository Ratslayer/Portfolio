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
using System.Collections;
namespace WindowsGame3
{
    /// <summary>
    /// A particle emitter class that allows to radially spawn particles.
    /// </summary>
    class ParticleEmitter : GameObject
    {
        public float maxAngle, minAngle, maxDistance, minDistance, minScale, maxScale;
        public float emissionRate, fadeRate, moveSpeed, moveAcc, particleLifeTime;
        public bool bDieOnFade, bHoming, bAccelerate;
        public int nEmits;
        public GameObject target;
        public Vector2 emissionVel;
        private float totalTime;
        private List<GameObject> particles;
        public ParticleEmitter(Texture2D texture)
            : base(texture)
        {
            particles = new List<GameObject>();
            maxAngle = 360;
            minAngle = 0;
            maxDistance = 2;
            minDistance = 1;
            minScale = maxScale = 1;
            bDieOnFade = true;
            bHoming = true;
            bAccelerate = false;
            emissionRate = 1;
            fadeRate = 1;
            nEmits = 1;
            particleLifeTime = 1;
            moveSpeed = moveAcc = 1;
            emissionVel = new Vector2(0, 0);
        }
        public int GetNumParticles()
        {
            return particles.Count;
        }
        public void Emit()
        {
            for (int i = 0; i < nEmits; i++)
            {
                GameObject particle = new GameObject(texture);
                particle.color = color;
                particle.scale = GetRandomScale();
                particle.pos = GetRandomPos();
                particle.vel = emissionVel;
                particle.depth = depth;
                if (particleLifeTime > 0.0f)
                {
                    particle.bOnTimer = true;
                    particle.lifeTime = particleLifeTime;
                }
                particles.Add(particle);
            }
        }
        public void EmitParticles(float time)
        {
            totalTime+=time;
            if(emissionRate>0.0f)
            {
                float emissionTime=1.0f/emissionRate;
                while(totalTime>=emissionTime)
                {
                    Emit();
                    totalTime-=emissionTime;
                }
            }
            else if(emissionRate==0.0f)
            {
                Emit();
                emissionRate=-1.0f;
            }
        }
        public float GetRandom(float max, float min)
        {
            float domain = max - min;
            float d = (float)Game1.random.NextDouble() * domain + min;
            return d;
        }
        public Vector2 GetRandomPos()
        {
            float angle = GetRandom(maxAngle, minAngle)*(float)Math.PI/180;
            float distance = GetRandom(maxDistance, minDistance);
            Vector2 dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))*distance;
            dir += pos;
            return dir;
        }
        public Vector2 GetRandomScale()
        {
            float scale = GetRandom(maxScale, minScale);
            return new Vector2(scale, scale);
        }
        public override void Update(float time)
        {
            EmitParticles(time);
            List<GameObject> deadParticles = new List<GameObject>();
            foreach (GameObject particle in particles)
            {
                if (particle.bDead
                    || bDieOnFade && particle.color.W <= 0.0f)
                    deadParticles.Add(particle);
                else
                {
                    if (target != null && bHoming)
                    {
                        Vector2 dir = target.pos - particle.pos;
                        if (dir.Length() > 0.0f)
                            dir.Normalize();
                        if (bAccelerate)
                            particle.acc += dir * moveAcc;
                        else particle.vel = dir * moveSpeed;
                    }
                    else particle.vel = emissionVel;
                    particle.color.W -= time / fadeRate;
                    particle.Update(time);
                }
            }
            foreach (GameObject particle in deadParticles)
                particles.Remove(particle);
            base.Update(time);
        }
        public override void Draw(SpriteBatch batch)
        {
            foreach (GameObject particle in particles)
                particle.Draw(batch);
        }
    }
}
