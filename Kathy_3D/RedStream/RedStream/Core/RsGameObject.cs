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
    public abstract class RsGameObject : GameComponent
    {
        public class Desc
        {
            public Desc()
            {
                Name = "Default";
                Pos = Vel = Acc = AngVel = AngAcc = Vector3.Zero;
                Orientation = Quaternion.Identity;
            }
            public string Name;
            public Vector3 Pos, Vel, Acc, AngVel, AngAcc;
            public Quaternion Orientation;
        }
        public RsGameObject(Desc desc) : base(RedStream.Game)
        {
            dead = false;
            /* Save the initial position, velocity, and angular velocity */
            Pos = desc.Pos;

            /* By using properties, this will call the setters */
            Velocity = desc.Vel;
            AngularVelocity = desc.AngVel;
            Acceleration = desc.Acc;
            AngularAcceleration = desc.AngAcc;

            Name = desc.Name;
            Orientation = desc.Orientation;
            RsContent.AddComponent(this);
        }
        
        public Vector3 Pos;

        #region Physical Properties

        private bool dead;
        private Vector3 Vel, AngVel;
        private Vector3 Acc, AngAcc;
        private bool HasAcceleration, HasAngularAcceleration;
        private float AngVelLength, VelLength;

        public bool isDead
        {
            get { return dead; }
            set { dead = value; }
        }

        public Vector3 AngularVelocity
        {
            get { return AngVel; }
            set
            {
                AngVel = value;
                AngVelLength = AngVel.Length();
            }
        }

        public Vector3 Velocity
        {
            get { return Vel; }
            set
            {
                Vel = value;
                VelLength = Vel.Length();
            }
        }

        public Vector3 AngularAcceleration
        {
            get { return AngAcc; }
            set
            {
                AngAcc = value;
                if (AngAcc.Length() != 0)
                {
                    HasAngularAcceleration = true;
                }
                else
                {
                    HasAngularAcceleration = false;
                }
            }
        }

        public Vector3 Acceleration
        {
            get { return Acc; }
            set
            {
                Acc = value;
                if (Acc.Length() != 0)
                {
                    HasAcceleration = true;
                }
                else
                {
                    HasAcceleration = false;
                }
            }
        }
        #endregion

        public Quaternion Orientation;
        public string Name;
#if false
        public BoundingSphere BoundingSphere
        {
            get
            {
                return _sphere;
            }
            set { }
        }
#endif
        public BoundingSphere BoundingSphereOnlyForAsenicsUse;
        public float Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                SetRadius(value);
            }
        }
        public override void Update(GameTime gameTime)
        {
            BoundingSphereOnlyForAsenicsUse.Center = Pos;
            base.Update(gameTime);
        }
        public virtual void ReactToCollision(RsGameObject obj)
        {
        }
        public virtual void Advance(float time)
        {
            /* Only update velocity if the object has acceleration */
            if (HasAcceleration)
            {
                /* It has acceleration, so speed it up */
                Velocity += Acceleration * time; // This upates VelLength
            }

            /* Only update position if the object has velocity */
            if (VelLength != 0)
            {
                /* It has velocity, so move it */
                Pos += Velocity * time;
            }

            /* Only update angular velocity if the object has angular acceleration */
            if (HasAngularAcceleration)
            {
                /* The object has angular acceleration, so compute the new angular velocity */
                AngularVelocity += AngularAcceleration * time; // This updates AngVelLength
            }

            /* Only update angular position if the object has angular velocity */
            if (AngVelLength != 0.0f)
            {
                Rotate(Quaternion.CreateFromAxisAngle(AngularVelocity / AngVelLength, MathHelper.ToRadians(AngVelLength) * time));
            }
        }
        public virtual void Rotate(Quaternion quat)
        {
            Orientation *= quat;
        }
        private float _radius;
        protected virtual void SetRadius(float rad)
        {
            _radius = rad;
            BoundingSphereOnlyForAsenicsUse.Radius = rad;
        }
        public virtual void Die()
        {
            dead = true;
            Delete();
        }
        public virtual void Delete()
        {
            RsContent.RemoveComponent(this);
        }
    }
}
