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
    public class Dweller : Actor
    {
        public enum ActingMode
        {
            None,
            Shooting,
            Migrating,
            Swarming,
            Falling
        };
        public Dweller(RsActor.Desc desc, Pit pit)
            : base(desc)
        {
            this.pit = pit;
            SetUp(Vector3.Up);
            pit.Dweller = this;
            MigrateCounter = RsUtil.GetRandomFloat(4, 8);
            MovementSpeed = DefaultMovementSpeed;
        }
        public float MovementSpeed, DefaultMovementSpeed = 50, MigrateCounter;
        public override void Update(GameTime gameTime)
        {
            MigrateCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Mode == ActingMode.None)
            {
                if (GameInfo.Kathy.hitPit != null || GameInfo.LightBall.DimCounter>0)
                    Swarm();
                else if (MigrateCounter < 0)
                    Migrate();
            }
            else if (Mode == ActingMode.Migrating)
            {
                Jump();
                if (GameInfo.Kathy.hitPit != null || GameInfo.LightBall.DimCounter > 0)
                    Swarm();
                else
                {
                    Vector3 dir = pit.Center - Pos;
                    dir.Normalize();
                    dir *= MovementSpeed;
                    Velocity = new Vector3(dir.X, Velocity.Y, dir.Z);
                }
            }
            else if (Mode == ActingMode.Swarming)
            {
                if (GameInfo.Kathy.hitPit == null && GameInfo.LightBall.DimCounter<=0)
                    Mode = ActingMode.Migrating;
                else
                {
                    Jump();
                    Vector3 dir = GameInfo.Kathy.Pos - Pos;
                    dir.Y = 0;
                    dir.Normalize();
                    dir *= DefaultMovementSpeed;
                    dir.Y = Velocity.Y;
                    Velocity = dir;
                }
            }
            else if (Mode == ActingMode.Shooting)
            {
                if (Pos.Y - Radius > 0)
                {
                    Projectile proj = new Projectile(new RsActor.Desc("KathyProjectile"));
                    proj.Pos = Pos;
                    Vector3 dir = GameInfo.lastKathyLitPos - Pos;
                    dir.Normalize();
                    FaceDirection(dir);
                    proj.Velocity = dir * 100;
                    Mode = ActingMode.Falling;
                }
            }
            else if (Mode == ActingMode.Falling)
            {
                Velocity = new Vector3(0, Velocity.Y, 0);
            }
            if (Velocity.X > 0 || Velocity.Z > 0)
                FaceDirection(new Vector3(Velocity.X, 0, Velocity.Z));
            base.Update(gameTime);
        }
        //public bool bShoot = false, bMigrating = false, bSwarming=false;
        public ActingMode Mode = ActingMode.None;
        public Pit pit;
        public void JumpAndShoot()
        {
            if (Pos.Y <= Radius - GameInfo.pitDepth && hitPit == pit)
            {
                Jump();
                Mode = ActingMode.Shooting;
            }
        }
        public void Migrate()
        {
            Pit selectedPit=null;
            foreach (Pit pit in GameInfo.Pits)
            {
                if (pit.Dweller == null)
                {
                    pit.Dweller = this;
                    this.pit.Dweller = null;
                    this.pit = pit;
                    Mode = ActingMode.Migrating;
                    Jump();
                    selectedPit = pit;
                    break;
                }
            }
            if (selectedPit != null)
            {
                GameInfo.Pits.Remove(selectedPit);
                GameInfo.Pits.Add(selectedPit);
            }
        }
        public override void HitPit(Pit pit)
        {
            if (Mode == ActingMode.Swarming && pit == GameInfo.Kathy.hitPit)
                Mode = ActingMode.Falling;
            if (this.pit == pit && Mode == ActingMode.Migrating)
            {
                Mode = ActingMode.Falling;
                MigrateCounter = RsUtil.GetRandomFloat(4, 8);
                MovementSpeed = DefaultMovementSpeed;
            }
            base.HitPit(pit);
        }
        public override void LandInPit(Pit pit)
        {
            Velocity = new Vector3();
            if (pit != this.pit)
                Jump();
            if (Mode == ActingMode.Falling)
                Mode = ActingMode.None;
            base.LandInPit(pit);
        }
        public void Swarm()
        {
            Jump();
            Mode = ActingMode.Swarming;
        }
        public override void ReactToCollision(RsGameObject obj)
        {
            if (obj is LightSphere)
                MovementSpeed = 75;
            base.ReactToCollision(obj);
        }
        public override void Die()
        {
            pit.Dweller = null;
            RsContent.PlayCue("Monster");
            base.Die();
        }
    }
}
